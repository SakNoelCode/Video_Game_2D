using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoSimple : MonoBehaviour
{
    [SerializeField] private Transform[] puntosMov;
    [SerializeField] private float velocidad;

    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Poner velocidad al enemigo
        transform.position = Vector2.MoveTowards(transform.position, puntosMov[i].transform.position, velocidad*Time.deltaTime);

        

    }
}
