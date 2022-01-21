using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{

    //Delegado y Eventos
    public delegate void MiDelegado();
    public event MiDelegado PlayerMuerto;

    [Header("Valores del Personaje")]
    [SerializeField] private float      velocidadPlayer;
    [SerializeField] private float      fuerzaSaltoPlayer;
    [SerializeField] private bool       saltoMejorado;
    [SerializeField] private float      saltoLargo;
    [SerializeField] private float      saltoCorto;
    [SerializeField] private Transform  checkGround;
    [SerializeField] private float      checkGroundRadio;
    [SerializeField] private LayerMask  capaSuelo;
    [SerializeField] private float      fuerzaToqueEnemigo;
    [SerializeField] private int        vidaPlayer = 3;


    [Header("Valores informativos del Personaje")] 
    [SerializeField] private bool isSaltando = false;
    [SerializeField] private bool isPuedoSaltar = false;
    [SerializeField] private bool isTocaSuelo = false;

    [Header("Barra de Vida")]
    [SerializeField] private GameObject barraVida;
    [SerializeField] private Sprite sprVida3, sprVida2, sprVida1, sprVida0;

    [Header("Efectos de Sonido")]
    [SerializeField] private GameObject objSaltoPlayer;
    [SerializeField] private GameObject objMuertePlayer;

    [Header("Moneda Para Puerta")]
    [SerializeField] private GameObject monedaParaPuerta;


    //Variables auxiliares
    private Vector2    nuevaVelocidad;
    private Color      colorInicialPlayer;

    //Variables para acceder a las propiedades del personaje
    private CapsuleCollider2D capsulecoliderPlayer; //ccPlayer
    private SpriteRenderer    spritePlayer;  //sPlayer
    private Rigidbody2D       rigibodyPlayer; //rPlayer
    private float             ejeHorizontal;        //h
    private bool              isMirandoDerecha = true;
    public static Vector3           posInicialPlayer;
    private Camera            camara;

    //Variables para las animaciones
    private Animator animatorPlayer;   //aPlayer

    //Variables para colision con el enemigo
    private bool isTocado = false;

    //Variable para saber si el Player esta muerto
    private bool isMuerto = false;

    //Calcular valores para la camara (Recarga escena)
    private float posPlayer, altCamara, altPlayer;

    //Variables para obtener los sonidos del Player
    private AudioSource asSaltoPlayer, asMuertePlayer;

    //Variable para permitir al personaje saltar o no
    private bool noSaltes = false;

    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        posInicialPlayer = transform.position;
        rigibodyPlayer = GetComponent<Rigidbody2D>();
        animatorPlayer = GetComponent<Animator>();
        spritePlayer = GetComponent<SpriteRenderer>();
        colorInicialPlayer = spritePlayer.color;
        capsulecoliderPlayer = GetComponent<CapsuleCollider2D>();
        camara = Camera.main;

        altCamara = camara.orthographicSize * 2;
        altPlayer = GetComponent<Renderer>().bounds.size.y;

        asSaltoPlayer = objSaltoPlayer.GetComponent<AudioSource>();
        asMuertePlayer = objMuertePlayer.GetComponent<AudioSource>();

        GameController2.respawn += Respawn;
    }


    //------------------------------------------METODO UPDATE-----------------------------------
    void Update()
    {
        animatorPlayer.SetBool("GameOn", GameController2.gameOn);
        if (GameController2.gameOn)
        {
            recibePulsaciones();
            asignarValoresAnimaciones();
        }
    }


    //------------------------------------------METODO FIXED UPDATE-----------------------------------
    void FixedUpdate()
    {
        if (GameController2.gameOn)
        {
            comprobarSiTocamosSuelo();
            if (!isTocado) moverPlayer();
        }
    }


    //------------------------------------------METODO RESPAWN-----------------------------------
    private void Respawn()
    {
        rigibodyPlayer.velocity = Vector2.zero;
        animatorPlayer.Play("quieto");
        if (!isMirandoDerecha) girarPlayer(1);
        if (capsulecoliderPlayer.enabled == false) capsulecoliderPlayer.enabled = true;
        isMuerto = false;
        transform.parent = null;
        transform.position = posInicialPlayer;

        //Vida y barra de vida
        barraVida.GetComponent<Image>().sprite = sprVida3;
        vidaPlayer = 3;
        
    }


    private void recibePulsaciones()
    {
        if (Input.GetKey(KeyCode.R)) GameController2.playerMuerto = true; 
         
        ejeHorizontal = Input.GetAxisRaw("Horizontal");

        //Para girar al Player
        girarPlayer(ejeHorizontal);

        //Para hacer saltar al Player
        saltarPlayer();

        //Salto Mejorado
        if (saltoMejorado) saltoMejoradoPlayerController();
    }


    private void girarPlayer(float ejeHorizontal)
    {
        if ((ejeHorizontal > 0 && !isMirandoDerecha) || (ejeHorizontal < 0 && isMirandoDerecha))
        {
            isMirandoDerecha = !isMirandoDerecha;
            Vector3 giro = transform.localScale;
            giro.x = giro.x * -1;
            transform.localScale = giro;
        }
    }

    private void moverPlayer()
     {
        if (isTocaSuelo && !isSaltando) //Velocidad en el Suelo
        {
            nuevaVelocidad.Set(velocidadPlayer * ejeHorizontal, rigibodyPlayer.velocity.y);
            rigibodyPlayer.velocity = nuevaVelocidad;
        }
        else
        {
            if (!isTocaSuelo) //Velocidad cuando salta
            {
                nuevaVelocidad.Set(velocidadPlayer * ejeHorizontal, rigibodyPlayer.velocity.y);
                rigibodyPlayer.velocity = nuevaVelocidad;
            }            
        }
     }

    private void saltarPlayer()
    {
        if (Input.GetButtonDown("Jump") && isPuedoSaltar && isTocaSuelo && !noSaltes)
        {
            isSaltando = true;
            isPuedoSaltar = false;
            rigibodyPlayer.velocity = new Vector2(rigibodyPlayer.velocity.x, 0f);//Anular cualquier velocidad en el ejer Y
            rigibodyPlayer.AddForce(new Vector2(0, fuerzaSaltoPlayer), ForceMode2D.Impulse);

            //Aplicar efecto de sonido (Jump)
            asSaltoPlayer.Play();
        }
    }

    private void comprobarSiTocamosSuelo()
    {
        isTocaSuelo = Physics2D.OverlapCircle(checkGround.position, checkGroundRadio, capaSuelo);

        if (rigibodyPlayer.velocity.y <= 0f)
        {
            isSaltando = false;

            //para comprobar si el enemigo nos toca
            if (isTocado && isTocaSuelo)
            {
                rigibodyPlayer.velocity = Vector2.zero;
                isTocado = false;
                spritePlayer.color = colorInicialPlayer;
            }
        }
        if(isTocaSuelo && !isSaltando)
        {
            isPuedoSaltar = true;     
        }
    }

    private void asignarValoresAnimaciones()
    {
        animatorPlayer.SetFloat("velocidadX", Mathf.Abs(rigibodyPlayer.velocity.x));
        animatorPlayer.SetFloat("velocidadY", rigibodyPlayer.velocity.y);
        animatorPlayer.SetBool("isTocaSuelo", isTocaSuelo);
    }

    private void saltoMejoradoPlayerController()
    {
        if (rigibodyPlayer.velocity.y < 0)
        {
            rigibodyPlayer.velocity += Vector2.up * Physics2D.gravity.y * saltoLargo * Time.deltaTime;
        }
        else if (rigibodyPlayer.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rigibodyPlayer.velocity += Vector2.up * Physics2D.gravity.y * saltoCorto * Time.deltaTime;
        }
    }

    private void recargarEscena()
    {
        posPlayer = camara.transform.InverseTransformDirection(transform.position - camara.transform.position).y;

        if(posPlayer < ((altCamara/2)*-1) - (altPlayer/2) )
        {
           Invoke("llamaFuncionRecarga", 1);
            isMuerto = false;
        }
    }

    /*private void llamaFuncionRecarga()
    {
        asMuertePlayer.Play();
        GameController2.playerMuerto = true;
    }*/


    //--------------------------------------GIZMOS--------------------------------------------  
    //Dibujar una esfera para detectar cuando se toca Tierra
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.position, checkGroundRadio);//Dibujar una esfera
    }


    //------------------------------DETECCION MEDIANTE TAGS Y COLISIONES---------------------------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlataformaMovil")
        {
            rigibodyPlayer.velocity = Vector3.zero;
            transform.parent = collision.transform; //Heredamos el transform de la plataforma que colisionemos
            //inPlataforma = true;
        }

        if (collision.gameObject.tag == "EnemigoPupa")
        {
            tocarEnemigo(collision.transform.position.x);
        }

        if (collision.gameObject.tag == "ChepaEnemigo" && !isTocado)
        {
            //Impulsar hacia arriba
            rigibodyPlayer.velocity = Vector2.zero;
            rigibodyPlayer.AddForce(new Vector2 (0f, 10f), ForceMode2D.Impulse);
            collision.gameObject.SendMessage("Muere");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlataformaMovil")
        {
            transform.parent = null;
            //inPlataforma = false;
        }
    }


    //------------------------------DETECCION CON TRIGGERS-----------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Pinchos" )  //DETECCION PINCHOS
        {
            muertePlayer(true);
        }

        if(collision.gameObject.tag == "CaidaVacio") //DETECCION CAIDA VACIO
        {
            muertePlayer(false);
        }
        if (collision.gameObject.tag == "FinNivel") //DETECCION CON FIN DE NIVEL
        {
            GameController2.gameOn = false;
            rigibodyPlayer.velocity = Vector3.zero;
            GameController2.finNivel();
        }
    }


    //------------------------------DETECCION CON LA  PUERTA FINAL-----------------------------------------------
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NoSaltar")
        {
            noSaltes = true;
        }
        if (collision.gameObject.tag == "SueltaMonedas" && !GameController2.isSoltandoMonedas && GameController2.monedas > 0
            && GameController2.monedasPuerta > 0)
        {
            GameController2.isSoltandoMonedas = true;
            Instantiate(monedaParaPuerta, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NoSaltar")
        {
            noSaltes = false;
        }
        
    }


    private void tocarEnemigo(float posX)
    {
        if (!isTocado)
        {
            if (vidaPlayer > 1)
            {
                //Cambiar color
                Color nuevoColor = new Color(255f / 255, 100f / 255, 100f / 255);
                spritePlayer.color = nuevoColor;

                isTocado = true;
                float lado = Mathf.Sign(posX - transform.position.x);
                rigibodyPlayer.velocity = Vector2.zero;
                rigibodyPlayer.AddForce(new Vector2(fuerzaToqueEnemigo * -lado, fuerzaToqueEnemigo), ForceMode2D.Impulse);
                vidaPlayer--;
                CambiaBarraVida(vidaPlayer);
            }
            else
            {
                muertePlayer(true);
            }
        }
    }

    private void CambiaBarraVida(int salud)
    {
        if (salud == 2) barraVida.GetComponent<Image>().sprite = sprVida2;
        if (salud == 1) barraVida.GetComponent<Image>().sprite = sprVida1;
    }

    private void muertePlayer(bool anim)
    {
        asMuertePlayer.Play();  //Efecto de Sonido Muerte
        barraVida.GetComponent<Image>().sprite = sprVida0; //Cambiar al Sprite Vida0  
        if (anim)
        {
            animatorPlayer.Play("Muerte");  //Animaci�n de muerte
                                            
            rigibodyPlayer.velocity = Vector2.zero;
            rigibodyPlayer.AddForce(new Vector2(0.0f, fuerzaSaltoPlayer), ForceMode2D.Impulse);//Cuando nos matan, hacemos un salto
        }
        GameController2.gameOn = false; //Detener el juego

        capsulecoliderPlayer.enabled = false; //Desactivar el Colider
        isMuerto = true;

        //Lanzar el evento de Delegado
        PlayerMuerto?.Invoke(); 
    }

    private void OnDisable()
    {
        GameController2.respawn -= Respawn;
    }

}
