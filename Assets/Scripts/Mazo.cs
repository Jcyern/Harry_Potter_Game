using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Mazo : MonoBehaviour
{
    public GameObject[] barritas = new GameObject[26];
    public GameObject[] minibarritas = new GameObject[26];
    public List<Carta> mazo = new List<Carta>();
    public List<Carta> mano = new List<Carta>();
    
     public  Carta cari;
    public List<GameObject> mano_interfaz = new List<GameObject>();
    
     public  int robos = 0;
    public void Asociar_Mazo()
    {
        Ordenar_mazo();
       int pos = 0;
        List<int> mazo_interface = new List<int>();
      
        
        for (int k = 0; k < barritas.Length; k++)
        {
            barritas[k].SetActive(false);
          
        }

        for (int i = 0; i < mazo.Count; i++)
        {  minibarritas[i].SetActive(false);
           

            int existe = -1;
            int cant = 1;
            for (int j = 0; j < i; j++)
            {
                if (mazo[i].Nombre == mazo[j].Nombre)
                {
                    if (existe == -1)
                    {
                        existe = j;
                        // Debug.Log("Entrando a existe");
                    }
                    cant += 1;
                }
            }
            if (existe == -1)
            {
                barritas[pos].SetActive(true);
                barritas[pos].GetComponent<Carta>().Nombre = mazo[i].Nombre;
                barritas[pos].GetComponent<Carta>().id = mazo[i].id;
                barritas[pos].GetComponent<Carta>().Efecto = mazo[i].Efecto;
                GameObject.Find("ctext_" + pos).GetComponent<TextMeshProUGUI>().text = mazo[i].Nombre;
                mazo_interface.Add(mazo[i].id);
                pos += 1;
            }
            else if (existe != -1)
            {
                for (int j = 0; j < mazo_interface.Count; j++)
                {
                    if (mazo_interface[j] == mazo[i].id)
                    {
                        existe = j;
                    }
                }
                minibarritas[existe].SetActive(true);
                // Debug.Log(existe);
                GameObject.Find("textimg_" + existe).GetComponent<TextMeshProUGUI>().text =
                    cant.ToString();
            }
        }
    }

    public void Ordenar_mazo()
    {
        List<Carta> heroes = new List<Carta>();
        for (int i = 0; i < mazo.Count; i++)
        {
            // Debug.Log(mazo[i].Nombre);
            if (mazo[i].Tipo == "1")
            {
                heroes.Add(mazo[i]);
                mazo.RemoveAt(i);
                i -= 1;
            }
        }
        //reccorriendo la lista de heroes
        for (int j = 0; j < heroes.Count; j++)
        {
            mazo.Insert(j, heroes[j]);
        }
    }

    public void Barajear_cartas()
    {
        List<Carta> cartas_desordenar = new List<Carta>();
        int i = 0;

        while (mazo.Count > 0)
        {   int a = UnityEngine.Random.Range(0, mazo.Count);
            Carta c2 = mazo[a];
            cartas_desordenar.Add(c2);
            i++;
            
            mazo.RemoveAt(a);

        }
         int c = -1;
         while (cartas_desordenar.Count > 0)
        {   
           
            c++;
            mazo.Add(cartas_desordenar[0]);
          // Debug.Log("Carta numero "+c+" del  desordenado  es "+cartas_desordenar[0].Nombre);
            cartas_desordenar.RemoveAt(0);
            
          // Debug.Log("carta numero "+c+" del mazo es "+mazo[c].Nombre);
            

        }
    
    }

    public void Robar_del_mazo(int cant)
    {
        if(robos== 0 )
        { 
        
            
            while (cant > 0 && mazo.Count > 0 && mano.Count < 10 && GameObject.Find("cambio_de_turno").GetComponent<Cambio_Turno>().turno==true )
            { 
              
              //  Debug.Log(cant);
                mano.Add(mazo[0]);
                mano_interfaz[mano.Count - 1].SetActive(true );
                mano_interfaz[mano.Count - 1].GetComponent<Carta>().Nombre = mano[  mano.Count - 1].Nombre;
                mano_interfaz[mano.Count - 1].GetComponent<Carta>().Efecto = mano[ mano.Count - 1    ].Efecto;
                mano_interfaz[mano.Count - 1].GetComponent<Carta>().Tipo = mano[ mano.Count - 1  ].Tipo;
                mano_interfaz[mano.Count - 1].GetComponent<Carta>().Faction = mano[ mano.Count - 1    ].Faction;
                mano_interfaz[mano.Count - 1].GetComponent<Carta>().id = mano[mano.Count - 1].id;
                mano_interfaz[mano.Count - 1].GetComponent<Carta>().Load_interface();
                mazo.RemoveAt(0);
                
              


                cant--;
            }
           
             

            GameObject.Find("mazo_contrario").GetComponent<Mazo_oponente>().Anadir_Script_carta_unidad(mano_interfaz);
            //asignarle el boton  correspondiente para que ejecute el Metodo Seleccion 
           
          
         robos++;
        }
    }
     
     
   



}
