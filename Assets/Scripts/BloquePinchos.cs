using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloquePinchos : MonoBehaviour
{
    [SerializeField] private Transform destino;
    [SerializeField] private float velocidad;
    [SerializeField] private float tiempoQuieto;

    private Vector3 posIni, posFin;
    private bool enMov;
    private float tiempo;

    void Start()
    {
        destino.parent = null; //el destino no depende de la plataforma
        posIni = transform.position;
        posFin = destino.position;

        enMov = true;
        tiempo = 0.0f;
    }


    void FixedUpdate()
    {
        if (enMov)
        {
            //Se encarga de mover el objeto
            transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

            if (transform.position == destino.position)
            {
                if (destino.position == posFin) destino.position = posIni;
                else destino.position = posFin;
                enMov = false;
            }
        }
        else
        {
            tiempo += Time.deltaTime;
            if(tiempo >= tiempoQuieto)
            { 
                tiempo = 0.0f;
                enMov = true;
            }
        }

    }
}
