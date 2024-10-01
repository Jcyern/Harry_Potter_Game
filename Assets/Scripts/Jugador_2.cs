using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Jugador_2 : MonoBehaviour
{     int tipo_hechizo_pociones=-1;
        int tipo_heroes=-1;
    int random_number;
   public  string rnd_Lider;
    string rnd_casa;
    public List<CardDisplay> mazo ;
  
    public void Escogiendo_casa_and_lider()
    {
        random_number = Random.Range(1, 4);
        Debug.Log("el generado es " + random_number);
        rnd_casa = GameObject.Find("Canvas").GetComponent<Manager>().Casa_string(random_number);

        Lider[] rnd_lideres = GameObject
            .Find("Canvas")
            .GetComponent<SQLiteDB>()
            .Obtener_lideres(random_number);
        int numero_liderato = Random.Range(0, 2);
        rnd_Lider = rnd_lideres[numero_liderato].Nombre;

        Debug.Log("+casa : " + rnd_casa + "LIder escogido " + rnd_Lider);

        Crear_Mazo();
    }

    public void Crear_Mazo()
    {
       
        List<Carta> total_cartas = GameObject .Find("Canvas").GetComponent<SQLiteDB>().Obtener_cartas(random_number);
        
        Debug.Log("total card cant:" + total_cartas.Count);
        for (int i = 0; i < total_cartas.Count; i++)
        { 
           
            
           
           
            int pos = 0;
            int random;
            bool carta_seleccionada = false;
            random = Random.Range(0, total_cartas.Count);
            Debug.Log("random:" + random);
            Debug.Log("mazo cant:" + mazo.Count);
            if (mazo.Count < 25)
            {



                if (mazo.Count > 0)
                {
                    for (int j = 0; j < mazo.Count; j++)
                    {
                        if (mazo[j].Name == total_cartas[random].Nombre)
                        {
                            pos++;
                            carta_seleccionada = true;
                        }


                    }
                }
                if (!carta_seleccionada || pos <= 3 && total_cartas[random].Tipo != "1")
                 {  
                   if(total_cartas[random].Tipo  == "5"||total_cartas[random].Tipo== "6" || total_cartas[random].Tipo== "7")
                   {
                     tipo_hechizo_pociones++;
                   }
                    
                   if(tipo_hechizo_pociones<=10)
                   {
                     mazo.Add(new CardDisplay(total_cartas[random]));
                   }
                 }
               
                if (total_cartas[random].Tipo == "1" && pos == 0 &&tipo_heroes<4)
                {  
                    tipo_heroes++;
                  
                   
                   mazo .Add(new CardDisplay(total_cartas[random]));
                }
            }

            if(mazo.Count==25)
            break;
        }
        Debug.Log("escribinedo cartas del Jugador 2 ");
        for (int i = 0; i < mazo.Count; i++)
        {
            
            Debug.Log($"Carta #{i} +  + {mazo[i].Name}  Tipo {mazo[i].Tipo}  Faction {mazo[i].Faction}");
            
        }
    }
}
