using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Unity.VisualScripting;
using UnityEngine;

public class Game_contex : MonoBehaviour
{
    // se le pasa el id del jugador y devuelve una lista de cartas correspondiente
    public int TriggerPlayer = 1;

    //Graveyard

    public Dictionary<int, List_Cards> Boards = new Dictionary<int, List_Cards>();
    public Dictionary<int, List_Cards> Hands = new Dictionary<int, List_Cards>();
    public Dictionary<int, List_Cards> Graveyards = new Dictionary<int, List_Cards>();
    public Dictionary<int, List_Cards> Fields = new Dictionary<int, List_Cards>(); // filas
    public Dictionary<int, List_Cards> Decks = new Dictionary<int, List_Cards>();

    //prpiedades autoimplementadas
    public List_Cards Hand => Hands[TriggerPlayer];
    public List_Cards GraveYard => Graveyards[TriggerPlayer];
    public List_Cards Deck => Decks[TriggerPlayer];
    public List_Cards Board => GetBoard();

    //public List_Cards Board => GetBoard();



    // representacion del tablero , filas , y ubicacion de cartas especiales
    //parte del jugador 1
    public List_Cards espada_1 = new List_Cards();
    public List_Cards sombrero_1 = new List_Cards();
    public List_Cards caldero_1 = new List_Cards();
    public List_Cards escudo_1 = new List_Cards();
    public List_Cards barita_1 = new List_Cards();

    //parte del jugador dos
    public List_Cards espada_2 = new List_Cards();
    public List_Cards sombrero_2 = new List_Cards();
    public List_Cards caldero_2 = new List_Cards();
    public List_Cards escudo_2 = new List_Cards();
    public List_Cards barita_2 = new List_Cards();

    public void InicializarTablero()
    {
        Hands[1] = new List_Cards();
        Decks[1] = new List_Cards();
        Graveyards[1] = new List_Cards();
        Fields[1] = new List_Cards();
        Hands[2] = new List_Cards();
        Decks[2] = new List_Cards();
        Graveyards[2] = new List_Cards();
        Fields[2] = new List_Cards();
    }

    public Game_contex()
    {
        InicializarTablero();
    }

    public List_Cards GetBoard()
    {
        List_Cards tableros = new List_Cards();

        tableros.AddRange(Fields[1]);
        tableros.AddRange(Fields[2]);
        return tableros;
    }

    // Metodos necesarios

    public List_Cards HandOfPlayer(int id) // devuelve la mano del jugador
    {
        return Hands[id];
    }

    public List_Cards DeckOfPLayer(int id) // devuelve el deck del jugador pasado
    {
        return Decks[id];
    }

    public List_Cards FieldOfPlayer(int id)
    {
        return Fields[id];
    }

    public List_Cards GraveyardOfPlayer(int id)
    {
        return Graveyards[id];
    }

    // Conectar con la interface del juego actual

