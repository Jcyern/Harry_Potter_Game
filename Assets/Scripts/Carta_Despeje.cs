using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carta_Despeje : Carta_Especial
{//elimina una o varias cartas climas 
public int cantidad_a_eliminar;

 public Carta_Despeje (int id , string nombre , string efecto ,string tipo,string faction, bool disponibilidad ):base(id, nombre,efecto,tipo,faction, disponibilidad)
  { this.id = id;
    this.Nombre= nombre;
    this.Efecto= efecto;
    this.Tipo= tipo;
    this.Faction=faction;
    this.disponibilidad = disponibilidad;
    
  }

}
