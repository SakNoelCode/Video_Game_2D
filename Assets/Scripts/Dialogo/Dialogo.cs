using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogo : MonoBehaviour
{

    [SerializeField] private GameObject warning;
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private TMP_Text textoDialogo ;
    [SerializeField, TextArea(4,6)] private string[] lineaDialogo;

    private float tiempoLinea = 0.05f;
    private bool isPlayerCerca;
    private bool isDialogoIniciado;

    private int lineIndex;
    
    void Update()
    {
        if(isPlayerCerca && Input.GetButtonDown("Fire1"))
        {
            if (!isDialogoIniciado) //El dialogo aún no Inicia
            {
                IniciarDialogo();
            }
            else if (textoDialogo.text == lineaDialogo[lineIndex]) //Siguiente línea
            {
                NextDialogo();   
            }
            else  //Mostrar línea si courotinas
            {
                StopAllCoroutines();
                textoDialogo.text = lineaDialogo[lineIndex];
            }
        }
    }

    private void IniciarDialogo()
    {
        isDialogoIniciado = true;
        panelDialogo.SetActive(true);
        warning.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(MostrarLineas());
    }

    private void NextDialogo()
    {
        lineIndex++;
        if(lineIndex < lineaDialogo.Length)
        {
            StartCoroutine(MostrarLineas());
        }
        else
        {
            isDialogoIniciado = false;
            panelDialogo.SetActive(false);
            warning.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    //Coroutina
    private IEnumerator MostrarLineas()
    {
        textoDialogo.text = string.Empty;

        foreach (char c in lineaDialogo[lineIndex])
        {
            textoDialogo.text += c;
            yield return new WaitForSecondsRealtime(tiempoLinea);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerCerca = true;
            warning.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerCerca = false;
            warning.SetActive(false);
        }
    }
}
