using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonedaPuerta : MonoBehaviour
{
    [SerializeField] private float velocidad;

    private GameObject destino;
    private Vector3 destinoFix;
    private ParticleSystem particulas;
    private SpriteRenderer sprMonedaPuerta;
    private AudioSource snd_monedaPuerta;

    private bool fin = false;

    // Start is called before the first frame update
    void Start()
    {
        destino = GameObject.FindGameObjectWithTag("MonedaPuerta");
        destinoFix = new Vector3(destino.transform.position.x, destino.transform.position.y, -1);
        particulas = gameObject.GetComponent<ParticleSystem>();
        sprMonedaPuerta = gameObject.GetComponent<SpriteRenderer>();
        snd_monedaPuerta = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        calcularDestino();
    }  

    private void calcularDestino()
    {
        if(!fin && GameController2.monedasPuerta > 0) {
            if (transform.position != destinoFix)
            {
                transform.position = Vector3.MoveTowards(transform.position, destinoFix, velocidad);
            }
            else
            {
                sprMonedaPuerta.enabled = false;
                particulas.Play();
                snd_monedaPuerta.Play();
                fin = true;
                GameController2.RestaMonedas(); 
                GameController2.RestaMonedasPuerta();
                if(GameController2.monedasPuerta == 0)
                {
                    Destroy(destino);
                }
            }
        }
        
    }

    private void OnDestroy()
    {
        GameController2.isSoltandoMonedas = false;
        if (GameController2.monedasPuerta == 0) GameController2.abrePuerta();
    }
}
