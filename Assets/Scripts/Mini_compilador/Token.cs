using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public TokenType Type;
    public string Value ;
  
    public int Linea;
    public int Columna ;
   public Token (TokenType type , string value , int linea , int columna)
   {
      this.Type = type;
      this. Value = value;
      this.Linea = linea;
      this.Columna = columna;   
   }



  //Definiendo las palabras claves de nuestro leguaje

  //tipos de palabras

   public  enum TokenType
   {
    //Operadores Aritmeticos
     Suma,  //+
     Resta,   //-
     Multiplicacion,  //*
     Divicion, //  /
     Potencia , // ^
     PlusPlus, // ++

     
     //Concatenacion de cadenas
     Cadenas_simples, //@
     Cadenas_multiples ,  //@@
     
     //Comparativos
     Mayor,   // >
     Menor,    // <
     Mayor_igual, // >=
     Menor_igual,  //<=
     Iguales,   // ==
     Incremento, // +=
     Relacional,  //-=


     // Asignacion
     Asignacion ,  // =

     //Simbolos 
     Left_Paran ,  // (
     Right_Paran,  // )
     Left_Key ,  //{
     Right_Key , //}
     Left_Corchete, // [
     Right_Corchete, // ]
     Coma,  // ,
     Punto_Coma ,  // ;
     Punto ,    // .
     Dos_Puntos , // :
     Flecha ,  // =>
     Comillas, // "


     //Operadores Logicos
     Or,  // ||
     And,  // &&
     

  

     //Bucles 
     While,    // while 
     For,

     //Booleano
    Booleano,
    

    //KeyWords
    If,
    ELse,
    In,
    Card,
    Effect,
    Effect_activacion,
    Number,
    Variable,
    Type,
    Amount,
    Params,
    Range,
    String, 
    Action,
    Name, 
    Faction,
    Power ,  // poder de la carta 
    
    OnActivacion ,  //efectos q se ejecutaran cuando se coloque la carta en el campo 
    Selector  , //Filtro de cartas a las cuales se le aplicara el efecto 
    Source ,   // fuene de donde se sacan los targett , obejtvos 
    Single , //  boleano q dira si se tomar la primera coincidencia del selector 
    Predicate ,
    PostAction,  // otra declaracion de efecto que se ejecutara  luego del primero 
    Targets, //  objetivos 
    Context ,   // Contiene informacion del  juego 
    TriggerPlayer ,  // identificador id del jugador q desencadeno el efecto 
    Board ,   // devuelve lista de todas las cartas del tablero 
    HandOfPlayer, 
    hand,
    FieldOfPlayer,
    field,
    GraveyardOfPlayer,
    graveyard,
    DeckOfPlayer,
    Deck,
    Owner ,   // devuelve el id del jugador de la carta 
  
    //Lista de Metos q implementan las listas de cartas 
    Find,   //busca las cartas q cumplen con un predicado 
    find,
    Push,   //agrega una carta al tope de la lista 
    SendBottom, //agrega una carta al final de la lista 
    Pop ,  //quita la carta que esta al tope de la lista 
    Remove ,   // remueve una carta del tope de la  lista 
    Shuffle  , //  mezcla la lista 

   Palabra,
   Desconocido,
   Digitos

   }


}
