using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoubleClickDetector : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private float intervalo = 2f;

    [SerializeField]
    private float intervalo_2 = 1f;
    private float entro_click = 0f;
    private int clickCount = 0;

    private void Start()
    {
        if (!button)
        {
            button = GetComponent<Button>();
        }

        if (button)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        clickCount++;

        if (clickCount == 1)
        {
            entro_click = Time.time;
            OnSingleClick();

            clickCount = 1;
        }
        if (clickCount == 2)
        {
            OnDoubleClick();
            Debug.Log("double click ");
            clickCount = 0;
        }
        else if (clickCount == 3)
        {
            if (Time.time - entro_click < intervalo_2)
            {
                OnTripleClick();
                Debug.Log("entro a tres cliks");
                clickCount = 0;
            }
        }

        // Reiniciamos el contador para el siguiente click
    }

    protected virtual void OnSingleClick()
    {
        Debug.Log("Single click ");
    }

    protected virtual void OnDoubleClick()
    {
        Debug.Log("Double click detected");
        GameObject.Find("Tablero").GetComponent<Ataques>().Posibles_ataques(gameObject);

        //es un tipo card
        if (gameObject.GetComponent<Carta>().IsCard == true)
        {
            GameObject
                .Find("mazo_card")
                .GetComponent<mazo_cards>()
                .ActivarEffect(gameObject.GetComponent<Carta>().id);
        }
    }

    protected virtual void OnTripleClick()
    {
        AudioSource audiocliclip = gameObject.GetComponent<AudioSource>();
        audiocliclip.Play();

        GameObject
            .Find("Tablero")
            .GetComponent<Ataques>()
            .fondos_vidas[gameObject.GetComponent<IsPlaying>().numero]
            .SetActive(true);

        GameObject
            .Find("salud_" + gameObject.GetComponent<IsPlaying>().numero)
            .GetComponent<TextMeshProUGUI>()
            .text = gameObject.GetComponent<Carta_Unidad>().salud.ToString();
        GameObject
            .Find("ataque_" + gameObject.GetComponent<IsPlaying>().numero)
            .GetComponent<TextMeshProUGUI>()
            .text = gameObject.GetComponent<Carta_Unidad>().ataque.ToString();
    }
}
