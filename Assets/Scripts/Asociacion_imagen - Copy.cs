using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Asociacion_imagen : MonoBehaviour
{
    public UnityEngine.UI.Image imagen;
    public UnityEngine.UI.Image imagen_2;

    public void Asociar()
    {
        string lider = GameObject.Find("Canvas").GetComponent<Manager>().Lider.Replace(' ', '_');
        string lider_enemigo = GameObject
            .Find("Canvas")
            .GetComponent<Jugador_2>()
            .rnd_Lider.Replace(' ', '_');
        Sprite spriteLider = Resources.Load<Sprite>("img/Diseno_de_cartas/lideres/" + lider);
        Sprite spriteLiderEnemigo = Resources.Load<Sprite>(
            "img/Diseno_de_cartas/lideres/" + lider_enemigo
        );

        imagen.sprite = spriteLider;

        imagen_2.sprite = spriteLiderEnemigo;
    }
}
