using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizController : MonoBehaviour
{
    /* [SerializeField] private AudioClip sound_rptaCorrecta = null;
     [SerializeField] private AudioClip sound_rptaIncorrecta =null;*/
    [SerializeField] private GameObject canvasQuiz;

    //Manejo del sonido
    [SerializeField] private GameObject snd_Correcta;
    [SerializeField] private GameObject snd_Incorrecta;
    private AudioSource snd_correcta, snd_incorrecta;


    [SerializeField] private Color colorCorrecta = Color.black;
    [SerializeField] private Color colorIncorrecta = Color.black;
       
    private float tiempoEspera = 1f;
    //private AudioSource audioSource =null;


    [SerializeField] private GameObject go_quizDB = null;
    [SerializeField] private GameObject go_quizUI = null;
    private QuizDB m_quizDB = null;
    private QuizUI m_quizUI = null;


    private bool continueGame = false;

    private void Start()
    {
        m_quizDB = go_quizDB.GetComponent<QuizDB>();
        m_quizUI = go_quizUI.GetComponent<QuizUI>();

        snd_correcta = snd_Correcta.GetComponent<AudioSource>();
        snd_incorrecta = snd_Incorrecta.GetComponent<AudioSource>();
        //m_quizUI = GameObject.FindObjectOfType<QuizUI>();
        //audioSource = GetComponent<AudioSource>();
        NextQuestion();
    }

    private void Update()
    {
        if (continueGame)
        {
            volverGame();
        }
    }

    private void NextQuestion()
    {
        m_quizUI.construct(m_quizDB.GetRandom(),GiveAnswer);
    }

    private void GiveAnswer(OptionsButtons optionButton)
    {
        StartCoroutine(ColoreaBotonRoutine(optionButton));
    }

    private IEnumerator ColoreaBotonRoutine(OptionsButtons optionbutton)
    {
        /*if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }*/
        if (optionbutton.options.isCorrecta)
        {
            snd_correcta.Play();
        }
        else
        {
            snd_incorrecta.Play();
        }
        /*
        audioSource.clip = optionbutton.options.isCorrecta ? sound_rptaCorrecta : sound_rptaIncorrecta;
        */
        optionbutton.SetColor(optionbutton.options.isCorrecta ? colorCorrecta : colorIncorrecta);

        //audioSource.Play();
        Debug.Log("Ejecutado1");

        continueGame = true;

        yield return new WaitForSeconds(tiempoEspera);

        //Debug.Log(tiempoEspera);
    }

    private void volverGame()
    {
        Time.timeScale = 1;
        GameController.ReanudarGame();
        canvasQuiz.SetActive(false);
    }
}
