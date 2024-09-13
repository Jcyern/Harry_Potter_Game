using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mime;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Progress;

//Arbol de sitaxis Abstracta
public abstract class AstNode
{
    // public abstract void Accept();
    public int Priority { get; set; }
    public int Fila { get; set; }
    public abstract void Debugear();
    public abstract object Evaluate(Context context);
}

public class EffectNode : AstNode
{
    public string Name { get; set; }
    public Dictionary<string, string> Params;
    public ActionNode Action { get; set; }

    public EffectNode()
    {
        Params = new Dictionary<string, string>();
        Action = new ActionNode();
    }

    public override void Debugear()
    {
        Debug.Log(" .....");
        Debug.Log($"Effect: {Name}");
        foreach (var parametro in Params)
        {
            Debug.Log(" .....");
            Debug.Log($" Amount: {parametro}");
        }
    }

    public override object Evaluate(Context context)
    {
        return 0;
    }
}

public class CardNode : AstNode
{
    public string Type { get; set; }
    public string Name { get; set; }
    public string Faction { get; set; }
    public int Power { get; set; }
    public List<string> Range { get; set; }

    public OnAcivationNode Activacion{get;set;}

    public override void Debugear()
    {
        Debug.Log(" .....");
        Debug.Log($" Typo : {Type}, Name: {Name}, Faction: {Faction}, Power:  {Power}, ");
        foreach (string rannge in Range)
        {
            Debug.Log(" .....");
            Debug.Log("Range:");
            Debug.Log(rannge);
        }
    }

    public override object Evaluate(Context context)
    {
        return 0;
    }
}

public class OnAcivationNode : AstNode //OnActivation{}
{
    public CardeffectNode Effect;
    public SelecterNode Selector;
    public PostActionNode PostAction;

    public override void Debugear()
    {
        this.Effect.Debugear();
        this.Selector.Debugear();
    }

    public override object Evaluate(Context context)
    {
        return -1;
    }
}

public class CardeffectNode : AstNode // Dentro de Card{ Effect{}  }
{
    public string Name { set; get; }
    public List<object> Params { get; set; } = new List<object>();

    public override void Debugear()
    {
        Debug.Log("Effect Card");
        Debug.Log(" .....");
        Debug.Log($" Name: {Name}");
        foreach (var param in Params)
        {
            Debug.Log($"Param :  {param}");
        }
    }

    public override object Evaluate(Context context)
    {
        return -1;
    }
}

public class ActionNode : AstNode
{
    public List<AstNode> Children { get; set; } = new();

    public override void Debugear()
    {
        Debug.Log(" .....");
        Debug.Log("Action:");
        foreach (var chid in Children)
        {
            Debug.Log(chid);
        }
    }

    public override object Evaluate(Context context)
    {
        return -1;
    }
}

public class SelecterNode : AstNode
{
    public string Source { get; set; }
    public bool Single { get; set; }
    public PredicateNode Predicate { get; set; }

    public override void Debugear()
    {
        Debug.Log(" .....");
        Debug.Log($" Source {Source}");
        Debug.Log($" Single {Single}");

        Predicate.Debugear();
    }

    public override object Evaluate(Context context)
    {
        return -1;
    }
}

public class PredicateNode : AstNode
{
    public string LeftMember { get; set; }
    public string Operator { get; set; }
    public object RightMember { get; set; }

    public override void Debugear()
    {
        Debug.Log(" .....");
        Debug.Log($"Left Member: {LeftMember}");
        Debug.Log($"Operator: {Operator} ");
        Debug.Log($"RightMemBer:  {RightMember}");
    }

    public override object Evaluate(Context context)
    {
        return -1;
    }
}

public class PostActionNode : AstNode
{
    public string Type { get; set; }
    public SelecterNode Selector;
    public string Source { get; set; }
    public bool Single { get; set; }
    public PredicateNode predicate { get; set; }
    public CardeffectNode Effect {get;set;}

