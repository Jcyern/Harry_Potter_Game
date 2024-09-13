using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using Mono.Data.Sqlite;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Mazo_oponente : MonoBehaviour
{
    public List<Carta> mazo_oponente;
    public List<Carta> mano_oponente;
    public List<GameObject> mano_interfaz_2 = new List<GameObject>();
    public List<GameObject> Cartas_escogidass;
    
    
    public List<Carta> cartas_no_jugadas;

    //asociar las cartas que estan en la lista de Jugador 2 a este script

    public  void Anadir_Script_carta_unidad(List <GameObject>  manos)
       
    {
         for ( int i = 0; i<manos.Count; i++)
         {
            
            
                if(manos[i].GetComponent<Carta>().Tipo == "2"|| manos[i].GetComponent<Carta>().Tipo == "3" || manos[i].GetComponent<Carta>().Tipo == "4" )
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

  
                  Conectar_base_ataque_salud(manos[i],manos[i].GetComponent<Carta>().id );
                }
         }
   
        
        
    }
    
    public void Conectar_base_ataque_salud( GameObject carta_unidad, int id )
    {
                  string q = "SElECT*FROM cards where id= '"+ id  + "'";
   
                   using (var connection = new SqliteConnection(GameObject.Find("Canvas").GetComponent<SQLiteDB>().dbName))
                   {
                     connection.Open();

                     using (var command = connection.CreateCommand())
                    {
                     command.CommandText = q;
                     using (IDataReader reader = command.ExecuteReader())
                     {
                      while (reader.Read() )
                      {
                        
                       carta_unidad .GetComponent<Carta_Unidad>().ataque =  reader.GetInt32(reader.GetOrdinal("attack"));
                       carta_unidad .GetComponent<Carta_Unidad>().salud =  reader.GetInt32(reader.GetOrdinal("health"));

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
            mano_interfaz_2[mano_oponente.Count - 1].SetActive(true);
            mano_interfaz_2[mano_oponente.Count - 1].GetComponent<Carta>().Nombre = mano_oponente[
                mano_oponente.Count - 1
            ].Nombre;
            mano_interfaz_2[mano_oponente.Count - 1].GetComponent<Carta>().Efecto = mano_oponente[
                mano_oponente.Count - 1
            ].Efecto;
            mano_interfaz_2[mano_oponente.Count - 1].GetComponent<Carta>().Tipo = mano_oponente[
                mano_oponente.Count - 1
            ].Tipo;
            mano_interfaz_2[mano_oponente.Count - 1].GetComponent<Carta>().Faction = mano_oponente[
                mano_oponente.Count - 1
            ].Faction;
            mano_interfaz_2[mano_oponente.Count - 1].GetComponent<Carta>().id = mano_oponente[
                mano_oponente.Count - 1
            ].id;
            mazo_oponente.RemoveAt(0);

            cant--;
        }
    }

    public void Jugar_carta()
    {
        int numero_random = 5;
        // do
        //  {
        //     numero_random = Random.Range(5,10);
        //     Debug.Log(numero_random);
        //  }while(mano_oponente.Count <=numero_random);

        if (numero_random > mano_oponente.Count)
        {
            numero_random = mano_oponente.Count;
        }
        Debug.Log("numero de cartas a escoger" + numero_random);

        for (int j = 0; j < numero_random; j++)
        {
            Debug.Log("entro al for " + j);
           
            GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada = mano_interfaz_2[ j ];
            //  if(mano_oponente.Count>numero_random)
          
            // se llama para ver si la carta se puede poner, de ser asi ,aociara la posicion a plaza escogida
            GameObject.Find("Tablero").GetComponent<Juego>().Colocar_tablero_j2( GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada);

            //si no hay plazas disponibles se dara a la variable el valor de null
            
           if(GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida != null && GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Tipo== "2" ||
             GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Tipo== "3" || GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Tipo== "4") 
            {
              GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida.SetActive(true);
              Poner_carta(GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida);
              Conectar_base_ataque_salud(GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida,  GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().id);
            }


            if (GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida != null)
            { mano_oponente.RemoveAt(j);
                Debug.Log("true");
                GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida.SetActive(true);

                Poner_carta(GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida);
               // Debug.Log("la plaza escogida es "+ GameObject.Find("Tablero").GetComponent<Juego>().plaza_escogida.GetComponent<Carta>().Nombre);
            }
        }
    }

    public void Poner_carta(GameObject plaza_disponible)
    {
        GameObject carta_a_asociar = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .carta_seleccionada;

        plaza_disponible.GetComponent<Carta>().Nombre = carta_a_asociar
            .GetComponent<Carta>() .Nombre;
        plaza_disponible.GetComponent<Carta>().Tipo = carta_a_asociar.GetComponent<Carta>().Tipo;
        plaza_disponible.GetComponent<Carta>().Efecto = carta_a_asociar
            .GetComponent<Carta>()
            .Efecto;
        plaza_disponible.GetComponent<Carta>().Nombre = carta_a_asociar
            .GetComponent<Carta>()
            .Nombre;
        plaza_disponible.GetComponent<Carta>().id = carta_a_asociar.GetComponent<Carta>().id;
        plaza_disponible.GetComponent<Carta>().Faction = carta_a_asociar
            .GetComponent<Carta>()
            .Faction;
        plaza_disponible.GetComponent<Carta>().Load_interface();
    }

   
    
    
}
