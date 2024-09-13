using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Carta_Senuelo : Carta_Unidad
{
    public string categoria;

    public  Carta_Senuelo(int id, string nombre, string efecto, string tipo, string faction, bool disponibilidad, UnityEngine.UI.Image marco ,  UnityEngine.UI.Image imagen_carta, TextMeshProUGUI name, TextMeshProUGUI effect):base(id, nombre,efecto,tipo,faction, disponibilidad,marco, imagen_carta,name,effect)
       
    {
        this.id = id;
        this.Nombre = nombre;
        this.Efecto = efecto;
        this.Tipo = tipo;
        this.Faction = faction;
        this.disponibilidad = disponibilidad;
    }
}
