using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using JetBrains.Annotations;
using Mono.Data.Sqlite;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SQLiteDB : MonoBehaviour
{
    public static SQLiteDB instance;
    public  string dbName = "URI=file:DataBase.db";

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CreateTable();

        //Query("SELECT * FROM user;");

        
    // Intenta seleccionar una fila usando las columnas 'salud' y 'ataque'.
    // Si las columnas no existen, SQLite lanzará un error.
    try
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {     //busca en la taba cards una fila (cuantificador E) que cumpla con la condicion de tener la columna ataque , y la columna salud 
                command.CommandText = "SELECT*FROM cards  WHERE health IS  NULL AND attack IS  NULL;";
                command.ExecuteNonQuery();
            }
        }
    }
    catch ( Exception ex)
    {   Debug.Log("Error al ejecutar la consulta: " + ex.Message);

        // Si llegamos aquí, significa que las columnas no existen.
        // Ahora se puede  proceder a agregarlas.
        AddColumnsToCardsTable();
    }
}
    

    private void CreateTable()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                string sqlcreation = "";

                sqlcreation += "CREATE TABLE IF NOT EXISTS cards(";
                sqlcreation += "id INTEGER NOT NULL ";          
                sqlcreation += "PRIMARY KEY AUTOINCREMENT,";
                sqlcreation += "name     VARCHAR(50) NOT NULL,";
                sqlcreation += "tipo    VARCHAR(50)NOT NULL,";
                sqlcreation += "effect VARCHAR(50) NOT NULL,";
                sqlcreation += "faction VARCHAR(50) NOT NULL";
                
                sqlcreation += ");";

                command.CommandText = sqlcreation;
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

private void AddColumnsToCardsTable()
{
    using (var connection = new SqliteConnection(dbName))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {    //ingresa en la tabla card las columnas con valor determinado 0 
            command.CommandText = "ALTER TABLE cards ADD COLUMN health INTEGER NOT NULL DEFAULT 0;";
            command.ExecuteNonQuery();

            command.CommandText = "ALTER TABLE cards ADD COLUMN attack INTEGER NOT NULL DEFAULT 0;";
            command.ExecuteNonQuery();
        }
        connection.Close();
    }
}

    public void Query(string q)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = q;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //  Debug.Log("name: " + reader["name"] + " password: " + reader["password"]);
                    }
                }
            }

            connection.Close();
        }
    }

    public void Crear_Carta()
    {
        string nombre = GameObject.Find("texto_nombre").GetComponent<TextMeshProUGUI>().text;
        string tipo = GameObject.Find("texto_tipo").GetComponent<TextMeshProUGUI>().text;
        string faccion = GameObject.Find("texto_faccion").GetComponent<TextMeshProUGUI>().text;
        string efecto = GameObject.Find("texto_efecto").GetComponent<TextMeshProUGUI>().text;
       
       
        string salud= GameObject.Find("texto_salud").GetComponent<TextMeshProUGUI>().text.Replace(" ","");

        
        string ataque= GameObject.Find("texto_ataque").GetComponent<TextMeshProUGUI>().text.Replace(" ","");
        

        Query(
            "INSERT INTO cards (name, tipo, effect, faction, health, attack) VALUES ('"
                + nombre
                + "' , '"
                + tipo
                + "', '"
                + efecto
                + "', '"
                + faccion
                + "', '"
                + salud
                + "', '"
                + ataque
                + "') "
        );
    }

    public Lider[] Obtener_lideres(int casa_select)
    {
        Lider[] lideres = new Lider[3];
        int pos = 0;

        string q = "SElECT*FROM cards where tipo= 0 AND faction= '" + casa_select + "'";
       // Debug.Log(q);
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = q;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read() )
                    {   
                          
                        lideres[pos]=  new Lider(reader["name"].ToString(),reader["effect"].ToString() );                        

                       // Debug.Log("nombre " + reader["name"] + " efecto: " + reader["effect"]);
                        pos += 1;
                    }
                }
            }

            connection.Close();
            return lideres;
        }
    }
     public List<Carta> Obtener_cartas (int casa_select)
        {  
            List<Carta> cartas = new List<Carta>();
            
                string q = "SElECT*FROM cards where tipo != 0 AND (faction= " + casa_select + " OR faction = 0)";
        //Debug.Log(q);
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = q;
                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read() )
                    {   // es necesarion hacer esto pq carta hereda de ModoBehabior , y por ende no se puede intanciar usando new, por reglas de Unity 
                       
                       
                     
                        cartas.Add(new Carta (reader.GetInt32(reader.GetOrdinal("id")),reader["name"].ToString(),reader["effect"].ToString(),reader["tipo"].ToString(),reader["faction"].ToString(),true) );
    


                        //  Debug.Log("nombre " + reader["name"] + " efecto: " + reader["effect"]);

                    }
                }
            }

            connection.Close();
          return cartas;
        }
        }


}
