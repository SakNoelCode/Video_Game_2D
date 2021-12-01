using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject camera;

    private AudioSource musicFondo;

    // Start is called before the first frame update
    void Start()
    {
        musicFondo = camera.GetComponent<AudioSource>();
        musicFondo.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
