using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject fondoLejosGO;
    [SerializeField] private GameObject fondoCercaGO;
    [SerializeField] private float velocidadScroll;

    private Renderer fondoLejosR, fondoCercaR;
    private float posCamIniX, difCamIniX;

    //Variables para obtener las posiciones minimas y maximas de la camara
    public Vector2 minCamaraPos, maxCamaraPos;

    //Variable que almacena el objeto que se desea seguir: en nuestro caso: Player
    public GameObject seguir;

    public float movSuave;
     
    private Vector2 velocidad;

    void Start()
    {
        fondoLejosR = fondoLejosGO.GetComponent<Renderer>();
        fondoCercaR = fondoCercaGO.GetComponent<Renderer>();
        posCamIniX = transform.position.x;
    }

    
    void Update()
    {
        //Mover el fondo a través de la propiedad OffSet
        difCamIniX = posCamIniX - transform.position.x;
        fondoLejosR.material.mainTextureOffset = new Vector2(difCamIniX * velocidadScroll * -1, 0.0f);
        fondoCercaR.material.mainTextureOffset = new Vector2(difCamIniX * (velocidadScroll * 1.5f) * -1, 0.0f);

        fondoLejosGO.transform.position = new Vector3(transform.position.x,fondoLejosGO.transform.position.y,fondoLejosGO.transform.position.z);
        fondoCercaGO.transform.position = new Vector3(transform.position.x,fondoCercaGO.transform.position.y,fondoCercaGO.transform.position.z);
            

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
