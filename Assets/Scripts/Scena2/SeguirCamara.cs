using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirCamara : MonoBehaviour
{
    [SerializeField] private Vector2    posMinCamara, posMaxCamara;
    [SerializeField] private GameObject objetoASeguir;
    [SerializeField] private float      tiempoRetardoCamara;

    private Vector2 velocidad;

    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        
    }

    //------------------------------------------METODO UPDATE-----------------------------------
    void Update()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, objetoASeguir.transform.position.x, ref velocidad.x, tiempoRetardoCamara);
        float posY = Mathf.SmoothDamp(transform.position.y, objetoASeguir.transform.position.y, ref velocidad.y, tiempoRetardoCamara);

        transform.position = new Vector3(
            Mathf.Clamp(posX, posMinCamara.x, posMaxCamara.x),
            Mathf.Clamp(posY, posMinCamara.y, posMaxCamara.y),
            transform.position.z);
    }
}
