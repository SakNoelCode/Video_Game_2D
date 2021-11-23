using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [SerializeField] private Transform destino;
    [SerializeField] private float velocidad;

    private Vector3 posIni, posFin;

    void Start()
    {
        destino.parent = null; //el destino no depende de la plataforma
        posIni = transform.position;
        posFin = destino.position;
    }
     
    
    void FixedUpdate()
    {
        //Se encarga de mover el objeto
        transform.position = Vector3.MoveTowards(transform.position,destino.position,velocidad * Time.deltaTime); 

        if(transform.position == destino.position)
        {
            if (destino.position == posFin) destino.position = posIni;
            else destino.position = posFin;
        }
    }
} 
