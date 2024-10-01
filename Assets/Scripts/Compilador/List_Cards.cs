using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class List_Cards : IEnumerable<Card>
{
    protected List<Card> cards = new List<Card>();

    public IEnumerator<Card> GetEnumerator()
    {
        return cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return cards.GetEnumerator();
    }

    public Card Find(Func<Card, bool> predicate) // devuelve una carta que cumple con el predicado
    {
        return cards.FirstOrDefault(predicate); // iter sobre la colleccion Ienumerable y aplica la func , el primer elemento que la cumpla lo devuelve , sino lo encuentra devuelve null
    }

    public void Push(Card card) // agrega una carta al tope de la lista
    {
        cards.Add(card);
    }

    public void SendBottom(Card card) // inserta una carta al fondo de la lista . el fonde se asume como la pos 0
    {
        cards.Insert(0, card);
    }

    public Card Pop() // quita la carta que esta al tope de la lista y la devuelve
    {
        if (cards.Count == 0)
            return null; // sino hay cartas

        Card topCard = cards[cards.Count - 1];
        cards.RemoveAt(cards.Count - 1);
        return topCard;
    }

    public void Remove(Card card) // remueve carta dela lista
    {
        cards.Remove(card);
    }

    public void Shuffle() // mezcla la lista
    {
        System.Random rng = new();

        int n = cards.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // genra un numero entre 0 y el numero pasado
            Card v = cards[k];
            cards[k] = cards[n];
            cards[n] = v;
        }
    }

    public void Add(Card card) // agrega carta a la lista
    {
        cards.Add(card);
    }

    public void Clear() // elimina todos los elementos de la lista
    {
        cards.Clear();
    }

    public void AddRange(List_Cards card)
    {
        cards.AddRange(card);
    }

    public int Count()
    {
        return cards.Count;
    }

    public void Insert(int pos, Card carta)
    {
        cards.Insert(pos, carta);
    }

    public bool Contains(Card card)
    {
        return cards.Contains(card);
    }
}
