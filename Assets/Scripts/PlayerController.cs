using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //---------------------------------------Variables---------------------------------------------------
     
    //[SerializeField] ==> Nos permite modificar el valor en el editor de  Unity
    [Header("Valores Configurables")]
    [SerializeField] private float velocidad;
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private bool isSaltoMejorado;
    [SerializeField] private float saltoLargo = 1.3f;
    [SerializeField] private float saltoCorto = 1f;
    [SerializeField] private Transform checkGround;
    [SerializeField] private float checkGroundRadio;
    [SerializeField] private LayerMask capaSuelo;

    
    [Header("Valores de Referencia")]


    //Variable para verificar si el player toca el suelo
    private bool tocaSuelo = false;

    //Variable para poder modificar las propiedades de Player
    private Rigidbody2D rPlayer;

    //Variable para poder obtener el valor de las animaciones
    private Animator aPlayer;

    //Variable flotante que tomara el valor del eje horizontal
    private float horizontal;

    //Variable que tendrá dos valores(x,y) y será encargada de la velocidad del Player
    private Vector2 nuevaVelocidad;

    //Variable inicializada con valor de True, el personaje inicia mirando a la derecha
    private bool miraDerecha = true;

    //Variable que se usa para verificar si el personaje esta saltando
    private bool estaSaltando = false;

    //Variable que se usa para verificar si el personaje puede saltar
    private bool puedoSaltar = false;



    //---------------------------------------Método Start---------------------------------------------------
    void Start()
    {
        //a rPlayer le asignamos el componente rigiBody, podremos modificar las propiedades de Player
        rPlayer = GetComponent<Rigidbody2D>();

        //a Player se le asigna el componente Animator, podremos modificar las propiedades Animator Player
        aPlayer = GetComponent<Animator>();
    }


    //---------------------------------------Método Update---------------------------------------------------
    void Update()
    {       
        recibePulsaciones();
        variablesAnimador();
    }


    //---------------------------------------Método Fixed Update---------------------------------------------------
    //Método encargado del sistema de fisicas de Unity
    void FixedUpdate()
    {
        checkTocaSuelo();
        movimientoPlayer();
    }

     
    private void movimientoPlayer()
    {
        //Player en el suelo
        if(tocaSuelo && !estaSaltando)
        {
            nuevaVelocidad.Set(velocidad * horizontal, 0.0f);
            rPlayer.velocity = nuevaVelocidad;
        }

        //Player Saltando
        else
        {
            if (!tocaSuelo)
            {
                nuevaVelocidad.Set(velocidad * horizontal, rPlayer.velocity.y);
                rPlayer.velocity = nuevaVelocidad;
            }     
        }
        
    }


    //Método para controlar las teclas que pulsa el jugador
    private void recibePulsaciones()
    {
        //La variable horizontal toma el valor del eje horizontal
        horizontal = Input.GetAxisRaw("Horizontal");

        //Se comprueba una condición para girar a rPlayer
        if ((horizontal > 0 && !miraDerecha) || (horizontal < 0 && miraDerecha)) girarPlayer();

        //Si se oprime el boton se Salto se ejecuta la función Salto
        if (Input.GetButton("Jump") && puedoSaltar) Salto();

        //Si la variable isSaltoMejorado es verdadera, se ejecuta el salto Mejorado
        if (isSaltoMejorado) SaltoMejorado();
    }


    //Funcion que permite a Player ejecutar el salto
    private void Salto()
    {
        estaSaltando = true;
        puedoSaltar = false;

        //Cambiamos la velocidad del Player en Y  en 0, de manera que si tuviera algun valor este se anule
        rPlayer.velocity = new Vector2(rPlayer.velocity.x, 0f);

        //Añadimos la fuerza del salto al Player, el tipo de fuerza establecido como impulso
        rPlayer.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
    }


    //Función que permite hacer el salto mejorado
    private void SaltoMejorado()
    {
        if (rPlayer.velocity.y < 0)
        {
            rPlayer.velocity += Vector2.up * Physics2D.gravity.y * saltoLargo * Time.deltaTime;
        }
        else if (rPlayer.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rPlayer.velocity += Vector2.up * Physics2D.gravity.y * saltoCorto * Time.deltaTime;
        }
    }


    //Función que permite verificar si el Player esta saltando o si puede Saltar
    private void checkTocaSuelo()
    {
        tocaSuelo = Physics2D.OverlapCircle(checkGround.position, checkGroundRadio, capaSuelo);
        if(rPlayer.velocity.y <= 0f)
        {
            estaSaltando = false;
        }
        if(tocaSuelo && !estaSaltando)
        {
            puedoSaltar = true;
        }
    }

   
    //Recoger los valores de las animaciones
    private void variablesAnimador()
    {
        //Pasar la velocidad del jugador a la variable Velocidad en el eje X
        aPlayer.SetFloat("VelocidadX", Mathf.Abs(rPlayer.velocity.x));

        //Pasar la velocidad del jugador a la variable Velocidad en el eje Y
        aPlayer.SetFloat("VelocidadY", (rPlayer.velocity.y));

        //Asignar el valor de tocaSuelo a la variable TocaSuelo
        aPlayer.SetBool("TocaSuelo", tocaSuelo);

        //Asignar el valor de estaSaltando a la variable EstaSaltando
        aPlayer.SetBool("EstaSaltando", estaSaltando);
    }



    //Método para girar al Player de Derecha a Izquierda o Viceversa
    void girarPlayer()
    {        
        miraDerecha = !miraDerecha;
        Vector3 escalaGiro = transform.localScale;
        escalaGiro.x = escalaGiro.x * -1;
        transform.localScale = escalaGiro;
    }


   /* private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.position, checkGroundRadio);
    }*/
} 
