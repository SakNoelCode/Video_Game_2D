using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    [SerializeField] private Vector2    posMinCamara, posMaxCamara;
    [SerializeField] private GameObject objetoASeguir;
    [SerializeField] private float      tiempoRetardoCamara;

    //Variables Fondo
    [SerializeField] private GameObject fondoLejosGO;
    [SerializeField] private GameObject fondoMedioGO;
    [SerializeField] private float velocidadScrollFondo;

    private Renderer fondoLejosR, fondoMedioR;
    private float iniCamX, difCamx;


    private Vector2 velocidad;

    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        fondoLejosR = fondoLejosGO.GetComponent<Renderer>();
        fondoMedioR = fondoMedioGO.GetComponent<Renderer>();
        iniCamX     = transform.position.x;
    }

    //------------------------------------------METODO UPDATE-----------------------------------
    void Update()
    {
        moverFondo();
        SeguirCamera();
                
    }

    private void moverFondo()
    {
        difCamx = iniCamX - transform.position.x;  //Obtener la diferencia

        //Crear valores offset para los fondos
        Vector2 offsetFondoLejos = new Vector2(difCamx * velocidadScrollFondo * -1, 0.0f);
        Vector2 offsetFondoMedio = new Vector2(difCamx * (velocidadScrollFondo*3f) * -1, 0.0f);

        //Asignar estos material offset para los Renderer
        fondoLejosR.material.mainTextureOffset = offsetFondoLejos;
        fondoMedioR.material.mainTextureOffset = offsetFondoMedio;

        //Mover los Quads con la camara
        fondoLejosGO.transform.position = new Vector3(transform.position.x,
                                                      transform.position.y,
                                                      fondoLejosGO.transform.position.z);

        fondoMedioGO.transform.position = new Vector3(transform.position.x,
                                                      transform.position.y,
                                                      fondoMedioGO.transform.position.z);
    }
     
    private void SeguirCamera()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, objetoASeguir.transform.position.x, ref velocidad.x, tiempoRetardoCamara);
        float posY = Mathf.SmoothDamp(transform.position.y, objetoASeguir.transform.position.y, ref velocidad.y, tiempoRetardoCamara);

        transform.position = new Vector3(
            Mathf.Clamp(posX, posMinCamara.x, posMaxCamara.x),
            Mathf.Clamp(posY, posMinCamara.y, posMaxCamara.y),
            transform.position.z);
    }
}
