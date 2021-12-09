using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos_Roca_Movil : MonoBehaviour
{
    [SerializeField] private Transform destino;
    [SerializeField] private float     velocidad;
    [SerializeField] private float     tiempoQuieto;

    private Vector3 posInicial, posFinal;

    //Variables para que el objeto se quede quieto al llegar a su destino
    private bool   inMovimiento;
    private float  tiempo;

    // --------------------------------------METODO START-------------------------------
    void Start()
    {
        destino.parent = null;  //El destino no depende de la plataforma
        posInicial = transform.position;
        posFinal = destino.position;

        //para validar movimiento
        inMovimiento = true;
        tiempo = 0.0f;
    }

    // --------------------------------------METODO FIXED UPDATE-------------------------------
    void FixedUpdate()
    {
        if (inMovimiento) moverPinchos();
        else retardoMovimientoPinchos();
        
    }

    private void moverPinchos()
    {
        //Ir al destino
        transform.position = Vector3.MoveTowards(transform.position, destino.position, velocidad * Time.deltaTime);

        //Regresar al origen
        if (transform.position == destino.position)
        {
            if (destino.position == posFinal) destino.position = posInicial;
            else destino.position = posFinal;

            inMovimiento = false;
        }

    }

    private void retardoMovimientoPinchos()
    {
        tiempo += Time.deltaTime; //Sumar el tiempo 
        if(tiempo >= tiempoQuieto)
        {
            tiempo = 0.0f;
            inMovimiento = true;
        }
    }
}
