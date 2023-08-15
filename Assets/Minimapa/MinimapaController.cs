using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinimapaController : MonoBehaviour
{
    [SerializeField] private Transform[] arrayPosiciones;
    [SerializeField] private Transform[] arrayCaminos;
    [SerializeField] private SpriteRenderer[] arrayPosicionesIcon;
    [SerializeField] private Sprite[] arraySpr_Texto;
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer titulo;
    [SerializeField] private AudioSource snd_Camino;

    private int posPlayer = 0;  //posiciones del player en el minimapa (0,1,2,3,4,5)
    private int maxNivel;   //Nivel al que podemos ir (1,2,3,4,5)
    private bool puedoMoverme = false;
    private string escena;

    //Ajustar Volumen Audio
    [SerializeField] private GameObject audioFondo;
    [SerializeField] private GameObject audioCamino;
    private AudioSource AudioFondo;
    private AudioSource AudioCamino;


    // ----------------------------------------Metodo Awake-----------------------------
    void Awake()
    {
        leerPreferencias();

        if (!Variables.isIniciado)//La primera vez que inicia el juego
        {
            Variables.nivel = 0;
            Variables.Maxnivel = 1;
            Variables.isIniciado = true;

            //Asignar estados iniciales a las posiciones
            Variables.estadoPosMenu = 2;
            Variables.estadoPos0 = 0;
            Variables.estadoPos1 = 0;
            Variables.estadoPos2 = 0;
            Variables.estadoPos3 = 0;
            Variables.estadoPos4 = 0;
        }
         
        //Comprobaciones Iniciales
        posPlayer = Variables.nivel;
        titulo.sprite = arraySpr_Texto[posPlayer];
        maxNivel = Variables.Maxnivel;

        //Manejar la variable PuedoMoverme
        if (Variables.estadoPosMenu != 2 &&
            Variables.estadoPos0 != 2 &&
            Variables.estadoPos1 != 2 &&
            Variables.estadoPos2 != 2 &&
            Variables.estadoPos3 != 2)
        {
            puedoMoverme = true;
        }
        else
        {
            puedoMoverme = false;
        }

        //Poner al Player en su Hueco correspondiente
        player.position = arrayPosiciones[posPlayer].position;

        //--------------------------COMPROBACION PARA CAMINOS-----------------------------
        //Abrir camino cuando estemos en la posicion de Menú 
        if (Variables.estadoPosMenu == 1)  //Si la posición ya se descubrió
        {
            arrayCaminos[0].localScale = new Vector3(1.6f, 1, 1); //Mostrar Camino
            arrayPosicionesIcon[0].enabled = true;  //Mostrar la Pos(Sprite)

        }
        else if (Variables.estadoPosMenu == 2) //Si la posición debe mostrarse con animaciones
        {
            StartCoroutine("AbreCamino", 0); //Menú tiene la posición 0
        }

        //Abrir camino cuando estemos en la posicion de Tutorial
        if (Variables.estadoPos0 == 1)  //Si la posición ya se descubrió
        {
            arrayCaminos[1].localScale = new Vector3(1.6f, 1, 1); //Mostrar Camino
            arrayPosicionesIcon[1].enabled = true;  //Mostrar la Pos(Sprite)

        }
        else if (Variables.estadoPos0 == 2) //Si la posición debe mostrarse con animaciones
        {
            StartCoroutine("AbreCamino", 1); //Tutorial tiene la posición 1
        }

        //Abrir camino cuando estemos en la posicion de Nivel 1
        if (Variables.estadoPos1 == 1)  //Si la posición ya se descubrió
        {
            arrayCaminos[2].localScale = new Vector3(1.6f, 1, 1); //Mostrar Camino
            arrayPosicionesIcon[2].enabled = true;  //Mostrar la Pos(Sprite)

        }
        else if (Variables.estadoPos1 == 2) //Si la posición debe mostrarse con animaciones
        {
            StartCoroutine("AbreCamino", 2); //Nivel 1 tiene la posición 2
        }

        //Abrir camino cuando estemos en la posicion de Nivel 2
        if (Variables.estadoPos2 == 1)  //Si la posición ya se descubrió
        {
            arrayCaminos[3].localScale = new Vector3(1.6f, 1, 1); //Mostrar Camino
            arrayPosicionesIcon[3].enabled = true;  //Mostrar la Pos(Sprite)

        }
        else if (Variables.estadoPos2 == 2) //Si la posición debe mostrarse con animaciones
        {
            StartCoroutine("AbreCamino", 3); //Nivel 2 tiene la posición 3
        }

        //Abrir camino cuando estemos en la posicion de Nivel 3
        if (Variables.estadoPos3 == 1)  //Si la posición ya se descubrió
        {
            arrayCaminos[4].localScale = new Vector3(1.6f, 1, 1); //Mostrar Camino
            arrayPosicionesIcon[4].enabled = true;  //Mostrar la Pos(Sprite)

        }
        else if (Variables.estadoPos3 == 2) //Si la posición debe mostrarse con animaciones
        {
            StartCoroutine("AbreCamino", 4); //Nivel 3 tiene la posición 4
        }

        //Abrir camino cuando estemos en la posicion de Nivel 4

    }

    // -------------------------------------------Metodo Update-----------------------
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Comprobaciones para que el player se mueva
        if((horizontal==1 && posPlayer<maxNivel && puedoMoverme) || //Moverse de izqu. a der.
            (horizontal == -1 && posPlayer > 0 && puedoMoverme))    //Moverse de der. a izqu.
        {
            puedoMoverme = false;
            StartCoroutine("MuevePlayer", horizontal);
        }
        
        //Detectar el clic y entrar en la escena
        if(Input.GetButton("Submit") && puedoMoverme)
        {
            Variables.nivel = posPlayer;
            if (posPlayer == 0) escena = "Menu";
            if (posPlayer == 1) escena = "Nivel_0";
            if (posPlayer == 2) escena = "Nivel_1";
            if (posPlayer == 3) escena = "Nivel_2";
            if (posPlayer == 4) escena = "Nivel_3";
            if (posPlayer == 5) escena = "Nivel_4";
            SceneManager.LoadScene(escena);
        }
    }


    //---------------------------------CORUTINAS----------------------------
    IEnumerator AbreCamino(int numCamino)
    {
        yield return new WaitForSeconds(0.5f);
        snd_Camino.Play();
        float porcentaje = 0;
        do
        {
            porcentaje += 0.1f;
            arrayCaminos[numCamino].localScale = new Vector3(porcentaje, 1, 1);
            yield return new WaitForSeconds(0.1f);
        } while (porcentaje < 1.6);
        snd_Camino.Stop();
        puedoMoverme = true;
        arrayPosicionesIcon[numCamino].enabled = true; //Mostar el icono

        //Comprobaciones (Si el camino que se abrió es ??)
        //estadoPos(0)  = No se ha descubierto
        //estadoPos(1)  = Descubierto
        //estadoPos(2)  = Tiene que aparecer poco a poco
        if (numCamino == 0) Variables.estadoPosMenu = 1;
        if (numCamino == 1) Variables.estadoPos0 = 1;
        if (numCamino == 2) Variables.estadoPos1 = 1;
        if (numCamino == 3) Variables.estadoPos2 = 1;
        if (numCamino == 4) Variables.estadoPos3 = 1;
    }

    IEnumerator MuevePlayer(int mov)
    {
        Vector3 distancia = Vector3.zero;
        posPlayer += mov;

        do
        {
            player.transform.Translate(0.025f * mov, 0, 0);
            distancia = arrayPosiciones[posPlayer].position - player.position;
            yield return new WaitForSeconds(0.01f);
        } while (distancia.sqrMagnitude > 0.001f);

        player.position = arrayPosiciones[posPlayer].position;
        titulo.sprite = arraySpr_Texto[posPlayer];
        yield return new WaitForSeconds(0.15f);
        puedoMoverme = true;
    }
      
    private void leerPreferencias()
    {
        int volumenMusica = PlayerPrefs.GetInt("VolumenMusica", 5);
        int volumenSonido = PlayerPrefs.GetInt("VolumenSonido", 4);

        //Obtener componentes
        AudioCamino = audioCamino.GetComponent<AudioSource>();
        AudioFondo = audioFondo.GetComponent<AudioSource>();

        //Ajustar Volumen
        AudioCamino.volume = volumenSonido / 10f;
        AudioFondo.volume = volumenMusica / 10f;
    }


    //Reiniciar valores cuando cerramos la aplicación, quitar esto cuando el juego este listo
    private void OnApplicationQuit()
    {
        Variables.isIniciado = false;
    }
}
