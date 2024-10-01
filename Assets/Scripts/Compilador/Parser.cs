using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
    public  Context context; // para poder usar los metodos de context y almacen de todo lo creado 

    public TextMeshProUGUI error_text;

    //creando un dicionariosin en lengaje regex






    // Es necesario movrnos por la linea inmensa de codigo osea por los Tokens

    //Metodos de movimiento


    public Token Current()
    {
        Current_Token = Tokens[pos];
        return Current_Token;

        // else
        // {
        //      Token nullToken = new(Token.TokenType.Nulo ,"null", 0, 0 );
        //      return nullToken;
        // }
    }

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
        else // si no hay siguiente
        {
            throw new Exception(" No hay mas tokens en la lista ");
        }
    }

    public Token LookNext(int i = 1)
    {
        if (IsNext())
        {
            return Tokens[pos + i];
        }
        return new Token(Token.TokenType.Nulo, "null", 0, 0);
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
                Mostrar_Error(
                    $"Error en la linea {Current().Linea} columna {Current().Columna}, se esperaba como tipo {tipo}, y se paso un {Current().Type} "
                );
                throw new Exception(
                    $"Error en la linea {Current().Linea} columna {Current().Columna}, se esperaba como tipo {tipo}, y se paso un {Current().Type} "
                        + $"Token trasero {Tokens[pos + 1]}"
                );
            }
        }

        return false;
    }

    public void Mostrar_Error(string error)
    {
        error_text.text = error;
    }

    //builder parser
    public Parser(List<Token> words, TextMeshProUGUI panel)
    {
        Tokens = words;
        context = new Context(null);
        pos = 0;
        if (words != null && words.Any())
        {
            Current_Token = words[0];
        }
        error_text = panel;
    }

    // Analizar las clases Efecto y Carta

    public List<AstNode> Parse() // principal
    {
        List<AstNode> nodos = new List<AstNode>();
        while (pos != Tokens.Count - 1)
        {
            if (Current().Type == Token.TokenType.Effect)
            {
                nodos.Add(Parse_Effect());
                if (IsNext())
                    NextToken();
            }
            else if (Current().Type == Token.TokenType.Card)
            {
                nodos.Add(Parse_Card());
                if (IsNext())
                    NextToken();
            }
            else
            {
                throw new Exception(
                    $"Error Linea:{Current().Linea}    Columna:{Current().Columna} La clase {Current().Value}  no esta reconocida por favor verifique "
                );
            }
        }

        return nodos;
    }

    public EffectNode Parse_Effect()
    {
        EffectNode efecto = new EffectNode();
        Context effectcontexto = new();
        // se supone q el token actual es effect
        ExpectedToken(Token.TokenType.Left_Key);
        NextToken(); // dentro de las llaves
        while (Current().Type != Token.TokenType.Right_Key)
        {
            if (Current().Type == Token.TokenType.Name)
            {
                Parse_EffectName(efecto);
                NextToken();
            }
            else if (Current().Type == Token.TokenType.Params)
            {
                Parse_EffectParams(efecto, effectcontexto);
                NextToken();
            }
            else if (Current().Type == Token.TokenType.Action)
            {
                Parse_EffectAction(efecto, effectcontexto);
                NextToken();
            }
            else
            {
                throw new Exception(
                    $" Error en Parse Effect  el tipo pasado no corresponde {Current().Type} -- {Current().Value}"
                );
            }
        }
        return efecto;
    }

    //Analizar el nombre del efecto

    public void Parse_EffectName(EffectNode effect)
    {
        // se suponee q estamos en name
        ExpectedToken(Token.TokenType.Dos_Puntos); //:
        ExpectedToken(Token.TokenType.Comillas); // "
        ExpectedToken(Token.TokenType.Palabra); // "
        effect.Name = Current().Value; //
        ExpectedToken(Token.TokenType.Comillas);
        ExpectedToken(Token.TokenType.Coma); // ,

        //agregar el efecto en los dicionarios
        context.DefinirEfecto(effect.Name, effect);
    }

    //Analizar parametros
    public void Parse_EffectParams(EffectNode effect, Context effect_context)
    {
        // se supone q estmo en params
        ExpectedToken(Token.TokenType.Dos_Puntos);
        ExpectedToken(Token.TokenType.Left_Key);
        NextToken(); // dentro de las llaves
        while (Current_Token.Value != "}")
        {
            var paramsname = Current().Value;
            ExpectedToken(Token.TokenType.Dos_Puntos);
            NextToken();
            var paramsType = Current().Value; // solo debe ser tipo Number o Bool o String

            if (!effect_context.Variables.ContainsKey(paramsname)) // sino hay ninguna variable con ese nombre definida
            {
                switch (paramsType)
                {
                    case "Number":
                        context.Definir_Variabless(paramsname, 0);
                        effect_context.Definir_Variabless(paramsname, 0);
                        effect.Params[paramsname] = paramsType;

                        break;
                    case "Booleano":
                        context.Definir_Variabless(paramsname, false);
                        effect_context.Definir_Variabless(paramsname, false);
                        effect.Params[paramsname] = paramsType;
                        break;
                    case "String":
                        context.Definir_Variabless(paramsname, "");
                        effect_context.Definir_Variabless(paramsname, "");
                        effect.Params[paramsname] = paramsType;
                        break;
                    default:

                        Mostrar_Error(
                            $"El tipo de dato no es permitido en {paramsname} solo acepta Bool , Number o String "
                        );
                        throw new Exception(
                            $"El tipo de dato no es permitido en {paramsname} solo acepta Bool , Number o String "
                        );
                }
            }
            else // en el caso de existir ya
            {
                effect_context.SetVariable(paramsname, paramsType);
            }

            NextToken();
            if (Current().Value != "}")
            {
                if (Current().Type != Token.TokenType.Coma)
                    throw new Exception(" Falta la coma ");

                NextToken(); // La siguiente palabra
            }
            else // si es igual } rompe el while
            {
                break;
            }
        }

        ExpectedToken(Token.TokenType.Coma); // , // termino los params
    }

    public void Parse_EffectAction(EffectNode effect, Context effectContext)
    {
        // se supone q esta en action
        ExpectedToken(Token.TokenType.Dos_Puntos); // :

        while (Current().Value != "}")
        {
            ExpectedToken(Token.TokenType.Left_Paran); // (
            ExpectedToken(Token.TokenType.Palabra); // targets
            ExpectedToken(Token.TokenType.Coma); // ,
            ExpectedToken(Token.TokenType.Palabra); // context
            ExpectedToken(Token.TokenType.Right_Paran); // )
            ExpectedToken(Token.TokenType.Flecha); // =>

            effect.Action.Children = Parse_BodyActions(effectContext);
        }
    }

    // cuerpo de las acciones
    private List<AstNode> Parse_BodyActions(Context currentContext)
    {
        List<AstNode> acciones = new();

        ExpectedToken(Token.TokenType.Left_Key); // {
        NextToken(); //entramos en las acciones
        while (Current().Value != "}")
        {
            if (
                Current().Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Punto
            ) // puede ser un metodo o una propiedad
            {
                acciones.Add(Parse_MemeberAccess(currentContext));
                NextToken();
            }
            else if (
                Current().Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Asignacion
            ) // asignacion
            {
                acciones.Add(Parse_Asigment(null, currentContext));
                NextToken();
            }
            else if (Current().Type == Token.TokenType.If)
            {
                acciones.Add(Parse_If(currentContext));
                NextToken();
            }
            else if (Current().Type == Token.TokenType.While)
            {
                acciones.Add(Parse_While(currentContext));
                NextToken();
            }
            else if (Current().Type == Token.TokenType.For)
            {
                acciones.Add(Parse_For(currentContext));
                //ultimo token la llave

                NextToken(); // pasa al siguiente
            }
            else
            {
                Mostrar_Error($"Expresión no reconocida en el cuerpo de Action: {Current().Value}");
                throw new Exception(
                    $"Expresión no reconocida en el cuerpo de Action: {Current().Value}"
                );
            }
        }
        return acciones;
    }

    // avanzar antes de llamar a estos metodos
    private AstNode Parse_MemeberAccess(Context current)
    { // Token.Type.Palabra
        string objetoname = Current().Value; // palabra inicial
        List<string> accesos = new() { objetoname }; // se le pasa como primer parametro objetoname

        while (LookNext().Type == Token.TokenType.Punto)
        {
            ExpectedToken(Token.TokenType.Punto);
            ExpectedToken(Token.TokenType.Palabra);
            string miembro = Current().Value;
            accesos.Add(miembro); // va anadiendo los accesos
        }
        Debug.Log("objeto" + objetoname);

        if (accesos.Any())
        {
            Debug.Log("token " + string.Join(".", accesos));
        }
        bool Ismetodo = false; //metodo or propiedad
        List<ExpressionNode> argumentos = new();

        if (LookNext().Type == Token.TokenType.Punto_Coma && LookNext() != null) // ; termino y es propiedad
        {
            Ismetodo = false; // es propiedad
            ExpectedToken(Token.TokenType.Punto_Coma);
        }
        else if (LookNext().Type == Token.TokenType.Left_Paran) // (
        {
            Ismetodo = true; //es un metodo;
            ExpectedToken(Token.TokenType.Left_Paran);
            NextToken(); // estamos dentro del parentesis
            argumentos = Parse_Arguments(); // Cierra el parentesis la funcion
            ExpectedToken(Token.TokenType.Punto_Coma);
        } // )
        else if (LookNext().Type == Token.TokenType.Asignacion) // =
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
        NextToken(); // avanzamos al sig q debe ser palabra // estamos en la parte derecha del igual

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
        { //forma lista con la parte derecha del igual ( ParseExpressionTokens)
            var valueExpression = Parse_Expression(Parse_ExpressionTokens(), false, current);
            Debug.Log(" debugeando el expresion");
          
           

            // todo oka

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
                current.SetVariable(variablename, valueExpression.Evaluate(current));
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
        for (int i = 0; i < expressionTok.Count; i++)
        {
            Debug.Log(expressionTok[i].Value);
        }
        var postfix = ConvertToPostFixExp(expressionTok); // pasa de infija a la posfija si tengo (a+b)   => ab+

        var parsePosFit = Parse_PostfixExpression(postfix, CurrentContext); // damelo en valor algortimico
         
        
       // Debug.Log(parsePosFit.Evaluate(CurrentContext));

        if (Condicion && parsePosFit.Evaluate(CurrentContext).GetType() != typeof(bool))
        {
            throw new Exception("La expresion de condicion debe evaluar a un booleano ");
        }
        // todo oka
        return parsePosFit;
    }

    private ExpressionNode Parse_PostfixExpression(List<Token> posfit, Context Current)
    { // debuggrear esto
        Stack<ExpressionNode> stack = new();
        Debug.Log("Los tokens ingresados son " + string.Join(",", posfit.Select(t => t.Value)));
        foreach (var token in posfit)
        {
            Debug.Log($" Trabajando con {token.Value} tipo {token.Type}");
            if (token.Type == Token.TokenType.Digitos)
            {
                Debug.Log($" se ingresara en la pila el elemento{token.Value} tipo {token.Type}");
                stack.Push(new Numero_Int { value = Convert.ToInt32(token.Value) }); // si es un numero asigna el valor y crea la expresion
            }
            else if (token.Type == Token.TokenType.Booleano)
            {
                Debug.Log($" se ingresa en la pila el elemento {token.Value} tipo {token.Type}");
                stack.Push(new BooleanoValor { value = Convert.ToBoolean(token.Value) }); // conviertelo a un bool
            }
            else if (token.Type == Token.TokenType.Palabra || !Operators.ContainsKey(token.Value))
            {
                Debug.Log($"Verificando la variable {token.Value} tipo {token.Type}");
                if (Current.Variables.ContainsKey(token.Value))
                {
                    Debug.Log(" Existe ");
                    var valuevr = Current.GetVariable(token.Value);
                    stack.Push(new VariablReference { name = token.Value, value = valuevr });
                }
                else
                {
                    Mostrar_Error($" La variable {token.name}");
                    throw new Exception($" La variable {token.name} no esta previamente definida ");
                }
            }
            else if (Operators.ContainsKey(token.Value))
            {
                Debug.Log("operador ");
                var right = stack.Pop(); // por el orden q definimos recuerden q si tengo a+b eso sale ab+  b - derecho a - izq
                var left = stack.Pop();
                var operador = token.Value;
                Debug.Log($"Binnary expresion {left}+ {operador}+ {right}");
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
        Debug.Log("Tokens de entrada: " + string.Join(", ", infix.Select(t => t.Value)));

        foreach (var Item in infix)
        {
            Debug.Log($" Token en proceso  {Item.Value}+ {Item.Type}");
            if (!Operators.ContainsKey(Item.Value))
            {
                Debug.Log($" se va a anadir a lalista el token  {Item.Value}+ {Item.Type}");
                tokens_salida.Add(Item);
            }
            else if (Operators.ContainsKey(Item.Value)) // contiene alguno de los operadores
            {
                Debug.Log($" El token pertenece a operadores  {Item.Value}+ {Item.Type}");
                //revisa si hay elemento en la pila  // verifica si es asociativo a la derecha de ser asi la presedencia el elemento de la pila con el  ultimo valor     sino es asociativo a la derecha se cambia por <=
                while (
                    pila.Any()
                    && (
                        ( // sino no lo incluye
                            Operators[Item.Value].rightAssociative
                            && Operators[Item.Value].precedence
                                < Operators[pila.Peek().Value].precedence
                        )
                        || ( // el casp q es asociativo a la dereha lo incluye
                            !Operators[Item.Value].rightAssociative
                            && Operators[Item.Value].precedence
                                <= Operators[pila.Peek().Value].precedence
                        )
                    )
                )
                {
                    Debug.Log(
                        $" se agrega a lista desde la pila el token  {Item.Value}+ {Item.Type}"
                    );
                    tokens_salida.Add(pila.Pop()); // agrega el ultimo elemento de la pila
                }
                Debug.Log($" insertamos en la pila el tooken  {Item.Value}+ {Item.Type}");
                pila.Push(Item); // agrega el operador actual
            }
            else if (Item.Value == "(")
            {
                pila.Push(Item);
            }
            else if (Item.Value == ")") // se desahace de los parentesis pq el postfix no se la hace falta los parentesis para saber el orden
            {
                if (pila.Any())
                {
                    while (pila.Peek().Value != "(")
                    {
                        tokens_salida.Add(pila.Pop());
                    }
                    pila.Pop();
                }
            }
        }

        while (pila.Any())
        {
            Debug.Log("queda elementos en la pila y los sacamso ");
            tokens_salida.Add(pila.Pop()); // limpia la pila en caso de quedar elementos
        }
        Debug.Log(" >>>>>>>>>>>.....");
        Debug.Log(
            $" Los elementos salen de la lista asi"
                + string.Join(",", tokens_salida.Select(token => token.Value))
        );

        return tokens_salida;
    }

    private bool FindType(Token.TokenType type)
    {
        int i = 1;
        while (pos + i < Tokens.Count)
        {
            if (LookNext(i).Type == type)
            {
                Debug.Log(LookNext(i).Type + " " + LookNext(i).Value);
                return true;
            }
            i += 1;
        }
        return false;
    }

    private WhileNode Parse_While(Context CurrentCont)
    {

       
        // Se supones q la palabra q vio es while
        ExpectedToken(Token.TokenType.Left_Paran);
        NextToken(); // dentro del parentesis

        WhileNode whilenoode = new();
        // revisar si cerro el parentesis



        if (!FindType(Token.TokenType.Right_Paran))
        {
            throw new Exception(" No se cerro el while con un parentesis ");
        }

        List<Token> list = Parse_ExpressionTokens();

        whilenoode.Condition = Parse_Expression(list, true, CurrentCont);
        // ultimo token es )
        ExpectedToken(Token.TokenType.Left_Key);
        NextToken(); // cuerpo del while
        while (Current().Type != Token.TokenType.Right_Key) //}// los while en este lenguaje   termina con el punto y coma y solo una accion
        {
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
            else if (Current().Type == Token.TokenType.Punto_Coma)
            {
                NextToken();
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
        NextToken(); // dentro de las llaves
        while (Current().Value != "}")
        {
            if (
                Current_Token.Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Punto
            )
            {
                NodeFor.body.Add(Parse_MemeberAccess(CurrentContext));
                NextToken();
            }
            else if (
                Current().Type == Token.TokenType.Palabra
                && LookNext().Type == Token.TokenType.Asignacion
            )
            {
                NodeFor.body.Add(Parse_Asigment(null, CurrentContext));

                NextToken();
            }
            else if (Current().Type == Token.TokenType.While)
            {
                NodeFor.body.Add(Parse_While(CurrentContext));

                NextToken();
            }
            else if (Current().Type == Token.TokenType.For)
            {
                NodeFor.body.Add(Parse_For(CurrentContext));
                NextToken();
            }
            else if (Current().Type == Token.TokenType.If)
            {
                NodeFor.body.Add(Parse_If(CurrentContext));
                NextToken();
            }
            else if (Current().Type == Token.TokenType.Punto_Coma)
            {
                NextToken();
            }
            else
            {
                Mostrar_Error(
                    $" Error en la linea {Current().Linea} Columna {Current().Columna} , el token no esta permitido en el ciclo for "
                );
                throw new Exception(
                    $" Error en la linea {Current().Linea} Columna {Current().Columna} , el token no esta permitido en el ciclo for , es {Current().Value} and {pos}"
                );
            }
        }
        ExpectedToken(Token.TokenType.Punto_Coma); // despues del for se pone punto y coma

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
        NextToken(); //estamos dentro de las llaves

        while (Current().Type != Token.TokenType.Right_Key)
        {
            if (Current().Type == Token.TokenType.Type)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Personaje);
                card.Type = Current().Value;
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);
                NextToken();
            }
            else if (Current().Type == Token.TokenType.Name)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Palabra);
                card.Name = Current().Value;
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);
                NextToken();

                context.DefinirCarta(card.Name, card);
            }
            else if (Current().Value == "Faction")
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Facciones);
                card.Faction = Current().Value;
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);
                NextToken();
            }
            else if (Current().Value == "Power")
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Digitos);
                card.Power = Convert.ToInt32(Current().Value);
                ExpectedToken(Token.TokenType.Coma);
                NextToken();
            }
            else if( Current().Value == "Health")
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Digitos);
                card.Health = Convert.ToInt32(Current().Value);
                ExpectedToken(Token.TokenType.Coma);
                NextToken();
            }
            else if (Current().Value == "Range")
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                Debug.Log(":");
                ExpectedToken(Token.TokenType.Left_Corchete);
                Debug.Log("[");

                if (!FindType(Token.TokenType.Right_Corchete))
                {
                    throw new Exception(" no encontro el corchete ");
                }

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
                    else
                    {
                        ExpectedToken(Token.TokenType.Right_Corchete); // ]
                    }
                }
                ExpectedToken(Token.TokenType.Coma);
                NextToken();
            }
            else if (Current().Value == "OnActivation")
            {
                card.Activacion.Add ( Parse_OnActivation());
            }
            else if (Current().Value == "}")
            {
                break;
            }
            else
            {
                throw new Exception(
                    $" No pertenece a card el tipo {Current().Type} con valor de {Current().Value}"
                );
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
         NextToken();
        while (Current().Value != "}" && Current().Type != Token.TokenType.PostAction)
        {
           
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
            ExpectedToken(Token.TokenType.Comillas);
            ExpectedToken(Token.TokenType.Palabra);
            postaction.Type = Current().Value;
            ExpectedToken(Token.TokenType.Comillas);
            ExpectedToken(Token.TokenType.Coma);
            NextToken();

            while (Current().Value != "}")
            {
                if (Current().Type == Token.TokenType.Selector)
                {
                    postaction.Selector = Parse_Selector();
                }
              else   if (Current().Type == Token.TokenType.Effect_activacion)
                {
                    postaction.Effect = Parse_CardEffect();
                }
                else 
                {
                    throw new Exception ( $" No pertence a PostAction el token {Current().Value} de tipo {Current().Type}");
                }
            }
            ExpectedToken(Token.TokenType.Coma);
            NextToken();// llave
            ExpectedToken(Token.TokenType.Right_Corchete);
            
            action.PostAction = postaction;
        }
        else
        
        {
            ExpectedToken(Token.TokenType.Right_Corchete);

        }  //si salimos del post action igual estamos en la llave 
        // Estamos en una llave ahora
        Debug.Log(" el current actual es " + Current().Value);
        NextToken(); // debe ser la llave final 
        

        return action;
    }

    private CardeffectNode Parse_CardEffect()
    {
        ExpectedToken(Token.TokenType.Dos_Puntos);
        var effectcard = new CardeffectNode();

       
        
        
            ExpectedToken(Token.TokenType.Left_Key);

            NextToken(); // dentro del effecto llaves
            while (Current().Value != "}")
            {
                if (Current().Type == Token.TokenType.Name)
                {
                    ExpectedToken(Token.TokenType.Dos_Puntos);
                    ExpectedToken(Token.TokenType.Comillas);
                    ExpectedToken(Token.TokenType.Palabra);

                    if (!context.Efectos.ContainsKey(Current().Value))
                    {
                        throw new Exception(
                            $" Error en la Linea {Current().Linea} y Columna {Current().Columna} el efecto no esta previamente definido "
                        );
                    }

                    effectcard.Name = Current_Token.Value;

                    ExpectedToken(Token.TokenType.Comillas);
                    ExpectedToken(Token.TokenType.Coma);
                    NextToken();
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

                            if ((int)tipo == 0)
                            {
                                ExpectedToken(Token.TokenType.Digitos);
                                context.SetVariable(name, Convert.ToInt32(Current().Value));
                                effectcard.Params.Add(Convert.ToInt32(Current().Value));
                                ExpectedToken(Token.TokenType.Coma);
                                NextToken();
                            }
                            else if ((bool)tipo == false)
                            {
                                ExpectedToken(Token.TokenType.Booleano);
                                context.SetVariable(name, Convert.ToBoolean(Current().Value));
                                effectcard.Params.Add(Convert.ToBoolean(Current().Value));
                                ExpectedToken(Token.TokenType.Coma);
                                NextToken();
                            }
                            else if ((string)tipo == "s")
                            {
                                ExpectedToken(Token.TokenType.Comillas);
                                ExpectedToken(Token.TokenType.Palabra);
                                context.SetVariable(name, Current().Value);
                                effectcard.Params.Add(Current().Value);
                                ExpectedToken(Token.TokenType.Comillas);
                                ExpectedToken(Token.TokenType.Coma);
                                NextToken();
                            }
                            else
                            {
                                throw new Exception(" NO,  se reconoce el tipo " + tipo);
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
                        object valor = context.GetVariable("Amount");
                    }
                }
                else
                {
                    throw new Exception(
                        $" Error en la linea {Current().Linea} el token {Current().Value} de tipo {Current().Type} no pertenece a parsear Efecto card"
                    );
                }
            
        }
        ExpectedToken(Token.TokenType.Coma); // termina con la coma del cmbio
        NextToken(); // ver q sigue 
        return effectcard;
    }

    private SelecterNode Parse_Selector()
    {
        ExpectedToken(Token.TokenType.Dos_Puntos);
        ExpectedToken(Token.TokenType.Left_Key);

        SelecterNode selector = new SelecterNode();
        NextToken();
        while (Current().Value != "}")
        {
            if (Current().Type == Token.TokenType.Source)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Comillas);
              
            
            
              if( LookNext().Value.ToLower() != "board" &&  LookNext().Value.ToLower() != "hand" &&   LookNext().Value.ToLower() != "cementery" &&  LookNext().Value.ToLower() != "deck")
               {
                 throw new Exception( "Se espera que el source sea tipo board , deck, cementery, hand");
               }

                NextToken();
                selector.Source = Current().Value;
                ExpectedToken(Token.TokenType.Comillas);
                ExpectedToken(Token.TokenType.Coma);
                NextToken();
            }
            else if (Current().Type == Token.TokenType.Single)
            {
                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Booleano);
                selector.Single = Convert.ToBoolean(Current().Value);
                ExpectedToken(Token.TokenType.Coma);
                NextToken();
            }
            else if (Current().Type == Token.TokenType.Predicate)
            {
               

                ExpectedToken(Token.TokenType.Dos_Puntos);
                ExpectedToken(Token.TokenType.Left_Paran);
                ExpectedToken(Token.TokenType.Palabra);
                ExpectedToken(Token.TokenType.Right_Paran);
                ExpectedToken(Token.TokenType.Flecha);
                ExpectedToken(Token.TokenType.Palabra);
                ExpectedToken(Token.TokenType.Punto);
                
                if (LookNext().Value != "Type" && LookNext().Value != "Range" && LookNext().Value != "Power"&& LookNext().Value !="Faction" )
                {
                    throw new Exception( " Se espera un tipo , Type , Range , Faction , Power");
                }
                NextToken();

                selector.Predicate.LeftMember = Current().Value;
                ExpectedToken(Token.TokenType.Relacional);
                selector.Predicate.Operator = Current().Value;
                NextToken();

                if (Current().Type == Token.TokenType.Digitos)
                {
                   selector. Predicate.RightMember = Convert.ToInt32(Current().Value);

                    NextToken();
                }
                else if (Current().Type == Token.TokenType.Comillas)
                {
                    ExpectedToken(Token.TokenType.Palabra);
                   selector.Predicate.RightMember = Current().Value;
                    ExpectedToken(Token.TokenType.Comillas);

                    NextToken();
                }
                else
                {
                    throw new Exception(
                        $"Error en la Linea {Current().Linea} Columna {Current().Columna}   Tipo de valor no permito para 'Predicate' se esperaba 'number' o 'string' pero se obtuvo {Current().Type}"
                    );
                }
            }
            else
            {
                throw new Exception(
                    $" No se reconoce el token {Current().Value} de tipo {Current().Type} en el cuerpo de Selector"
                );
            }
        }
        ExpectedToken(Token.TokenType.Coma);
        NextToken(); // el proximo token depues del crear el selector

        return selector;
    }
}
