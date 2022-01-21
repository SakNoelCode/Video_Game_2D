using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController2 : MonoBehaviour
{
    public delegate void Respawn();
    public static event Respawn respawn;

    static GameController2 current;

    [SerializeField] private GameObject fundidoNegro;
    [SerializeField] private Text contadorMonedas;
    [SerializeField] private GameObject contadorMonedasPuerta;
    [SerializeField] private GameObject camaraPrincipal;
    [SerializeField] private GameObject zonapuerta;
    [SerializeField] private GameObject puerta;

    [SerializeField] private PlayerController2 playerController;
    [SerializeField] private CheckPoint checkP;


    //Variable que controla al Player y a la camara, si se pueden mover o no
    public static bool gameOn = false;

    private Image sprFundidoNegro;
    private AudioSource musicaFondo;


    //Gestion de monedas en la Puerta Final
    public static int monedas;
    public static int monedasPuerta;
    public static bool isSoltandoMonedas = false;
    private int monedasIni;
    private TextMesh textoPuerta;

    public static bool playerMuerto;

    private BoxCollider2D boxColPuerta;
    private Animator animacionPuerta;



    private void Awake()
    {
        current = this;
        fundidoNegro.SetActive(true);
        monedasIni = 0;
    }


    private void Start()
    {
        musicaFondo = camaraPrincipal.GetComponent<AudioSource>();
        sprFundidoNegro = fundidoNegro.GetComponent<Image>();
        boxColPuerta = zonapuerta.GetComponent<BoxCollider2D>();
        animacionPuerta = puerta.GetComponent<Animator>();

        playerController.PlayerMuerto += PlayerMuerto;//Suscribirse al evento
        checkP.checkP += chekPoint;

        StartCoroutine(FundidoNegroOFF(0.5f));

        //Gestión de las monedas en la Puerta final
        textoPuerta = contadorMonedasPuerta.GetComponent<TextMesh>();
        monedas = monedasIni;
        monedasPuerta = 5;

        //Actualizar valor de las monedas en la Puerta
        if (monedasPuerta < 10) textoPuerta.text = "0" + monedasPuerta;
        else textoPuerta.text = monedasPuerta.ToString();
    }


    private void chekPoint()
    {
        monedasIni = monedas;
    }

    private void PlayerMuerto()
    {
        gameOn = false;
        musicaFondo.Stop();
        StartCoroutine("FundidoNegroON");
    }



    public static void sumaMonedas()
    {
        monedas++;
        if (monedas < 10)
        {
            current.contadorMonedas.text = "0" + monedas;
        }
        else
        {
            current.contadorMonedas.text = monedas.ToString();
        }
    }

    public static void RestaMonedas()
    {
        monedas--;
        if (monedas < 10)
        {
            current.contadorMonedas.text = "0" + monedas;
        }
        else
        {
            current.contadorMonedas.text = monedas.ToString();
        }
    }

    public static void RestaMonedasPuerta()
    {
        monedasPuerta--;
        //Actualizar valor de las monedas en la Puerta
        if (monedasPuerta < 10) current.textoPuerta.text = "0" + monedasPuerta;
        else current.textoPuerta.text = monedasPuerta.ToString();
    }

    //----------------------MANEJO DE FUNDIDOS DE NEGRO------------------------------
    IEnumerator FundidoNegroOFF(float retardo)
    {
        yield return new WaitForSeconds(retardo);

        for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime * 2f)
        {
            sprFundidoNegro.color = new Color(sprFundidoNegro.color.r,
                                                       sprFundidoNegro.color.g,
                                                       sprFundidoNegro.color.b,
                                                       alpha);
            yield return null;
        }
        gameOn = true;
        musicaFondo.Play();  /////Poner a la musica en Play
    }

    IEnumerator FundidoNegroON()
    {
        for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * 2f)
        {
            sprFundidoNegro.color = new Color(sprFundidoNegro.color.r,
                                                       sprFundidoNegro.color.g,
                                                       sprFundidoNegro.color.b,
                                                       alpha);
            yield return null;
        }

        monedas = monedasIni - 1;
        sumaMonedas();

        respawn();

        StartCoroutine(FundidoNegroOFF(0.5f));
    }


    //----------------------MANEJO DE ABRIR Y CERRAR LA PUERTA------------------------------
    public static void permitePaso()
    {
        current.boxColPuerta.isTrigger = true;
    }

    public static void abrePuerta()
    {
        current.animacionPuerta.Play("PuertaAbriendose");
    }

    //----------------------MANEJO DEL FIN DEL NIVEL------------------------------
    public static void finNivel()
    {
        current.musicaFondo.Stop();
        current.StartCoroutine("FundidoNegroFinNivel");
    }


    //---------------------FUNDIDO PARA EL FIN DE NIVEL------------------------------
    IEnumerator FundidoNegroFinNivel()
    {
        for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * 2f)
        {
            sprFundidoNegro.color = new Color(sprFundidoNegro.color.r,
                                                       sprFundidoNegro.color.g,
                                                       sprFundidoNegro.color.b,
                                                       alpha);
            yield return null;
        }
    }
}
