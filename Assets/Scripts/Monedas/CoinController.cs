using UnityEngine;

public class CoinController : MonoBehaviour
{

    private ParticleSystem particulas;
    private SpriteRenderer sprMoneda;
    private bool isActiva = true;
    private AudioSource snd_Moneda;


    private void Awake()
    {
        particulas = GetComponent<ParticleSystem>();
        sprMoneda = GetComponent<SpriteRenderer>();
        snd_Moneda = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isActiva)
        {
            GameController2.sumaMonedas();
            //Destroy(gameObject);
            sprMoneda.enabled = false;
            particulas.Play();
            isActiva = false;
            snd_Moneda.Play();//Hacer sonido de la moneda
        }  
    }
}
