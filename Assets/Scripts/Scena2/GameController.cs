using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public delegate void Respawn();
    public static event Respawn respawn;

    static GameController current;

    [SerializeField] private GameObject fundidoNegro;
    [SerializeField] private Text contadorMonedas;
    [SerializeField] private GameObject contadorMonedasPuerta;
    [SerializeField] private GameObject camaraPrincipal;
    [SerializeField] private GameObject zonapuerta;
    [SerializeField] private GameObject zonaNoPermitirPaso;
    [SerializeField] private GameObject puerta;
    [SerializeField] private PlayerController playerController;
    //[SerializeField] private CheckPoint checkP;
    [SerializeField] private CheckPoint[] checkP;
    [SerializeField] private int MonedasPuertaNivel;

    //Gestión de CheckPoint
    public static int identificadorCheckPoint;
    public static bool nuevoCheckPoint = false;

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

    //Variables para controlar el fundido con efecto de la mascara
    [SerializeField] private Transform mascara;
    [SerializeField] private SpriteRenderer sprColorMascara;
    [SerializeField] private float velocidadMascara;
    private Vector3 escalaFinal;
    private Vector3 diferencia;
    private float opacidadMascara;
    private bool fundidoOn;

    public static bool playerMuerto;

    private BoxCollider2D boxColPuerta;
    private BoxCollider2D boxColNoPermitirPaso;
    private Animator animacionPuerta;



    private void Awake()
    {
        leerPreferencias();  //PlayerPref
        current = this;
        fundidoNegro.SetActive(true);
        monedasIni = 0;
        fundidoOn = false;
        identificadorCheckPoint = 0;
    }


    private void Start()
    {  
        sprFundidoNegro = fundidoNegro.GetComponent<Image>();
        boxColPuerta = zonapuerta.GetComponent<BoxCollider2D>();
        boxColNoPermitirPaso = zonaNoPermitirPaso.GetComponent<BoxCollider2D>();
        animacionPuerta = puerta.GetComponent<Animator>();

        playerController.PlayerMuerto += PlayerMuerto;//Suscribirse al evento
        //Suscribirse al evento del CheckPoint según el identificador
        //checkP.checkP += chekPoint;
        checkP[identificadorCheckPoint].checkP += chekPoint;

        StartCoroutine(FundidoNegroOFF(0.5f));

        //Gestión de las monedas en la Puerta final
        textoPuerta = contadorMonedasPuerta.GetComponent<TextMesh>();
        monedas = monedasIni;
        monedasPuerta = MonedasPuertaNivel;

        //Actualizar valor de las monedas en la Puerta
        if (monedasPuerta < 10) textoPuerta.text = "0" + monedasPuerta;
        else textoPuerta.text = monedasPuerta.ToString();
    }


    private void Update()
    {
        //Suscribirse al checkPoint
        if(nuevoCheckPoint)
        {
            checkP[identificadorCheckPoint].checkP += chekPoint;
            nuevoCheckPoint = false;
            monedasIni = int.Parse(contadorMonedas.text);  //Actualizar valor de MonedasIni
        }

        if (fundidoOn)
        {
            //Mover la escala del Sprite Renderer de la máscara
            mascara.localScale = Vector3.MoveTowards(mascara.localScale,
                                                     escalaFinal,
                                                     velocidadMascara * Time.deltaTime);

            //Sumar la opacidad con cada segundo del tiempo(de transparente a oscuro)
            opacidadMascara += Time.deltaTime * 1.25f;

            //Aplicar esta opacidad al Sprite renderer de la mascara
            sprColorMascara.color = new Color(sprColorMascara.color.r,
                                              sprColorMascara.color.g,
                                              sprColorMascara.color.b,
                                              opacidadMascara);

            //Si se ha llegado al final de la escala hacer:
            diferencia = mascara.localScale - escalaFinal;
            if (diferencia.sqrMagnitude < 0.0001f)
            {
                mascara.localScale = escalaFinal;

                //Activar fundido negro
                sprFundidoNegro.color = new Color(sprFundidoNegro.color.r,
                                                           sprFundidoNegro.color.g,
                                                           sprFundidoNegro.color.b,
                                                           1);

                //Desactivar fundido máscara
                sprColorMascara.color = new Color(sprColorMascara.color.r,
                                              sprColorMascara.color.g,
                                              sprColorMascara.color.b,
                                              0);
                fundidoOn = false;

                //Realizar acciones después del fundido
                //Le quitamos 1 para que no afecte cuando sume 1
                monedas = monedasIni - 1;
                sumaMonedas();
                respawn();

                //Iniciar Courutina de Fundido Negro
                StartCoroutine(FundidoNegroOFF(0.5f));
            }
        }
    }


    private void leerPreferencias()
    {
        int volumenMusica = PlayerPrefs.GetInt("VolumenMusica", 5);
        int volumenSonido = PlayerPrefs.GetInt("VolumenSonido", 4);

        musicaFondo = camaraPrincipal.GetComponent<AudioSource>(); //Obtener el objeto

        //Ajustar volumen de música
        musicaFondo.volume = volumenMusica / 10f;

        //Ajustar volumen de sonido
        GameObject[] sonidos = GameObject.FindGameObjectsWithTag("Sonido");
        foreach(GameObject sonido in sonidos)
        {
            sonido.GetComponent<AudioSource>().volume = volumenSonido / 10f;
        }
    }

    private void activaFundido()
    {
        diferencia = Vector3.zero;
        escalaFinal = new Vector3(0, 0, 1);
        fundidoOn = true;
        opacidadMascara = 0;
    }

    private void chekPoint()
    {
        monedasIni = monedas;
    }

    private void PlayerMuerto()
    {
        gameOn = false;
        musicaFondo.Stop();
        activaFundido();
    }

    //Metodos Publicos para Controlar la Pausa
    public static void PausarGame()
    {
        gameOn = false;
        current.musicaFondo.Pause();
    }
    public static void ReanudarGame()
    {
        gameOn = true;
        current.musicaFondo.Play();
    }
    public static void SalirGame()
    {
        SceneManager.LoadScene("Minimapa");
    }
    // Fin de metodos Publicos para Controlar la Pausa

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
            //Hacer al fundido negro completamente transparente
            sprFundidoNegro.color = new Color(sprFundidoNegro.color.r,
                                              sprFundidoNegro.color.g,
                                              sprFundidoNegro.color.b,
                                              0);

        gameOn = true;
        musicaFondo.Play();  /////Poner a la musica en Play

        //Reiniciar valores de la mascara
        opacidadMascara = 0;
        mascara.localScale = new Vector3(2,2,1);
    }

    
    //----------------------FUNCIONES Y PROCEDIMIENTOS DE LA PUERTA FINAL------------------------------

    //----------------------MANEJO DE ABRIR Y CERRAR LA PUERTA------------------------------
    public static void permitePaso()
    {
        current.boxColPuerta.isTrigger = true;
    }

    public static void NoPermitePaso()
    {
        current.boxColNoPermitirPaso.isTrigger = false;
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

        //Salir al minimapa
        if (Variables.Maxnivel < 3)
        {
            //Comprobaciones
            //estadoPos(0)  = No se ha descubierto
            //estadoPos(1)  = 
            //estadoPos(2)  = Tiene que aparecer poco a poco

            if (Variables.nivel == 1 && Variables.estadoPos2 == 0)
            {
                Variables.Maxnivel++;
                Variables.estadoPos2 = 2;
            }
            if (Variables.nivel == 2 && Variables.estadoPos3 == 0)
            {
                Variables.Maxnivel++;
                Variables.estadoPos3 = 2;
            }
        }

        SceneManager.LoadScene("Minimapa");
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
