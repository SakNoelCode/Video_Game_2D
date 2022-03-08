using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartel : MonoBehaviour
{
 

    [SerializeField] private GameObject canvasQuiz;
    [SerializeField] private GameObject cartel;


    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            //inPausa = true;
            Time.timeScale = 0; //Paralizar el tiempo
            GameController.PausarGame();
            //StartCoroutine("FadeIn");
            canvasQuiz.SetActive(true);
            cartel.SetActive(false);

        }
    }

    private void VolverGame()
    {
        
        Time.timeScale = 1; //Paralizar el tiempo
        GameController.ReanudarGame();
        canvasQuiz.SetActive(false);
        


        //quiz.SetActive(true);
        //inPausa = false;
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //StartCoroutine("FadeOut");
            inPausa = false;
        }
    }*/


    /*private void cambiarTransparenciaMensaje(float alfa)
    {
        Color colorSpriteMensaje = sprMensaje.material.color;
        colorSpriteMensaje.a = alfa;
        sprMensaje.material.color = colorSpriteMensaje;
    }

    IEnumerator FadeIn()
    {
        for (float i = 0.0f; i <= 1; i += 0.02f)
        {
            cambiarTransparenciaMensaje(i);
            yield return (0.05f);
        }
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= 0.02f)
        {
            cambiarTransparenciaMensaje(i);
            yield return (0.05f);
        }
    }*/
}
