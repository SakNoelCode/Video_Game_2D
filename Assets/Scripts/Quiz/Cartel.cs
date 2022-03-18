using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartel : MonoBehaviour
{


    [SerializeField] private GameObject canvasQuiz;
    [SerializeField] private GameObject cartel;
    [SerializeField] private GameObject player;

    private Rigidbody2D rbPlayer;
    private Animator aPlayer;

    public static bool colisionWithPlayer = false;

    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        rbPlayer = player.GetComponent<Rigidbody2D>();
        aPlayer = player.GetComponent<Animator>();
    }

    private void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            colisionWithPlayer = true;
            rbPlayer.velocity = Vector3.zero;
            rbPlayer.gravityScale = 0;
            aPlayer.Play("quieto");            
            GameController.PausarGame();
            canvasQuiz.SetActive(true);

            Debug.Log("Colisión hecha" + colisionWithPlayer);
            //cartel.SetActive(false);

        }
    }
}
