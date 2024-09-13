using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carta_Clima : Carta_Especial
{
    public int fila;
    public int penalizacion;

    public Carta_Clima(int id, string nombre, string efecto, string tipo, string faction, bool disponibilidad)
        : base(id, nombre, efecto, tipo, faction, disponibilidad)
    {
        this.id = id;
        this.Nombre = nombre;
        this.Efecto = efecto;
        this.Tipo = tipo;
        this.Faction = faction;
        this.disponibilidad = disponibilidad;
    }
}
