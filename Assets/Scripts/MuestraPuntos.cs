using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Clase para dibujar esferas mediante un origen y un destino

public class MuestraPuntos : MonoBehaviour
{
    [SerializeField] private Transform origen;
    [SerializeField] private Transform destino;

    private void OnDrawGizmosSelected() //Dibujar Gizmos
    {
        Gizmos.color = Color.cyan; //color del Gizmo
        Gizmos.DrawSphere(origen.position, 0.1f); //Dibujar esfera en origen
        Gizmos.DrawSphere(destino.position, 0.1f); //Dibujar esfera en destino
    }
}
 