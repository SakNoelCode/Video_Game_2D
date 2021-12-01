using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Opciones Generales")]
    [SerializeField] float tiempoCambiarOpcion;
    [SerializeField] GameObject pantallaMenu;
    [SerializeField] GameObject pantallaOpciones;

    [Header("Elementos del Menú")]
    [SerializeField] SpriteRenderer comenzar;  //Opcion 1
    [SerializeField] SpriteRenderer opciones;  //Opcion 2
    [SerializeField] SpriteRenderer salir;     //Opcion 3

    [Header("Sprites del Menú")]
    [SerializeField] Sprite comenzarOff;   
    [SerializeField] Sprite comenzarOn;
    [SerializeField] Sprite opcionesOff;
    [SerializeField] Sprite opcionesOn;
    [SerializeField] Sprite salirOff;
    [SerializeField] Sprite salirOn;

    [Header("Sonidos del Menú")]
    [SerializeField] AudioSource snd_opcion;
    [SerializeField] AudioSource snd_seleccion;

    int pantalla;  //Pantalla Menú =0 o Pantalla Opciones=1
    int opcionMenu, opcionMenuAnt;//Encender o Apagar Opciones
    bool isPulsadoEnter; //Saber si se pulso enter
    float v, h; //Vertical y horizontal(Desplazamiento)
    float tiempoV, tiempoH;

     
    void Awake()
    {
        pantalla = 0;
        tiempoV = tiempoH = 0;
        opcionMenu = opcionMenuAnt = 1;
        ajustarOpciones();

    }

    void ajustarOpciones()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Submit")) isPulsadoEnter = false;
        if (v == 0) tiempoV = 0;
        if (pantalla == 0) MenuPrincipal();
    }
     
    void MenuPrincipal()
    {
        if (v != 0)
        {
            if (tiempoV == 0 || tiempoV > tiempoCambiarOpcion)
            {
                if (v == 1 && opcionMenu > 1) seleccionaMenu(opcionMenu - 1); //Subir Opcion
                if (v == -1 && opcionMenu < 3) seleccionaMenu(opcionMenu + 1);//Bajar Opcion
                if (tiempoV > tiempoCambiarOpcion) tiempoV = 0;
            } 
            tiempoV += Time.deltaTime;
        } 
        if(Input.GetButtonDown("Submit") && !isPulsadoEnter)
        {
            snd_seleccion.Play();
            if (opcionMenu == 1) SceneManager.LoadScene("Nivel1");
            if (opcionMenu == 2) CargaPantallaOpciones();
            if (opcionMenu == 3) Application.Quit();
        }
    }

    void CargaPantallaOpciones()
    {
             
    }

    void seleccionaMenu(int opc) 
    {
        snd_opcion.Play();
        opcionMenu = opc;
        
        //Activar Sprites 
        if (opc == 1) comenzar.sprite = comenzarOn;
        if (opc == 2) opciones.sprite = opcionesOn;
        if (opc == 3) salir.sprite = salirOn;

        //Desactivar Sprites
        if (opcionMenuAnt == 1) comenzar.sprite = comenzarOff;
        if (opcionMenuAnt == 2) opciones.sprite = opcionesOff;
        if (opcionMenuAnt == 3) salir.sprite = salirOff;
        opcionMenuAnt = opc; 

    }
}