    public void ActualContext(int owner)
    {
        Hands[owner].Clear();
        Decks[owner].Clear();
        Graveyards[owner].Clear();
        Fields[owner].Clear();

        if (owner == 1)
        {
            espada_1.Clear();
            sombrero_1.Clear();
            caldero_1.Clear();
            barita_1.Clear();
            escudo_1.Clear();

            // sincronizar el deck del  jugador
            foreach (var carta in GameObject.Find("Canvas").GetComponent<Mazo>().mazo) // sincro mazo
            {
              //  Card card = TransformCard(carta, 1);
              //  Decks[1].Add(card);
            }
            foreach (var carta in GameObject.Find("mazo").GetComponent<Mazo>().mano) // sincro mano
            {
               // Card card = TransformCard(carta, 1);
               // Hands[1].Add(card);
            }
            if (GameObject.Find("cementerio") != null)
            {
                foreach (
                    var gameobj in GameObject.Find("mazo").GetComponent<Cementerio>().cementerio
                ) //sincro cementerio
                {
                    Card card = TransformCard(gameobj,1);
                    Graveyards[1].Add(card);
                }
            }
            int i = 0;
            foreach (var casilla in GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1)
            {
                if (casilla.activeSelf)
                {
                    Card card = TransformCard(casilla, 1);
                    Fields[1].Add(card);

                    if (i == 5 || i == 0 || i == 10)
                    {
                        barita_1.Add(card);
                    }
                    else if (i == 1 || i == 6 || i == 11)
                    {
                        escudo_1.Add(card);
                    }
                    else if (i == 2 || i == 3 || i == 4)
                    {
                        espada_1.Add(card);
                    }
                    else if (i == 7 || i == 8 || i == 9)
                    {
                        sombrero_1.Add(card);
                    }
                    else if (i == 12 || i == 13 || i == 14)
                    {
                        caldero_1.Add(card);
                    }
                }
                i++;
            }
        }
        else if (owner == 2)
        {
            espada_2.Clear();
            sombrero_2.Clear();
            caldero_2.Clear();
            barita_2.Clear();
            escudo_2.Clear();

            foreach (
                var carta in GameObject
                    .Find("mazo_contrario")
                    .GetComponent<Mazo_oponente>()
                    .mazo_oponente
            ) //deck
            {
              //  Card card = TransformCard(carta, 2);
               // Decks[2].Add(card);
            }

            foreach (
                var carta in GameObject
                    .Find("mazo_contrario")
                    .GetComponent<Mazo_oponente>()
                    .mano_oponente
            ) // mano
            {
                //Card card = TransformCard(carta, 2);
                //Hands[2].Add(card);
            }
            if (GameObject.Find("cementerio_oponente") != null) // grave
            {
                foreach (
                    var gameobj in GameObject
                        .Find("mazo")
                        .GetComponent<Cementerio>()
                        .cementerio_contrario
                )
                {
                    Card card = TransformCard(gameobj, 2);
                    Graveyards[2].Add(card);
                }     
            }
            int j = 0;
            foreach (var casilla in GameObject.Find("Tablero").GetComponent<Juego>().tablero_j2) // filas
            {
                if (casilla.activeSelf)
                {
                    Card card = TransformCard(casilla, 2);
                    Fields[2].Add(card);

                    if (j == 0 || j == 5 || j == 10)
                    {
                        barita_2.Add(card);
                    }
                    else if (j == 1 || j == 6 || j == 11)
                    {
                        escudo_2.Add(card);
                    }
                    else if (j == 2 || j == 3 || j == 4)
                    {
                        caldero_2.Add(card);
                    }
                    else if (j == 7 || j == 8 || j == 9)
                    {
                        sombrero_2.Add(card);
                    }
                    else if (j == 12 || j == 13 || j == 14)
                    {
                        espada_2.Add(card);
                    }
                }
            }
        }
        else
        {
            throw new System.Exception(" el numero pasado no es ni 1 ni 2 ");
        }
    }

    public Card TransformCard(GameObject carta, int owner)
    {
        Card card = new Card();
        card.Name = carta.GetComponent<Carta>().Nombre;
        card.Faction = carta.GetComponent<Carta>().Faction;
        card.Owner = owner;
        card.Type = carta.GetComponent<Carta>().Tipo;
        if (card.Type == "1" || card.Type == "2" || card.Type == "3" || card.Type == "4")
        {
        
            card.Power = carta.GetComponent<Carta_Unidad>().ataque;
            card.Health =  carta.GetComponent<Carta_Unidad>().salud;
            Debug.Log(card.Power);
            Debug.Log(card.Health);
        }
        return card;
    }

    public void RemoveCard(Card card)
    {
        SearchCardInLIst(card, espada_1);
        SearchCardInLIst(card, espada_2);

        SearchCardInLIst(card, sombrero_1);
        SearchCardInLIst(card, sombrero_2);

        SearchCardInLIst(card, caldero_1);
        SearchCardInLIst(card, caldero_2);

        SearchCardInLIst(card, barita_1);
        SearchCardInLIst(card, barita_2);

        SearchCardInLIst(card, escudo_1);
        SearchCardInLIst(card, escudo_2);
    }

    public void SearchCardInLIst(Card card, List_Cards list)
    {
        List_Cards cartas_to_remove = new();

        foreach (var item in list)
        {
            if (item == card)
            {
                cartas_to_remove.Add(card); // cartas a remover
            }
        }
        foreach (var carta in cartas_to_remove)
        {
            list.Remove(carta);
        }
    }

    public Game_contex GenerarGameContext()
    {
        Game_contex contexto = new Game_contex();

        contexto.ActualContext(1);
        contexto.ActualContext(2);
        return contexto;
    }




    public void ActualizarVisual (  int Owner , List<GameObject> Hand  )
    {
    
      //tiene card display 
        void  Sincronizacion ( List_Cards cards  , List<GameObject> objeto  )
        { 
            int i  = 0;
           foreach ( var card in cards)
           {
             objeto[i].GetComponent<CardDisplay>().card = card;
             objeto[i].GetComponent<CardDisplay>().LoadCard();
             i++;
           }
           i=0;
              
           
          
        }

      // Sincronizacion(Hands[1], GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz );
        
    }
}
