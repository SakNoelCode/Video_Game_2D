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
    [SerializeField] private int vida=3;


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

    private Vector3 posIni;

    

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

    private bool enPlataforma = false;  //estado Plataforma



    //---------------------------------------Método Start---------------------------------------------------
    void Start()
    {
        //Asignar al jugador la posicion Inicial
        posIni = transform.position;


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
            nuevaVelocidad.Set(velocidad * horizontal, rPlayer.velocity.y);
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
        //Colocar al Player en la posicion Inicial
        if (Input.GetKey(KeyCode.R)) Reaparece();

        //La variable horizontal toma el valor del eje horizontal
        horizontal = Input.GetAxisRaw("Horizontal");

        //Se comprueba una condición para girar a rPlayer
        if ((horizontal > 0 && !miraDerecha) || (horizontal < 0 && miraDerecha)) girarPlayer();

        //Si se oprime el boton se Salto se ejecuta la función Salto
        if (Input.GetButton("Jump") && puedoSaltar && tocaSuelo) Salto();

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


    //----------------------------------------CHECK PENDIENTE------------------------------------------

    private void checkPendiente()
    {
        if (!enPlataforma) { 
        Vector2 posPies = transform.position - (Vector3)(new Vector2(0.0f, ccSize.y / 2));
        checkPenHorizontal(posPies);
            checkPenVertical(posPies);
        }
    }


     private void checkPenHorizontal(Vector2 posPies)
    {
        //Crear un rayo hacia la derecha
        RaycastHit2D hitDelante = Physics2D.Raycast(posPies, Vector2.right, addRayo, capaSuelo);

        //Crear un rayo hacia la izquierda
        RaycastHit2D hitDetras = Physics2D.Raycast(posPies, -Vector2.right, addRayo, capaSuelo);

        //Pintar los Rayos en la escena
        Debug.DrawRay(posPies, Vector2.right * addRayo, Color.cyan);
        Debug.DrawRay(posPies, -Vector2.right * addRayo, Color.red);

        if (hitDelante)
        {
            anguloLateral = Vector2.Angle(hitDelante.normal, Vector2.up);
            if (anguloLateral > 0)enPendiente = true;
        }
        else if (hitDetras)
        {
            anguloLateral = Vector2.Angle(hitDetras.normal, Vector2.up);
            if (anguloLateral > 0)enPendiente = true;
        }
        else  
        {
            enPendiente = false; 
            anguloLateral = 0.0f;
        }

    }

    private void checkPenVertical(Vector2 posPies)
    {
        RaycastHit2D hitVertical = Physics2D.Raycast(ccPlayer.bounds.center, Vector2.down, ccPlayer.bounds.extents.y + addRayo, capaSuelo);

        if (hitVertical)
        {
            anguloPendiente = Vector2.Angle(hitVertical.normal, Vector2.up);
            anguloPerpendicular = Vector2.Perpendicular(hitVertical.normal).normalized;
            if (anguloPendiente != anguloAnterior && anguloPendiente >0) enPendiente = true;
            anguloAnterior = anguloPendiente; 
            //Pintar los rayos en escena
            Debug.DrawRay(hitVertical.point, anguloPerpendicular, Color.blue);
            Debug.DrawRay(hitVertical.point, hitVertical.normal, Color.green);
        }

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



    //----------------------------------DETECCION DE PLATAFORMAS MOVILES---------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlataformaMovil")
        {  
            rPlayer.velocity = Vector3.zero;
            transform.parent = collision.transform; //Hereda tipo de colision
            enPlataforma = true;                    //Cambio de estado
        }
    }
     
    private void OnCollisionExit2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "PlataformaMovil")
        {
            transform.parent = null;               //Herencia Nula
            enPlataforma = false;                  //Cambio de estado
        }
    }


    //-------------------------DETECCION DE COLISIONES PARA EL PLAYER------------------
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.tag == "Pinchos")//COLISION CON LOS PINCHOS
        {
            Debug.Log("Quita Salud");
            pierdeVida();
        }

        if(collision.gameObject.tag == "CaidaVacio")//COLISION CON CAIDA AL VACIO
        {
            Debug.Log("Muerte por caida al vacio");
            pierdeVida(); 
        }
    }

    private void pierdeVida()
    {
        Debug.Log("estoy perdiendo vida");
        Reaparece();
    }

    private void Reaparece()
    {
        transform.position = posIni;
        rPlayer.velocity = Vector3.zero;
    }









}
