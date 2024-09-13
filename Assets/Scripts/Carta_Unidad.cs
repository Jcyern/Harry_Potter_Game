using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Carta_Unidad : Carta  
{
   public int ataque ;
   public int salud ;
   
   

    public Carta_Unidad (int id , string nombre , string efecto ,string tipo,string faction, bool disponibilidad, UnityEngine.UI.Image marco ,  UnityEngine.UI.Image imagen_carta, TextMeshProUGUI name, TextMeshProUGUI effect):base(id, nombre,efecto,tipo,faction, disponibilidad,marco, imagen_carta,name,effect)
  { this.id = id;
    this.Nombre= nombre;
    this.Efecto= efecto;
    this.Tipo= tipo;
    this.Faction=faction;
    this.disponibilidad= disponibilidad;
    this.marco =  marco;
    this.imagen_carta = imagen_carta;
    this.nombre = name;
    this.efecto = effect;
    
  }
}
