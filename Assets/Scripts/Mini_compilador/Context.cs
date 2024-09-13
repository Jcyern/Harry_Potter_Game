using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
public class Context 
{
    public Dictionary<string , object> Variables {get;} = new();
    public Dictionary<string , CardNode> Cartas {get;} = new();
    public Dictionary<string , EffectNode> Efectos {get;} = new ();

    Context pere;  
 
    
    public Context ( Context Pere = null)
    {
        this.pere = Pere;
    }
    public void Definir_Variabless(string name , object value)  
    {
        if(Variables.ContainsKey(name))
        {
            SetVariable(name , value );
        }
       
       Variables[name] = value;
    }

    public object GetVariable ( string name ) // te devuelve la variable que tenga ese nombre 
    {
        if ( Variables.ContainsKey(name))
        {
            return Variables[name ];
        }
        else if ( pere != null)
        {
            pere.GetVariable(name );
        }
        throw new  Exception ( $" La variable {name } no esta definida");
    }
    public void SetVariable ( string name , object value ) // cambiando el valor de una variable especifica en el caso de existir 
    {
       if( Variables.ContainsKey(name )== false )
       {
         throw new Exception ( $" La variable {name } no esta definida");

       }
       
        var current_context = Variables[name ];

        if ( current_context.GetType() != value.GetType()) // verifica si el valor que se pasa no  es del mismo tipo que el existia 
        {
            throw new Exception ( $"El valor que se le quiere es del tipo {value.GetType()}y esta variable solo acepta {current_context.GetType()}  ");
        }
         Variables[name ]= value;
    }
    
    public void DefinirEfecto(string name , EffectNode effect )  // creando un efecto 
    {
      if (Efectos.ContainsKey(name ))
      {
         throw new Exception ( $" El efecto {name }ya esta definido ");
      }
      Efectos[name] = effect;
    }

    public object GetEffect ( string name )  // dame el efecto si existe 
    {
        if(Efectos.ContainsKey(name))
        {
               return Efectos[name];
        }
        else if( pere != null)
        {
            return pere.GetEffect(name);
        }
       throw new Exception( $" El efecto { name} no esta definido");
        
    }

    public void DefinirCarta( string name, CardNode carta) // agregando carta 
    {
        if( Cartas.ContainsKey(name))
        {
            throw new Exception ( $" La carta {name} ya esta definida ");
        }
        Cartas[name] = carta;
    }

    public object GetCarta ( string name) //busca una carta por su nombre si aparece se devolvera 
    {
         if ( Cartas.ContainsKey(name))
         {
            return Cartas[ name ];
         }
         else if (  pere != null)
         {
            return pere.GetCarta(name);
         }
         throw new Exception ( $" la carta {name }no esta definida ");
    }


    public void DebugVariables()  // muestra las variables que hay existentes 
    {   Debug.Log( "Variables :");
        foreach ( var x in Variables)
        { 
            Debug.Log( $"Name {x.Key}  Value : {x.Value}");
        }
    }
    public void DebugEfectos()  // muestra los efectos que hay existetes
    {
        Debug.Log("Efectos:");
        foreach ( var effect in Efectos)
        {
            Debug.Log($"Name: {effect.Key}  Effect { effect.Value }");
        }
    }

    public void DebugCartas ()  // muestra las cartas que hay existentes 
    {
        Debug.Log( "Cartas");
        foreach (var carta in Cartas)
        {
            Debug.Log ( $" Name: { carta.Key}  Carta: {carta.Value}");
        } 

    }
}

