using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Mono.Data.Sqlite;
using TMPro;
using Unity.Loading;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public Card card;
    public Carta carta;

    public Carta_Unidad carta_Unidad;

    public CardDisplay(Carta C)
    {
        card = new Card();
        carta = C;
        CartaLoad(C);
    }

    // propiedades

    public string Name;

    public int Id;
    public string Tipo;
    public string Faction;

    public int salud;
    public int ataque;
    public string Efecto;
    public bool IsCard;

    public bool disponibilidad;

    //propiedades visuales
    public UnityEngine.UI.Image marco;
    public UnityEngine.UI.Image imagen_carta;
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI efecto;

    public void LoadCard()
    {
        marco.sprite = card.marco.sprite;
        imagen_carta = card.imagen_carta;
        nombre.text = card.Name;
        efecto = card.efecto;

        //componentes
        carta.Nombre = card.Name;
        carta.Tipo = card.Type;

        carta.Faction = card.Faction;
        carta.IsCard = card.IsCard;
        carta.disponibilidad = card.disponibilidad;

        // carta_Unidad.salud = card.Health;
        //carta_Unidad.ataque = card.Power;
    }

    public void Load_interface()
    {
        this.marco.sprite = Resources.Load<Sprite>("img/Diseno_de_cartas/cartas/" + Faction);
        this.imagen_carta.sprite = Resources.Load<Sprite>(
            "img/Diseno_de_cartas/lideres/" + Name.Replace(' ', '_')
        );
       this. nombre.text = Name;
        this.efecto.text = Efecto;
    }

    public void Load_interface(Carta carta)
    {
        marco.sprite = Resources.Load<Sprite>("img/Diseno_de_cartas/cartas/" + Faction);
        imagen_carta.sprite = Resources.Load<Sprite>(
            "img/Diseno_de_cartas/lideres/" + Name.Replace(' ', '_')
        );
        nombre.text = Name;
        efecto.text = Efecto;
    }

    public void CartaLoad(Carta c)
    {
        carta = c;
        carta.id =c.id;
        carta.Nombre = c.Nombre;
        carta.Efecto =c.Efecto;
        carta.Tipo = c.Tipo;
        carta.Faction = c.Faction;
        carta.disponibilidad =c.disponibilidad;
        carta.IsCard = c.IsCard; 



        Name = c.Nombre;
        Tipo = c.Tipo;
        Id = c.id;
        Faction = c.Faction;
        IsCard = c.IsCard;
        disponibilidad = c.disponibilidad;
        Efecto = c.Efecto;


        // de ser carta unidad la crea y le asigna sus valores propuestos en la base de datos

        if (c.Tipo == "1" || c.Tipo == "2" || c.Tipo == "3" || c.Tipo == "4")
        {
            Tuple<int, int> dupla = Conectar_base_ataque_salud(c.id);

            carta_Unidad = new Carta_Unidad(
                c.id,
                c.Nombre,
                c.Efecto,
                c.Tipo,
                c.Faction,
                c.disponibilidad,
                c.IsCard,
                dupla.Item1,
                dupla.Item2
            );
            ataque = dupla.Item1;
            salud = dupla.Item2;
        }
    }
   // public void ActualirVidaSalud()
    public void CartaDebug()
    {
        Debug.Log(
            $"carta : {carta.Nombre} faction : {carta.Faction} id {carta.id}  tipo : {carta.Tipo}  disponibilidad {carta.disponibilidad} salud {salud}, ataque {ataque}"
       
        );
    }

    public Tuple<int, int> Conectar_base_ataque_salud(int id_carta)
    {
        int fuerza = 0;
        int salud = 0;
        string q = "SElECT*FROM cards where id= '" + id_carta + "'";

        using (
            var connection = new SqliteConnection(
                GameObject.Find("Canvas").GetComponent<SQLiteDB>().dbName
            )
        )
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = q;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fuerza = reader.GetInt32(reader.GetOrdinal("attack"));
                        salud = reader.GetInt32(reader.GetOrdinal("health"));

                        // Debug.Log("nombre " + reader["name"] + " efecto: " + reader["effect"]);
                    }
                }
            }

            connection.Close();
        }

        return Tuple.Create(fuerza, salud);
    }

    // metodos para mover carta en el tablero


    public void Seleccion()
    {
        GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada = this.gameObject;
       // GameObject.Find("Tablero").GetComponent<Juego>().carta_seleccionada.GetComponent<CardDisplay>().CartaLoad(this.gameObject.GetComponent<CardDisplay>().carta);

        GameObject.Find("Tablero").GetComponent<Juego>().Colocar_tablero();

        //this.gameObject.SetActive(false);
        if (GameObject.Find("mazo").GetComponent<Mazo>().mano.Count > 0)
        {
            for (int i = 0; i < GameObject.Find("mazo").GetComponent<Mazo>().mano.Count; i++)
            {
                if (
                    GameObject.Find("mazo").GetComponent<Mazo>().mano[i].carta.Nombre
                    == GameObject
                        .Find("Tablero")
                        .GetComponent<Juego>()
                        .carta_seleccionada.GetComponent<CardDisplay>()
                        .carta.Nombre
                )
                    GameObject.Find("Tablero").GetComponent<Juego>().posicion_carta_mano = i;
            }
        }
    }
}
