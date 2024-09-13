using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interprete : MonoBehaviour
{    
  
   

   public void Desglozando_lineas ()
   {
      string  Texto_ingresado= GameObject.Find("texto_codigo").GetComponent<TMP_InputField>().text;

       string[] lineas = Texto_ingresado.Split('\n');

       foreach( string line in lineas)
       {
         string trim_line = line.Trim();  //elimina espacios del principio y al final 

         if( !string.IsNullOrEmpty(trim_line) ) // sino es una linea con espacios en blanco 
         {
          
       ///     Analizador_lexico instancia_analizador_lexico = new Analizador_lexico(  Normalizador(trim_line));
       //    List<Token> palabras=  instancia_analizador_lexico.Analizador(instancia_analizador_lexico.script);
         //   for ( int i = 0 ; i<palabras.Count; i ++ )
            {
           //    Debug.Log(palabras[i].Value);
            }
         }
       }
   }
 public string Normalizador(string lineadcodigo)
 {
   string script = lineadcodigo.Trim();
   StringBuilder resultado = new StringBuilder(script);
    for(int i = 0 ; i <resultado.Length ; i++)
   {
     if(char.IsWhiteSpace(resultado[i]))
     {
       if ( i+1<resultado.Length && char.IsWhiteSpace(resultado[i+1]))
       {
         resultado.Remove(i,1);
        i--;
       }
     }
   }
   return resultado.ToString();
 }

  

}





// public class Analizador_lexico
// {
//    public string script;
//    private int index;

//    private const string numbers ="0123456789";
//    private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";


  // public Analizador_lexico ( string script)
  // {
  //   this.script = script;
  //   index = 0 ; 
  // }

  // public List<Token> Analizador(string script )
  // {
     

  //     this.script = script;
  //     this.index = 0;
  //     List<Token> tokens = new List<Token>();

      // while(index< script.Length)
      // {
      //    if(char.IsWhiteSpace(script[index]))
      //    {  
      //      Debug.Log("se detecto un espacio en blanco ");
      //       index++;
            
      //    }
        
      //     //comprueba si es un numero 
      //    else  if( char.IsDigit(script[index]))
      //    {
      //       int num_start = index;

      //       while ( index<script.Length && char.IsDigit(script[index])) // si es numero
      //       {
      //          index++;
      //       }
      //    //   tokens.Add(new Token(TokenType.NUMBER,script.Substring(num_start,index-num_start+1) )); // creando  number 
      //       Debug.Log("se guardo como numero "+script.Substring(num_start,index-num_start)+1);
      //    }

         // comprueba si es una palabra 

//          else if(char.IsLetter(script[index]))  // si es una letra
//          {       
//                int word_start = index;

//                while ( index+1< script.Length && char.IsLetter(script[index]) || char.IsDigit(script[index]))
//                 {
//                   index++;
//                 }
//              string word = script.Substring(word_start,index-(word_start+1));

//               //Verificar si es int 
//                 if(word.Equals("int")&& index+1<script.Length)
//                 {
//                    tokens.Add(new Token( TokenType.Type_int, word));
//                    Debug.Log("se guardo un int ");    
//                    index ++;  
//                 }
//                 if ( word.Equals("float"))
//                 {
//                   tokens.Add(new Token( TokenType.Type_float, word));
//                    Debug.Log("se guardo un float ");    
//                    index ++; 
//                 }
//                 if ( word.Equals("string"))
//                 {
//                   tokens.Add(new Token( TokenType.Type_string, word));
//                    Debug.Log("se guardo un string ");    
//                    index ++; 
//                 }

//                 else if (word.Equals("int")&& index+1> script.Length )
//                 {  //significa q solo quiere guardar el int 
//                   throw new Exception(" quieres guardar solo un tipo int , eso no es posible");
//                 }
//                   //verificar si se definio el tipo de la variable
              
//                else 
//                 { if(tokens.Count>0)
//                   { if( tokens[tokens.Count-1].Type==TokenType.Type_int || tokens[tokens.Count-1].Type==TokenType.Type_string||  tokens[tokens.Count-1].Type==TokenType.Type_char ||  tokens[tokens.Count-1].Type==TokenType.Type_float)
//                     {
//                       tokens.Add(new Token ( TokenType.VARIABLE , script.Substring(word_start,index-word_start+1))); // creando variable
//                       Debug.Log("se guardo la variable ");
//                       index ++;
//                     } 
//                     else
//                     {
//                      throw new Exception (" La variable:"+  script.Substring(word_start,index-word_start+1)+ " no se sabe de que  tipo es , defina el tipo antes de asignar el nombre a la variable ");
//                     }
//                   }
//                      else
//                     {
//                      throw new Exception (" La variable: "+  script.Substring(word_start,index-word_start+1)+ " no se sabe de que  tipo es , defina el tipo antes de asignar el nombre a la variable ");
//                     }
//                 }
             
//          }
//          else if ( "+-*/=".Contains(script[index].ToString()))
//          {
//            tokens.Add(new Token(TokenType.OPERATOR, script[index].ToString()));
//            index ++;
//          }
//          else if (script[index]== '{')
//         {
//            tokens.Add(new Token( TokenType.LEFT_BRACE , "{"));
//            index++;
//         }
//          else if ( script[index]== '}')
//          {
//            tokens.Add( new Token ( TokenType.RIGHT_BRACE, "}"));
//            index ++;
//          }
//         else if (script[index]== '(')
//         {
//            tokens.Add(new Token ( TokenType.LEFT_PAREN , "("));
//            index++;
//         }
//         else if(  script[index]== ')')
//         {
//           tokens.Add(new Token (TokenType.RIGHT_PAREN, ")"));
//           index++;
//         }
//         else if(script[index]== '|')
//         {
//            if( script[index+1]=='|')
//            {
//             tokens.Add(new Token(TokenType.OPERADOR_BOOL,"||"));
//             index+= 2 ;
//            }
//         }


//          else if(script[index]== ';')
//          {
//           tokens.Add(new Token( TokenType.KEYWORD, ";"));

//           index++;
//          }
//          else 
//            {
//              Debug.Log( "/////////////////////////////////////////////");
//              Debug.Log("Caracter desconocido:"+ script[index]);
//             throw new Exception($"Caracter desconocido: '{script[index]}'");
           
//            }
//       }
         
//            return tokens;
           
//   }
  
// }

