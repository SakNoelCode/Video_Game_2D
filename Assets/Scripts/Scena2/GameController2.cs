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
    [SerializeField] private GameObject camaraPrincipal;

    [SerializeField] private PlayerController2 playerController;
    [SerializeField] private CheckPoint checkP;

    

    //Variable que controla al Player y a la camara, si se pueden mover o no
    public static bool gameOn = false;

    private Image       sprFundidoNegro;
    private AudioSource musicaFondo;
    private int         monedas;
    private int         monedasIni;

    public static bool playerMuerto;



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

        playerController.PlayerMuerto += PlayerMuerto;//Suscribirse al evento
        checkP.checkP += chekPoint;

        StartCoroutine(FundidoNegroOFF(0.5f));
    }


    private void chekPoint()
    {
        monedasIni = current.monedas;
    }

    private void PlayerMuerto()
    {
        gameOn = false;
        musicaFondo.Stop();
        StartCoroutine("FundidoNegroON");
    }

    

    public static void sumaMonedas()
    {
        current.monedas++;
        if (current.monedas < 10)
        {
            current.contadorMonedas.text = "0" + current.monedas;
        }
        else
        {
            current.contadorMonedas.text = current.monedas.ToString();
        }
    }


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

        current.monedas = monedasIni - 1;
        sumaMonedas();

        respawn();

        StartCoroutine(FundidoNegroOFF(0.5f));
    }
    
}
   