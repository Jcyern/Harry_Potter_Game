using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Ataques : MonoBehaviour
{
    public List<GameObject> fondos_vidas;
    private GameObject Atacante;
    public List<int> ataque = new List<int>();
    private GameObject Atacado;
    public int puntos_ganados;
    bool gano_puntos;

    public int fuerza_atacante;
    public int contraataque_defensa;

    public int vida__atacado;
    public int contraataque_fuerza;
    bool salto_fila = true;

    public Dictionary<string, int> valores_salud_ataque = new Dictionary<string, int>()
    {
        { "ataque_j1", 0 },
        { "ataque_j2", 0 },
        { "salud_j1", 0 },
        { "salud_j2", 0 }
    };

    //0 ataque 1
    //1 salud 1
    //2 ataque 2
    //3 salud 2
    //true --gano el atacante
    //false gano el atacado con su contradefensa
    public void Posibles_ataques(GameObject gameObject)
    {
        gameObject.GetComponent<IsPlaying>().playing = true;
        gameObject.GetComponent<IsPlaying>().En_mesa = true;
        for (int i = 0; i < GameObject.Find("Tablero").GetComponent<Juego>().cruces.Count; i++)
        {
            if (GameObject.Find("Tablero").GetComponent<Juego>().cruces[i].activeSelf == true)
                GameObject.Find("Tablero").GetComponent<Juego>().cruces[i].SetActive(false);
        }

        if (GameObject.Find("cambio_de_turno").GetComponent<Cambio_Turno>().ronda > 1)
        {
            //Asociar_atacante(gameObject).GetComponent<Carta_Unidad>().ataque






            int cruz;

            if (
                gameObject.GetComponent<CardDisplay>().Tipo == "3"
                || gameObject.GetComponent<CardDisplay>().Tipo == "2"
                || gameObject.GetComponent<CardDisplay>().Tipo == "4"
            )
            {
                cruz = 12;

                for (int i = 2; i <= 4; i++)
                {
                    if (cruz < 15)
                    {
                        if (
                            GameObject
                                .Find("Tablero")
                                .GetComponent<Juego>()
                                .tablero_j2[2, i]
                                .GetComponent<CardDisplay>()
                                .Name != ""
                        )
                        {
                            GameObject
                                .Find("Tablero")
                                .GetComponent<Juego>()
                                .cruces[cruz]
                                .SetActive(true);

                            if (
                                gameObject.GetComponent<CardDisplay>().Tipo == "2"
                                || gameObject.GetComponent<CardDisplay>().Tipo == "4"
                            )
                            {
                                salto_fila = false;
                            }
                        }
                        cruz++;
                    }
                }
            }

            if (
                gameObject.GetComponent<CardDisplay>().Tipo == "2" && salto_fila == true
                || gameObject.GetComponent<CardDisplay>().Tipo == "4" && salto_fila == true
                || gameObject.GetComponent<CardDisplay>().Tipo == "3"
            )
            {
                cruz = 7;

                for (int i = 2; i <= 4; i++)
                {
                    if (cruz < 10)
                    {
                        if (
                            GameObject
                                .Find("Tablero")
                                .GetComponent<Juego>()
                                .tablero_j2[1, i]
                                .GetComponent<CardDisplay>()
                                .Name != ""
                        )
                        {
                            GameObject
                                .Find("Tablero")
                                .GetComponent<Juego>()
                                .cruces[cruz]
                                .SetActive(true);
                        }
                        cruz++;
                    }
                }
            }
        }
    }

    public void Atacar()
    {
        //Recorrer el tablero j1 en posicion espada y sommbrero  //Cogiendo Valores de Atacante
        for (int i = 0; i < 2; i++)
        {
            for (int j = 2; j <= 4; j++)
            {
                if (
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<IsPlaying>()
                        .playing == true
                )
                {
                    Debug.Log("entro a valores ataque ");
                    valores_salud_ataque["ataque_j1"] = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<CardDisplay>()
                        .ataque;
                    valores_salud_ataque["salud_j1"] = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<CardDisplay>()
                        .salud;
                    Atacante = GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i, j];
                    Atacante
                        .GetComponent<CardDisplay>()
                        .CartaLoad(
                            GameObject
                                .Find("Tablero")
                                .GetComponent<Juego>()
                                .tablero_j1[i, j]
                                .GetComponent<CardDisplay>()
                                .carta
                        );
                    Atacante.GetComponent<CardDisplay>().salud = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<CardDisplay>()
                        .salud;
                    Atacante.GetComponent<CardDisplay>().ataque = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<CardDisplay>()
                        .ataque;
                    Atacante.GetComponent<Carta>().Nombre = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<Carta>()
                        .Nombre;
                    Atacante.GetComponent<Carta>().Tipo = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<Carta>()
                        .Tipo;
                    Atacante.GetComponent<Carta>().Efecto = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<Carta>()
                        .Efecto;
                    Atacante.GetComponent<Carta>().marco = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<Carta>()
                        .marco;
                    Atacante.GetComponent<Carta>().Faction = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<Carta>()
                        .Faction;
                    Atacante.GetComponent<Carta>().imagen_carta = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<Carta>()
                        .imagen_carta;
                    Atacante.GetComponent<Carta>().disponibilidad = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<Carta>()
                        .disponibilidad;
                    Atacante.GetComponent<Carta>().id = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<Carta>()
                        .id;
                    Atacante.GetComponent<IsPlaying>().En_mesa = GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<IsPlaying>()
                        .En_mesa;
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j1[i, j]
                        .GetComponent<IsPlaying>()
                        .playing = false;
                }
                else
                {
                    Debug.Log("no hay valores para ataque");
                }
            }
        }

        valores_salud_ataque["ataque_j2"] = Cruces_Y_Posicion(
                gameObject.GetComponent<Carta>().Nombre
            )
            .GetComponent<CardDisplay>()
            .ataque;
        valores_salud_ataque["salud_j2"] = Cruces_Y_Posicion(
                gameObject.GetComponent<Carta>().Nombre
            )
            .GetComponent<CardDisplay>()
            .salud;

        //Vamos a asociar los valores


        Debug.Log(valores_salud_ataque["ataque_j1"]);
        Debug.Log(valores_salud_ataque["salud_j1"]);
        Debug.Log(valores_salud_ataque["ataque_j2"]);
        Debug.Log(valores_salud_ataque["salud_j2"]);

        gameObject.SetActive(false);

        // Recorre las cruces resantes para apagarlas
        for (int i = 0; i < GameObject.Find("Tablero").GetComponent<Juego>().cruces.Count; i++)
        {
            if (GameObject.Find("Tablero").GetComponent<Juego>().cruces[i].activeSelf == true)
            {
                GameObject.Find("Tablero").GetComponent<Juego>().cruces[i].SetActive(false);
            }
        }

        int danger = valores_salud_ataque["salud_j2"] - valores_salud_ataque["ataque_j1"];

        Debug.Log("el dano es " + danger);
        if (danger > 0) //sigue vivo quitale la vida
        {
            Atacado.GetComponent<CardDisplay>().salud = danger;
            Debug.Log("lavida actual es" + Atacado.GetComponent<CardDisplay>().salud);
            Contra_ataque();
        }
        else if (danger < 0 || danger == 0) //significa que murio
        {
            gano_puntos = true;
            puntos_ganados = danger * -1;
            int puntos_acumulados =
                Convert.ToInt32(GameObject.Find("pto_1").GetComponent<TextMeshProUGUI>().text)
                + puntos_ganados;
            GameObject.Find("pto_1").GetComponent<TextMeshProUGUI>().text =
                puntos_acumulados.ToString();
            Desincronizar_carta(true);
        }
    }

    private void Contra_ataque()
    {
        int dano_contrataque = valores_salud_ataque["salud_j1"] - valores_salud_ataque["ataque_j2"];

        Debug.Log("El dano contrataque es" + dano_contrataque);

        if (dano_contrataque > 0) //sigue vivo
        {
            Atacante.GetComponent<CardDisplay>().salud = dano_contrataque;
        }
        else if (dano_contrataque < 0 || dano_contrataque == 0) //murio
        {
            gano_puntos = false;

            int puntos_ganado_contra = dano_contrataque * -1;
            GameObject.Find("pto_2").GetComponent<TextMeshProUGUI>().text = (
                Convert.ToInt32(GameObject.Find("pto_2").GetComponent<TextMeshProUGUI>().text)
                + puntos_ganado_contra
            ).ToString();

            Desincronizar_carta(false);
        }
    }

    public void Desincronizar_carta(bool ganador)
    {
        if (ganador == true)
        {
            if (GameObject.Find("mazo").GetComponent<Cementerio>().cementerio_contrario.Count == 0)
            {
                GameObject
                    .Find("mazo")
                    .GetComponent<Cementerio>()
                    .Cementerio_oponente.SetActive(true);
            }

            GameObject.Find("mazo").GetComponent<Cementerio>().cementerio_contrario.Add(Atacado);
            //elimniando los hashset
            switch (Atacado.GetComponent<Muerte>().tipo)
            {
                case "espada":
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .generator_espada.Remove(
                            Atacado.GetComponent<Muerte>().hashset[
                                Atacado.GetComponent<Muerte>().tipo
                            ]
                        );
                    break;
                case "sombrero":
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .generator_sombrero.Remove(
                            Atacado.GetComponent<Muerte>().hashset[
                                Atacado.GetComponent<Muerte>().tipo
                            ]
                        );
                    break;
                case "caldero":
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .generator_caldero.Remove(
                            Atacado.GetComponent<Muerte>().hashset[
                                Atacado.GetComponent<Muerte>().tipo
                            ]
                        );
                    break;
                case "barita":
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .generator_barita.Remove(
                            Atacado.GetComponent<Muerte>().hashset[
                                Atacado.GetComponent<Muerte>().tipo
                            ]
                        );
                    break;
                case "escudo":
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .generator_escudo.Remove(
                            Atacado.GetComponent<Muerte>().hashset[
                                Atacado.GetComponent<Muerte>().tipo
                            ]
                        );
                    break;
                default:
                    throw new Exception($" no se recnoce {Atacado.GetComponent<Muerte>().tipo}");
            }

            Atacado.GetComponent<CardDisplay>().Name = "";
            Atacado.GetComponent<CardDisplay>().Tipo = "";
            Atacado.GetComponent<CardDisplay>().Efecto = "";
            Atacado.GetComponent<CardDisplay>().marco.sprite = Resources.Load<Sprite>(
                "img/Diseno_de_cartas/cartas/" + "0"
            );
            Atacado.GetComponent<CardDisplay>().Faction = "";
            Atacado.GetComponent<CardDisplay>().imagen_carta.sprite = Resources.Load<Sprite>(
                "img/Diseno_de_cartas/lideres/" + "None"
            );
            Atacado.GetComponent<CardDisplay>().disponibilidad = false;
            Atacado.GetComponent<CardDisplay>().ataque = 0;
            Atacado.GetComponent<CardDisplay>().salud = 0;
            Atacado.GetComponent<CardDisplay>().Id = 0;
            Atacado.GetComponent<CardDisplay>().carta.Nombre = "";
            Atacado.GetComponent<CardDisplay>().carta.Tipo = "";
            Atacado.GetComponent<CardDisplay>().carta.id = 0;
            Atacado.GetComponent<CardDisplay>().carta.Faction = "";
            Atacado.GetComponent<CardDisplay>().carta.disponibilidad = false;
            Atacado.GetComponent<CardDisplay>().carta.IsCard = false;

            for (
                int i = 0;
                i < GameObject.Find("Tablero").GetComponent<Juego>().tablero_j2.GetLength(0);
                i++
            )
            {
                for (
                    int j = 0;
                    j < GameObject.Find("Tablero").GetComponent<Juego>().tablero_j2.GetLength(1);
                    j++
                )
                {
                    if (
                        GameObject
                            .Find("Tablero")
                            .GetComponent<Juego>()
                            .tablero_j2[i, j]
                            .GetComponent<Carta>()
                            .Nombre == ""
                    )
                    {
                        GameObject
                            .Find("Tablero")
                            .GetComponent<Juego>()
                            .tablero_j2[i, j]
                            .SetActive(false);
                    }
                }
            }
        }
        else if (ganador == false) //oponente gano , e muere el j1
        {
            if (GameObject.Find("mazo").GetComponent<Cementerio>().cementerio.Count == 0)
            {
                GameObject.Find("mazo").GetComponent<Cementerio>().Cementer.SetActive(true);
            }
            GameObject.Find("mazo").GetComponent<Cementerio>().cementerio.Add(Atacante);

            Atacante.GetComponent<CardDisplay>().Name = "";
            Atacante.GetComponent<CardDisplay>().Tipo = "";
            Atacante.GetComponent<CardDisplay>().Efecto = "";
            Atacante.GetComponent<CardDisplay>().marco.sprite = Resources.Load<Sprite>(
                "img/Diseno_de_cartas/cartas/" + "0"
            );
            Atacante.GetComponent<CardDisplay>().Faction = "";
            Atacante.GetComponent<CardDisplay>().imagen_carta.sprite = Resources.Load<Sprite>(
                "img/Diseno_de_cartas/lideres/" + "None"
            );
            Atacante.GetComponent<CardDisplay>().disponibilidad = false;
            Atacante.GetComponent<CardDisplay>().ataque = 0;
            Atacante.GetComponent<CardDisplay>().salud = 0;
            Atacante.GetComponent<CardDisplay>().Id = 0;
            Atacante.GetComponent<CardDisplay>().carta.Nombre = "";
            Atacante.GetComponent<CardDisplay>().carta.Tipo = "";
            Atacante.GetComponent<CardDisplay>().carta.Faction = "";
            Atacante.GetComponent<CardDisplay>().carta.disponibilidad = false;
            Atacante.GetComponent<CardDisplay>().carta.IsCard = false;
            Atacante.GetComponent<CardDisplay>().carta.id = 0;

            Atacante.GetComponent<IsPlaying>().En_mesa = false;

            for (
                int i = 0;
                i < GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1.GetLength(0);
                i++
            )
            {
                for (
                    int j = 0;
                    j < GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1.GetLength(1);
                    j++
                )
                {
                    if (
                        GameObject
                            .Find("Tablero")
                            .GetComponent<Juego>()
                            .tablero_j1[i, j]
                            .GetComponent<Carta>()
                            .Nombre == ""
                    )
                    {
                        GameObject
                            .Find("Tablero")
                            .GetComponent<Juego>()
                            .tablero_j1[i, j]
                            .SetActive(false);
                    }
                }
            }
        }
    }

    public GameObject Cruces_Y_Posicion(string nombre)
    {
        int i;
        int j;
        string numero = nombre;

        if (numero == "1")
        {
            i = 0;
            j = 0;
        }
        else if (numero == "2")
        {
            i = 0;
            j = 1;
        }
        else if (numero == "3")
        {
            i = 0;
            j = 2;
        }
        else if (numero == "4")
        {
            i = 0;
            j = 3;
        }
        else if (numero == "5")
        {
            i = 0;
            j = 4;
        }
        else if (numero == "6")
        {
            i = 1;
            j = 0;
        }
        else if (numero == "7")
        {
            i = 1;
            j = 1;
        }
        else if (numero == "8")
        {
            i = 1;
            j = 2;
        }
        else if (numero == "9")
        {
            i = 1;
            j = 3;
        }
        else if (numero == "10")
        {
            i = 1;
            j = 4;
        }
        else if (numero == "11")
        {
            i = 2;
            j = 0;
        }
        else if (numero == "12")
        {
            i = 2;
            j = 1;
        }
        else if (numero == "13")
        {
            i = 2;
            j = 2;
        }
        else if (numero == "14")
        {
            i = 2;
            j = 3;
        }
        else
        {
            i = 2;
            j = 4;
        }

        Atacado = GameObject.Find("Tablero").GetComponent<Juego>().tablero_j2[i, j];
        Atacado
            .GetComponent<CardDisplay>()
            .CartaLoad(
                GameObject
                    .Find("Tablero")
                    .GetComponent<Juego>()
                    .tablero_j2[i, j]
                    .GetComponent<CardDisplay>()
                    .carta
            );
        Atacado.GetComponent<Carta_Unidad>().salud = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta_Unidad>()
            .salud;
        Atacado.GetComponent<Carta_Unidad>().ataque = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta_Unidad>()
            .ataque;
        Atacado.GetComponent<Carta>().id = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .id;
        Atacado.GetComponent<Carta>().Nombre = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .Nombre;
        Atacado.GetComponent<Carta>().Efecto = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .Efecto;
        Atacado.GetComponent<Carta>().Faction = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .Faction;
        Atacado.GetComponent<Carta>().marco = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .marco;
        Atacado.GetComponent<Carta>().imagen_carta = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .imagen_carta;
        Atacado.GetComponent<Carta>().nombre = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .nombre;
        Atacado.GetComponent<Carta>().efecto = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .efecto;
        Atacado.GetComponent<Carta>().disponibilidad = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .disponibilidad;
        Atacado.GetComponent<Carta>().Tipo = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Carta>()
            .Tipo;
        Atacado.GetComponent<Muerte>().tipo = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Muerte>()
            .tipo;
        Atacado.GetComponent<Muerte>().hashset[Atacado.GetComponent<Muerte>().tipo] = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Muerte>()
            .hashset[Atacado.GetComponent<Muerte>().tipo];
        Atacado.GetComponent<Muerte>().IsDeath = GameObject
            .Find("Tablero")
            .GetComponent<Juego>()
            .tablero_j2[i, j]
            .GetComponent<Muerte>()
            .IsDeath;

        return Atacado;
    }
}
