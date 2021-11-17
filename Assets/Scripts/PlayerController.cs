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
    [SerializeField] private float addRayo;
    [SerializeField] private float anguloMax;
    [SerializeField] private PhysicsMaterial2D sinF;
    [SerializeField] private PhysicsMaterial2D maxF;


    [Header("Valores Informátivas")]
    [SerializeField] private bool tocaSuelo = false;
    [SerializeField] private bool enPendiente;


    //Variables Capsule Colider para recoger los valores de la cápsula (Método chechPendiente)
    private CapsuleCollider2D ccPlayer;
    private Vector2 ccSize;

    //Variables booleanas y float para los rayos de las pendientes (Método chechPendiente)
    private float anguloLateral;
    private float anguloPendiente;
    private float anguloAnterior;
    private Vector2 anguloPerpendicular;

    

    //Variable para poder modificar las propiedades de Player
    private Rigidbody2D rPlayer;

    //Variable para poder obtener el valor de las animaciones
    private Animator aPlayer;

    //Variable flotante que tomara el valor del eje horizontal valores ( 1 , -1)
    private float horizontal;

    //Variable que tendrá dos valores(x,y) y será encargada de la velocidad del Player
    private Vector2 nuevaVelocidad;

    //Variable inicializada con valor de True, el personaje inicia mirando a la derecha
    private bool miraDerecha = true;

    //Variable que se usa para verificar si el personaje esta saltando
    private bool estaSaltando = false;

    //Variable que se usa para verificar si el personaje puede saltar
    private bool puedoSaltar = false;

    //Variable que se usa para verificar si el personaje puede caminar
    private bool puedoCaminar;



    //---------------------------------------Método Start---------------------------------------------------
    void Start()
    {
        //a rPlayer le asignamos el componente rigiBody, podremos modificar las propiedades de Player
        rPlayer = GetComponent<Rigidbody2D>();

        //a Player se le asigna el componente Animator, podremos modificar las propiedades Animator Player
        aPlayer = GetComponent<Animator>();

        //Obtener el Capsule Colider y el tamaño(Método chechPendiente)
        ccPlayer = GetComponent<CapsuleCollider2D>();
        ccSize = ccPlayer.size;
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
        checkPendiente();
        movimientoPlayer();
    }

     
    private void movimientoPlayer()
    {
        //Player en el suelo
        if(tocaSuelo && !estaSaltando && !enPendiente)
        {
            nuevaVelocidad.Set(velocidad * horizontal, 0.0f);
            rPlayer.velocity = nuevaVelocidad;

        //Player en una pendiente
        }else if(tocaSuelo && !estaSaltando && puedoCaminar && enPendiente)
        {
            nuevaVelocidad.Set(velocidad * anguloPerpendicular.x * -horizontal,velocidad * anguloPerpendicular.y * -horizontal);
            rPlayer.velocity = nuevaVelocidad;
        }

        //Player Saltando
        else if (!tocaSuelo)
        {
            nuevaVelocidad.Set(velocidad * horizontal, rPlayer.velocity.y);
            rPlayer.velocity = nuevaVelocidad;
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


    //Función que permite verificar si el Player esta tocando el suelo
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


    //Función que permite verificar si el Player esta en una pendiente
    private void checkPendiente()
    {

        //----------------------------------COLISIONES HORIZONTALES-------------------------------
        //Obtener la posición de los pies
        Vector2 posPies = transform.position - (Vector3)(new Vector2(0.0f, ccSize.y / 2));

        //Crear un rayo hacia la derecha
        RaycastHit2D hitDelante = Physics2D.Raycast(posPies, Vector2.right, addRayo, capaSuelo);

        //Crear un rayo hacia la izquierda
        RaycastHit2D hitDetras = Physics2D.Raycast(posPies, -Vector2.right, addRayo, capaSuelo);

        //Pintar los Rayos en la escena
        Debug.DrawRay(posPies, Vector2.right * addRayo, Color.cyan);
        Debug.DrawRay(posPies, -Vector2.right * addRayo, Color.red);

        if (hitDelante)
        {
            enPendiente = true;
            anguloLateral = Vector2.Angle(hitDelante.normal, Vector2.up);
        } else if (hitDetras)
        {
            enPendiente = true;
            anguloLateral = Vector2.Angle(hitDetras.normal, Vector2.up);
        }
        else
        {
            enPendiente = false;
            anguloLateral = 0;
        }

        //----------------------------------COLISIONES VERTICALES-------------------------------
        RaycastHit2D hitVertical = Physics2D.Raycast(posPies, Vector2.down, addRayo, capaSuelo);

        if (hitVertical)
        {
            anguloPendiente = Vector2.Angle(hitVertical.normal, Vector2.up);
            anguloPerpendicular = Vector2.Perpendicular(hitVertical.normal).normalized;
            if (anguloPendiente != anguloAnterior) enPendiente = true;
            anguloAnterior = anguloPendiente;
            //Pintar los rayos en escena
            Debug.DrawRay(hitVertical.point, anguloPerpendicular, Color.blue);
            Debug.DrawRay(hitVertical.point, hitVertical.normal, Color.green);
        }


        //----------------------------------OTRAS COMPROBACIONES-------------------------------
        //Si el angulo es mayor que el anguloMax, no se podrá caminar
        if (anguloPendiente > anguloMax || anguloLateral > anguloMax) puedoCaminar = false;
        else puedoCaminar = true;
        
        //Comprobar pendientes y asignar un tipo de material
        if (enPendiente && puedoCaminar && horizontal == 0.0f) rPlayer.sharedMaterial = maxF;
        else rPlayer.sharedMaterial = sinF;   

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
     
    //Para dibujar el Gizmo en escena 
   private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.position, checkGroundRadio);
    }
} 
