using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class mazo_cards : MonoBehaviour
{
    public List<GameObject> CreatedCards;

    public List<Card> cards = new List<Card>();

    public void Roba()
    {
        //GameObject.Find("mazo").GetComponent<Mazo>().RobarCard(10);
    }

    public void SaludAtaque(int id, GameObject CartaUnidad)
    {
        foreach (var obj in cards)
        {
            if (obj.Id == id)
            {
                CartaUnidad.GetComponent<Carta_Unidad>().salud = obj.Health;
                CartaUnidad.GetComponent<Carta_Unidad>().ataque = obj.Power;
            }
        }
    }

    public void ActivarEffect(int id)
    {
        foreach (var card in cards)
        {
            if (card.Id == id)
            {
                
                card.AcivateEffect();
            }
        }
    }
}
