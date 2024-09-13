using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Cambio_Turno : MonoBehaviour
{  //true -- Jugador 1 
   //false -- Jugador 2 (cpu)
    public bool turno = true ;
    public int ronda =1;
     List<GameObject> mano ;
     List<GameObject> mano_provicional;
    int cant_turno;

   
    public void Turnar()
    { 
      if(turno== true )
     { turno = false ;
      
       
      
         GameObject.Find("mazo_contrario").GetComponent<Mazo_oponente>().Robo_Aut(10);
      
         GameObject.Find("mazo_contrario").GetComponent<Mazo_oponente>().Anadir_Script_carta_unidad(GameObject.Find("mazo_contrario").GetComponent<Mazo_oponente>().mano_interfaz_2);
         
         GameObject.Find("mazo_contrario").GetComponent<Mazo_oponente>().Jugar_carta();
        
        
             
          
        
     }
      else if(turno == false )
      {
        turno= true;
        ronda++;  
        GameObject.Find("numero_ronda").GetComponent<TextMeshProUGUI>().text = ronda.ToString();
       if( GameObject.Find("mazo").GetComponent<Mazo>().robos==1)
        GameObject.Find("mazo").GetComponent<Mazo>().robos--;

      }
     
  
      
    }

   
      
    
  
}
