using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Tracing;
using Microsoft.Unity.VisualStudio.Editor;
using Mono.Data.Sqlite;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Carta : MonoBehaviour
{
    public List<int> tipo_colocado = new List<int>();
    public bool entro_a_seleccion = false;
    public string Nombre;
    public string Tipo;
    public string Faction;
    public string Efecto;
    public string Imagen_carta;
    public int id;
    public bool IsCard;

    public bool disponibilidad;

    public UnityEngine.UI.Image marco;
    public UnityEngine.UI.Image imagen_carta;
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI efecto;

    public Carta(
        int id,
        string nombre,
        string efecto,
        string tipo,
        string faction,
        bool disponibilidad,
        bool IsCard
    )
    {
        this.id = id;
        this.Nombre = nombre;
        this.Efecto = efecto;
        this.Tipo = tipo;
        this.Faction = faction;
        this.disponibilidad = disponibilidad;
        this.IsCard = IsCard;
    }

    public Carta(
        int id,
        string nombre,
        string efecto,
        string tipo,
        string faction,
        bool disponibilidad,
        UnityEngine.UI.Image marco,
        UnityEngine.UI.Image imagen_carta,
        TextMeshProUGUI name,
        TextMeshProUGUI effect
    )
    {
        this.id = id;
        this.Nombre = nombre;
        this.Efecto = efecto;
        this.Tipo = tipo;
        this.Faction = faction;
        this.disponibilidad = disponibilidad;
        this.marco = marco;
        this.imagen_carta = imagen_carta;
        this.nombre = name;
        this.efecto = effect;
    }

    public Carta(Carta c1)
    {
        this.id = c1.id;
        this.Nombre = c1.Nombre;
        this.Efecto = c1.Efecto;
        this.Tipo = c1.Tipo;
        this.Faction = c1.Faction;
    }

    public void Remover__carta_del_mazo()
    {
        List<CardDisplay> mazo = GameObject.Find("Canvas").GetComponent<Mazo>().mazo;
        for (int i = 0; i < mazo.Count; i++)
        {
            if (mazo[i].GetComponent<CardDisplay>().carta.id == id)
            {
                GameObject.Find("Canvas").GetComponent<Mazo>().mazo.RemoveAt(i);
                i = mazo.Count;
            }
        }
        GameObject.Find("Canvas").GetComponent<Mazo>().Asociar_Mazo();
    }

    public void Mover_carta()
    {
        if (
            GameObject
                .Find("Tablero")
                .GetComponent<Juego>()
                .carta_seleccionada.GetComponent<CardDisplay>()
                .Tipo == "2"
                && GameObject
                    .Find("Tablero")
                    .GetComponent<Juego>()
                    .carta_seleccionada.GetComponent<CardDisplay>()
                    .disponibilidad == false
            || GameObject
                .Find("Tablero")
                .GetComponent<Juego>()
                .carta_seleccionada.GetComponent<CardDisplay>()
                .Tipo == "3"
                && GameObject
                    .Find("Tablero")
                    .GetComponent<Juego>()
                    .carta_seleccionada.GetComponent<CardDisplay>()
                    .disponibilidad == false
            || GameObject
                .Find("Tablero")
                .GetComponent<Juego>()
                .carta_seleccionada.GetComponent<CardDisplay>()
                .Tipo == "4"
                && GameObject
                    .Find("Tablero")
                    .GetComponent<Juego>()
                    .carta_seleccionada.GetComponent<CardDisplay>()
                    .disponibilidad == false
            || GameObject
                .Find("Tablero")
                .GetComponent<Juego>()
                .carta_seleccionada.GetComponent<CardDisplay>()
                .Tipo == "1"
                && GameObject
                    .Find("Tablero")
                    .GetComponent<Juego>()
                    .carta_seleccionada.GetComponent<CardDisplay>()
                    .disponibilidad == false
        )
        {
            //por comodidad para poder ver el ataque y la salud mas facil;
            gameObject.GetComponent<Carta_Unidad>().salud = GameObject
                .Find("Tablero")
                .GetComponent<Juego>()
                .carta_seleccionada.GetComponent<CardDisplay>()
                .carta_Unidad.salud;
            gameObject.GetComponent<Carta_Unidad>().ataque = GameObject
                .Find("Tablero")
                .GetComponent<Juego>()
                .carta_seleccionada.GetComponent<CardDisplay>()
                .carta_Unidad.ataque;
        }

        if (
            GameObject
                .Find("Tablero")
                .GetComponent<Juego>()
                .carta_seleccionada.GetComponent<CardDisplay>()
                .disponibilidad == false
                && gameObject.GetComponent<CardDisplay>().Name =="" 
        )   //si la disponibilidad es falsa 
        {
            gameObject
                .GetComponent<CardDisplay>()
                .CartaLoad(
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .carta_seleccionada.GetComponent<CardDisplay>()
                        .carta
                );

            gameObject.GetComponent<CardDisplay>().Load_interface();

            gameObject.GetComponent<CardDisplay>().disponibilidad = true;
            gameObject.GetComponent<IsPlaying>().En_mesa = true;

            //desactivando las posiciones no  ocupadas
            for (
                int k = 0;
                k < GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1.GetLength(0);
                k++
            )
            {
                for (
                    int l = 0;
                    l < GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1.GetLength(1);
                    l++
                )
                {
                    if (
                        GameObject
                            .Find("Tablero")
                            .GetComponent<Juego>()
                            .tablero_j1[k, l]
                            .GetComponent<CardDisplay>()
                            .Name == ""
                    )
                        GameObject
                            .Find("Tablero")
                            .GetComponent<Juego>()
                            .tablero_j1[k, l]
                            .SetActive(false);
                }
            }

            //organizando la mano interfaz pq se quito una carta , osea la q e jugo

            for (
                int i = 0;
                i < GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz.Count;
                i++
            )
            {
                if (GameObject.Find("mazo").GetComponent<Mazo>().mano.Count > i)
                {
                    // Debug.Log("mano:"+GameObject.Find("mazo").GetComponent<Mazo>().mano.Count +" y el valor de i es:"+i);
                    
                  GameObject.Find("mazo").GetComponent<Mazo>().mano_interfaz[i].GetComponent<CardDisplay>().CartaLoad(GameObject.Find("mazo").GetComponent<Mazo>().mano[i].carta);

                    
                    GameObject
                        .Find("mazo")
                        .GetComponent<Mazo>()
                        .mano_interfaz[i]
                        .GetComponent<CardDisplay>()
                        .Load_interface();
                    // Debug.Log(i);
                }
            }

            //elimnia la carta jugada de la mano
            GameObject
                .Find("mazo")
                .GetComponent<Mazo>()
                .mano.RemoveAt(
                    GameObject.Find("Tablero").GetComponent<Juego>().posicion_carta_mano
                );

            // desactiva la la carta de la mano vacia
            GameObject
                .Find("mazo")
                .GetComponent<Mazo>()
                .mano_interfaz[GameObject.Find("mazo").GetComponent<Mazo>().mano.Count]
                .SetActive(false);
        }
    }
}
