using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Mathematics;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class Analizador_Lexico : MonoBehaviour
{
    public string[] script;
    private List<Token> tokens = new();

    // Crear diccionario para el uso de los patrones

    public Dictionary<string, Token.TokenType> palabras_reservadas =
        new()
        {
            //keywords
            { "if", Token.TokenType.If },
            { "else", Token.TokenType.ELse },
            { "in", Token.TokenType.In },
            { "card", Token.TokenType.Card },
            { "effect", Token.TokenType.Effect },
            { "Effect", Token.TokenType.Effect_activacion },
            { "Number", Token.TokenType.Number },
            { "Variable", Token.TokenType.Variable },
            { "Amount", Token.TokenType.Amount },
            { "Type", Token.TokenType.Type },
            { "Params", Token.TokenType.Params },
            { "Range", Token.TokenType.Range },
            { "String", Token.TokenType.String },
            { "Action", Token.TokenType.Action },
            { "Name", Token.TokenType.Name },
            { "Faction", Token.TokenType.Faction },
            { "Power", Token.TokenType.Power },
            { "OnActivacion", Token.TokenType.OnActivacion },
            { "Selector", Token.TokenType.Selector },
            { "Source", Token.TokenType.Source },
            { "Single", Token.TokenType.Single },
            { "Predicate", Token.TokenType.Predicate },
            { "PostAction", Token.TokenType.PostAction },
            { "targets", Token.TokenType.Targets },
            { "context", Token.TokenType.Context },
            { "TriggerPlayer", Token.TokenType.TriggerPlayer },
            { "Board", Token.TokenType.Board },
            { "HandOfPlayer", Token.TokenType.HandOfPlayer },
            { "hand", Token.TokenType.hand },
            { "FieldOfPlayer", Token.TokenType.FieldOfPlayer },
            { "field", Token.TokenType.field },
            { "GraveyardOfPlayer", Token.TokenType.GraveyardOfPlayer },
            { "graveyard", Token.TokenType.graveyard },
            { "DeckOfPlayer", Token.TokenType.DeckOfPlayer },
            { "deck", Token.TokenType.Deck },
            { "Owner", Token.TokenType.Owner },
            { "Find", Token.TokenType.Find },
            { "find", Token.TokenType.find },
            { "Push", Token.TokenType.Push },
            { "SendBottom", Token.TokenType.SendBottom },
            { "Pop", Token.TokenType.Pop },
            { "Remove", Token.TokenType.Remove },
            { "Shuffle", Token.TokenType.Shuffle },
            // vamos hacer uso de la Lenguaje Regex , en este lenguaje alguno de los caracteres q se pasan pertenecen a ese lenguaje
            // por ende se usara esta estructura \\ + caracter problematico para q sepa que no nos estamos refiriendo al caracter del lenguaje

            //Simbologia
            { "=>", Token.TokenType.Flecha },
            { ">=", Token.TokenType.Relacional },
            { "<=", Token.TokenType.Relacional},
            { ">", Token.TokenType.Relacional },
            { "<", Token.TokenType.Relacional },
            { "==", Token.TokenType.Relacional },
            { "-=", Token.TokenType.Asignacion },
            { "\\+=", Token.TokenType.Asignacion},
            
            { Regex.Escape("++"), Token.TokenType.PlusPlus },
            { "=", Token.TokenType.Asignacion },
            { Regex.Escape("["), Token.TokenType.Left_Corchete },
            { Regex.Escape("]"), Token.TokenType.Right_Corchete },
            { "\\(", Token.TokenType.Left_Paran },
            { "\\)", Token.TokenType.Right_Paran },
            { "{", Token.TokenType.Left_Key },
            { "}", Token.TokenType.Right_Key },
            { Regex.Escape(","), Token.TokenType.Coma },
            { "\\.", Token.TokenType.Punto },
            { @"\:", Token.TokenType.Dos_Puntos },
            { ";", Token.TokenType.Punto_Coma },
            { "\"", Token.TokenType.Comillas },
            //operadores aritmeticos
            { "\\+", Token.TokenType.Suma },
            { "-", Token.TokenType.Resta },
            { "\\*", Token.TokenType.Multiplicacion },
            { "/", Token.TokenType.Divicion },
            { "\\^", Token.TokenType.Potencia },
            //concatenacion de cadenas
            { "@@", Token.TokenType.Cadenas_multiples },
            { "@", Token.TokenType.Cadenas_simples },
            //bucles
            { "while", Token.TokenType.While },
            { "for", Token.TokenType.For },
            //booleano
            { "true", Token.TokenType.Booleano},
            { "false", Token.TokenType.Booleano},
            //operadores logicos
            { "&&", Token.TokenType.And },
            { "\\|\\|", Token.TokenType.Or },
         
            {"^[A-Za-z]+$",Token.TokenType.Palabra},
            { "[a-zA-Z_]\\w*", Token.TokenType.Desconocido }, // se usas par encontrar todas las palabras q enpiece por mayuscula minuscala contenga numeros o guion bajo
            { "\\d+", Token.TokenType.Digitos } // \\+d en el lengujae regex se usas para encontrar hasta  al menos un digito o mas
        };

    public void Convertirdor_de_array()
    {
        script = GameObject.Find("texto_codigo").GetComponent<TMP_InputField>().text.Split("\n");
        Desglozador(script);

        for (int i = 0; i < tokens.Count; i++)
        {
            Debug.Log(tokens[i].Value);
            Debug.Log(tokens[i].Type);
        }
    }

    //Metodos para localizar la poscion de la palabras

    //linea se empieza aa contar desde 1
    public int Linea(string codigo, int index)
    {
        int linea = 1;

        for (int i = 0; i <= index; i++)
        {
            if (codigo[i] == '\n')
            {
                linea++;
            }
        }
        return linea;
    }

    //columnas se empiezan a contar a partir de 0
    public int Columnna(string codigo, int index)
    {
        int columna = 1;
        for (int i = index; i >= 0; i--)
        {
            if (codigo[index] == '\n')
            {
                columna = index - i;
                break;
            }
        }
        return columna;
    }

    //  se desgloza todo el string en palabras reservadas
    public void Desglozador(string[] code_lines)
    {
        string input = string.Join('\n', code_lines);
        /// uniendo las lineas de codigo en una sola cadena
        string pattern = $"{string.Join('|', palabras_reservadas.Keys)}"; // une los llaves de plabras reservadas , separadas por un or para saber el patron a seguir para buscar matches

        MatchCollection matches = Regex.Matches(input, pattern);
        // creara grupos alternartivos , q se distruibuiran segun el orden de las keys del diccionario


        foreach (Match match in matches)
        { //palabras reservadas
            if (palabras_reservadas.TryGetValue(match.Value, out var tokenType))
            {
                tokens.Add(
                    new Token(
                        tokenType,
                        match.Value,
                        Linea(input, match.Index),
                        Columnna(input, match.Index)
                    )
                );
            }
            //numeros
            else if (match.Value.All(char.IsDigit) && Regex.IsMatch(match.Value, "\\d+"))
            {
                tokens.Add(
                    new Token(
                        Token.TokenType.Digitos,
                        match.Value,
                        Linea(input, match.Index),
                        Columnna(input, match.Index)
                    )
                );
            }
            // simbolos
            else
            {
                foreach (string signo in palabras_reservadas.Keys)
                {
                    if (Regex.IsMatch(match.Value, signo))
                    {
                        tokens.Add(
                            new Token(
                                palabras_reservadas[signo],
                                match.Value,
                                Linea(input, match.Index),
                                Columnna(input, match.Index)
                            )
                        );
                        break;
                    }
                }
            }
        }
    }
}
