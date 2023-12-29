using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausa : MonoBehaviour
{
    [SerializeField] private GameObject btnPausa;
    [SerializeField] private GameObject panelPausa;

    private bool juegoPausado = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                pausa();
            }

        }
    }

    public void pausa()
    {
        juegoPausado = true;
        Time.timeScale = 0;
        GameController.PausarGame();
        btnPausa.SetActive(false);
        panelPausa.SetActive(true);
    }

    public void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1;
        GameController.ReanudarGame();
        btnPausa.SetActive(true);
        panelPausa.SetActive(false); 
    }

    public void Reiniciar()
    {
        juegoPausado = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Salir()
    {
        //GameController.SalirGame();
    }
}
 