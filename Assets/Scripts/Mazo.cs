using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Mazo : MonoBehaviour
{
    public GameObject[] barritas = new GameObject[26];
    public GameObject[] minibarritas = new GameObject[26];
    public List<CardDisplay> mazo = new List<CardDisplay>();

    public List<CardDisplay> mano = new List<CardDisplay>();
    public List<Carta> manocard = new List<Carta>();
    public Carta cari;

    public List<GameObject> mano_interfaz = new List<GameObject>();

    //llamar siempre al mazo q se encuentra en el canvas , script manager
    public int robos = 0;

    public int position = 0;

    public void Asociar_Mazo()
    {
        Ordenar_mazo();

        int pos = 0;
        List<int> mazo_interface = new List<int>();

        //apagando las baritas
        for (int k = 0; k < barritas.Length; k++)
        {
            barritas[k].SetActive(false);
        }

        for (int i = 0; i < mazo.Count; i++)
        {
            minibarritas[i].SetActive(false);

            bool existe = false;
            int cant = 1;

            for (int j = 0; j < i; j++)
            { // j recorre las cartas q ya hay  en el mazo
                if (
                    GameObject.Find("Canvas").GetComponent<Mazo>().mazo[i].carta.Nombre
                    == GameObject.Find("Canvas").GetComponent<Mazo>().mazo[j].carta.Nombre
                )
                {
                    if (!existe)
                    {
                        //la carta ya existe en el mzo
                        existe = true;
                        // Debug.Log("Entrando a existe");
                    }
                    cant += 1;
                }
            }
            if (existe == false)
            {
                barritas[pos].SetActive(true);
                barritas[pos].GetComponent<Carta>().Nombre = GameObject
                    .Find("Canvas")
                    .GetComponent<Mazo>()
                    .mazo[i]
                    .carta.Nombre;
                barritas[pos].GetComponent<Carta>().id = GameObject
                    .Find("Canvas")
                    .GetComponent<Mazo>()
                    .mazo[i]
                    .carta.id;
                barritas[pos].GetComponent<Carta>().Efecto = GameObject
                    .Find("Canvas")
                    .GetComponent<Mazo>()
                    .mazo[i]
                    .carta.Efecto;
                GameObject.Find("ctext_" + pos).GetComponent<TextMeshProUGUI>().text = GameObject
                    .Find("Canvas")
                    .GetComponent<Mazo>()
                    .mazo[i]
                    .carta.Nombre;
                //anadiendo los id de las carta colocadaas en las barras
                mazo_interface.Add(GameObject.Find("Canvas").GetComponent<Mazo>().mazo[i].carta.id);
                pos += 1;
            }
            else if (existe == true)
            {
                for (int j = 0; j < mazo_interface.Count; j++)
                {
                    if (
                        mazo_interface[j]
                        == GameObject.Find("Canvas").GetComponent<Mazo>().mazo[i].carta.id
                    )
                    {
                        position = j;
                    }
                }
                minibarritas[position].SetActive(true);
                // Debug.Log(existe);
                GameObject.Find("textimg_" + position).GetComponent<TextMeshProUGUI>().text =
                    cant.ToString();
            }
        }
    }

    public void Ordenar_mazo()
    {
        List<CardDisplay> heroes = new List<CardDisplay>();
        for (int i = 0; i < mazo.Count; i++)
        {
            // Debug.Log(mazo[i].Nombre);
            if (GameObject.Find("Canvas").GetComponent<Mazo>().mazo[i].carta.Tipo == "1")
            {
                heroes.Add(mazo[i]);
                mazo.RemoveAt(i);
                i -= 1;
            }
        }
        //reccorriendo la lista de heroes
        for (int j = 0; j < heroes.Count; j++)
        {
            GameObject.Find("Canvas").GetComponent<Mazo>().mazo.Insert(j, heroes[j]);
        }
    }

    public void Barajear_cartas()
    {
        List<CardDisplay> cartas_desordenar = new List<CardDisplay>();

        while (mazo.Count > 0)
        {
            int a = UnityEngine.Random.Range(0, mazo.Count);
            CardDisplay c2 = mazo[a];
            cartas_desordenar.Add(c2);

            mazo.RemoveAt(a);
        }

        while (cartas_desordenar.Count > 0)
        {
            mazo.Add(cartas_desordenar[0]);

            // Debug.Log("Carta numero "+c+" del  desordenado  es "+cartas_desordenar[0].Nombre);
            cartas_desordenar.RemoveAt(0);

            // Debug.Log("carta numero "+c+" del mazo es "+mazo[c].Nombre);
        }
    }

    // public void RobarCard(int cant)
    // {
    //     if (robos == 0)
    //     {
    //         while (
    //             cant > 0
    //             &&GameObject.Find("mazo_card").GetComponent<mazo_cards>().CreatedCards .Count > 0
    //             && mano.Count < 10
    //             && GameObject.Find("cambio_de_turno").GetComponent<Cambio_Turno>().turno == true
    //         )
    //         {
    //             //  Debug.Log(cant);
    //             mano.Add(GameObject.Find("mazo_card").GetComponent<mazo_cards>().CreatedCards[0].GetComponent<Carta>());
    //             mano_interfaz[mano.Count - 1].SetActive(true);
    //             mano_interfaz[mano.Count - 1].GetComponent<Carta>().Nombre = mano[
    //                 mano.Count - 1
    //             ].Nombre;
    //             mano_interfaz[mano.Count - 1].GetComponent<Carta>().Efecto = mano[
    //                 mano.Count - 1
    //             ].Efecto;
    //             mano_interfaz[mano.Count - 1].GetComponent<Carta>().Tipo = mano[
    //                 mano.Count - 1
    //             ].Tipo;
    //             mano_interfaz[mano.Count - 1].GetComponent<Carta>().Faction = mano[
    //                 mano.Count - 1
    //             ].Faction;
    //             mano_interfaz[mano.Count - 1].GetComponent<Carta>().id = mano[mano.Count - 1].id;
    //             mano_interfaz[mano.Count - 1].GetComponent<Carta>().Load_interface();
    //              mano_interfaz[mano.Count - 1].GetComponent<Carta>().IsCard = true ;


    //             mano_interfaz[mano.Count - 1].AddComponent<Carta_Unidad>();
    //             mano_interfaz[mano.Count - 1].GetComponent<Carta_Unidad>().salud = GameObject.Find("mazo_card").GetComponent<mazo_cards>().CreatedCards[0].GetComponent<Carta_Unidad>().salud;
    //             mano_interfaz[mano.Count - 1].GetComponent<Carta_Unidad>().ataque =GameObject.Find("mazo_card").GetComponent<mazo_cards>().CreatedCards[0].GetComponent<Carta_Unidad>().ataque;


    //             GameObject.Find("mazo_card").GetComponent<mazo_cards>().CreatedCards.RemoveAt(0);


    //             //agregar el Boton
    //              mano_interfaz[mano.Count - 1].AddComponent<Button>();

    //              //agregando el metodo para q pueda moverse , ser jugado
    //               mano_interfaz[mano.Count - 1].GetComponent<Button>().onClick.AddListener(mano_interfaz[mano.Count - 1].GetComponent<Carta>().Seleccion);



    //             cant--;
    //         }
    //     }
    // }





    public void Robar_del_mazo(int cant)
    {
        if (robos == 0)
        {
            while (
                cant > 0
                && mazo.Count > 0
                && mano.Count < 10
                && GameObject.Find("cambio_de_turno").GetComponent<Cambio_Turno>().turno == true
            )
            {
                Debug.Log("escribiendo");

                mazo[0].CartaDebug();

                //  Debug.Log(cant);
                mano.Add(mazo[0]);


              
              
                mano_interfaz[mano.Count - 1].SetActive(true);

            

                 mano_interfaz[mano.Count - 1].GetComponent<CardDisplay>().CartaLoad(mazo[0].carta);

               mano_interfaz[mano.Count - 1].GetComponent<CardDisplay>().Load_interface();
              
              
              mazo.RemoveAt(0);

                cant--;
            }

            robos++;
        }
    }
}
