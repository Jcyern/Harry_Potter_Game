using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.ShortcutManagement;
using TMPro;


public class Atajos : MonoBehaviour
{
   
  
     private void Update()
    { if(GameObject.Find("Tablero")!= null )
       { if (Input.GetKeyDown(KeyCode.L) )
        {
            ExecuteCustomAction();
        }
        if(Input.GetKeyDown(KeyCode.K))
        {
            Desactivar();
        }
       }
    }

    private void ExecuteCustomAction()
    {
        Debug.Log(" L ejecutado");
     
           GameObject.Find("Tablero").GetComponent<AudioSource>().Play();
           for( int i =0 ; i<2 ; i++)
           {
            for( int j = 2; j<=4;j++)
            {
               if( GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<IsPlaying>().En_mesa == true)
                {
                    if( i== 0 && j==2)
                    {
                        GameObject.Find("Tablero").GetComponent<Ataques>().fondos_vidas[0].SetActive(true);
                        GameObject.Find("salud_"+0).GetComponent<TextMeshProUGUI>().text=  GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().salud.ToString();
                        GameObject.Find("ataque_"+0).GetComponent<TextMeshProUGUI>().text=  GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().ataque.ToString();
                    }
                     else  if(i==0 && j==3)
                      {  
                        GameObject.Find("Tablero").GetComponent<Ataques>().fondos_vidas[1].SetActive(true);
                        GameObject.Find("salud_"+1).GetComponent<TextMeshProUGUI>().text=  GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().salud.ToString();
                        GameObject.Find("ataque_"+1).GetComponent<TextMeshProUGUI>().text= GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().ataque.ToString();
                      }
                     else  if(i==0 && j==4)
                       {
                         GameObject.Find("Tablero").GetComponent<Ataques>().fondos_vidas[2].SetActive(true);
                        GameObject.Find("salud_"+2).GetComponent<TextMeshProUGUI>().text=  GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().salud.ToString();
                        GameObject.Find("ataque_"+2).GetComponent<TextMeshProUGUI>().text= GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().ataque.ToString();
                       }
                       else  if(i==1 && j==2)
                       {
                         GameObject.Find("Tablero").GetComponent<Ataques>().fondos_vidas[3].SetActive(true);
                        GameObject.Find("salud_"+3).GetComponent<TextMeshProUGUI>().text=  GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().salud.ToString();
                        GameObject.Find("ataque_"+3).GetComponent<TextMeshProUGUI>().text=  GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().ataque.ToString();
                       }
                       else  if(i==1 && j==3)
                       {
                         GameObject.Find("Tablero").GetComponent<Ataques>().fondos_vidas[4].SetActive(true);
                         GameObject.Find("salud_"+4).GetComponent<TextMeshProUGUI>().text= GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().salud.ToString();
                         GameObject.Find("ataque_"+4).GetComponent<TextMeshProUGUI>().text=  GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().ataque.ToString();
                       }
                       else  if(i==1 && j==4)
                       {
                         GameObject.Find("Tablero").GetComponent<Ataques>().fondos_vidas[5].SetActive(true);
                         GameObject.Find("salud_"+5).GetComponent<TextMeshProUGUI>().text=  GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().salud.ToString();
                         GameObject.Find("ataque_"+5).GetComponent<TextMeshProUGUI>().text=  GameObject.Find("Tablero").GetComponent<Juego>().tablero_j1[i,j].GetComponent<CardDisplay>().ataque.ToString();
                       }
                      
                }
                    
            }
                
           }
        
       
     
    }
    private void  Desactivar ()
    {     Debug.Log(" K ejecutado");
         for ( int i= 0; i< GameObject.Find("Tablero").GetComponent<Ataques>().fondos_vidas.Count; i++)
            {
                GameObject.Find("Tablero").GetComponent<Ataques>().fondos_vidas[i].SetActive(false);
            }

    }
}
