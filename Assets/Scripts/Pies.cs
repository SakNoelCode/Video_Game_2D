using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pies : MonoBehaviour
{
    //Variable statica para que pueda ser compartida por otros Scripts
    public static bool colPies;

    //Método de Unity para trabajar con Triggers, lo usamos para saber que objeto esta colisionando con nuestro 
    //Objeto Pies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag== "Terreno")//Si pies esta colisionando con Terreno
        {
            colPies = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Terreno")//Si pies no esta colisionando con Terreno
        {
            colPies = false;
        }
    }


}
