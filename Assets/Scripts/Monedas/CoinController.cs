using UnityEngine;

public class CoinController : MonoBehaviour
{

    private ParticleSystem particulas;
    private SpriteRenderer sprMoneda;
    private bool isActiva = true;


    private void Awake()
    {
        particulas = GetComponent<ParticleSystem>();
        sprMoneda = GetComponent<SpriteRenderer>();
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
        }  
    }
}
