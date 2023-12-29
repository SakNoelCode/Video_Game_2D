using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarMensaje2 : MonoBehaviour
{
    [SerializeField] private GameObject mensaje;
    [SerializeField] private GameObject SndCofreAbriendose;

    private SpriteRenderer sprMensaje;
    private AudioSource snd_cofre;
    private Animator aniPlayer;

    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        sprMensaje = mensaje.GetComponent<SpriteRenderer>();
        snd_cofre = SndCofreAbriendose.GetComponent<AudioSource>();
        aniPlayer = GetComponent<Animator>();
        cambiarTransparenciaMensaje(0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            aniPlayer.SetBool("IsCerca",true);
            snd_cofre.Play();
            mensaje.SetActive(true);
            StartCoroutine("FadeIn");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            aniPlayer.SetBool("IsCerca", false);
            StartCoroutine("FadeOut");
            mensaje.SetActive(false);
        }
    }


    private void cambiarTransparenciaMensaje(float alfa)
    {
        Color colorSpriteMensaje = sprMensaje.material.color;
        colorSpriteMensaje.a = alfa;
        sprMensaje.material.color = colorSpriteMensaje;
    }

    IEnumerator FadeIn()
    {
        for(float i=0.0f; i <= 1; i += 0.02f)
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
    }
}
