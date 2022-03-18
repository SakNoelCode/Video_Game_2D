using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizController : MonoBehaviour
{
    [SerializeField] private GameObject canvasQuiz;

    //Manejo del sonido
    [SerializeField] private GameObject snd_Correcta;
    [SerializeField] private GameObject snd_Incorrecta;
    private AudioSource snd_correcta, snd_incorrecta;


    [SerializeField] private Color colorCorrecta = Color.black;
    [SerializeField] private Color colorIncorrecta = Color.black;
       
    private float tiempoEspera = 1f;


    [SerializeField] private GameObject go_quizDB = null;
    [SerializeField] private GameObject go_quizUI = null;
    private QuizDB m_quizDB = null;
    private QuizUI m_quizUI = null;

    public static bool finQuizz = false;

    private void Start()
    {
        m_quizDB = go_quizDB.GetComponent<QuizDB>();
        m_quizUI = go_quizUI.GetComponent<QuizUI>();

        snd_correcta = snd_Correcta.GetComponent<AudioSource>();
        snd_incorrecta = snd_Incorrecta.GetComponent<AudioSource>();
        NextQuestion();
    }

    private void Update()
    {
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
        if (optionbutton.options.isCorrecta)
        {
            snd_correcta.Play();
        }
        else
        {
            snd_incorrecta.Play();
        }

        optionbutton.SetColor(optionbutton.options.isCorrecta ? colorCorrecta : colorIncorrecta);

        

        yield return new WaitForSeconds(tiempoEspera);

        //NextQuestion();
        cerrarQuiz();
    }

    private void cerrarQuiz()
    {
        finQuizz = true;
        Cartel.colisionWithPlayer = false;
        GameController.ReanudarGame();
        canvasQuiz.SetActive(false);

        Debug.Log("Quizz terminada");
        NextQuestion();
    }
    
}
