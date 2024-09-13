using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using Microsoft.Unity.VisualStudio.Editor;
using Mono.Data.Sqlite;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Carta : MonoBehaviour
{    public List<int> tipo_colocado = new List<int>();
   public  bool entro_a_seleccion = false ;
    public string Nombre;
    public string Tipo;
    public string Faction;
    public string Efecto;
    public string Imagen_carta;
    public int id;

    public bool disponibilidad ;
   

    public UnityEngine.UI.Image marco;
    public UnityEngine.UI.Image imagen_carta;
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI efecto ;

    public Carta(int id, string nombre, string efecto, string tipo, string faction, bool disponibilidad )
    {
        this.id = id;
        this.Nombre = nombre;
        this.Efecto = efecto;
        this.Tipo = tipo;
        this.Faction = faction;
        this.disponibilidad = disponibilidad;
       
        
    }
    public Carta( int id, string nombre, string efecto, string tipo, string faction, bool disponibilidad ,UnityEngine.UI.Image marco,  UnityEngine.UI.Image imagen_carta, TextMeshProUGUI name, TextMeshProUGUI effect)
     {
      this.id = id;
        this.Nombre = nombre;
        this.Efecto = efecto;
        this.Tipo = tipo;
        this.Faction = faction;
        this.disponibilidad = disponibilidad;
        this.marco = marco;
        this.imagen_carta = imagen_carta;
        this.nombre = name;
        this.efecto = effect;
     }


    public Carta ( Carta c1)
    {
        this.id = c1.id;
        this.Nombre = c1.Nombre;
        this.Efecto = c1.Efecto;
        this.Tipo = c1.Tipo;
        this.Faction = c1.Faction;
    }

    public void Condicion_de_anadir()
    {
        
        if (GameObject.Find("Menu_de_construir_mazo").GetComponent<Mazo>().mazo.Count < 25)
        {
            int index = 0;
            for (int i = 0; i < GameObject.Find("Menu_de_construir_mazo").GetComponent<Mazo>().mazo.Count; i++)
            {
                if (GameObject.Find("Menu_de_construir_mazo").GetComponent<Mazo>().mazo[i].Nombre == this.Nombre)
                    index += 1;
            }
            if ((this.Tipo == "1" && index == 0 ) || (this.Tipo != "1" && index < 3))
             {
                 Anadir_mazo(gameObject.GetComponent<Carta>())
;             }
        }
    }

  
    public void Anadir_mazo(Carta cartica)
    {  
        GameObject.Find("Menu_de_construir_mazo").GetComponent<Mazo>().mazo.Add(new Carta(cartica.id,cartica.Nombre,cartica.Efecto,cartica.Tipo,cartica.Faction,true));
       
        if(GameObject.Find("Menu_de_construir_mazo").GetComponent<Mazo>().mazo.Count == 25)GameObject.Find("Canvas").GetComponent<Manager>().button_star.SetActive(true);

        GameObject.Find("Menu_de_construir_mazo").GetComponent<Mazo>().Asociar_Mazo();
    }

       public void Remover__carta_del_mazo()
    {  List<Carta>mazo  =   GameObject.Find("Menu_de_construir_mazo").GetComponent<Mazo>().mazo;
        for (int i = 0; i < mazo.Count; i++)
        {
            if (mazo[i].id == id)
            { 
                  GameObject.Find("Menu_de_construir_mazo").GetComponent<Mazo>().mazo.RemoveAt(i);
                i = mazo.Count;
            }
        } 
         GameObject.Find("Menu_de_construir_mazo").GetComponent<Mazo>().Asociar_Mazo();
    }




    public void Load_interface()
    {
      nombre.text= Nombre;
      efecto.text = Efecto;
      marco.sprite = Resources.Load<Sprite>("img/Diseno_de_cartas/cartas/"+Faction);
      imagen_carta.sprite = Resources.Load<Sprite>("img/Diseno_de_cartas/lideres/"+Nombre.Replace(' ', '_'));
        
    }
    public void Load_interface(GameObject  carta_unidad , GameObject carta_a_asociar)
    {
      carta_unidad.GetComponent<Carta_Unidad>().nombre = carta_a_asociar.GetComponent<Carta>().nombre;
      carta_unidad.GetComponent<Carta_Unidad>().efecto= carta_a_asociar.GetComponent<Carta>().efecto;
      carta_unidad.GetComponent<Carta_Unidad>().marco = carta_a_asociar.GetComponent<Carta>().marco;
      carta_unidad.GetComponent<Carta_Unidad>().imagen_carta =  carta_a_asociar.GetComponent<Carta>().imagen_carta;
    }
    
     

    public void Seleccion ( )
    {    
     
         
  
               GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada  = this.gameObject;
               GameObject.Find("Tablero").GetComponent<Juego>().Colocar_tablero();
               Debug.Log(this.gameObject.name);
               //this.gameObject.SetActive(false);
                 if(GameObject.Find("mazo").GetComponent<Mazo>().mano.Count>0)
                { for ( int i = 0; i<GameObject.Find("mazo").GetComponent<Mazo>().mano.Count;i++)
                  {
                    if ( GameObject.Find("mazo").GetComponent<Mazo>().mano[i].Nombre== GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Nombre)
                    GameObject.Find("Tablero").GetComponent<Juego>().posicion_carta_mano = i;
                  }
                } 



             
        
       
    }



    public void Mover_carta ()
    {  
       if(GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Tipo =="2" && disponibilidad == false || GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Tipo == "3" && disponibilidad == false || GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Tipo == "4" && disponibilidad == false )
        {
          // gameObject.GetComponent<Carta_Unidad>().Nombre = GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Nombre;
          // gameObject.GetComponent<Carta_Unidad>().Tipo = GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Tipo;
          // gameObject.GetComponent<Carta_Unidad>().Efecto = GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Efecto;
          // gameObject.GetComponent<Carta_Unidad>().id = GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().id;
          // gameObject.GetComponent<Carta_Unidad>().Faction = GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Faction;     
          //Load_interface(gameObject,  GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada);
          GameObject.Find("mazo_contrario").GetComponent<Mazo_oponente>().Conectar_base_ataque_salud(gameObject, GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().id);
          
          

      
          

        }


       if(disponibilidad == false )
       { 
        this.Nombre = GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Nombre;
        this.Tipo = GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Tipo;
        this.Efecto = GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Efecto;
        this.id= GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().id;
        this.Faction = GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<Carta>().Faction;
         Load_interface();
        disponibilidad = true;
         gameObject.GetComponent<IsPlaying>().En_mesa = true;
       
       
       
           for ( int k = 0 ;  k<GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1.GetLength(0);k++ )
           {
             for ( int l = 0 ;  l<GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1.GetLength(1);l++ )
             {
             

                if(GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[k,l].GetComponent<Carta>().Nombre== "")
                GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[k,l].SetActive(false);
             }
            }
  
            // Debug.Log(".");
            // Debug.Log(GameObject.Find("Tablero").GetComponent<Juego>().posicion_carta_mano);
            // GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.SetActive(false);
         
    
       
        

         for ( int i = 0 ; i <GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz.Count; i++ )
          { 
      
            if(GameObject.Find("mazo").GetComponent<Mazo>().mano.Count>i)
            {
               // Debug.Log("mano:"+GameObject.Find("mazo").GetComponent<Mazo>().mano.Count +" y el valor de i es:"+i);
             
              GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz[i].GetComponent<Carta>().Nombre =GameObject.Find("mazo").GetComponent<Mazo>().mano[i].Nombre;
              GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz[i].GetComponent<Carta>().Efecto =GameObject.Find("mazo").GetComponent<Mazo>().mano[i].Efecto;
              GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz[i].GetComponent<Carta>().Tipo =GameObject.Find("mazo").GetComponent<Mazo>().mano[i].Tipo;
              GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz[i].GetComponent<Carta>().Faction =GameObject.Find("mazo").GetComponent<Mazo>().mano[i].Faction;
              GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz[i].GetComponent<Carta>().Load_interface();
             // Debug.Log(i);
            }

          }
          GameObject.Find("mazo").GetComponent<Mazo>().mano.RemoveAt( GameObject.Find("Tablero").GetComponent<Juego>().posicion_carta_mano);
          GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz[GameObject.Find("mazo").GetComponent<Mazo>().mano.Count].SetActive(false);
         
          
       }
       
       
       
      
    }

}
