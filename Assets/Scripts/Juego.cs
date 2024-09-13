
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Juego : MonoBehaviour
{   bool stop ;
    public GameObject carta_seleccionada;
    public GameObject[,] tablero_j1 = new GameObject[3, 5];
    public GameObject[,] tablero_j2 = new GameObject[3, 5];
    public GameObject[,] TABLERO;
    public int posicion_carta_mano;
    public GameObject cementerio;
    GameObject tablero;
    public GameObject plaza_escogida;
    public List <GameObject> cruces ;
    private HashSet<int>  generator_espada =new HashSet<int>();
    private HashSet<int>  generator_sombrero =new HashSet<int>();
    private HashSet<int>  generator_caldero =new HashSet<int>();
    private HashSet<int>  generator_barita =new HashSet<int>();
    private HashSet<int>  generator_escudo =new HashSet<int>();
    
    public List<GameObject> posiciones_tablero_j2 ;
    
    public List<GameObject> posiciones_tablero_j1;

    public List<GameObject> posiciones_sombrero_espada;
    private int rango ;
    
    
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
     //Ocultar tablero j2
        // for (int i = 0; i < tablero_j2.GetLength(0); i++)
        // {
        //     for (int j = 0; j < tablero_j2.GetLength(1); j++)
        //     {
               
        //     }
        // }

        //Agregando los  Gameobject a la lista
      for(int i= 1; i<=2; i++)
      {
        for(int j=2 ; j<=4 ; j++)
        {
           posiciones_tablero_j2.Add(tablero_j2[i,j]);
        }
      }

       for(int i= 0; i<=1; i++)
      {
        for(int j=2 ; j<=4 ; j++)
        {
           posiciones_tablero_j1.Add(tablero_j2[i,j]);
        }
      }

      GameObject.Find("numero_ronda").GetComponent<TextMeshProUGUI>().text =  GameObject.Find("cambio_de_turno").GetComponent<Cambio_Turno>().ronda.ToString();
    }

    public void Colocar_tablero()
    { 
       if(GameObject.Find("cambio_de_turno").GetComponent<Cambio_Turno>().turno==true )
       { for (int i = 0; i < tablero_j1.GetLength(0); i++)
        {
            for (int j = 0; j < tablero_j1.GetLength(1); j++)
            {
                if (tablero_j1[i, j].GetComponent<Carta>().Nombre == "")
                    tablero_j1[i, j].SetActive(false);
            }
        }

        //criatura ---espada
        if (carta_seleccionada.GetComponent<Carta>().Tipo == "2")
        {
            tablero_j1[0, 2].SetActive(true);
            tablero_j1[0, 3].SetActive(true);
            tablero_j1[0, 4].SetActive(true);
        }
        //mago ---sombrero
        if (carta_seleccionada.GetComponent<Carta>().Tipo == "3")
        {
            tablero_j1[1, 2].SetActive(true);
            tablero_j1[1, 3].SetActive(true);
            tablero_j1[1, 4].SetActive(true);
        }

        //ser---espada
        if (carta_seleccionada.GetComponent<Carta>().Tipo == "4")
        {
            tablero_j1[0, 2].SetActive(true);
            tablero_j1[0, 3].SetActive(true);
            tablero_j1[0, 4].SetActive(true);
        }

        //objeto---caldero

        if (carta_seleccionada.GetComponent<Carta>().Tipo == "5")
        {
            tablero_j1[2, 2].SetActive(true);
            tablero_j1[2, 3].SetActive(true);
            tablero_j1[2, 4].SetActive(true);
        }

        //hechizo--- barita
        if (carta_seleccionada.GetComponent<Carta>().Tipo == "6")
        {
            tablero_j1[0, 0].SetActive(true);
            tablero_j1[1, 0].SetActive(true);
            tablero_j1[2, 0].SetActive(true);
        }

        //pocion---caldero
        if (carta_seleccionada.GetComponent<Carta>().Tipo == "7")
        {
            tablero_j1[2, 2].SetActive(true);
            tablero_j1[2, 3].SetActive(true);
            tablero_j1[2, 4].SetActive(true);
        }

        //lugar
        if (carta_seleccionada.GetComponent<Carta>().Tipo == "8")
        {
            tablero_j1[0, 1].SetActive(true);
            tablero_j1[1, 1].SetActive(true);
            tablero_j1[2, 1].SetActive(true);
        }


       
       }

       
    }
   

    private int GenerarNumero(int min, int max_excluido, HashSet<int> Not_dupla)//imposible que dupla pase .Count mayorque 3 por Verificacion ()
    {
        int numero;
        
        do
        {
            numero =  Random.Range(min,max_excluido);

        } while (Not_dupla.Contains(numero) );
        
         Not_dupla.Add(numero);
       
         return numero;
        
     
        
    }
    

   public bool Verificacion( GameObject card )
    {  int repeticiones= 0;
      
      
         // Debug.Log("el tipo de la crad selec "+card.GetComponent<Carta>().Tipo );
           
      if(card.GetComponent<Carta>().Tipo=="2"|| card.GetComponent<Carta>().Tipo=="4")
      {   
        for(int q = 2; q <=4 ;q++)
        { 
          if( GameObject.Find("Tablero").GetComponent<Juego>().tablero_j2[2,q].GetComponent<Carta>().Nombre != "")
           {
              repeticiones++; 
           }
        }    
      }
      if(card.GetComponent<Carta>().Tipo=="5" ||card.GetComponent<Carta>().Tipo=="7" )
      {       
         for(int q = 2; q<=4;q++)
         { 
           if(tablero_j2[0,q].GetComponent<Carta>().Nombre!= "" )
           {
               repeticiones++;
           }
         }
              
      }
      else 
      {
         for( int k = 0; k< tablero_j2.GetLength(0);k++)
          { 
            for(int q = 0; q<tablero_j2.GetLength(1);q++)
            { 
              if(tablero_j2[k,q].GetComponent<Carta>().Tipo == card.GetComponent<Carta>().Tipo )
              repeticiones++;
            }
          }

      }
      

      // Debug.Log("la cant es "+repeticiones);
         if(repeticiones<3)
         return true;
      
         return false ;
       
    }
  
   
   
   public void  Colocar_tablero_j2(GameObject carta_selec)
   { 
     rango = 0;

     if(Verificacion(carta_selec))
     {   //Tablero J2
      
       //criatura ---espada   and ser---espada
       if (carta_selec.GetComponent<Carta>().Tipo == "2"||carta_selec.GetComponent<Carta>().Tipo == "4"||carta_selec.GetComponent<Carta>().Tipo== "1")
      {   
          
           //tablero_j2[2, 2]
           //tablero_j2[2, 3]
           //tablero_j2[2, 4] 
        
        rango=GenerarNumero(2,5,generator_espada);
         
         // Debug.Log("else   "+rango);
         plaza_escogida =  tablero_j2[2,rango];

      }
       //mago ---sombrero
       if (carta_selec.GetComponent<Carta>().Tipo == "3"|| carta_selec.GetComponent<Carta>().Tipo == "4"||carta_selec.GetComponent<Carta>().Tipo== "1")
        { 
          //tablero_j1[1, 2].
          //tablero_j1[1, 3].SetActive(true);
          //tablero_j1[1, 4].SetActive(true);
 
          rango=GenerarNumero(2,5,generator_sombrero);
            
          // Debug.Log("else   "+rango);
          plaza_escogida =  tablero_j2[1,rango];
                
           
        }    
       //objeto---caldero and pocion --- caldero
       if (carta_selec.GetComponent<Carta>().Tipo == "5"|| carta_selec.GetComponent<Carta>().Tipo == "7"||carta_selec.GetComponent<Carta>().Tipo== "1")
         {
            //tablero_j2[0, 2]
            //tablero_j2[0, 3]
            //tablero_j2[0, 4]
              
              rango=GenerarNumero(2,5,generator_caldero);
             
               // Debug.Log("else   "+rango);
              plaza_escogida =  tablero_j2[0,rango];
                
         }
       // hechizo--- barita
       if (carta_selec.GetComponent<Carta>().Tipo == "6")
        {
            //tablero_j2[0, 0]
            //tablero_j2[1, 0]
            //tablero_j2[2, 0]
           
            rango=GenerarNumero(0,3,generator_barita);
                
            // Debug.Log("else   "+rango);
            plaza_escogida =  tablero_j2[rango,0];
        
        }
       //lugar--escudo
       if (carta_selec.GetComponent<Carta>().Tipo == "8")
            {
              //tablero_j1[0, 1]
              //tablero_j1[1, 1]
              //tablero_j1[2, 1]
            
              rango=GenerarNumero(0,3,generator_escudo);
              
               // Debug.Log("else   "+rango);
               plaza_escogida =  tablero_j2[rango,1]; 
            
             }
      
       
      }
     
     if(Verificacion(carta_selec)== false )
      plaza_escogida =null;

   }
  
      
            
  

   

}
