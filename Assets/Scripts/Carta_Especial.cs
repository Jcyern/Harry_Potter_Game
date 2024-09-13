using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carta_Especial : Carta 
{
    public int categoria;
    //1__clima
    //2__aumentos
    //3__despejes
    //4__senuelos  

     public Carta_Especial (int id , string nombre , string efecto ,string tipo,string faction, bool disponibilidad):base( id, nombre,efecto,tipo,faction,disponibilidad)
  { this.id = id;
    this.Nombre= nombre;
    this.Efecto= efecto;
    this.Tipo= tipo;
    this.Faction=faction;
    this.disponibilidad= disponibilidad;
    
  }
      
  
}
