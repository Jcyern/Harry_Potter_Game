using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public string Name { get; set; }
    public List<object> Params { get; set; }
    public string Source { get; set; }
    public bool Single { get; set; }

   public Predicate Predicado { get; set; }

   public List_Cards Targets => CreateTargets();

    public List_Cards CreateTargets()
    {
        List_Cards cards = new();
        
        foreach ( var card in GameObject.Find("Canvas").GetComponent<Manager>().contexto_game.Board )
        {
          UnityEngine.Debug.Log(card.name);
        }
        //no se aun si halla q escapar los caracteres
        if (Source.ToLower() == "board")
        {
             
            foreach (
                Card unit in GameObject.Find("Canvas").GetComponent<Manager>().contexto_game.Board
            )
            {
                if (Predicado == null || EvaluatePredicate(unit))
                {
                    cards.Add(unit);
                }
            }
        }
        else if (Source.ToLower() == "hand")
        {
            foreach (
                Card unit in GameObject.Find("Canvas").GetComponent<Manager>().contexto_game.Hand
            )
            {
                if (Predicado == null || EvaluatePredicate(unit))
                {
                    cards.Add(unit);
                }
            }
        }
        else if (Source.ToLower() == "deck")
        {
            foreach (
                Card unit in GameObject.Find("Canvas").GetComponent<Manager>().contexto_game.Deck
            )
            {
                if (Predicado == null || EvaluatePredicate(unit))
                {
                    cards.Add(unit);
                }
            }
        }
        else if (Source.ToLower() == "cementery")
        {
            foreach (
                Card unit in GameObject.Find("Canvas").GetComponent<Manager>().contexto_game.GraveYard

            )
            {
                if (Predicado == null || EvaluatePredicate(unit))
                {
                    cards.Add(unit);
                }
            }
        }
        else
        {
            throw new Exception($" NO se reconocio el Source: {Source}");
        }

       foreach (var card in cards )
       {
         UnityEngine.Debug.Log( $"Card:  {card.Name}  vida: {card.Health} power: { card.Power}");
       }
    
        return cards;
    }

    //meoods del Predicado

    public bool EvaluatePredicate(Card unit)
    { 
        
        switch (Predicado.Operator)
        {
            case "==":
                return EvaluateEqualPredicate(unit);
            case "!=":
                return EvaluateNotEquals(unit);
            case "<=":
                return EvaluateLessOrEqual(unit);
            case ">=":
                return EvaluateMoreOrEqual(unit);
            case "<":
                return EvaluateLess(unit);
            case ">":
                return EvaluateMore(unit);
            default:
                throw new Exception($" NO se encuentra el operador {Predicado.Operator}");
        }
    }

    // ==
    public bool EvaluateEqualPredicate(Card unit)
    {
        switch (Predicado.LeftMember)
        {
            case "Type":
                return unit.Type == Predicado.RightMemeber.ToString();
            case "Faction":
                return unit.Faction == Predicado.RightMemeber.ToString();
            case "Power":
                return unit.Power.ToString() == Predicado.RightMemeber.ToString();
            case "Range":
                return unit.Range.Contains(Enum.Parse<Range>(Predicado.RightMemeber.ToString()));
            default:
                throw new Exception(
                    $" No se reconoce el miembro del predicado {Predicado.LeftMember}"
                );
        }
    }

    //!=
    public bool EvaluateNotEquals(Card unit)
    {
        switch (Predicado.LeftMember)
        {
            case "Type":
                return unit.Type != Predicado.RightMemeber.ToString();
            case "Faction":
                return unit.Faction != Predicado.RightMemeber.ToString();
            case "Power":
                return unit.Power.ToString() != Predicado.RightMemeber.ToString();
            case "Range":
                return !unit.Range.Contains(Enum.Parse<Range>(Predicado.RightMemeber.ToString()));
            default:
                throw new Exception(
                    $" No se reconoce el miembro del predicado {Predicado.LeftMember}"
                );
        }
    }

    // <=
    public bool EvaluateLessOrEqual(Card unit)
    {
        switch (Predicado.LeftMember)
        {
            case "Power":
                return int.Parse(unit.Power.ToString())
                    <= int.Parse(Predicado.RightMemeber.ToString());
            default:
                throw new Exception(
                    $"Operador '<=' solo se puede usar con 'Power': {Predicado.LeftMember}"
                );
        }
    }

    //>=

    public bool EvaluateMoreOrEqual(Card unit)
    {
        switch (Predicado.LeftMember)
        {
            case "Power":
                return int.Parse(unit.Power.ToString())
                    >= int.Parse(Predicado.RightMemeber.ToString());
            default:
                throw new Exception(
                    $"Operador '>=' solo se puede usar con 'Power': {Predicado.LeftMember}"
                );
        }
    }

    public bool EvaluateMore(Card unit)
    {
        switch (Predicado.LeftMember)
        {
            case "Power":
                return int.Parse(unit.Power.ToString())
                    > int.Parse(Predicado.RightMemeber.ToString());
            default:
                throw new Exception(
                    $"Operador '>' solo se puede usar con 'Power': {Predicado.LeftMember}"
                );
        }
    }

    public bool EvaluateLess(Card unit)
    {
        switch (Predicado.LeftMember)
        {
            case "Power":
                return int.Parse(unit.Power.ToString())
                    < int.Parse(Predicado.RightMemeber.ToString());
            default:
                throw new Exception(
                    $"Operador '>' solo se puede usar con 'Power': {Predicado.LeftMember}"
                );
        }
    }
}

public class Predicate
{
    public string LeftMember { get; set; }
    public string Operator { get; set; }
    public object RightMemeber { get; set; }
}