    public override void Debugear()
    {
        Debug.Log(" .....");
        Debug.Log("Post Action ");
        Debug.Log($"Type:  {Type} ");
        this.Selector.Debugear();
        Debug.Log($" Source: {Source}");
        Debug.Log($" Single:  {Single}");
        predicate.Debugear();
    }

    public override object Evaluate(Context context)
    {
        return -1;
    }
}

public abstract class ExpressionNode : AstNode { }

public class Numero : ExpressionNode
{
    public int Value { get; set; }

    public override void Debugear()
    {
        Debug.Log(" .....");
        Debug.Log($"Numero es : {Value}");
    }

    public override object Evaluate(Context context)
    {
        return -1;
    }
}

public class VariablReference : ExpressionNode
{
    public string name { get; set; }
    public object value { get; set; }

    public override void Debugear()
    {
        Debug.Log(".........");
        Debug.Log($" LA variable Referenciada es {name}");
    }

    public override object Evaluate(Context context)
    {
        return context.GetVariable(name);
    }
}

public class Numero_Int : ExpressionNode
{
    public int value { get; set; }

    public override void Debugear()
    {
        Debug.Log(".........");
        Debug.Log($" El valor del Numero es {value}");
    }

    public override object Evaluate(Context context)
    {
        return value;
    }
}

public class BooleanoValor : ExpressionNode
{
    public bool value { get; set; }

    public override void Debugear()
    {
        Debug.Log(".........");
        Debug.Log($"El valor del Bool es {value}");
    }

    public override object Evaluate(Context context)
    {
        return value;
    }
}

public class CondicionNode : ExpressionNode
{
    Func<Context, bool> Condicion { get; set; } // recibe un context y devuelve un bool

    public override void Debugear()
    {
        Debug.Log("......");
    }

    public override object Evaluate(Context context)
    {
        return Condicion(context);
    }
}

public class BinaryExpressionNode : ExpressionNode
{
    public ExpressionNode Left;
    public string Operator;
    public ExpressionNode Right;

    public override void Debugear()
    {
        Debug.Log("..........");
        Left.Debugear();
        Debug.Log($"Operator: {Operator}");
        Right.Debugear();
    }

    public override object Evaluate(Context context)
    {
        var left = Left.Evaluate(context);
        var right = Right.Evaluate(context);

        switch (Operator)
        {
            case "+":
                return (int)left + (int)right;
            case "-":
                return (int)left - (int)right;
            case "*":
                return (int)left * (int)right;
            case "/":
                return (int)left / (int)right;
            case "&&":
                return (bool)left && (bool)right;
            case "||":
                return (bool)left || (bool)right;
            case "==":
                return (int)left == (int)right;
            case "!=":
                return (int)left != (int)right;
            case ">":
                return (int)left > (int)right;
            case "<":
                return (int)left < (int)right;
            case ">=":
                return (int)left >= (int)right;
            case "<=":
                return (int)left <= (int)right;

            default:
                throw new InvalidOperationException($" el operador {Operator} es desconocido");
        }
    }
}

public class AssignmentNode : ExpressionNode
{
    public string VariableName;
    public AstNode ValueExp;
    public string Operator;
    public List<string> Anidados = new();

    public override void Debugear()
    {
        Debug.Log("............");
        if (Anidados != null && Anidados.Any())
        {
            foreach (var item in Anidados)
            {
                Debug.Log(item + "+");
            }
        }
        Debug.Log($" VariableName: {VariableName}   Operator : {Operator}   ");
        ValueExp.Debugear();
    }

    public override object Evaluate(Context context)
    {
        var value = ValueExp.Evaluate(context);
        if (context.Variables.ContainsKey(VariableName))
        {
            context.SetVariable(VariableName, value);
        }
        else
        {
            context.Definir_Variabless(VariableName, value);
        }
        return value;
    }
}

