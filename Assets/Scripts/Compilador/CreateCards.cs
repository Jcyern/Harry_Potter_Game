using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateCards : MonoBehaviour
{
    public GameObject Panel_de_errores;
    public GameObject TextCodigo;
    public GameObject MazoCard;
    List<Token> tokens;
    List<AstNode> nodos;
    Parser parseo;
    List<Card> CreatedCards;
    Context generalcontext;
  

    public void Verificador() // funcion asociada al metodo verificador
    {
        string text = TextCodigo.GetComponent<TMP_InputField>().text;
        string[] script = text.Split('\n');
        //creando los tokens
        tokens = LexerFuntion(script);
        Debug.Log(" Creados los tokens ");

        // parsear los tokens
        nodos = ParseFuntion(tokens);
        Debug.Log(" se crearon los nodos");


     
        // generar el codigo

        Code_GeneratorFuntion(parseo.context, nodos);
        Debug.Log(
            "Se genero la clase EffectCreted  , con sus respectivos metodos que son las cartas pasadas"
        );

        //Activar el mazo en el tablero
        GameObject.Find("Canvas").GetComponent<Manager>().Tablero.SetActive(true);
        MazoCard.SetActive(true);

        GameObject.Find("mazo_card").GetComponent<mazo_cards>().cards = CreatedCards;
        // convierte las cartas a GameObjets para poder usarlas en el juego
        GameObject.Find("mazo_card").GetComponent<mazo_cards>().CreatedCards = CreatedCards[0]
            .CreateGameObjectList(CreatedCards);

        //cambio de escenas
        GameObject.Find("Canvas").GetComponent<Manager>().SceneCodificar.SetActive(false);
    }

  

    public List<Token> LexerFuntion(string[] lineas) // analiza el texto y devuelve una list de tokens
    {
        Analizador_Lexico AnaliceLex = new Analizador_Lexico(lineas);
        AnaliceLex.Desglozador(AnaliceLex.script);

        return AnaliceLex.tokens; //asociando las listas
    }

    public List<AstNode> ParseFuntion(List<Token> words)
    {
        parseo = new Parser(words, Panel_de_errores.GetComponent<TextMeshProUGUI>());
        return parseo.Parse();
    }

    public void Code_GeneratorFuntion(Context contexto, List<AstNode> nodos)
    {
        Code_Generator generador = new Code_Generator(nodos, contexto);
        string outpath = "Assets/Scripts/Compilador/EffectCreated.cs";

        generador.GenerarCodigo(outpath);
        generalcontext = generador.context; //actualizando los contextos de las variables actualizadas y los efectos
        CreatedCards = generador.cards; //asociando las cartas creadas
    }
}
