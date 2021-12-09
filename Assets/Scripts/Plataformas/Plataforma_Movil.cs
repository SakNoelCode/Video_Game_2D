using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma_Movil : MonoBehaviour
{
    [SerializeField] private Transform destino;
    [SerializeField] private float velocidad;

    private Vector3 posInicial, posFinal;

    // --------------------------------------METODO START-------------------------------
    void Start()
    {
        destino.parent = null;  //El destino no depende de la plataforma
        posInicial = transform.position;
        posFinal = destino.position;
    }

    // --------------------------------------METODO UPDATE-------------------------------
    void Update()
    {
        moverPlataforma();
    }

    private void moverPlataforma()
    {
        //Ir al destino
        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

        //Regresar al origen
        if (transform.position == destino.position)
        {
            if (destino.position == posFinal) destino.position = posInicial;
            else destino.position = posFinal;
        }

    }
}
