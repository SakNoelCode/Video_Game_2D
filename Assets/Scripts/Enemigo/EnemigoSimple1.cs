using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoSimple1 : MonoBehaviour
{
    [SerializeField] private Transform[] puntosMov;
    [SerializeField] private float velocidadEnemigo;
    [SerializeField] private GameObject goContEnemigo;
    [SerializeField] private GameObject parteCuerpo, parteCabeza;
    [SerializeField] private GameObject player;

    private PolygonCollider2D polyColCuerpo, polColCabeza;
    private int i = 0;
    private float velocidadNormal;

    //Variables para desvanecer al enemigo
    private SpriteRenderer sprCuerpo, sprCabeza;

    //Variables para dar movimiento al enemigo
    private Vector3 escalaIni,escalaTemp;
    private float direccion = 1;   //      1---Mira derecha     -1---Mira izquierda

    //private bool isActiva = true;

    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        escalaIni = transform.localScale;
        polyColCuerpo = parteCuerpo.GetComponent<PolygonCollider2D>();
        polColCabeza = parteCabeza.GetComponent<PolygonCollider2D>();
        sprCuerpo = parteCuerpo.GetComponent<SpriteRenderer>();
        sprCabeza = parteCabeza.GetComponent<SpriteRenderer>();
        velocidadNormal = velocidadEnemigo;
    }

    //------------------------------------------METODO UPDATE-----------------------------------
    void Update()
    {
        movimientoEnemigo();
    }

    //------------------------------------------METODO FIXED UPDATE-----------------------------------
    void FixedUpdate()
    {
        
        if (player != null)
        {
            float lado = Mathf.Sign(player.transform.position.x
            - transform.position.x);  // 1 o -1
                                      
            //Si nos ve el enemigo
            if ((Mathf.Abs(transform.position.x - player.transform.position.x) < 5) && (lado == direccion))
            {
                atacar();
            }
            else
            {
                patrulla();
            }

        }
        
    }

    private void patrulla()
    {
        parteCabeza.transform.rotation = Quaternion.Lerp(parteCabeza.transform.rotation,
            Quaternion.Euler(0, 0, 0),
            10 * Time.deltaTime);

        velocidadEnemigo = velocidadNormal;
    }

    private void atacar()
    {
        parteCabeza.transform.rotation = Quaternion.Lerp(parteCabeza.transform.rotation,
           Quaternion.Euler(0, 0, -45),
           10 * Time.deltaTime);

        velocidadEnemigo *= 1.08f;
    }

    private void movimientoEnemigo()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, 
            puntosMov[i].transform.position, 
            velocidadEnemigo * Time.deltaTime);

        if (Vector2.Distance(
            transform.position,
            puntosMov[i].transform.position) < 0.1f)
        {
            if (puntosMov[i] != puntosMov[puntosMov.Length - 1]) i++;
            else i = 0;

            //Obtener valor de dirección  
            direccion = Mathf.Sign(puntosMov[i].transform.position.x - transform.position.x);
            girarEnemigo(direccion);
        }

        

    }

    private void girarEnemigo(float direccion)
    {
        if (direccion == -1)
        {
            escalaTemp = transform.localScale;
            escalaTemp.x = escalaTemp.x * -1;
        }
        else escalaTemp = escalaIni;
        transform.localScale = escalaTemp;
    }

    public void Muere()
    {
        //isActiva = false;
        polyColCuerpo.enabled = false;
        polColCabeza.enabled = false;
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        for (float i = 1f; i >= 0; i -= 0.2f)
        {
            cambiarTransparencia(sprCuerpo, i);
            cambiarTransparencia(sprCabeza, i);
            yield return new WaitForSeconds(0.025f);
        }
        Destroy(goContEnemigo);
    }

    private void cambiarTransparencia(SpriteRenderer spr, float alfa)
    {
        Color color = spr.material.color;
        color.a = alfa;
        spr.material.color = color;
    }
}
 