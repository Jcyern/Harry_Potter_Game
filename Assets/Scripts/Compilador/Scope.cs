using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scope : MonoBehaviour
{

    public Dictionary< string , string > variable ;  //alamacena nombre de la variable con tipo
    public Dictionary<string , string > valores ; //almacena el valor de la variable y la variable

    public Dictionary<string , Tipos>   prop_types ; 
    public Scope Parent {get;}


   public Scope ( Scope parent = null)
   {
     Parent = parent;
   }

   //Metodos
}
