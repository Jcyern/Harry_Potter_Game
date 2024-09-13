using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public abstract class Tipos 
{
     public Dictionary< string, string>  Propiedades ;
}
public class CardPropieties : Tipos
{
      //builder 
   public CardPropieties( )
   {
       Propiedades = new()
       {
           //pripiedades q tiene card
           ["Type"] = "string",
           ["Name"] = "string",
           ["Faction"] = "string",
           ["Power"] = "int",
           ["Range"] = "List<string>",
           ["Owner"] = "Player"
       };
   }
}  

     public class EffectPropiedades :Tipos
      {
         public EffectPropiedades()
         {
            Propiedades = new()
           {
              ["Name"]= "string",
              ["Params"]= "variable int",
              ["Amount"]= "int",
              ["Action"]= "Lambda Funtion"


           } ; 

        }
      }
   public class Context_type :Tipos
   {
       public Context_type( )
       {
         
         Propiedades = new()
         {
             ["TriggerPLayer"] = "id Player",
             ["Board "] = " List<Card>  ",
             ["HandOfPLayer"] = "List<Card> ",
             ["FieldOfPlayer"] = "List<Card>",
             ["GraveYardOfPlayer"] = "List<Cards> ",
             ["DeckOfPlayer"] = "List<cards>",
             ["Hand"] = "List<Card>",
             ["Deck"] = "List<card>",
             ["OtherDeck"] = "List<card>",
             ["OtherField"] = "List<card>",
             ["OtherGraveyard"] = "List<card>",
             ["OtherHand"] = "List<card>"
         };

       }
     
   }
