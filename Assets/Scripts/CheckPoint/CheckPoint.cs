using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //Delegado y Eventos
    public delegate void MiDelegadoPoint();
    public event MiDelegadoPoint checkP;

    [SerializeField] private Sprite spriteOn;
    [SerializeField] private GameObject posPlayer;

    private SpriteRenderer sprCheckPoint;
    private AudioSource    sonidoCheckPoint;
    private BoxCollider2D  boxColider;

    public GameObject[] monedas;

    //Este método se ejecuta antes del Start
    void Awake()
    {
        sprCheckPoint = GetComponent<SpriteRenderer>();
        sonidoCheckPoint = GetComponent<AudioSource>();
        boxColider = GetComponent<BoxCollider2D>();

        monedas = GameObject.FindGameObjectsWithTag("Monedas");
    }

    //Método para verificar Colisiones a través del BoxColider
    private void OnTriggerEnter2D(Collider2D collision)
    {
     if(collision.gameObject.tag == "Player")
        {
            boxColider.enabled = false;
            sonidoCheckPoint.Play();
            sprCheckPoint.sprite = spriteOn;
            checkP?.Invoke();
            PlayerController.posInicialPlayer = posPlayer.transform.position;
            destruyeObjetos();
        }   
    }

    private void destruyeObjetos()
    {
        foreach(GameObject moneda in monedas)
        {
            if (!moneda.activeSelf) Destroy(moneda);
        }
    }
}
