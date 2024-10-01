using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Carta_Senuelo : Carta_Unidad
{
    public string categoria;

    public  Carta_Senuelo(int id, string nombre, string efecto, string tipo, string faction, bool disponibilidad, bool Iscard, int ataque , int salud):base(id, nombre,efecto,tipo,faction, disponibilidad,Iscard, ataque , salud )
       
    {
        this.id = id;
        this.Nombre = nombre;
        this.Efecto = efecto;
        this.Tipo = tipo;
        this.Faction = faction;
        this.disponibilidad = disponibilidad;
        this.IsCard = Iscard;
        this.ataque = ataque;
        this.salud = salud ;
     
    }
}
