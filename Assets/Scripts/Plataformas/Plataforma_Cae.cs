using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma_Cae : MonoBehaviour
{
    [SerializeField] private float tiempoEspera;
    [SerializeField] private float tiempoReaparicion;
    [SerializeField] private GameObject sprite1;
    [SerializeField] private GameObject sprite2;
    [SerializeField] private float margenMovimiento;

    private Rigidbody2D rigibodyPlataforma;
    private Vector3 posInicialPlataforma;

    //Variables para los sprites
    private SpriteRenderer spr1, spr2;

    //Variables para el movimiento al romperse
    private bool isMoviendose;
    private float valorMovimiento = 0.025f;

    // ------------------------------------METODO START-------------------------------
    void Start()
    {
        rigibodyPlataforma = GetComponent<Rigidbody2D>();
        posInicialPlataforma = transform.position;
        spr1 = sprite1.GetComponent<SpriteRenderer>();
        spr2 = sprite2.GetComponent<SpriteRenderer>();
    }

    // ------------------------------------METODO FIXED UPDATE-------------------------------
    void FixedUpdate()
    {
        movimientoCaidaPlataforma();
    }

    private void movimientoCaidaPlataforma()
    {
        if (isMoviendose)
        {
            transform.position = new Vector3(
                transform.position.x + valorMovimiento, 
                transform.position.y, 
                transform.position.z);

            if( (transform.position.x >= posInicialPlataforma.x + margenMovimiento) || 
                (transform.position.x <= posInicialPlataforma.x - margenMovimiento))
            {
                valorMovimiento *= -1;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Invoke("Cae", tiempoEspera);
            Invoke("Reaparece", tiempoReaparicion);
            isMoviendose = true;
        }
    }

    private void Cae()
    {
        rigibodyPlataforma.isKinematic = false;
    }

    private void Reaparece()
    {
        isMoviendose = false;
        rigibodyPlataforma.velocity = Vector3.zero;
        rigibodyPlataforma.isKinematic = true;
        transform.position = posInicialPlataforma;

        //Hacer invisble a los Sprites
        cambiarTransparencia(spr1, 0);
        cambiarTransparencia(spr2, 0);

        //Hacer visibles a los Sprites
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        for(float i=0f; i <= 1; i+= 0.2f)
        {
            cambiarTransparencia(spr1, i);
            cambiarTransparencia(spr2, i);
            yield return new WaitForSeconds(0.025f);
            //Debug.Log("I: " + i);
        }
    }

    private void cambiarTransparencia(SpriteRenderer spr, float alfa)
    {
        Color color = spr.material.color;
        color.a = alfa;
        spr.material.color = color;
    }
}
