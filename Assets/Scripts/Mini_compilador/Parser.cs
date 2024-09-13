using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Parser : MonoBehaviour
{
    //atributos
    public static readonly Dictionary<string, (int precedence, bool rightAssociative)> Operators =
        new()
        {
            { "+", (4, false) },
            { "-", (4, false) },
            { "*", (5, false) },
            { "/", (5, false) },
            { "&&", (1, false) },
            { "||", (2, false) },
            { "!", (1, true) },
            { "==", (3, false) },
            { "!=", (3, false) },
            { "<", (3, false) },
            { ">", (3, false) },
            { ">=", (3, false) },
            { "<=", (3, false) }
        };

    private List<Token> Tokens; // las tokens creadas anteriormente
    private Token Current_Token;
    private int pos;
    private Context context; // para poder usar los metodos de context

    public TextMeshProUGUI error_text;

    //creando un dicionariosin en lengaje regex




    void Start()
    {
        error_text.text = GameObject.Find("Error_Panel").GetComponent<TextMeshProUGUI>().text;
    }

    // Es necesario movrnos por la linea inmensa de codigo osea por los Tokens

    //Metodos de movimiento


    public Token Current()
    { if(IsNext())
       {
        Current_Token = Tokens[pos];
        return Current_Token;
       }
       Current_Token = null; 
       return   Current_Token;  }

    public bool IsNext() //verifica si hay un elemento siguiente
    {
        if (pos + 1 < Tokens.Count)
        {
            return true;
        }
        return false;
    }

    public void NextToken() // dame el siguiente
    {
        if (IsNext())
            pos++;
        else
          {
            Current_Token = null ;
           Debug.Log (" se acabaron los tokens ");
          }
    }

    public Token LookNext(int i = 1)
    {
        if (IsNext())
        {
            return Tokens[pos + i];
        }
        return null;
    }

    public bool ExpectedToken(Token.TokenType tipo) // verificar si coincide el proximmo tokentype con el tipoesperado
    {
        if (IsNext())
        {
            NextToken();
            if (Current().Type == tipo)
                return true;
            else
            {
                throw new Exception(
                    $"Error en la linea {Current().Linea} columna {Current().Columna}, se esperaba como tipo {tipo}, y se paso un {Current().Type} "
                );
            }
        }
        Debug.Log("no hay mas posiciones en la lista de tokens ");
        return false;
    }

    public bool FindType(Token.TokenType findtype) // mejor uso para buscar los string
    {
        while (IsNext())
        {
            if (Current().Type == findtype)
            {
                return true;
            }
            NextToken();
        }
        throw new Exception($"Error No se encontro  el tipo {findtype}");
    }

    public void Mostrar_Error(string error)
    {
        error_text.text = error;
    }

    //builder parser
    public Parser(List<Token> words)
    {
        Tokens = words;
        context = new Context(null);
        pos = 0;
        if (words != null && words.Any())
        {
            Current_Token = words[0];
        }
    }
// Analizar las clases Efecto y Carta 

// public List<AstNode> nodes  Parse()
// {
//   List<AstNode> nodes  = new List<AstNode>();

//   while ( Current() != null)
//   { 
//     NextToken();


//   }


}

  







    //Analizar el nombre del efecto

    public void Parse_EffectName(EffectNode effect)
    {
        ExpectedToken(Token.TokenType.Name); // Name
        ExpectedToken(Token.TokenType.Dos_Puntos); //:
        ExpectedToken(Token.TokenType.Comillas); // "
        FindType(Token.TokenType.Comillas); // "
        effect.Name = Tokens[pos - 1].Value; // dentro del string
        ExpectedToken(Token.TokenType.Coma); // ,

        //definir el efecto en los dicionarios
        context.DefinirEfecto(effect.Name, effect);
    }

    //Analizar parametros
    public void Parse_EffectParams(EffectNode effect, Context effect_context)
    {
        ExpectedToken(Token.TokenType.Params);
        ExpectedToken(Token.TokenType.Dos_Puntos);
        ExpectedToken(Token.TokenType.Left_Key);

        while (IsNext() && Current_Token.Value != "}")
        {
            NextToken();
            var paramsname = Current().Value;
            ExpectedToken(Token.TokenType.Dos_Puntos);
            NextToken();
            var paramsType = Current().Value; // solo debe ser tipo Number o Bool

            if (!effect_context.Variables.ContainsKey(paramsname))
            {
                switch (paramsType)
                {
                    case "Number":
                        effect_context.Definir_Variabless(paramsname, 0);
                        break;
                    case "Bool":
                        effect_context.Definir_Variabless(paramsname, false);
                        break;
                    case "String":
                        effect_context.Definir_Variabless(paramsname, "s");
                        break;
                    default:
                        Mostrar_Error(
                            $"El tipo de dato no es permitido en {paramsname} solo acepta Bool , Number o String "
                        );
                        break;
                }
            }
            else // en el caso de existir ya
            {
                effect_context.SetVariable(paramsname, paramsType);
            }

            NextToken();
            if (Current().Value != "}")
            {
                if (Current_Token.Value != ",")
                {
                    throw new Exception(
                        $"Error en la Linea:{Current_Token.Linea} Columna:{Current_Token.Columna} se espera  una coma ( , )"
                    );
                }
            }
            else // si es igual } rompe el while
            {
                break;
            }
        }
        ExpectedToken(Token.TokenType.Coma); // ,
    }

    public void Parse_EffectAction(EffectNode effect, Context effectContext)
    {
        ExpectedToken(Token.TokenType.Action); // Action
        ExpectedToken(Token.TokenType.Dos_Puntos); // :

        while (IsNext() && Current().Value != "}")
        {
            ExpectedToken(Token.TokenType.Left_Paran); // (
            ExpectedToken(Token.TokenType.Palabra); // targets
            ExpectedToken(Token.TokenType.Coma); // ,
            ExpectedToken(Token.TokenType.Palabra); // context
            ExpectedToken(Token.TokenType.Right_Paran); // )
            ExpectedToken(Token.TokenType.Flecha); // =>
        }
    }

    // cuerpo de las acciones
    private List<AstNode> Parse_BodyActions(Context currentContext)
    {
        List<AstNode> acciones = new();

        //entramos en las acciones
        ExpectedToken(Token.TokenType.Left_Key); // {
        NextToken();
        while (IsNext() && Current().Value != "}")
        {
            switch (Current().Type)
            {
                case Token.TokenType.Palabra when LookNext().Type == Token.TokenType.Punto: // puede ser un metodo o una propiedad
                    acciones.Add(Parse_MemeberAccess(currentContext));
                    break;
                case Token.TokenType.Palabra when LookNext().Type == Token.TokenType.Asignacion: // asignacion
                    acciones.Add(Parse_Asigment(null, currentContext));
                    break;
                case Token.TokenType.If:

                default:
                    Mostrar_Error(
                        $"Expresión no reconocida en el cuerpo de Action: {Current().Value}"
                    );
                    break;
            }
        }
        return acciones;
    }

    // avanzar antes de llamar a estos metodos
    private AstNode Parse_MemeberAccess(Context current)
    { // Token.Type.Palabra
        string objetoname = Current().Value; // palabra inicial
        List<string> accesos = new() { objetoname }; // se le pasa como primer parametro objetoname

        while (LookNext() != null && LookNext().Type == Token.TokenType.Punto)
        {
            ExpectedToken(Token.TokenType.Punto);
            ExpectedToken(Token.TokenType.Palabra);
            string miembro = Current().Value;
            accesos.Add(miembro);
        }

        bool Ismetodo = false; //metodo or propiedad
        List<ExpressionNode> argumentos = new();

        if (LookNext().Type == Token.TokenType.Punto_Coma && LookNext() != null) // ; termino y es propiedad
        {
            Ismetodo = false; // es propiedad
            ExpectedToken(Token.TokenType.Punto_Coma);
        }
        else if (LookNext() != null && LookNext().Type == Token.TokenType.Left_Paran) // (
        {
            Ismetodo = true; //es un metodo;
            ExpectedToken(Token.TokenType.Left_Paran);
            NextToken(); // estamos dentro del parentesis
            argumentos = Parse_Arguments(); // Cierra el parentesis la funcion
            ExpectedToken(Token.TokenType.Punto_Coma);
        } // )
        else if (LookNext() != null && LookNext().Type == Token.TokenType.Asignacion) // =
        {
            //manejar asignaciones
            //palabra
            var asigmentnd = Parse_Asigment(accesos, context);
            return asigmentnd;
        }
        return new AccesoMetOrProp
        {
            Secuence = accesos,
            Argumento = argumentos,
            IsMetodo = Ismetodo
        };
    }

    private List<ExpressionNode> Parse_Arguments()
    {
        List<ExpressionNode> Arguments = new();

        while (Current().Type != Token.TokenType.Right_Paran) // )
        {
            switch (Current_Token.Type)
            {
                case Token.TokenType.Coma:
                    NextToken();
                    break;
                case Token.TokenType.Palabra:
                    Arguments.Add(new VariablReference { name = Current_Token.Value });
                    NextToken();
                    break;
                case Token.TokenType.Digitos:
                    Arguments.Add(new Numero_Int { value = Convert.ToInt32(Current().Value) });
                    NextToken();
                    break;
                case Token.TokenType.Booleano:
                    Current();
                    bool current = Current_Token.Value == "true"; // convierte de string a bool si value
                    Arguments.Add(new BooleanoValor { value = current });
                    NextToken();
                    break;
                default:
                    Mostrar_Error(
                        $" Error en la Linea {Current().Linea} y Columna{Current().Columna} el tipo de argumento no es reconocido "
                    );
                    break;
            }
        } // termino de recorrer los argumentos
        return Arguments;
        //ultimo token  // )



        // hacer parser de la asignacion y aumenta el token de asignacion . :""
    }

    private List<Token> Parse_ExpressionTokens()
    {
        // crea una lista  para q parsea  a la derecha del igual
        List<Token> right_expression = new();

        while (Current().Value != ";" && Current().Value != "}" && Current().Value != ")")
        {
            right_expression.Add(Current());
            NextToken();
        }
        return right_expression;
    }

    private AssignmentNode Parse_Asigment(List<string> access, Context current)
    {
        string variablename = Current().Value; // palabra  name
        ExpectedToken(Token.TokenType.Asignacion); // =
        string operador = Current().Value;
        NextToken(); // avanzamos al sig q debe ser palabra

        if (LookNext().Type == Token.TokenType.Punto)
        {
            var member = Parse_MemeberAccess(current);
            return new AssignmentNode
            {
                VariableName = variablename,
                ValueExp = member,
                Anidados = access,
                Operator = operador
            };
            // se le esta asignando a la variable el valor de un metodo o una propiedad
        }
        else
        {
            var valueExpression = Parse_Expression(Parse_ExpressionTokens(), false, current);
            ExpectedToken(Token.TokenType.Punto_Coma);
            AssignmentNode assigment =
                new()
                {
                    VariableName = variablename,
                    ValueExp = valueExpression,
                    Anidados = access,
                    Operator = operador
                };

            if (current.Variables.ContainsKey(variablename))
            {
                current.SetVariable(variablename, valueExpression);
            }
            else
            {
                current.Definir_Variabless(variablename, valueExpression.Evaluate(current));
            }
            return assigment;
        }
    }

    private ExpressionNode Parse_Expression(
        List<Token> expressionTok,
        bool Condicion,
        Context CurrentContext
    )
    {
        var postfix = ConvertToPostFixExp(expressionTok); // pasa de infija a la posfija si tengo (a+b)   => ab+
        var parsePosFit = Parse_PostfixExpression(postfix, CurrentContext); // damelo en valor algortimico

        if (Condicion && parsePosFit.Evaluate(CurrentContext).GetType() != typeof(bool))
        {
            Mostrar_Error("La expresion de condicion debe evaluar a un booleano ");
        }
        return parsePosFit;
    }

    private ExpressionNode Parse_PostfixExpression(List<Token> posfit, Context Current)
    {
        Stack<ExpressionNode> stack = new();

        foreach (var token in posfit)
        {
            if (token.Type == Token.TokenType.Digitos)
            {
                stack.Push(new Numero_Int { value = Convert.ToInt32(token.Value) }); // si es un numero asigna el valor y crea la expresion
            }
            else if (token.Type == Token.TokenType.Booleano)
            {
                stack.Push(new BooleanoValor { value = Convert.ToBoolean(token.Value) }); // conviertelo a un bool
            }
            else if (token.Type == Token.TokenType.Palabra)
            {
                if (Current.Variables.ContainsKey(token.Value))
                {
                    var valuevr = Current.GetVariable(token.Value);
                    stack.Push(new VariablReference { name = token.Value, value = valuevr });
                }
                else
                {
                    Mostrar_Error($" La variable {token.name}");
                }
            }
            else if (Operators.ContainsKey(token.Value))
            {
                var right = stack.Pop(); // por el orden q definimos recuerden q si tengo a+b eso sale ab+  b - derecho a - izq
                var left = stack.Pop();
                var operador = token.Value;
                stack.Push(
                    new BinaryExpressionNode
                    {
                        Left = left,
                        Right = right,
                        Operator = operador
                    }
                );
            }
        }
        return stack.Pop();
    }

    private List<Token> ConvertToPostFixExp(List<Token> infix) // (a+b)* (c+d) == ab+ cd+ *
    {
        Stack<Token> pila = new();
        List<Token> tokens_salida = new();

        foreach (Token Item in infix)
        {
            if (
                Item.Type == Token.TokenType.Digitos
                || Item.Type == Token.TokenType.Palabra
                || Item.Type == Token.TokenType.Booleano
            )
            {
                tokens_salida.Add(Item);
            }
            else if (Operators.ContainsKey(Item.Value)) // contiene alguno de los operadores
            {
                //revisa si hay elemento en la pila  // verifica si es asociativo a la derecha de ser asi la presedencia el elemento de la pila con el  ultimo valor     sino es asociativo a la derecha se cambia por <=
                while (
                    pila.Any()
                        && (
                            Operators[Item.Value].rightAssociative
                            && Operators[Item.Value].precedence
                                < Operators[pila.Peek().Value].precedence
                        )
                    || (
                        !Operators[Item.Value].rightAssociative
                        && Operators[Item.Value].precedence
                            <= Operators[pila.Peek().Value].precedence
                    )
                )
                {
                    Tokens.Add(pila.Pop()); // agrega el ultimo elemento de la pila
                }

                pila.Push(Item); // agrega el operador actual
            }
            else if (Item.Value == "(")
            {
                pila.Push(Item);
            }
            else if (Item.Value == ")") // se desahace de los parentesis pq el postfix no se la hace falta los parentesis para saber el orden
            {
                while (pila.Peek().Value != "(")
                {
                    tokens_salida.Add(pila.Pop());
                }
                pila.Pop();
            }
        }

        while (pila.Any())
        {
            tokens_salida.Add(pila.Pop()); // limpia la pila en caso de quedar elementos
        }
        return tokens_salida;
    }

    private WhileNode Parse_While(Context CurrentCont)
    {
        // Se supones q la palabra q vio es while
        ExpectedToken(Token.TokenType.Left_Paran);
        NextToken(); // dentro del parentesis

        WhileNode whilenoode = new();

        whilenoode.Condition = Parse_Expression(Parse_ExpressionTokens(), true, CurrentCont);
        ExpectedToken(Token.TokenType.Right_Paran);
        ExpectedToken(Token.TokenType.Left_Key);

        while (Current().Type != Token.TokenType.Right_Key) //}
        {
            NextToken(); //dentro de las llaves

            if (
                Current_Token.Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Punto
            )
            {
                whilenoode.Body.Add(Parse_MemeberAccess(CurrentCont));
            }
            else if (
                Current_Token.Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Asignacion
            )
            {
                whilenoode.Body.Add(Parse_Asigment(null, CurrentCont));
            }
            else if (Current_Token.Type == Token.TokenType.While)
            {
                whilenoode.Body.Add(Parse_While(CurrentCont));
            }
            else if (Current().Type == Token.TokenType.For)
            {
                whilenoode.Body.Add(Parse_For(CurrentCont));
            }
            else if (Current().Type == Token.TokenType.If)
            {
                whilenoode.Body.Add(Parse_If(CurrentCont));
            }
            else
            {
                Mostrar_Error(
                    $" Error en la Linea {Current().Linea} Columna {Current().Columna} no se reconoce el token dentro del While"
                );
            }
        }
        return whilenoode;
    }

    private ForNode Parse_For(Context CurrentContext)
    {
        //esta en la palabra for
        // este no lleva parentesis
        NextToken(); //target

        var NodeFor = new ForNode();

        NodeFor.Item = Current().Value; // el target
        ExpectedToken(Token.TokenType.In); //in
        ExpectedToken(Token.TokenType.Palabra); // es la collecttion
        NodeFor.Collection = new VariablReference { name = Current().Value };

        ExpectedToken(Token.TokenType.Left_Key);

        while (Current().Value != "}")
        {
            NextToken(); // dentro de las llaves

            if (
                Current_Token.Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Punto
            )
            {
                NodeFor.body.Add(Parse_MemeberAccess(CurrentContext));
            }
            else if (
                Current().Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Asignacion
            )
            {
                NodeFor.body.Add(Parse_Asigment(null, CurrentContext));
            }
            else if (Current().Type == Token.TokenType.While)
            {
                NodeFor.body.Add(Parse_While(CurrentContext));
            }
            else if (Current().Type == Token.TokenType.For)
            {
                NodeFor.body.Add(Parse_For(CurrentContext));
            }
            else if (Current().Type == Token.TokenType.If)
            {
                NodeFor.body.Add(Parse_If(CurrentContext));
            }
            else
            {
                Mostrar_Error(
                    $" Error en la linea {Current().Linea} Columna {Current().Columna} , el token no esta permitido en el ciclo for "
                );
            }
        }
        return NodeFor;
    }

    private IFNode Parse_If(Context CurrentC)
    {
        // se supone q estamos en la palabra If
        ExpectedToken(Token.TokenType.Left_Paran);
        NextToken(); //estamosdentro del parentesis

        IFNode ifnode = new IFNode();

        ifnode.Condition = Parse_Expression(Parse_ExpressionTokens(), true, CurrentC); //Evelua la expresion dentro del parenteiss , debe devolver un boolenao
        ExpectedToken(Token.TokenType.Right_Paran);

        //Parsear el cuerpo del if
        ExpectedToken(Token.TokenType.Left_Key);

        while (
            Current().Type != Token.TokenType.ELse && Current().Type != Token.TokenType.Right_Key
        )
        {
            NextToken(); //estamos dentro del cuerpo

            if (
                Current().Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Punto
            )
            {
                ifnode.Body.Add(Parse_MemeberAccess(CurrentC));
            }
            else if (
                Current().Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Asignacion
            )
            {
                ifnode.Body.Add(Parse_Asigment(null, CurrentC));
            }
            else if (Current().Type == Token.TokenType.For)
            {
                ifnode.Body.Add(Parse_For(CurrentC));
            }
            else if (Current().Type == Token.TokenType.While)
            {
                ifnode.Body.Add(Parse_While(CurrentC));
            }
            else if (Current().Type == Token.TokenType.If)
            {
                ifnode.Body.Add(Parse_If(CurrentC));
            }
            else
            {
                Mostrar_Error(
                    $" Error en la LInea{Current().Linea}, Columna {Current().Columna} estructura no reconocida en el cuerpo del if "
                );
            }
        }
        if (Current().Type == Token.TokenType.ELse)
        {
            ExpectedToken(Token.TokenType.Left_Key);

            // Parsear el cuerpo del ELse

            while (Current().Type != Token.TokenType.Right_Key)
            {
                NextToken();

                if (
                    Current().Type == Token.TokenType.Palabra
                    && LookNext().Type == Token.TokenType.Punto
                )
                {
                    ifnode.Body.Add(Parse_MemeberAccess(CurrentC));
                }
                else if (
                    Current().Type == Token.TokenType.Palabra
                    && LookNext().Type == Token.TokenType.Asignacion
                )
                {
                    ifnode.Body.Add(Parse_Asigment(null, CurrentC));
                }
                else if (Current().Type == Token.TokenType.For)
                {
                    ifnode.Body.Add(Parse_For(CurrentC));
                }
                else if (Current().Type == Token.TokenType.While)
                {
                    ifnode.Body.Add(Parse_While(CurrentC));
                }
                else if (Current().Type == Token.TokenType.If)
                {
                    ifnode.Body.Add(Parse_If(CurrentC));
                }
                else
                {
                    Mostrar_Error(
                        $" Error en la LInea{Current().Linea}, Columna {Current().Columna} estructura no reconocida en el cuerpo del else "
                    );
                }
            }
            ExpectedToken(Token.TokenType.Right_Key);
        }

        return ifnode;
    }

    private CardNode Parse_Card()
    {
        // se supone q recocio la palabra clave carta
        ExpectedToken(Token.TokenType.Left_Key);

        var card = new CardNode();

        while (Current().Value != "}")
        {
            NextToken(); //estamos dentro de las llaves

            if (Current().Type == Token.TokenType.Type)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Palabra);
                card.Type = Current().Value;
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);
            }
            else if (Current().Type == Token.TokenType.Name)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Comillas);
                card.Name = Current().Value;
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);

                context.DefinirCarta(card.Name, card);
            }
            else if (Current().Type == Token.TokenType.Faction)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Palabra);
                card.Faction = Current().Value;
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);
            }
            else if (Current().Type == Token.TokenType.Power)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Palabra);
                card.Power = Convert.ToInt32(Current().Value);
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);
            }
            else if (Current().Type == Token.TokenType.Range)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Left_Corchete);

                while (Current().Value != "]")
                {
                    ExpectedToken(Token.TokenType.Comillas);
                    ExpectedToken(Token.TokenType.Palabra);
                    card.Range.Add(Current().Value);
                    ExpectedToken(Token.TokenType.Comillas);
                    if (LookNext().Type != Token.TokenType.Right_Corchete)
                    {
                        ExpectedToken(Token.TokenType.Coma);
                    }
                }
                ExpectedToken(Token.TokenType.Coma);
            }
            else if (Current().Type == Token.TokenType.OnActivacion)
            {
                card.Activacion = Parse_OnActivation();

            }

        }
        //salio pq encontro la llave 
        return card;
    }

    private OnAcivationNode Parse_OnActivation()
    {
        var action = new OnAcivationNode();
        // estamos en la palbra clave ONActivation
        ExpectedToken(Token.TokenType.Dos_Puntos);
        ExpectedToken(Token.TokenType.Left_Corchete); //[
        ExpectedToken(Token.TokenType.Left_Key); //{

        while (Current().Value != "}" && Current().Type != Token.TokenType.PostAction)
        {
            NextToken();
            if (Current().Type == Token.TokenType.Effect_activacion)
            {
                action.Effect = Parse_CardEffect();
            }
            else if (Current().Type == Token.TokenType.Selector)
            {
                action.Selector = Parse_Selector();
            }
        }
        if (Current().Type == Token.TokenType.PostAction)
        {
            PostActionNode postaction = new();
            ExpectedToken(Token.TokenType.Dos_Puntos);
            ExpectedToken(Token.TokenType.Left_Key);
            ExpectedToken(Token.TokenType.Type);
            ExpectedToken(Token.TokenType.Dos_Puntos);
            ExpectedToken(Token.TokenType.Palabra);
            postaction.Type = Current().Value;
            ExpectedToken(Token.TokenType.Comillas);
            ExpectedToken(Token.TokenType.Coma);

            while (Current().Value != "}")
            {
                if (Current().Type == Token.TokenType.Selector)
                {
                    postaction.Selector = Parse_Selector();
                }
                if (Current().Type == Token.TokenType.Effect_activacion)
                {
                    postaction.Effect = Parse_CardEffect();
                }
            }
            ExpectedToken(Token.TokenType.Coma);
            action.PostAction = postaction;
        }
        // Estamos en una llave ahora 
        ExpectedToken(Token.TokenType.Right_Corchete);

        return action;
    }

    private CardeffectNode Parse_CardEffect()
    {
        ExpectedToken(Token.TokenType.Dos_Puntos);
        ExpectedToken(Token.TokenType.Left_Key);

        var effectcard = new CardeffectNode();

        while (Current().Value != "}")
        {
            NextToken();
            if (Current().Type == Token.TokenType.Name)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Comillas);
                if (!context.Efectos.ContainsKey(Current().Value))
                {
                    Mostrar_Error(
                        $" Error en la Linea {Current().Linea} y Columna {Current().Columna} el efecto no esta previamente definido "
                    );
                }
                effectcard.Name = Current_Token.Value;

                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);
            }
            else if (Current().Type == Token.TokenType.Palabra)
            { // palabra q debe estar creada anteriormente en params
                while (Current().Value != "}")
                {
                    if (context.Variables.ContainsKey(Current().Value))
                    {
                        var name = Current().Value;
                        var tipo = context.GetVariable(Current().Value);
                        ExpectedToken(Token.TokenType.Dos_Puntos);

                        if (tipo == (object)0)
                        {
                            ExpectedToken(Token.TokenType.Digitos);
                            context.SetVariable(name, Convert.ToInt32(Current().Value));
                            effectcard.Params.Add(Convert.ToInt32(Current().Value));
                            ExpectedToken(Token.TokenType.Coma);
                        }
                        else if (tipo == (object)false)
                        {
                            ExpectedToken(Token.TokenType.Booleano);
                            context.SetVariable(name, Convert.ToBoolean(Current().Value));
                            effectcard.Params.Add(Convert.ToBoolean(Current().Value));
                            ExpectedToken(Token.TokenType.Coma);
                        }
                        else if (tipo == (object)"s")
                        {
                            ExpectedToken(Token.TokenType.Comillas);
                            ExpectedToken(Token.TokenType.Palabra);
                            context.SetVariable(name, Current().Value);
                            effectcard.Params.Add(Current().Value);
                            ExpectedToken(Token.TokenType.Comillas);
                            ExpectedToken(Token.TokenType.Coma);
                        }
                    }
                    else
                    {
                        Mostrar_Error(
                            $"Error en la Linea {Current().Linea} Columna:{Current().Columna} la variable no esta definida previamnete en los params del efecto"
                        );
                        throw new Exception(
                            $"Error en la Linea {Current().Linea} Columna:{Current().Columna} la variable no esta definida previamnete en los params del efecto"
                        );
                    }
                }
            }
        }
        ExpectedToken(Token.TokenType.Coma); // termina con la coma del cmbio
        return effectcard;
    }

    private SelecterNode Parse_Selector()
    {
        ExpectedToken(Token.TokenType.Dos_Puntos);
        ExpectedToken(Token.TokenType.Left_Key);

        SelecterNode selector = new SelecterNode();
        while (Current().Value != "}")
        {
            NextToken();

            if (Current().Type == Token.TokenType.Source)
            {
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Palabra);
                selector.Source = Current().Value;
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);
            }
            else if (Current().Type == Token.TokenType.Single)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Booleano);
                selector.Single = Convert.ToBoolean(Current().Value);
                ExpectedToken(Token.TokenType.Coma);
            }
            else if (Current().Type == Token.TokenType.Predicate)
            {
                PredicateNode predicado = new();

                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Left_Paran);
                ExpectedToken(Token.TokenType.Palabra);
                ExpectedToken(Token.TokenType.Right_Paran);
                ExpectedToken(Token.TokenType.Flecha);
                ExpectedToken(Token.TokenType.Palabra);
                ExpectedToken(Token.TokenType.Punto);
                ExpectedToken(Token.TokenType.Palabra);

                predicado.LeftMember = Current().Value;
                ExpectedToken(Token.TokenType.Relacional);
                predicado.Operator = Current().Value;
                ExpectedToken(Token.TokenType.Palabra);

                if (Current().Type == Token.TokenType.Digitos)
                {
                    predicado.RightMember = Convert.ToInt32(Current().Value);
                    ExpectedToken(Token.TokenType.Coma);
                }
                else if (Current().Type == Token.TokenType.Comillas)
                {
                    ExpectedToken(Token.TokenType.Palabra);
                    predicado.RightMember = Current().Value;
                    ExpectedToken(Token.TokenType.Comillas);
                    ExpectedToken(Token.TokenType.Coma);
                }
                else
                {
                    Mostrar_Error(
                        $"Error en la Linea {Current().Linea} Columna {Current().Columna}   Tipo de valor no permito para 'Predicate' se esperaba 'number' o 'string' pero se obtuvo {Current().Type}"
                    );
                }
            }
        }
        ExpectedToken(Token.TokenType.Coma);
        return selector;
    }
}
