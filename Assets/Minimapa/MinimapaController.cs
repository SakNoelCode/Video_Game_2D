using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinimapaController : MonoBehaviour
{
    //[SerializeField] private float velocidad = 5;
    [SerializeField] private Transform[] arrayHuecos;
    [SerializeField] private Transform[] arrayCaminos;
    [SerializeField] private SpriteRenderer[] arrayPosiciones;
    [SerializeField] private Sprite[] arraySpr_Texto;
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer titulo;
    [SerializeField] private AudioSource snd_Camino;

    private int posPlayer = 0;
    private int maxNivel;   
    private bool puedoMoverme = false;
    private string escena;

    //Ajustar Volumen Audio
    [SerializeField] private GameObject audioFondo;
    [SerializeField] private GameObject audioCamino;
    private AudioSource AudioFondo;
    private AudioSource AudioCamino;


    // ----------------------------------------Metodo Start
    void Awake()
    {
        leerPreferencias();

        if (!Variables.isIniciado)//La primera vez que inicia el juego
        {
            Variables.nivel = 0;
            Variables.Maxnivel = 1;
            Variables.isIniciado = true;

            Variables.estadoPos1 = 2;
            Variables.estadoPos2 = 0;
            Variables.estadoPos3 = 0;
        }

        //Comprobaciones Iniciales
        posPlayer = Variables.nivel;
        titulo.sprite = arraySpr_Texto[posPlayer];
        maxNivel = Variables.Maxnivel;

        //Manejar la variable PuedoMoverme
        if (Variables.estadoPos1 != 2 &&
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
        player.position = arrayHuecos[posPlayer].position;

        //Abrir caminos para ir a la Posición 1
        if (Variables.estadoPos1 == 1)
        {
            arrayCaminos[0].localScale = new Vector3(1, 1, 1); //Mostrar Camino
            arrayPosiciones[0].enabled = true;  //Mostrar la Pos(Sprite)

        } else if (Variables.estadoPos1 == 2)
        {
            StartCoroutine("AbreCamino", 0);
        }

        //Abrir caminos para ir a la Posición 2
        if (Variables.estadoPos2 == 1)
        {
            arrayCaminos[1].localScale = new Vector3(1, 1, 1); //Mostrar Camino
            arrayPosiciones[1].enabled = true;  //Mostrar la Pos(Sprite)

        }
        else if (Variables.estadoPos2 == 2)
        {
            StartCoroutine("AbreCamino", 1);
        }

        //Abrir caminos para ir a la Posición 3
        if (Variables.estadoPos3 == 1)
        {
            arrayCaminos[2].localScale = new Vector3(1, 1, 1); //Mostrar Camino
            arrayPosiciones[2].enabled = true;  //Mostrar la Pos(Sprite)

        }
        else if (Variables.estadoPos3 == 2)
        {
            StartCoroutine("AbreCamino", 2);
        }
    }

    // -------------------------------------------Metodo Update
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

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
            if (posPlayer == 1) escena = "Nivel1";
            if (posPlayer == 2) escena = "Nivel2";
            if (posPlayer == 3) escena = "Nivel3";
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
        } while (porcentaje < 1);
        snd_Camino.Stop();
        puedoMoverme = true;
        arrayPosiciones[numCamino].enabled = true;

        //Comprobaciones
        //estadoPos(0)  = No se ha descubierto
        //estadoPos(1)  = 
        //estadoPos(2)  = Tiene que aparecer poco a poco
        if (numCamino == 0) Variables.estadoPos1 = 1;
        if (numCamino == 1) Variables.estadoPos2 = 1;
        if (numCamino == 2) Variables.estadoPos3 = 1;
    }

    IEnumerator MuevePlayer(int mov)
    {
        Vector3 distancia = Vector3.zero;
        posPlayer += mov;

        do
        {
            player.transform.Translate(0.025f * mov, 0, 0);
            distancia = arrayHuecos[posPlayer].position - player.position;
            yield return new WaitForSeconds(0.01f);
        } while (distancia.sqrMagnitude > 0.001f);

        player.position = arrayHuecos[posPlayer].position;
        titulo.sprite = arraySpr_Texto[posPlayer];
        yield return new WaitForSeconds(0.15f);
        puedoMoverme = true;
    }

    private void OnApplicationQuit()
    {
        Variables.isIniciado = false;
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
}
