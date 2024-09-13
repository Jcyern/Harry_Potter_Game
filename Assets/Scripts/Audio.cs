using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource harrypotter_sound;

    void Start()
    {
        harrypotter_sound.loop = true; //repitelo en bucle
        harrypotter_sound.Play();
    }
}
