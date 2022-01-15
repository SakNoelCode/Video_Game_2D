using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController2 : MonoBehaviour
{
    static GameController2 current; 

    [SerializeField] private GameObject fundidoNegro;
    [SerializeField] private Text contadorMonedas;

    private int monedas;

    //Variable que controla al Player y a la camara, si se pueden mover o no
    public static bool gameOn = false;

    private Image sprFundidoNegro;

    public static bool playerMuerto;


    private void Start()
    {
        sprFundidoNegro = fundidoNegro.GetComponent<Image>(); 
        Invoke("quitarFundido", 0.5f);
    }

    private void Update()
    {
        if(playerMuerto){
            StartCoroutine("PonFC");
            playerMuerto = false;
        }
    }

    private void Awake()
    {
        #region
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        current = this;
        DontDestroyOnLoad(gameObject);
        #endregion

        fundidoNegro.SetActive(true); 
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

       
    private void quitarFundido()
    {
        StartCoroutine("QuitaFC");
    }



    IEnumerator QuitaFC()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime * 2f)
        {
            sprFundidoNegro.color = new Color(sprFundidoNegro.color.r, 
                                                       sprFundidoNegro.color.g, 
                                                       sprFundidoNegro.color.b, 
                                                       alpha);
            yield return null; 
        }
        gameOn = true;
    }

    IEnumerator PonFC()
    {
        for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * 2f)
        {
            sprFundidoNegro.color = new Color(sprFundidoNegro.color.r,
                                                       sprFundidoNegro.color.g,
                                                       sprFundidoNegro.color.b,
                                                       alpha);
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    
}
