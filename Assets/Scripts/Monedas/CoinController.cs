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

        //Eventos y Delegados
        GameController.respawn += Respawn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isActiva)
        {
            GameController.sumaMonedas();
            sprMoneda.enabled = false;
            particulas.Play();
            isActiva = false;
            snd_Moneda.Play();//Hacer sonido de la moneda
        }  
    }

    private void Respawn()
    {
        isActiva = true;
        gameObject.SetActive(true);
        sprMoneda.enabled = true;
    }

    private void OnDestroy()
    {
        GameController.respawn -= Respawn;
    }
}
