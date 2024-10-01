using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public enum Faction
{
    Gryffindor,
    Slytherin,
    Ravenclub,
    Hufflepuff
};

public enum EffectType
{
    Oro,
    Plata,
    Clima,
    Aumento,
    Despeje,
    Senuelo,
    Dark,
    Celestial,
    NotEffect
}

public enum CardType
{
    Oro,
    Plata,
    Clima,
    Aumento,
    Despeje,
    Senuelo,
    Lider
}

public enum Range
{
    Melee,
    Ranged,
    Siege
}

public class Card : ScriptableObject
{
    public int Owner;
    public int Id;
    public bool IsCreated;
    public bool disponibilidad;
    public bool IsCard;
    public Sprite Sprite;
    public string Type;
    public string Name;
    public EffectType Effect;
    public string Faction;
    public int Power;
    public int Health;
    public Range[] Range;

    public List<Effects> OnActivation;
    public EffectCreated EffectCreated;

    //visuales
    public UnityEngine.UI.Image marco;
    public UnityEngine.UI.Image imagen_carta;
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI efecto;

    //crear los efectos . par a guardarlos

    public void AcivateEffect()
    {
        if (IsCreated) //si la carta esta creada
        {
            foreach (var effect in OnActivation) //recorre los efectos creados y guardados en card
            {
                // Debug.Log("Owner: " + Owner);
                ActivateSpecificEffect(effect, effect.Params);
            }
        }
        else
        {
            Debug.Log("NO esta creada");
        }
    }

    public void ActivateSpecificEffect(Effects efecto, List<object> param)
    { // .GetMethod(efecto.Name + "Effect");//
        // busca el nombre dek metodo en la clase EffectCreated que ya debe estar creado
        var effectMethot = typeof(EffectCreated).GetMethod(efecto.Name + "Effect");
        var effect = efecto.Name + "Effect";
        if (effectMethot != null)
        {
            if (param.Count == 0 || param == null) // sino hay parametros
            {
                var targetslist = efecto.Targets; // crea los targets y me los devuelve

                //invocando el dicho metodo ( se pasa el nombre de la clase, array objetos d los parametros )
                effectMethot.Invoke(
                    EffectCreated,
                    new object[]
                    {
                        targetslist,
                        GameObject.Find("Canvas").GetComponent<Manager>().contexto_game,
                    }
                );
            }
            else if (param.Count == 1)
            {
                var targetslist = efecto.Targets;
                foreach (var target in targetslist)
                {
                    Debug.Log($"{target.Name}  Power {target.Power} Salud {target.Health}");
                }

                effectMethot.Invoke(
                    EffectCreated,
                    new object[]
                    {
                        targetslist,
                        GameObject.Find("Canvas").GetComponent<Manager>().contexto_game,
                        int.Parse(param[0].ToString())!
                    }
                );
                Debug.Log("Board");
                foreach (
                    var target in GameObject
                        .Find("Canvas")
                        .GetComponent<Manager>()
                        .contexto_game.Board
                )
                {
                    Debug.Log($"{target.Name}  Power {target.Power} Salud {target.Health}");
                }
            }
            else if (param.Count == 2)
            {
                var targetslist = efecto.Targets;
                effectMethot.Invoke(
                    EffectCreated,
                    new object[]
                    {
                        targetslist,
                        GameObject.Find("Canvas").GetComponent<Manager>().contexto_game,
                        int.Parse(param[0].ToString())!,
                        int.Parse(param[1].ToString())!
                    }
                );
            }
            else if (param.Count == 3)
            {
                var targetslist = efecto.Targets;
                effectMethot.Invoke(
                    EffectCreated,
                    new object[]
                    {
                        targetslist,
                        GameObject.Find("Canvas").GetComponent<Manager>().contexto_game,
                        int.Parse(param[0].ToString())!,
                        int.Parse(param[1].ToString())!,
                        int.Parse(param[2].ToString())!
                    }
                );
            }
            else
            {
                var targetslist = efecto.Targets;
                effectMethot.Invoke(
                    EffectCreated,
                    new object[]
                    {
                        targetslist,
                        GameObject.Find("Canvas").GetComponent<Manager>().contexto_game,
                        int.Parse(param[0].ToString())!,
                        int.Parse(param[1].ToString())!,
                        int.Parse(param[2].ToString())!,
                        int.Parse(param[3].ToString())!
                    }
                );
            }
        }
        else
        {
            throw new System.Exception(
                $" Efecto no encontrado verifique su existencia {efecto.Name + "Effect"} "
            );
        }
    }

    public List<GameObject> CreateGameObjectList(List<Card> cards)
    {
        List<GameObject> list = new();
        foreach (var card in cards)
        {
            GameObject carta = new GameObject();

            carta.AddComponent<Carta>();

            carta.GetComponent<Carta>().Nombre = card.Name;
            carta.GetComponent<Carta>().Tipo = NumPersonajes(card.Type);

            carta.GetComponent<Carta>().Faction = NumFac(card.Faction);
            carta.GetComponent<Carta>().IsCard = true;
            carta.GetComponent<Carta>().id = card.Id;
            carta.GetComponent<Carta>().Efecto = card.OnActivation[0].Name;

            //agregando el power y la salud
            carta.AddComponent<Carta_Unidad>();
            //   carta.GetComponent<Carta_Unidad>().salud = card.Salud;

            carta.GetComponent<Carta_Unidad>().ataque = card.Power;
            carta.GetComponent<Carta_Unidad>().salud = card.Health;

            list.Add(carta);
        }
        return list;
    }

    public string NumFac(string Faction)
    {
        if (Faction == "Gryffindor")
        {
            return "1";
        }
        else if (Faction == "Slytherin")
        {
            return "2";
        }
        else if (Faction == "Hufflepuff")
        {
            return "3";
        }
        else if (Faction == "Ravenclaw")
        {
            return "4";
        }
        else
        {
            return "0";
        }
    }

    public string NumPersonajes(string type)
    {
        if (type == "Heroe")
        {
            return "1";
        }
        else if (type == "Criatura")
        {
            return "2";
        }
        else if (type == "Mago")
        {
            return "3";
        }
        else if (type == "Ser")
        {
            return "4";
        }
        else if (type == "Objeto")
        {
            return "5";
        }
        else if (type == "Hechizo")
        {
            return "6";
        }
        else if (type == "Pocion")
        {
            return "7";
        }
        else if (type == "Lugar")
        {
            return "8";
        }
        else
        {
            return "0";
        }
    }
}
