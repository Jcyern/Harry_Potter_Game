using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lider : MonoBehaviour
{
    public string Nombre;
    public string Efecto;

    public Lider(string nombre, string efecto)
    {
        this.Nombre = nombre;
        this.Efecto = efecto;
    }
}
