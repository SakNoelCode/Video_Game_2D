using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController2 : MonoBehaviour
{

    [SerializeField] private GameObject fundidoNegro;

    //Variable que controla al Player y a la camara, si se pueden mover o no
    public static bool gameOn = false;

    private SpriteRenderer sprFundidoNegro;

    public static bool playerMuerto;


    private void Start()
    {
        sprFundidoNegro = fundidoNegro.GetComponent<SpriteRenderer>();
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
        fundidoNegro.SetActive(true); 
    }

       
    private void quitarFundido()
    {
        StartCoroutine("QuitaFC");
    }



    IEnumerator QuitaFC()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= Time.deltaTime * 2f)
        {
            sprFundidoNegro.material.color = new Color(sprFundidoNegro.material.color.r, 
                                                       sprFundidoNegro.material.color.g, 
                                                       sprFundidoNegro.material.color.b, 
                                                       alpha);
            yield return null; 
        }
        gameOn = true;
    }

    IEnumerator PonFC()
    {
        for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * 2f)
        {
            sprFundidoNegro.material.color = new Color(sprFundidoNegro.material.color.r,
                                                       sprFundidoNegro.material.color.g,
                                                       sprFundidoNegro.material.color.b,
                                                       alpha);
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    
}
