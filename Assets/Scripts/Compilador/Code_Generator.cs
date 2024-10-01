using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Code_Generator : MonoBehaviour
{
    private List<AstNode> nodos;
     public   List<Card> cards;
    public   Context context; // para llevar las variables
    int cantCards;

    //builder

    public Code_Generator(List<AstNode> nodes, Context contexto)
    {
        nodos = nodes;
        cards = new List<Card>();
        context = contexto;
        cantCards = 0;
    }

    public void GenerarCodigo(string outpath)
    {
        using (StreamWriter writer = new StreamWriter(outpath))
        {
            //escribir la def de la clase effect
            writer.WriteLine(" public class EffectCreated");
            writer.WriteLine("{");

            foreach (var node in nodos)
            {
                if (node is EffectNode efecto)
                {
                    writer.WriteLine();
                    writer.WriteLine();
                    writer.WriteLine();

                    GenerateEffectMethot(writer, efecto);
                }
                else if (node is CardNode carta)
                {
                    writer.WriteLine();
                    writer.WriteLine();
                    writer.WriteLine();
                    
                    CreateCardInstancia(carta);
                }
                else
                {
                    throw new Exception(" no se reconoce el nodo");
                }
            }
            writer.WriteLine("}");
        }
    }

    public void GenerateEffectMethot(StreamWriter writer, EffectNode effectNode)
    {
        string ParamsString;

        //generar una lista de los parametros pasados en EffectNode
        if (effectNode.Params.Count != 0)
        {
            var parametros = new List<string>();
            foreach (var param in effectNode.Params)
            {
                var type = param.Value;
                if (type == "Number")
                {
                    parametros.Add($"int {param.Key}");
                }
                else if (type == "Bool")
                {
                    parametros.Add($"bool {param.Key}");
                }
                else if (type == "String")
                {
                    parametros.Add($"string {param.Key}");
                }
            }
            ParamsString = "," + string.Join(",", parametros);
        }
        else
        {
            ParamsString = "";
        }

        // eescribirlo
        writer.WriteLine(
            $"     public void {effectNode.Name}Effect(List_Cards targets , Game_contex  context  {ParamsString})"
        );
        writer.WriteLine("      {");

        foreach (var Action in effectNode.Action.Children)
        {
            GenerateActionCode(writer, Action);
        }
        writer.WriteLine("      }");
        writer.WriteLine();
    }

    public void GenerateActionCode(StreamWriter writer, AstNode action)
    {
        if (action is AssignmentNode asignacion)
        { //si contiene la variablle
            string variable_declaration = context.Variables.ContainsKey(asignacion.VariableName)
                ? " "
                : "var";
            string access = "";

            if (asignacion.Anidados != null)
            {
                for (int i = 0; i < asignacion.Anidados.Count; i++)
                {
                    if (i < asignacion.Anidados.Count - 1) // si es menor q el ultimmo elemento de la lista
                    {
                        access += asignacion.Anidados[i] + ".";
                    }
                    else
                        access += asignacion.Anidados[i];
                }
            }
            else
            {
                access = asignacion.VariableName;
            }

            //genrrar el value expresion    var topcard = 5 ejemplo sencillo
         writer.WriteLine(
                $"         {variable_declaration} {access} {asignacion.Operator} {GenerateValueExpressionCode(asignacion.ValueExp)} ;"
            );

            if (!context.Variables.ContainsKey(asignacion.VariableName))
            {
                context.Definir_Variabless(asignacion.VariableName, null); // asumiendo qe el valor se asignara mas adelante o irrelevante en este contexto
            }
        }
        else if (action is WhileNode whilenode)
        {
            writer.WriteLine($"        while( {GenerateValueExpressionCode(whilenode.Condition)})");
            writer.WriteLine("                {");
            foreach (var stament in whilenode.Body)
            {
                GenerateActionCode(writer, stament);
            }
            writer.WriteLine("                }");
        }
        else if (action is ForNode fornode)
        {
            writer.WriteLine(
                $"        foreach( Card {fornode.Item} in {GenerateValueExpressionCode(fornode.Collection)})"
            );
            writer.WriteLine("          {");
            foreach (var stament in fornode.body)
            {
                GenerateActionCode(writer, stament);
            }
            writer.WriteLine("           }");
        }
        else if (action is IFNode ifnode)
        {
            writer.WriteLine($"             if({GenerateValueExpressionCode(ifnode.Condition)})");
            writer.WriteLine("               {");
            foreach (var stament in ifnode.Body)
            {
                GenerateActionCode(writer, stament);
            }
            writer.WriteLine("                }");
        }
        else if (action is AccesoMetOrProp acceso)
        {
            if (!acceso.IsMetodo) // es propiedad
            {
                writer.WriteLine($"   {string.Join(".", acceso.Secuence)} ;");
            }
            else
            {
                string Arguments = string.Join(
                    ",",
                    acceso.Argumento.Select(arg => GenerateValueExpressionCode(arg))
                );
                writer.WriteLine($"          {string.Join(".", acceso.Secuence)}({Arguments}) ;");
            }
        }
    }

    public string GenerateValueExpressionCode(AstNode ValueExpresion)
    {
        if (ValueExpresion is Numero_Int numero)
        {
            return numero.value.ToString(); // es un numero
        }
        else if (ValueExpresion is BooleanoValor boolean)
        {
            return boolean.value.ToString().ToLower(); // es un true or false
        }
        else if (ValueExpresion is VariablReference reference) // es una variable referencia , se da el nombre de la variable
        {
            return reference.name.ToString();
        }
        else if (ValueExpresion is BinaryExpressionNode binary)
        {
            return $"{GenerateValueExpressionCode(binary.Left)} {binary.Operator} {GenerateValueExpressionCode(binary.Right)}";
        }
        else if (ValueExpresion is AccesoMetOrProp pom) //propiedad o metodo
        {
            // aqui se manejan los llamados a metodos y propiedades

            if (!pom.IsMetodo) // es propiedad
            {
                //simplemente accede a la propiedad
                return string.Join(".", pom.Secuence);
            }
            else //es un metodo
            {
                //dara los argumentos divididos por coma
                string argumentos = string.Join(
                    ",",
                    pom.Argumento.Select(argument => GenerateValueExpressionCode(argument))
                );
                return $" {string.Join(".", pom.Secuence)}({argumentos})";
            }
        }
        //agregar mas casa si fuera necesario


        //sino reconoce el tipo retorna una cadena vacia
        return "";
    }

    public void CreateCardInstancia(CardNode cardnode)
    {  
        cantCards ++;

        Card cardata = ScriptableObject.CreateInstance<Card>();
        cardata.IsCreated = true;
        cardata.Id = cantCards;
        cardata.Name = cardnode.Name;
        cardata.Type = cardnode.Type;
        cardata.Faction = cardnode.Faction;
        cardata.Power = cardnode.Power;
        cardata.Health =cardnode.Health;
        cardata.Range = cardnode
            .Range.Select(rango => (Range)Enum.Parse(typeof(Range), rango))
            .ToArray();
        cardata.OnActivation = new List<Effects>();
        cardata.EffectCreated = new EffectCreated();

        //manejar los efectos de activacion
        foreach (var action in cardnode.Activacion)
        {
            cardata.OnActivation.Add(CreateEffect(action));
        }

        cards.Add(cardata);
    }

    public Effects CreateEffect(OnAcivationNode activation)
    {
        Effects efecto = new Effects
        {
            Name = activation.Effect?.Name ?? "DefaultEffectName",
            Params = activation.Effect != null ? activation.Effect.Params : new List<object>(),
            Source = activation.Selector?.Source ?? "DefaultSource",
            Single = activation.Selector?.Single ?? false,
            Predicado = new Predicate // Aquí se crea una nueva instancia de Predicate y se asignan sus propiedades
            {
                LeftMember = activation.Selector?.Predicate?.LeftMember ?? "DefaultLeftMember",
                Operator = activation.Selector?.Predicate?.Operator ?? "DefaultOperator",
                RightMemeber = activation.Selector?.Predicate?.RightMember ?? "DefaultValue"
            },
        };
        return efecto;
    }
}
