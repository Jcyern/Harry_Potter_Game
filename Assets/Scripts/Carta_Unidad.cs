using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Carta_Unidad : Carta
{
    public int ataque = 0;
    public int salud = 0;

    public Carta_Unidad(
        int id,
        string nombre,
        string efecto,
        string tipo,
        string faction,
        bool disponibilidad,
        bool IScard,
        int ataque ,
        int salud 
        )

        : base(id, nombre, efecto, tipo, faction, disponibilidad,IScard)
    {
        this.id = id;
        this.Nombre = nombre;
        this.Efecto = efecto;
        this.Tipo = tipo;
        this.Faction = faction;
        this.disponibilidad = disponibilidad;
        this.IsCard =  IScard;
        this.ataque = ataque;
        this.salud = salud;
    }
}
