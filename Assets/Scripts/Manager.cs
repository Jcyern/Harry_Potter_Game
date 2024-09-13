using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public string Lider;
    public int casa_player;
    public GameObject Seleccion_de_fac;
    public GameObject Menu_de_construir_mazo;
    public GameObject Seleccion_lideres;

     public GameObject Tablero;
   public   Carta carta_ ; //contiene solo el script carta 
    public Sprite[] escudos = new Sprite[4];
    public Sprite escudo;

    public GameObject izq;
    public GameObject derecha;

    public GameObject button_star;

    public int pagina = 0;

    public GameObject[] array = new GameObject[10];

    private List<Carta> cartas;

    public void Seleccion_fac(int casa)
    {
        casa_player = casa;

        Seleccion_de_fac.SetActive(false);
        Seleccion_lideres.SetActive(true);

        Lider[] lideres = this.GetComponent<SQLiteDB>().Obtener_lideres(casa_player);
        for (int i = 0; i < lideres.Length; i++)
        {
            GameObject.Find("name_" + (i + 1)).GetComponent<TextMeshProUGUI>().text = lideres[
                i
            ].Nombre;
            GameObject.Find("effect_" + (i + 1)).GetComponent<TextMeshProUGUI>().text = lideres[
                i
            ].Efecto;
            GameObject.Find("img_" + (i + 1)).GetComponent<Image>().sprite = Resources.Load<Sprite>(
                "img/Diseno_de_cartas/cartas/" + casa_player + ""
            );
            string sprite = "img/Diseno_de_cartas/lideres/" + lideres[i].Nombre.Replace(' ', '_');
            GameObject.Find("back_" + (i + 1)).GetComponent<Image>().sprite =
                Resources.Load<Sprite>(sprite);
            Debug.Log(sprite);
        }
    }

    public string Casa_string(int home)
    {
        if (home == 1)
            return "Gryffindor";
        else if (home == 2)
            return "Slytherin";
        else if (home == 3)
            return "Hufflepuff";
        else if (home == 4)
            return "Ravenclaw";
        else
            return "Neutral";
    }

    public void Seleccion_lider(int lider)
    {
        Lider[] lideres = this.GetComponent<SQLiteDB>().Obtener_lideres(casa_player);
         
        //Cargando las cosas del jugador cpu
         GameObject.Find("Canvas").GetComponent<Jugador_2>().Escogiendo_casa_and_lider();
        
        
        Seleccion_lideres.SetActive(false);
        Menu_de_construir_mazo.SetActive(true);

        GameObject.Find("Title_mazo").GetComponent<TextMeshProUGUI>().text =
            "Creando mazo de " + lideres[lider - 1].Nombre;
        GameObject.Find("escudo_casa").GetComponent<Image>().sprite = escudos[casa_player - 1];
    
        Lider= lideres[lider - 1].Nombre;
        cartas = this.GetComponent<SQLiteDB>().Obtener_cartas(casa_player);

        Asociar_card();
    }

    public void Asociar_card()
    {
        for (int i = 0; i < 10; i++)
        {
            if (i + (pagina * 10) < cartas.Count)
            { //
                array[i].SetActive(true);
                array[i].GetComponent<Carta>().id = cartas[i + (pagina * 10)].id;
                array[i].GetComponent<Carta>().Nombre = cartas[i + (pagina * 10)].Nombre;
                array[i].GetComponent<Carta>().Tipo = cartas[i + (pagina * 10)].Tipo;
                array[i].GetComponent<Carta>().Faction = cartas[i + (pagina * 10)].Faction;
                array[i].GetComponent<Carta>().Efecto = cartas[i + pagina * 10].Efecto;
               
               array[i].GetComponent<Carta>().Load_interface();
               Debug.Log( cartas[i+ ( pagina * 10 )].Nombre.Replace(' ', '_'));
               
               
               
                // GameObject.Find("name_" + (i + 1)).GetComponent<TextMeshProUGUI>().text = cartas[
                //     i + (pagina * 10)
                // ].Nombre;
                // GameObject.Find("effect_" + (i + 1)).GetComponent<TextMeshProUGUI>().text = cartas[
                //     i + (pagina * 10)
                // ].Efecto;
                // GameObject.Find("img_" + (i + 1)).GetComponent<Image>().sprite =
                //     Resources.Load<Sprite>(
                //         "img/Diseno_de_cartas/cartas/" + cartas[i + (pagina * 10)].Faction + ""
                //     );
                // string sprite = "img/Diseno_de_cartas/lideres/" + cartas[i + (pagina * 10)].Nombre.Replace(' ', '_');
                // GameObject.Find("back_" + (i + 1)).GetComponent<Image>().sprite =
                //     Resources.Load<Sprite>(sprite);
                // Debug.Log(sprite);


            }
            else
            {
                array[i].SetActive(false);
            }


        }
    }

    public void Next_Pag()
    {
        pagina += 1;

        Asociar_card();
        if (((pagina + 1) * 10) > cartas.Count)
            derecha.SetActive(false);
        else
            derecha.SetActive(true);
        if (pagina == 0)
            izq.SetActive(false);
        else
            izq.SetActive(true);
    }

    public void Back_Pag()
    {
        pagina -= 1;
        Asociar_card();
        if (pagina == 0)
            izq.SetActive(false);
        else
            izq.SetActive(true);
        if (((pagina + 1) * 10) > cartas.Count)
            derecha.SetActive(false);
        else
            derecha.SetActive(true);
    }

    public void Scene_Tablero()
    {
        if (Menu_de_construir_mazo.GetComponent<Mazo>().mazo.Count == 25)
        {
            
            int count= Menu_de_construir_mazo.GetComponent<Mazo>().mazo.Count;
            Tablero.SetActive(true);

            Tablero.GetComponent<Asociacion_imagen>().Asociar();

            GameObject.Find("mazo_contrario").GetComponent<Mazo_oponente>().Asociar_el_mazo();
            
            //Debug.Log();
            for (int i = 0; i < count; i++)
            { 
               GameObject.Find("mazo").GetComponent<Mazo>().mazo.Add(Menu_de_construir_mazo.GetComponent<Mazo>().mazo[i]);
            }

            Menu_de_construir_mazo.SetActive(false);
        }
         GameObject.Find("mazo").GetComponent<Mazo>().Barajear_cartas();

    
    }
}
