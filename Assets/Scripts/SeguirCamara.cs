using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirCamara : MonoBehaviour
{
    //Variables para obtener las posiciones minimas y maximas de la camara
    public Vector2 minCamaraPos, maxCamaraPos;

    //Variable que almacena el objeto que se desea seguir: en nuestro caso: Player
    public GameObject seguir;

    public float movSuave;

    private Vector2 velocidad;

    void Start()
    {
        
    }

    
    void Update()
    {
        //Obtener los valores del objeto Player
        float posX = Mathf.SmoothDamp(transform.position.x,seguir.transform.position.x,ref velocidad.x, movSuave);
        float posY = Mathf.SmoothDamp(transform.position.y,seguir.transform.position.y, ref velocidad.y, movSuave);

        //Mover la camara según se mueva Player
        transform.position = new Vector3(
            Mathf.Clamp( posX,minCamaraPos.x,maxCamaraPos.x), 
            Mathf.Clamp( posY,minCamaraPos.y,maxCamaraPos.y),  
            transform.position.z);
        
    }
}
