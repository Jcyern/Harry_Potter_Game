using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using Mono.Data.Sqlite;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Mazo_oponente : MonoBehaviour
{
    public Juego juego = new();
    public List<CardDisplay> mazo_oponente;
    public List<CardDisplay> mano_oponente;
    public List<GameObject> mano_interfaz_2 = new List<GameObject>();
    public List<GameObject> Cartas_escogidass;

    public List<Carta> cartas_no_jugadas;

    //asociar las cartas que estan en la lista de Jugador 2 a este script

    public void Anadir_Script_carta_unidad(List<GameObject> manos)
    {
        for (int i = 0; i < manos.Count; i++)
        {
            if (
                manos[i].GetComponent<Carta>().Tipo == "2"
                || manos[i].GetComponent<Carta>().Tipo == "3"
                || manos[i].GetComponent<Carta>().Tipo == "4"
            )
            {
                manos[i].AddComponent<Carta_Unidad>();
                // manos[i].GetComponent<Carta_Unidad>().Nombre = manos[i].GetComponent<Carta>().Nombre;
                // manos[i].GetComponent<Carta_Unidad>().Tipo = manos[i].GetComponent<Carta>().Tipo;
                // manos[i].GetComponent<Carta_Unidad>().Faction = manos[i].GetComponent<Carta>().Faction;
                // manos[i].GetComponent<Carta_Unidad>().Efecto= manos[i].GetComponent<Carta>().Efecto;
                // manos[i].GetComponent<Carta_Unidad>().id = manos[i].GetComponent<Carta>().id;
                // manos[i].GetComponent<Carta_Unidad>().marco =manos[i].GetComponent<Carta>().marco;
                // manos[i].GetComponent<Carta_Unidad>().imagen_carta =manos[i].GetComponent<Carta>().imagen_carta;
                // manos[i].GetComponent<Carta_Unidad>().nombre =manos[i].GetComponent<Carta>().nombre;
                // manos[i].GetComponent<Carta_Unidad>().efecto =manos[i].GetComponent<Carta>().efecto;



                //Abriendo conexion con la base de datos para pasar los valores de  salud y ataque


                Conectar_base_ataque_salud(manos[i], manos[i].GetComponent<Carta>().id);
            }
        }
    }

    public void Conectar_base_ataque_salud(GameObject carta_unidad, int id)
    {
        string q = "SElECT*FROM cards where id= '" + id + "'";

        using (
            var connection = new SqliteConnection(
                GameObject.Find("Canvas").GetComponent<SQLiteDB>().dbName
            )
        )
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = q;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        carta_unidad.GetComponent<Carta_Unidad>().ataque = reader.GetInt32(
                            reader.GetOrdinal("attack")
                        );
                        carta_unidad.GetComponent<Carta_Unidad>().salud = reader.GetInt32(
                            reader.GetOrdinal("health")
                        );

                        // Debug.Log("nombre " + reader["name"] + " efecto: " + reader["effect"]);
                    }
                }
            }

            connection.Close();
        }
    }

  

    public void Asociar_el_mazo()
    {
        for (int i = 0; i < GameObject.Find("Canvas").GetComponent<Jugador_2>().mazo.Count; i++)
        {
            mazo_oponente.Add(GameObject.Find("Canvas").GetComponent<Jugador_2>().mazo[i]);
        }

        for (int i = 0; i < GameObject.Find("Canvas").GetComponent<Jugador_2>().mazo.Count; i++)
        {
            GameObject.Find("Canvas").GetComponent<Jugador_2>().mazo.RemoveAt(i);
        }
    }

    public void Robo_Aut(int cant)
    {
        while (cant > 0 && mazo_oponente.Count > 0 && mano_oponente.Count < 10)
        {
            Debug.Log("robando mazo" + cant);
            mano_oponente.Add(mazo_oponente[0]);
            mano_oponente[mano_oponente.Count-1].CartaLoad(mazo_oponente[0].carta);
            mano_interfaz_2[mano_oponente.Count - 1].SetActive(true);
            
            mano_interfaz_2[mano_oponente.Count - 1].GetComponent<CardDisplay>().CartaLoad( mano_oponente[
                mano_oponente.Count - 1
            ].carta);
         


            mazo_oponente.RemoveAt(0);

            cant--;
        }
        Debug.Log("Cartas robadas por el jugador 2");
        foreach ( var mano in mano_interfaz_2)
        {
            mano.GetComponent<CardDisplay>().CartaDebug();

        }
    }

    public void Jugar_carta()
    {  
        int numero_random = 5;

        if (numero_random > mano_oponente.Count)
        {
            numero_random = mano_oponente.Count;
        }

        Debug.Log("numero de cartas a escoger" + numero_random);

        for (int j = 1; j <= numero_random; j++)
        {
            
           GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada_2=mano_interfaz_2[j];
           GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada_2.GetComponent<CardDisplay>().CartaLoad(mano_interfaz_2[j].GetComponent<CardDisplay>().carta);


            Debug.Log("entro al for " + j);
            if (GameObject.Find("numero_ronda").GetComponent<TextMeshProUGUI>().text == "1")
            {
                 //solo se pueden jugar cartas de ataque  , si es la primera ronda
                if (
                    mano_interfaz_2[j].GetComponent<CardDisplay>().Tipo == "1"
                    || mano_interfaz_2[j].GetComponent<CardDisplay>().Tipo == "2"
                    || mano_interfaz_2[j].GetComponent<CardDisplay>().Tipo == "3"
                    || mano_interfaz_2[j].GetComponent<CardDisplay>().Tipo == "4"
                )
                {
                    {
                        
                         //asociando los tipos
                        AsociarGameObject(
                            mano_interfaz_2[j],
                            GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada_2
                        );
                       
                        GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada_2.GetComponent<CardDisplay>().CartaLoad( mano_interfaz_2[j].GetComponent<CardDisplay>().carta);


                        //si no hay plazas disponibles se dara a la variable el valor de null

                        //verifica lel gameobjet carta seleccionada
                        GameObject.Find("Tablero").GetComponent<Juego>().Colocar_tablero_j2();

                        if (GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida != null)
                        {
                            //Eliminarla de la mano
                            mano_oponente.RemoveAt(j);
                        }
                        else
                        {
                            Debug.Log(" no hay plaza");
                        }

                        //limpinado el gameobject 
                     
                    }
                    
                }
                else
                {
                    Debug.Log ( " no play");
                }
            }
            else
            { 
                
                
                //asociando los tipos
                AsociarGameObject(
                    mano_interfaz_2[j],
                    GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada_2
                );

                //si no hay plazas disponibles se dara a la variable el valor de null

                //verifica lel gameobjet carta seleccionada
                GameObject.Find("Tablero").GetComponent<Juego>().Colocar_tablero_j2();

                if (GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida != null)
                {
                    //Eliminarla de la mano
                    mano_oponente.RemoveAt(j);
                }
                else
                {
                    Debug.Log(" no hay plaza");
                }

                 //limpinado el gameobject 
               
            }
        }
    }

   

    public void AsociarGameObject(GameObject a, GameObject b)
    {   
        b = a;
         if(a.GetComponent<CardDisplay>())
         {
            b.GetComponent<CardDisplay>().CartaLoad(a.GetComponent<CardDisplay>().carta);
            Debug.Log("Debugrando objeto b");
            b.GetComponent<CardDisplay>().CartaDebug();
            
         }

       
    }
}
