using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Variables
    public float velocidad;
    public float velocidadMax;
    public float fuerzaSalto;

    //Variable del colisionador Pies
    public bool colPies = false;

    //Variable para poder modificar las propiedades de Player
    private Rigidbody2D rPlayer;

    //Variable que toma el valor horizontal
    private float horizontal;

    //Variable inicializada con valor de True, el personaje inicia mirando a la derecha
    private bool miraDerecha = true;
    
    void Start()
    {
        //a rPlayer le asignamos el componente rigiBody
        rPlayer = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        //Enviamos al método girarPlayer el valor del eje horizontal
        girarPlayer(horizontal);

        //Código para el salto
        //Comprobar si estamos tocando Terreno
        colPies = Pies.colPies;

        if (Input.GetButton("Jump") && colPies)//Si seleccionamos la opcion de saltar
        {
            //Cambiamos la velocidad del Player en Y  en 0, de manera que si tuviera algun valor este se anule
            rPlayer.velocity = new Vector2(rPlayer.velocity.x,0f);

            //Añadimos la fuerza del salto al Player, el tipo de fuerza establecido como impulso
            rPlayer.AddForce(new Vector2(0, fuerzaSalto),ForceMode2D.Impulse); 
        }

         

    }

    //Método encargado del sistema de fisicas de Unity
    void FixedUpdate()
    {
        //La variable horizontal toma el valor del eje horizontal
        horizontal = Input.GetAxisRaw("Horizontal");

        //a rPlayer se le añade una fuerza hacia la derecha 
        rPlayer.AddForce(Vector2.right * velocidad * horizontal);

        //Creamos un variable para que la velocidad tenga un valor mínimo y un valor máximo
        float limiteVelocidad = Mathf.Clamp(rPlayer.velocity.x, -velocidadMax,velocidadMax);

        //Asignamos esa velocidad al Player, la velocidad en el eje Y, lo dejamos tál cuál esta
        rPlayer.velocity = new Vector2(limiteVelocidad,rPlayer.velocity.y);
    }

    //Método para girar al Player de Derecha a Izquierda o Viceversa
    void girarPlayer(float h)
    {
        if( (h>0 && !miraDerecha) || (h<0 && miraDerecha) )
        {
            miraDerecha = !miraDerecha;
            Vector3 escalaGiro = transform.localScale;
            escalaGiro.x = escalaGiro.x * -1;
            transform.localScale = escalaGiro;
        }
    }
} 