public class AccesoMetOrProp : AstNode // acceso a  propiedades o metodos
{
    public List<string> Secuence { get; set; } = new(); //representar una secuencia de propiedades o metodos
    public List<ExpressionNode> Argumento { get; set; } = new(); // los argumentos q se pasaran si es un metodo

    public bool IsMetodo = false;

    public override void Debugear()
    {
        Debug.Log("......");

        if (Secuence != null && Secuence.Count > 0)
        {
            Debug.Log("Secuencias ...");
            foreach (var secuencia in Secuence)
            {
                Debug.Log(secuencia);
            }
        }
    }

    public override object Evaluate(Context context)
    {
        var objeto = context.GetVariable(Secuence[0]); //target
        for (int i = 1; i < Secuence.Count; i++)
        {
            if (!IsMetodo) //es propiedad
            {
                var propiedad = objeto.GetType().GetProperty(Secuence[i]); // busca la clase , y dame la propiedad
                if (propiedad == null) // no existe la propiiedad
                {
                    throw new Exception(
                        $" Propiedad: {Secuence[i]} no encontrada en {objeto.GetType()} "
                    );
                }
                objeto = propiedad.GetValue(objeto);
            }
            else
            {
                var MetodoInf = objeto.GetType().GetMethod(Secuence[i]);
                if (MetodoInf == null) //no existe el metodo
                {
                    throw new Exception($" El metodo {MetodoInf} no existe en {objeto.GetType()}");
                }
                objeto = MetodoInf.Invoke(objeto, Argumento.ToArray()); // ( target, []argumentos )
            }
        }
        return objeto; //valor final
    }
}

public class WhileNode : AstNode
{
    public ExpressionNode Condition { get; set; }
    public List<AstNode> Body { get; set; }

    public override void Debugear()
    {
        Debug.Log($" While ({Condition})" + "{");
        foreach (var stament in Body)
        {
            stament.Debugear();
        }
        Debug.Log("}");
    }

    public override object Evaluate(Context context)
    {
        while ((bool)Condition.Evaluate(context))
        {
            foreach (var stament in Body)
            {
                stament.Evaluate(context);
            }
        }
        return null;
    }
}

public class IFNode : AstNode
{
    public ExpressionNode Condition { get; set; }
    public List<AstNode> Body { get; set; }
    public List<AstNode> Elsebody { get; set; }

    public override void Debugear()
    {
        Debug.Log($"if( {Condition})");
        Debug.Log("{");
        foreach (var stament in Body)
        {
            stament.Debugear();
        }
        Debug.Log("}");

        if (Elsebody.Any()) // si tiene algun elemento
        {
            Debug.Log("Else {");
            foreach (var stament in Elsebody)
            {
                stament.Debugear();
            }
            Debug.Log("}");
        }
    }

    public override object Evaluate(Context context)
    {
        if ((bool)Condition.Evaluate(context))
        {
            foreach (var stament in Body)
            {
                stament.Evaluate(context);
            }
        }
        else
        {
            if (Elsebody.Any())
            {
                foreach (var stament in Elsebody)
                {
                    stament.Evaluate(context);
                }
            }
        }
        return null;
    }
}

public class ForNode : AstNode
{
    public string Item { get; set; }
    public VariablReference Collection { get; set; }
    public List<AstNode> body { get; set; } = new();

    public override void Debugear()
    {
        Debug.Log($" For item:{Item} in Collection: {Collection} ");
        Debug.Log("{");
        foreach (var item in body)
        {
            item.Debugear();
        }
        Debug.Log("}");
    }

    public override object Evaluate(Context context)
    {
        var collec = Collection.Evaluate(context) as IEnumerable<object>;
        if (collec == null)
        {
            throw new Exception($"La coleccion '{Collection.name}' no es un enumerable.");
        }

        foreach (var item in collec)
        {
            context.Definir_Variabless(Item, item);
            foreach (var stament in body)
            {
                stament.Evaluate(context);
            }
        }

        return null;
    }
}
