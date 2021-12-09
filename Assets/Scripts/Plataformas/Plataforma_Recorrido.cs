using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma_Recorrido : MonoBehaviour
{
    [SerializeField] private Transform origen;
    [SerializeField] private Transform destino;

    //Mostrar Gizmos en los objetos seleccionados
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origen.position, destino.position); //Dibujar una línea

        Gizmos.DrawSphere(origen.position, 0.1f);   //Dibujar una esfera
        Gizmos.DrawSphere(destino.position, 0.1f);  //Dibujar una esfera
    }
}
