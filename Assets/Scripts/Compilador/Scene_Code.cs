using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene_Code : MonoBehaviour
{
    public void ActivarAreaText()
    {
        //actulizar el context




        GameObject.Find("Canvas").GetComponent<Manager>().SceneCodificar.SetActive(true);



     //actualizar los contextos del tablero , decks , hands , board ,etc
        GameObject.Find("Canvas").GetComponent<Manager>().contexto_game = GameObject
            .Find("Canvas")
            .GetComponent<Game_contex>()
            .GenerarGameContext();

        

        GameObject.Find("Canvas").GetComponent<Manager>().Tablero.SetActive(false);
    }
}
