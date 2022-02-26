using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{

    private AudioSource snd;
    // Start is called before the first frame update
    void Start()
    {
        snd = GetComponent<AudioSource>();
        
    }

    private void Sonido()
    {
        snd.Play();
    }
     
    private void puertaAbierta()
    {
        GameController.permitePaso();
        snd.Stop(); 
    }
}
