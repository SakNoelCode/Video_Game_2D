using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script encargado de dibujar Gizmos en los objetos 

public class RecorridoPlataforma : MonoBehaviour
{

    [SerializeField] private Transform desde;
    [SerializeField] private Transform hasta;

    //Dibuja Gizmos al objeto seleccionado
    private void OnDrawGizmosSelected()
    {  
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(desde.position, hasta.position);

        //Dibujar esferas en ambas posiciones
        Gizmos.DrawSphere(desde.position, 0.1f);
        Gizmos.DrawSphere(hasta.position, 0.1f);
    }
}
