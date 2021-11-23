using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarMensaje : MonoBehaviour
{

    [SerializeField] private GameObject mensaje;

    private SpriteRenderer spr;

    void Start()
    {
        //Obtener el Sprite Renderer del objeto
        spr = mensaje.GetComponent<SpriteRenderer>();

        ajustarMensaje(spr);
    }

    //Función encargada de detectar colisiones de Entrada
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine("FadeIn");
        }
    }

    //Función encargada de detectar colisiones de Salida
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine("FadeOut");
        }
    }

    private void ajustarMensaje(SpriteRenderer spr)
    {        
        Color c = spr.material.color; //Obtener el color 
        c.a = 0f;                     //Transparencia
        spr.material.color = c;       //Asignar el nuevo valor al spr
    }


    //Animación para mostrar el mensaje
    IEnumerator FadeIn()
    {
        for(float f = 0.0f; f <= 1; f+=0.02f) 
        {
            Color c = spr.material.color; //Obtener el color 
            c.a = f;                      //Transparencia
            spr.material.color = c;       //Asignar el nuevo valor al spr
            yield return (0.05f);
        }
    }


    //Animación para ocultar el mensaje
    IEnumerator FadeOut()
    {
        for (float f = 1f; f >= 0; f -= 0.01f)
        {
            Color c = spr.material.color; //Obtener el color 
            c.a = f;                      //Transparencia
            spr.material.color = c;       //Asignar el nuevo valor al spr
            yield return (0.05f); 
        }
    }


}
