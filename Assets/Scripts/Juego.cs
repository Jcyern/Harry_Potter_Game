using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Juego : MonoBehaviour
{
    bool stop;
    Cementerio cementery = new Cementerio();
    public GameObject carta_seleccionada;
    public GameObject carta_seleccionada_2  ;
    public GameObject[,] tablero_j1 = new GameObject[3, 5];
    public GameObject[,] tablero_j2 = new GameObject[3, 5];
    public GameObject[,] TABLERO;
    public int posicion_carta_mano;
    public GameObject cementerio;
    GameObject tablero;
    public GameObject plaza_escogida;
    public List<GameObject> cruces;
    public HashSet<int> generator_espada = new HashSet<int>();
    public HashSet<int> generator_sombrero = new HashSet<int>();
    public HashSet<int> generator_caldero = new HashSet<int>();
    public HashSet<int> generator_barita = new HashSet<int>();
    public HashSet<int> generator_escudo = new HashSet<int>();

    public List<GameObject> posiciones_tablero_j2;

    public List<GameObject> posiciones_tablero_j1;

    public List<GameObject> posiciones_sombrero_espada;
    private int rango = 0;

    void Start()
    {
        //tablero j1

        //fila 1
        tablero_j1[0, 0] = GameObject.Find("hechizo_3");
        tablero_j1[0, 1] = GameObject.Find("escudo_3");
        tablero_j1[0, 2] = GameObject.Find("espada_1");
        tablero_j1[0, 3] = GameObject.Find("espada_2");
        tablero_j1[0, 4] = GameObject.Find("espada_3");

        //fila 2
        tablero_j1[1, 0] = GameObject.Find("hechizo_2");
        tablero_j1[1, 1] = GameObject.Find("escudo_2");
        tablero_j1[1, 2] = GameObject.Find("sombrero_1");
        tablero_j1[1, 3] = GameObject.Find("sombrero_2");
        tablero_j1[1, 4] = GameObject.Find("sombrero_3");

        //fila 3
        tablero_j1[2, 0] = GameObject.Find("hechizo_1");
        tablero_j1[2, 1] = GameObject.Find("escudo_1");
        tablero_j1[2, 2] = GameObject.Find("caldero_1");
        tablero_j1[2, 3] = GameObject.Find("caldero_2");
        tablero_j1[2, 4] = GameObject.Find("caldero_3");

        //Tablero del Jugador 2

        //fila 1
        tablero_j2[0, 0] = GameObject.Find("hechizo_1_cpu");
        tablero_j2[0, 1] = GameObject.Find("escudo_1_cpu");
        tablero_j2[0, 2] = GameObject.Find("caldero_1_cpu");
        tablero_j2[0, 3] = GameObject.Find("caldero_2_cpu");
        tablero_j2[0, 4] = GameObject.Find("caldero_3_cpu");

        //fila 2
        tablero_j2[1, 0] = GameObject.Find("hechizo_2_cpu");
        tablero_j2[1, 1] = GameObject.Find("escudo_2_cpu");
        tablero_j2[1, 2] = GameObject.Find("sombrero_1_cpu");
        tablero_j2[1, 3] = GameObject.Find("sombrero_2_cpu");
        tablero_j2[1, 4] = GameObject.Find("sombrero_3_cpu");

        //fila 3
        tablero_j2[2, 0] = GameObject.Find("hechizo_3_cpu");
        tablero_j2[2, 1] = GameObject.Find("escudo_3_cpu");
        tablero_j2[2, 2] = GameObject.Find("espada_1_cpu");
        tablero_j2[2, 3] = GameObject.Find("espada_2_cpu");
        tablero_j2[2, 4] = GameObject.Find("espada_3_cpu");

        //Ocultar tablero j1
        for (int i = 0; i < tablero_j1.GetLength(0); i++)
        {
            for (int j = 0; j < tablero_j1.GetLength(1); j++)
            {
                tablero_j1[i, j].SetActive(false);
                tablero_j2[i, j].SetActive(false);
            }
        }

        for (int i = 1; i <= 2; i++)
        {
            for (int j = 2; j <= 4; j++)
            {
                posiciones_tablero_j2.Add(tablero_j2[i, j]);
            }
        }

        for (int i = 0; i <= 1; i++)
        {
            for (int j = 2; j <= 4; j++)
            {
                posiciones_tablero_j1.Add(tablero_j2[i, j]);
            }
        }

        GameObject.Find("numero_ronda").GetComponent<TextMeshProUGUI>().text = GameObject
            .Find("cambio_de_turno")
            .GetComponent<Cambio_Turno>()
            .ronda.ToString();
    }

    public void Colocar_tablero()
    {
        if (GameObject.Find("cambio_de_turno").GetComponent<Cambio_Turno>().turno == true)
        {
            for (int i = 0; i < tablero_j1.GetLength(0); i++)
            {
                for (int j = 0; j < tablero_j1.GetLength(1); j++)
                {
                    if (tablero_j1[i, j].GetComponent<CardDisplay>().Name == "")
                        tablero_j1[i, j].SetActive(false);
                }
            }

            //criatura ---espada
            if (carta_seleccionada.GetComponent<CardDisplay>().Tipo == "2")
            {
                tablero_j1[0, 2].SetActive(true);
                tablero_j1[0, 3].SetActive(true);
                tablero_j1[0, 4].SetActive(true);
            }
            //mago ---sombrero
            if (carta_seleccionada.GetComponent<CardDisplay>().Tipo == "3")
            {
                tablero_j1[1, 2].SetActive(true);
                tablero_j1[1, 3].SetActive(true);
                tablero_j1[1, 4].SetActive(true);
            }

            //ser---espada
            if (carta_seleccionada.GetComponent<CardDisplay>().Tipo == "4")
            {
                tablero_j1[0, 2].SetActive(true);
                tablero_j1[0, 3].SetActive(true);
                tablero_j1[0, 4].SetActive(true);
            }

            //objeto---caldero

            if (carta_seleccionada.GetComponent<CardDisplay>().Tipo == "5")
            {
                tablero_j1[2, 2].SetActive(true);
                tablero_j1[2, 3].SetActive(true);
                tablero_j1[2, 4].SetActive(true);
            }

            //hechizo--- barita
            if (carta_seleccionada.GetComponent<CardDisplay>().Tipo == "6")
            {
                tablero_j1[0, 0].SetActive(true);
                tablero_j1[1, 0].SetActive(true);
                tablero_j1[2, 0].SetActive(true);
            }

            //pocion---caldero
            if (carta_seleccionada.GetComponent<CardDisplay>().Tipo == "7")
            {
                tablero_j1[2, 2].SetActive(true);
                tablero_j1[2, 3].SetActive(true);
                tablero_j1[2, 4].SetActive(true);
            }

            //lugar
            if (carta_seleccionada.GetComponent<CardDisplay>().Tipo == "8")
            {
                tablero_j1[0, 1].SetActive(true);
                tablero_j1[1, 1].SetActive(true);
                tablero_j1[2, 1].SetActive(true);
            }
        }
    }






    private int GenerarNumero(int min, int max_excluido, HashSet<int> Not_dupla) //imposible que dupla pase .Count mayorque 3 por Verificacion ()
    {
        int numero;

        do
        {
            numero = Random.Range(min, max_excluido);
        } while (Not_dupla.Contains(numero));

        Not_dupla.Add(numero);

        return numero;
    }

    public bool Verificacion(GameObject card)
    {
        int repeticiones = 0;

        // Debug.Log("el tipo de la crad selec "+card.GetComponent<Carta>().Tipo );



        // Heroe puuede estar tanto en espada como sombrero
        if (card.GetComponent<CardDisplay>().Tipo == "1")
        { //recorre primero sombrero
            for (int q = 2; q <= 4; q++)
            {
                if (
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j2[1, q]
                        .GetComponent<CardDisplay>()
                        .Name != ""
                )
                {
                    repeticiones++;
                }
            }

            //si todas las casillas de sombrero estan ocupada
            //recorre casillas de espada
            if (repeticiones == 3)
            {
                repeticiones = 0;
                for (int q = 2; q <= 4; q++)
                {
                    if (
                        GameObject
                            .Find("Tablero")
                            .GetComponent<Juego>()
                            .tablero_j2[2, q]
                            .GetComponent<CardDisplay>()
                            .Name != ""
                    )
                    {
                        repeticiones++;
                    }
                }
            }
        }
        // Criatura  ||  Ser --- espada
        else if (card.GetComponent<CardDisplay>().Tipo == "2" || card.GetComponent<CardDisplay>().Tipo == "4")
        {
            for (int q = 2; q <= 4; q++)
            {
                if (
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j2[2, q]
                        .GetComponent<CardDisplay>()
                        .Name != ""
                )
                {
                    repeticiones++;
                }
            }
        }
        // Mago ---- Sombrero
        else if (card.GetComponent<CardDisplay>().Tipo == "3")
        {
            for (int q = 2; q <= 4; q++)
            {
                if (
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j2[1, q]
                        .GetComponent<CardDisplay>()
                        .Name!= ""
                )
                {
                    repeticiones++;
                }
            }
        }
        //  Objeto o pocion ---- caldero
        else if (card.GetComponent<CardDisplay>().Tipo == "5" || card.GetComponent<CardDisplay>().Tipo == "7")
        {
            for (int q = 2; q <= 4; q++)
            {
                if (
                    GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .tablero_j2[0, q]
                        .GetComponent<CardDisplay>()
                        .Name!= ""
                )
                {
                    repeticiones++;
                }
            }
        }
        //Hechizo ---- Barita
        else if (card.GetComponent<CardDisplay>().Tipo == "6")
        {
            for (int q = 0; q <= 2; q++)
            {
                if (tablero_j2[q, 0].GetComponent<CardDisplay>().Name != "")
                {
                    repeticiones++;
                }
            }
        }
        //Lugar ----- Escudo
        else if (card.GetComponent<CardDisplay>().Tipo == "8")
        {
            for (int q = 0; q <= 2; q++)
            {
                if (tablero_j2[q, 1].GetComponent<CardDisplay>().Name!= "")
                {
                    repeticiones++;
                }
            }
        }
        else
        {
            throw new System.Exception(
                $" NO se reconoce el tipo : {card.GetComponent<Carta>().Tipo}"
            );
        }

        // Debug.Log("la cant es "+repeticiones);
        if (repeticiones < 3)
            return true;
        else
            return false;
    }

    public void Colocar_tablero_j2()
    {
        rango = 0;

        if (Verificacion(GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada_2))
        { //Tablero J2
            //criatura ---espada   and ser---espada  //heroe
            if (
                carta_seleccionada_2.GetComponent<CardDisplay>().Tipo == "2"
                || carta_seleccionada_2.GetComponent<CardDisplay>().Tipo == "4"
                || carta_seleccionada_2.GetComponent<CardDisplay>().Tipo == "1"
            )
            {
                //tablero_j2[2, 2]
                //tablero_j2[2, 3]
                //tablero_j2[2, 4]

                rango = GenerarNumero(2, 5, generator_espada);

                // Debug.Log("else   "+rango);
                plaza_escogida = tablero_j2[2, rango];
                tablero_j2[2, rango].SetActive(true);
                //para cuando se desincronice la carta
                tablero_j2[2, rango].AddComponent<Muerte>();
                tablero_j2[2, rango].GetComponent<Muerte>().hashset["espada"] = rango;
                tablero_j2[2, rango].GetComponent<Muerte>().tipo = "espada";
                //asociando los gameobject

                tablero_j2[2, rango].GetComponent<CardDisplay>().CartaLoad(carta_seleccionada_2.GetComponent<CardDisplay>().carta);
                tablero_j2[2, rango].GetComponent<CardDisplay>().Name = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Name;
                tablero_j2[2, rango].GetComponent<CardDisplay>().Tipo = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Tipo;
                tablero_j2[2, rango].GetComponent<CardDisplay>().Id = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Id;
                tablero_j2[2, rango].GetComponent<CardDisplay>().Faction = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Faction;
                tablero_j2[2, rango].GetComponent<CardDisplay>().disponibilidad = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .disponibilidad;
                tablero_j2[2, rango].GetComponent<CardDisplay>().Load_interface();

                if (tablero_j2[2, rango].GetComponent<Carta_Unidad>() == null)
                {
                    tablero_j2[2, rango].AddComponent<Carta_Unidad>();
                }
                tablero_j2[2, rango].GetComponent<Carta_Unidad>().salud = carta_seleccionada_2
                    .GetComponent<CardDisplay>().salud
                    ;
                tablero_j2[2, rango].GetComponent<Carta_Unidad>().ataque = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .ataque;
            }
            //mago ---sombrero  //heroe
            else if (
                carta_seleccionada_2.GetComponent<CardDisplay>().Tipo == "3"
                || carta_seleccionada_2.GetComponent<CardDisplay>().Tipo == "1"
            )
            {
                //tablero_j1[1, 2].
                //tablero_j1[1, 3].SetActive(true);
                //tablero_j1[1, 4].SetActive(true);

                rango = GenerarNumero(2, 5, generator_sombrero);

                // Debug.Log("else   "+rango);
                plaza_escogida = tablero_j2[1, rango];
                 tablero_j2[1, rango].GetComponent<CardDisplay>().CartaLoad(carta_seleccionada_2.GetComponent<CardDisplay>().carta);

                tablero_j2[1, rango].AddComponent<Muerte>();
                tablero_j2[1, rango].GetComponent<Muerte>().hashset["sombrero"] = rango;
                tablero_j2[1, rango].GetComponent<Muerte>().tipo = "sombrero";

                tablero_j2[1, rango].SetActive(true);
                tablero_j2[1, rango].GetComponent<CardDisplay>().Name = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Name;
                tablero_j2[1, rango].GetComponent<CardDisplay>().Tipo = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Tipo;
                tablero_j2[1, rango].GetComponent<CardDisplay>().Id = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Id;
                tablero_j2[1, rango].GetComponent<CardDisplay>().Faction = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Faction;
                tablero_j2[1, rango].GetComponent<CardDisplay>().disponibilidad = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .disponibilidad;
                tablero_j2[1, rango].GetComponent<CardDisplay>().Load_interface();
               
                tablero_j2[1, rango].AddComponent<Carta_Unidad>();
               
                tablero_j2[1, rango].GetComponent<Carta_Unidad>().salud = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .salud;
                tablero_j2[1, rango].GetComponent<Carta_Unidad>().ataque = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .ataque;
            }
            //objeto---caldero and pocion --- caldero
            else if (
                carta_seleccionada_2.GetComponent<CardDisplay>().Tipo == "5"
                || carta_seleccionada_2.GetComponent<CardDisplay>().Tipo == "7"
                
            )
            {
                //tablero_j2[0, 2]
                //tablero_j2[0, 3]
                //tablero_j2[0, 4]

                rango = GenerarNumero(2, 5, generator_caldero);

                // Debug.Log("else   "+rango);
                plaza_escogida = tablero_j2[0, rango];
                  tablero_j2[0, rango].GetComponent<CardDisplay>().CartaLoad(carta_seleccionada_2.GetComponent<CardDisplay>().carta);

                tablero_j2[0, rango].AddComponent<Muerte>();
                tablero_j2[0, rango].GetComponent<Muerte>().tipo = "caldero";
                tablero_j2[0, rango].GetComponent<Muerte>().hashset["calero"] = rango;

                tablero_j2[0, rango].SetActive(true);
                tablero_j2[0, rango].GetComponent<CardDisplay>().Name = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Name;
                tablero_j2[0, rango].GetComponent<CardDisplay>().Tipo = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Tipo;
                tablero_j2[0, rango].GetComponent<CardDisplay>().Id = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Id;
                tablero_j2[0, rango].GetComponent<CardDisplay>().Faction = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Faction;
                tablero_j2[0, rango].GetComponent<CardDisplay>().disponibilidad = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .disponibilidad;
                tablero_j2[0, rango].GetComponent<CardDisplay>().Load_interface();
            }
            // hechizo--- barita
            else if (carta_seleccionada_2.GetComponent<CardDisplay>().Tipo == "6")
            {
                //tablero_j2[0, 0]
                //tablero_j2[1, 0]
                //tablero_j2[2, 0]

                rango = GenerarNumero(0, 3, generator_barita);

                // Debug.Log("else   "+rango);
                plaza_escogida = tablero_j2[rango, 0];

               tablero_j2[rango, 0] .GetComponent<CardDisplay>().CartaLoad(carta_seleccionada_2.GetComponent<CardDisplay>().carta);
                tablero_j2[rango, 0].AddComponent<Muerte>();
                tablero_j2[rango, 0].GetComponent<Muerte>().tipo = "barita";
                tablero_j2[rango, 0].GetComponent<Muerte>().hashset["barita"] = rango;

                tablero_j2[rango, 0].SetActive(true);

                tablero_j2[rango, 0].GetComponent<CardDisplay>().Name = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Name;
                tablero_j2[rango, 0].GetComponent<CardDisplay>().Tipo = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Tipo;
                tablero_j2[rango, 0].GetComponent<CardDisplay>().Id = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Id;
                tablero_j2[rango, 0].GetComponent<CardDisplay>().Faction = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Faction;
                tablero_j2[rango, 0].GetComponent<CardDisplay>().disponibilidad = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .disponibilidad;
                tablero_j2[rango, 0].GetComponent<CardDisplay>().Load_interface();
            }
            //lugar--escudo
            else if (carta_seleccionada_2.GetComponent<CardDisplay>().Tipo == "8")
            {
                //tablero_j1[0, 1]
                //tablero_j1[1, 1]
                //tablero_j1[2, 1]

                rango = GenerarNumero(0, 3, generator_escudo);

                // Debug.Log("else   "+rango);
                plaza_escogida = tablero_j2[rango, 1];
                  
                tablero_j2[rango, 1].AddComponent<Muerte>();
                tablero_j2[rango, 1].GetComponent<Muerte>().tipo = "escudo";
                tablero_j2[rango, 1].GetComponent<Muerte>().hashset["escudo"] = rango;

                tablero_j2[rango, 1].SetActive(true);
                tablero_j2[rango, 1] .GetComponent<CardDisplay>().CartaLoad(carta_seleccionada_2.GetComponent<CardDisplay>().carta);
                tablero_j2[rango, 1].GetComponent<CardDisplay>().Name = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Name;
                tablero_j2[rango, 1].GetComponent<CardDisplay>().Tipo = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Tipo;
                tablero_j2[rango, 1].GetComponent<CardDisplay>().Id = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Id;
                tablero_j2[rango, 1].GetComponent<CardDisplay>().Faction = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .Faction;
                tablero_j2[rango, 1].GetComponent<CardDisplay>().disponibilidad = carta_seleccionada_2
                    .GetComponent<CardDisplay>()
                    .disponibilidad;
                tablero_j2[rango, 1].GetComponent<CardDisplay>().Load_interface();
            }
        }
        else if (!Verificacion(carta_seleccionada_2))
            plaza_escogida = null;
        else
        {
            throw new System.Exception(" error en la card");
        }
    }
}
