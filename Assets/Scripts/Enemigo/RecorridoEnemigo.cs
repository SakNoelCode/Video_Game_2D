using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorridoEnemigo : MonoBehaviour
{
    [SerializeField] private Transform origen;
    [SerializeField] private Transform destino;


    //Mostrar Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
       
        Gizmos.DrawSphere(origen.position, 0.1f);   //Dibujar una esfera
        Gizmos.DrawSphere(destino.position, 0.1f);  //Dibujar una esfera
    }
}
 