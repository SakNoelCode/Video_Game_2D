using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSkaheCamera : MonoBehaviour
{
    //Script que controlla el screen skake de la camará, para un efecto de caída del player

    [Header("ScreenSkake")]
    [SerializeField] private float cantidadRotacion;
    [SerializeField] private float cantidadFuerza;

    public static ScreenSkaheCamera instance; //Intancia estática, para poder usar la clase en cualquier script

    private float tiempoRestante;
    private float fuerzaShake;
    private float tiempo;
    private float rotacion;

    private Vector3 posInicialCamara;
    private bool isSkake;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        isSkake = false;
    }

    void LateUpdate()
    {
        if (isSkake)
        {
            if (tiempoRestante > 0f)
            {
                tiempoRestante -= Time.deltaTime;
                float cantidadX = posInicialCamara.x + Random.Range(-cantidadFuerza, cantidadFuerza) * fuerzaShake;
                float cantidadY = posInicialCamara.y + Random.Range(-cantidadFuerza, cantidadFuerza) * fuerzaShake;
                cantidadX = Mathf.MoveTowards(cantidadX, posInicialCamara.x, tiempo * Time.deltaTime);
                cantidadY = Mathf.MoveTowards(cantidadY, posInicialCamara.x, tiempo * Time.deltaTime);
                transform.position = new Vector3(cantidadX, cantidadY, posInicialCamara.z);

                rotacion = Mathf.MoveTowards(rotacion,0f,tiempo*cantidadRotacion*Time.deltaTime);
                transform.rotation = Quaternion.Euler(0F,0F,rotacion * Random.Range(-1f,1f));
            }
            else
            {
                transform.position = posInicialCamara;
                isSkake = false;
            } 
        }
    }

    public void StarShake(float duracion, float fuerza)
    {
        posInicialCamara = transform.position;
        isSkake = true;
        tiempoRestante = duracion;
        fuerzaShake = fuerza;
        tiempo = fuerza / duracion;
        rotacion = fuerza * cantidadRotacion;
    }
}
