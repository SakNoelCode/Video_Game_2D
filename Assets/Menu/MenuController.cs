using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Opciones Generales")]
    [SerializeField] int volumenMusica;
    [SerializeField] int volumenSonido;
    [SerializeField] float tiempoCambiarOpcion;
    [SerializeField] GameObject pantallaMenu;
    [SerializeField] GameObject pantallaOpciones;

    [Header("Elementos del Menú")]
    [SerializeField] SpriteRenderer comenzar;  //Opcion 1
    [SerializeField] SpriteRenderer opciones;  //Opcion 2
    [SerializeField] SpriteRenderer salir;     //Opcion 3

    [Header("Elementos de Opciones")]
    [SerializeField] SpriteRenderer musica;  //Opcion 1
    [SerializeField] SpriteRenderer sonido;  //Opcion 2
    [SerializeField] SpriteRenderer volver;  //Opcion 3

    [Header("Sprites del Menú")]
    [SerializeField] Sprite comenzarOff;
    [SerializeField] Sprite comenzarOn;
    [SerializeField] Sprite opcionesOff;
    [SerializeField] Sprite opcionesOn;
    [SerializeField] Sprite salirOff;
    [SerializeField] Sprite salirOn;

    [Header("Sprites de Opciones")]
    [SerializeField] Sprite sonidoOn;
    [SerializeField] Sprite sonidoOff;
    [SerializeField] Sprite musicaOn;
    [SerializeField] Sprite musicaOff;
    [SerializeField] Sprite volverOn;
    [SerializeField] Sprite volverOff;
    [SerializeField] Sprite volumenOn;
    [SerializeField] Sprite volumenOff;
    [SerializeField] SpriteRenderer[] musica_spr;
    [SerializeField] SpriteRenderer[] sonido_spr;


    [Header("Sonidos del Menú")]
    [SerializeField] AudioSource musicMenu;
    [SerializeField] AudioSource snd_opcion;
    [SerializeField] AudioSource snd_seleccion;

    int pantalla;  //Pantalla Menú =0 o Pantalla Opciones=1
    int opcionMenu, opcionMenuAnt;//Encender o Apagar Opciones del Menu
    int opcionOpciones, opcionOpcionesAnt;//Encender o Apagar Opciones de Opciones
    bool isPulsadoEnter; //Saber si se pulso enter
    float v, h; //Vertical y horizontal(Desplazamiento)
    float tiempoV, tiempoH;  

     
    void Awake()
    {
        leerPreferencias();
        pantalla = 0;
        tiempoV = tiempoH = 0;
        opcionMenu = opcionMenuAnt = 1;
        ajustarOpciones();

    }

    void ajustarOpciones()
    {
        ajustaMusica();
        ajustaSonido(); 
    }

    // Update is called once per frame
    void Update()
    {
        v = Input.GetAxisRaw("Vertical"); 
        h = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Submit")) isPulsadoEnter = false;
        if (v == 0) tiempoV = 0;
        if (pantalla == 0) MenuPrincipal();
        if (pantalla == 1) MenuOpciones();
    }


    //----------------------------PANTALLA OPCIONES-----------------------------------------

    void MenuOpciones()
    {
        if (v != 0)
        {
            if (tiempoV == 0 || tiempoV > tiempoCambiarOpcion)
            {
                if (v == 1 && opcionOpciones > 1) seleccionaOpcion(opcionOpciones - 1); //Subir Opcion
                if (v == -1 && opcionOpciones < 3) seleccionaOpcion(opcionOpciones + 1);//Bajar Opcion
                if (tiempoV > tiempoCambiarOpcion) tiempoV = 0;
            }
            tiempoV += Time.deltaTime;
        }

        ///Ajustar animaciones y control para el volumen de musica y volumen
        if (h == 0) tiempoH = 0;
        else
        {
            if ((tiempoH == 0 || tiempoH > tiempoCambiarOpcion) && (opcionOpciones == 1 || opcionOpciones == 2))
            {
                if (opcionOpciones == 1 && ((h < 0 && volumenMusica > 0) || (h > 0 && volumenMusica < 10)))
                {
                    volumenMusica += (int)h;
                    ajustaMusica();
                    snd_opcion.Play();
                }

                if (opcionOpciones == 2 && ((h < 0 && volumenSonido > 0) || (h > 0 && volumenSonido < 10)))
                {
                    volumenSonido += (int)h;
                    ajustaSonido();
                    snd_opcion.Play();
                }
                if (tiempoH > tiempoCambiarOpcion) tiempoH = 0;
            }
            tiempoH += Time.deltaTime;
        }

        if (Input.GetButtonDown("Submit") && opcionOpciones == 3 && !isPulsadoEnter)
        {
            cargarPreferencias();
            CargaPantallaMenu();
        }
    }

    private void cargarPreferencias()
    {
        PlayerPrefs.SetInt("VolumenMusica",volumenMusica);
        PlayerPrefs.SetInt("VolumenSonido", volumenSonido);
        PlayerPrefs.Save();
    }

    private void leerPreferencias()
    {
        volumenMusica = PlayerPrefs.GetInt("VolumenMusica", 5);
        volumenSonido = PlayerPrefs.GetInt("VolumenSonido", 4);
    }

    private void ajustaMusica()
    {
        if (volumenMusica == 0) musica_spr[0].enabled = true;
        else musica_spr[0].enabled = false;

        for(int i = 1; i <= 10; i++)
        {
            if (i <= volumenMusica) musica_spr[i].sprite = volumenOn;
            else musica_spr[i].sprite = volumenOff;
        }
        musicMenu.volume = (volumenMusica / 10f);
    }

    private void ajustaSonido()
    {
        if (volumenSonido == 0) sonido_spr[0].enabled = true;
        else sonido_spr[0].enabled = false;

        for (int i = 1; i <= 10; i++)
        {
            if (i <= volumenSonido) sonido_spr[i].sprite = volumenOn;
            else sonido_spr[i].sprite = volumenOff;
        }
        GameObject[] sonidos = GameObject.FindGameObjectsWithTag("Sonido");
        foreach(GameObject sonido in sonidos)
        {
            sonido.GetComponent<AudioSource>().volume = volumenSonido / 10f;
        }
         
    }

    void CargaPantallaMenu()
    {
        isPulsadoEnter = true;
        snd_seleccion.Play();
        pantalla = 0;
        pantallaOpciones.SetActive(false);
        pantallaMenu.SetActive(true);
    }

    void seleccionaOpcion(int opc)
    {
        snd_opcion.Play();
        opcionOpciones = opc;

        //Activar Sprites 
        if (opc == 1) musica.sprite = musicaOn;
        if (opc == 2) sonido.sprite = sonidoOn;
        if (opc == 3) volver.sprite = volverOn;

        //Desactivar Sprites
        if (opcionOpcionesAnt == 1) musica.sprite = musicaOff;
        if (opcionOpcionesAnt == 2) sonido.sprite = sonidoOff;
        if (opcionOpcionesAnt == 3) volver.sprite = volverOff;
        opcionOpcionesAnt = opc;

    }



    //----------------------------------PANTALLA MENU PRINCIPAL------------------------------------------
    void MenuPrincipal()
    {
        if (v != 0) //Si tenemos pulsado las teclas del eje Vertical
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



    void CargaPantallaOpciones()
    {
        isPulsadoEnter = true;
        pantallaMenu.SetActive(false);
        pantalla = 1;
        opcionOpciones = opcionOpcionesAnt = 1;
        musica.sprite = musicaOn;
        sonido.sprite = sonidoOff;
        volver.sprite = volverOff;
        pantallaOpciones.SetActive(true);
    }
}
