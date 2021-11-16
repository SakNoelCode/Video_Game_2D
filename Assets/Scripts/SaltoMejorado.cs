using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltoMejorado : MonoBehaviour
{
    public float saltoLargo = 1.3f;
    public float saltoCorto = 1f;

    Rigidbody2D rb;
    
    void Start()
    {
        //Obtener el componente del RigiBody
        rb = GetComponent<Rigidbody2D>();        
    }

   
    //Aplicar el salto largo y corto al Player
    void Update()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * saltoLargo * Time.deltaTime;
        }else if(rb.velocity.y > 0 && !Input.GetButton("Jump")){
            rb.velocity += Vector2.up * Physics2D.gravity.y * saltoCorto * Time.deltaTime;
        }
        
    }
}
