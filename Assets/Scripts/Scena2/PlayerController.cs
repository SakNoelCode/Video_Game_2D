using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    //Delegado y Eventos
    public delegate void MiDelegado();
    public event MiDelegado PlayerMuerto;

    //Para métodos estaticos
    //static PlayerController current;

    [Header("Valores del Personaje")]
    [SerializeField] private float velocidadPlayer;
    [SerializeField] private float fuerzaSaltoPlayer;
    [SerializeField] private bool saltoMejorado;
    [SerializeField] private float saltoLargo;
    [SerializeField] private float saltoCorto;
    [SerializeField] private float checkGroundRadio;
    [SerializeField] private float fuerzaToqueEnemigo;
    [SerializeField] private int vidaPlayer = 3;
    [SerializeField] private float addRayoDebajo;
    [SerializeField] private float tiempoCoyoteTime = 0.1f;
    [SerializeField] private float tiempoBufferSalto = 0.2f;

    [Header("Objetos")]
    [SerializeField] private GameObject monedaParaPuerta;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private LayerMask capaEscalera;
    [SerializeField] private Transform checkGround;

    [Header("Partículas")]
    [SerializeField] private ParticleSystem polvoPies;
    [SerializeField] private ParticleSystem polvoSalto;

    [Header("Valores informativos del Personaje")]
    [SerializeField] private bool isSaltando = false;
    [SerializeField] private bool isPuedoSaltar = false;
    [SerializeField] private bool isTocaSuelo = false;
    [SerializeField] private bool coyoteTime = false;

    [Header("Barra de Vida")]
    [SerializeField] private GameObject barraVida;
    [SerializeField] private Sprite sprVida3, sprVida2, sprVida1, sprVida0;

    [Header("Efectos de Sonido")]
    [SerializeField] private GameObject objSaltoPlayer;
    [SerializeField] private GameObject objMuertePlayer;
    [SerializeField] private GameObject objGolpeEnemigo;
    [SerializeField] private GameObject objMuerteEnemigo;

    //Componentes
    private Rigidbody2D rigibodyPlayer; //rPlayer
    private Animator animatorPlayer;   //aPlayer //Variables para las animaciones
    private CapsuleCollider2D capsulecoliderPlayer; //ccPlayer
    private SpriteRenderer spritePlayer;  //sPlayer                           
    private AudioSource asSaltoPlayer, asMuertePlayer,asGolpeEnemigo,asMuerteEnemigo;//Variables para obtener los sonidos

    //Obtención del movimiento Horizontal y Vertical
    private float ejeHorizontal;        //h
    private float ejeVertical;          //v

    //Variables de Comprobación
    private bool isMirandoDerecha = true;
    private bool isTocado = false;//Variables para colision con el enemigo
    //private bool isMuerto = false;//Variable para saber si el Player esta muerto
    private bool noSaltes = false;//Variable para permitir al personaje saltar o no
    private bool isCayendo = false;

    //Accesibles desde otros Scripts
    public static Vector3 posInicialPlayer;
    public static bool enEscalera = false;

    //OTRAS
    private Vector2 ccSize;   //Tamaño del Player
    private Vector2 nuevaVelocidad;
    private Color colorInicialPlayer;
    private float dirX = 1;
    private float gravedad;
    private float tiempoCoyote; //Coyote Time
    private float tiempoBuffer; //Buffer Salto

    //Partículas
    private ParticleSystem.EmissionModule emisionPolvoPies;

    //Variables para la escalera
    private GameObject escaleraActiva;

    private void Awake()
    {
        //scapePulsada = false;
    }

    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        rigibodyPlayer = GetComponent<Rigidbody2D>();
        animatorPlayer = GetComponent<Animator>();
        capsulecoliderPlayer = GetComponent<CapsuleCollider2D>();
        spritePlayer = GetComponent<SpriteRenderer>();
        ccSize = capsulecoliderPlayer.size;
        colorInicialPlayer = spritePlayer.color;
        posInicialPlayer = transform.position;
        gravedad = rigibodyPlayer.gravityScale;

        //camara = Camera.main;
        //altCamara = camara.orthographicSize * 2;
        //altPlayer = GetComponent<Renderer>().bounds.size.y;

        asSaltoPlayer = objSaltoPlayer.GetComponent<AudioSource>();
        asMuertePlayer = objMuertePlayer.GetComponent<AudioSource>();
        asGolpeEnemigo = objGolpeEnemigo.GetComponent<AudioSource>();
        asMuerteEnemigo = objMuerteEnemigo.GetComponent<AudioSource>();

        GameController.respawn += Respawn;

        //Sistema de Partículas
        emisionPolvoPies = polvoPies.emission;
    }


    //------------------------------------------METODO UPDATE-----------------------------------
    void Update()
    {
        animatorPlayer.SetBool("GameOn", GameController.gameOn);
        if (GameController.gameOn)
        {
            ValidaCoyoteTime();
            ControlBufferSalto();
            recibePulsaciones();
            asignarValoresAnimaciones();
            checkPolvoPies();
        }

        //Detectar final del Quizz
        if (QuizController.finQuizz)
        {
            continuarPlayer();
        }
        

    }


    //------------------------------------------METODO FIXED UPDATE-----------------------------------
    void FixedUpdate()
    {
        if (GameController.gameOn)
        {
            checkEscalera();
            comprobarSiTocamosSuelo();
            if (!enEscalera && !isTocado) moverPlayer();
        }
    }


    //------------------------------------------METODO RESPAWN-----------------------------------
    private void Respawn()
    {
        rigibodyPlayer.velocity = Vector2.zero;
        animatorPlayer.Play("quieto");
        if (!isMirandoDerecha) girarPlayer(1);
        if (capsulecoliderPlayer.enabled == false) capsulecoliderPlayer.enabled = true;
        //isMuerto = false;
        transform.parent = null;
        transform.position = posInicialPlayer;

        //Vida y barra de vida
        barraVida.GetComponent<Image>().sprite = sprVida3;
        vidaPlayer = 3;

    }


    private void recibePulsaciones()
    {
        ejeHorizontal = Input.GetAxisRaw("Horizontal");

        ejeVertical = Input.GetAxisRaw("Vertical");

        //Para girar al Player
        girarPlayer(ejeHorizontal);

        //Para hacer saltar al Player
        if (!enEscalera)
        {
            if (tiempoBuffer >= 0 && isPuedoSaltar && (isTocaSuelo || coyoteTime))
            {
                saltarPlayer();
            }
            //Salto Mejorado
            if (saltoMejorado) saltoMejoradoPlayerController();
        }
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
        if (ejeVertical >= 0) saltoNormal();
        else saltoParaAbajo();
    }


    private void saltoNormal()
    {
        if (!noSaltes)
        {
            isSaltando = true;
            isPuedoSaltar = false;
            rigibodyPlayer.velocity = new Vector2(rigibodyPlayer.velocity.x, 0f);//Anular cualquier velocidad en el ejer Y
            rigibodyPlayer.AddForce(new Vector2(0, fuerzaSaltoPlayer), ForceMode2D.Impulse);

            //Aplicar efecto de sonido (Jump)
            asSaltoPlayer.Play();
            //TiempoBufferSalto
            tiempoBuffer = 0;
        }
    }

    private void saltoParaAbajo()
    {
        // Debug.Log(ejeVertical);
        RaycastHit2D hitTerreno = Physics2D.Raycast(transform.position,
                                                    Vector2.down,
                                                    (ccSize.y / 2) + addRayoDebajo,
                                                    capaSuelo);


        if (hitTerreno)
        {
            if (hitTerreno.transform.gameObject.tag == "TerrenoAtravesable")
            {
                hitTerreno.transform.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                hitTerreno.transform.gameObject.tag = "Untagged";
                hitTerreno.transform.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    private void comprobarSiTocamosSuelo()
    {
        isTocaSuelo = Physics2D.OverlapCircle(checkGround.position, checkGroundRadio, capaSuelo);

        //Verificar si tocamos Suelo y establecer los valores para el coyote Time
        if (isTocaSuelo)
        {
            coyoteTime = true;
            tiempoCoyote = 0;
        }//Fin de Implementacion del coyote Time
        else
        {
            if (rigibodyPlayer.velocity.y < 0 && !isCayendo) isCayendo = true;
        }

        if (rigibodyPlayer.velocity.y <= 0f)
        {
            isSaltando = false;
            if (isCayendo && isTocaSuelo)
            {
                polvoSalto.Play();
                isCayendo = false;
            }


            //para comprobar si el enemigo nos toca
            if (isTocado && isTocaSuelo)
            {
                rigibodyPlayer.velocity = Vector2.zero;
                isTocado = false;
                spritePlayer.color = colorInicialPlayer;
            }
        }
        if (isTocaSuelo && !isSaltando)
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


    //--------------------------------------GIZMOS--------------------------------------------  
    //Dibujar una esfera para detectar cuando se toca Tierra
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(checkGround.position, checkGroundRadio);//Dibujar una esfera
    }


    //------------------------------DETECCION MEDIANTE TAGS Y COLISIONES---------------------------------------
    //----------------------LAS COLISIONES SE COMPORTAN COMO OBJETOS QUE PODEMOS CHOCAR
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

        //Matar Enemigo
        if (collision.gameObject.tag == "ChepaEnemigo" && !isTocado)
        {
            //Impulsar hacia arriba
            rigibodyPlayer.velocity = Vector2.zero;
            rigibodyPlayer.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
            collision.gameObject.SendMessage("Muere");
            asMuerteEnemigo.Play();
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
    //---------------------------LOS TRIGGERS SE COMPORTAN COMO ACTIVAR UNA ALARMA------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pinchos")  //DETECCION PINCHOS
        {
            tocarEnemigo(collision.transform.position.x);
        }
        if (collision.gameObject.tag == "PinchosMuerte")  //DETECCION PINCHOS
        {
            muertePlayer(true);
        }

        if (collision.gameObject.tag == "CaidaVacio") //DETECCION CAIDA VACIO
        {
            muertePlayer(false);
        }
        if (collision.gameObject.tag == "FinNivel") //DETECCION CON FIN DE NIVEL
        {
            GameController.gameOn = false;
            rigibodyPlayer.velocity = Vector3.zero;
            GameController.finNivel();
        }
    }


    //------------------------------DETECCION CON LA  PUERTA FINAL-----------------------------------------------
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NoSaltar")
        {
            noSaltes = true;
            isPuedoSaltar = false;
        }
        if (collision.gameObject.tag == "SueltaMonedas" && !GameController.isSoltandoMonedas && GameController.monedas > 0
            && GameController.monedasPuerta > 0)
        {
            GameController.isSoltandoMonedas = true;
            Instantiate(monedaParaPuerta, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "NoSaltar")
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
                //Sonido
                asGolpeEnemigo.Play();

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
            animatorPlayer.Play("Muerte");  //Animación de muerte

            rigibodyPlayer.velocity = Vector2.zero;
            rigibodyPlayer.AddForce(new Vector2(0.0f, fuerzaSaltoPlayer), ForceMode2D.Impulse);//Cuando nos matan, hacemos un salto
        }
        GameController.gameOn = false; //Detener el juego

        capsulecoliderPlayer.enabled = false; //Desactivar el Colider
        //isMuerto = true;

        //Lanzar el evento de Delegado
        PlayerMuerto?.Invoke();
    }

    //-------------METODO PARA COMPROBAR SI ESTAMOS INTERACTUANDO CON UNA ESCALERA----
    private void checkEscalera()
    {
        comprobarDir();

        Vector2 posRayoDerecha = new Vector2(transform.position.x + (ccSize.x / 2) * dirX,
                                             transform.position.y - (ccSize.y / 2) + 0.01f);

        Vector2 posRayoMedio = new Vector2(transform.position.x,
                                           transform.position.y - (ccSize.y / 2) + 0.01f);

        //Zona de Debug
        //Debug.DrawRay(posRayoDerecha, Vector2.up * (ccSize.y), Color.blue);
        //Debug.DrawRay(posRayoMedio, Vector2.up * (ccSize.y), Color.blue); 

        RaycastHit2D hitRayoDerecha = Physics2D.Raycast(posRayoDerecha,
                                                        Vector2.up,
                                                        (ccSize.y - 0.01f),
                                                        capaEscalera);

        RaycastHit2D hitRayoMedio = Physics2D.Raycast(posRayoMedio,
                                                        Vector2.up,
                                                        (ccSize.y - 0.01f),
                                                        capaEscalera);

        if (hitRayoDerecha || hitRayoMedio) //Si se activan cualquier RayCast
        {
            if (hitRayoDerecha) escaleraActiva = hitRayoDerecha.transform.gameObject;
            if (hitRayoMedio) escaleraActiva = hitRayoMedio.transform.gameObject;

            //Comnprobación si pulsamos arriba y si estamos dentro de una escalera
            if (ejeVertical > 0 && !enEscalera && rigibodyPlayer.velocity.y >= 0) entroEscalera(escaleraActiva);
        }
        else if (enEscalera) salgoEscalera();


        //Comprobación si pulsamos abajo y tenemos una escalera justo debajo
        if (ejeVertical < 0)
        {
            Vector2 posRayoAB = new Vector2(transform.position.x - (ccSize.x / 2),
                                            transform.position.y - (ccSize.y / 2) - 0.1f);

            RaycastHit2D hitRayoAB = Physics2D.Raycast(posRayoAB,
                                                       Vector2.right,
                                                       ccSize.x,
                                                       capaEscalera);

            //Debug.DrawRay(posRayoAB, Vector2.right * (ccSize.x), Color.red);
            //Si se activa el RayCast
            if (hitRayoAB) entroEscalera(hitRayoAB.transform.gameObject);
            //Si bajamos de la escalera y nos topamos con el suelo
            else if (enEscalera && isTocaSuelo) salgoEscalera();
        }


        //Movernos cuando estamos en la escalera
        if (enEscalera)
        {
            //Cambiar animaciones
            if (ejeVertical != 0) animatorPlayer.Play("SubeEscalera");
            else animatorPlayer.Play("QuietoEscalera");



            nuevaVelocidad = new Vector2(rigibodyPlayer.velocity.x,
                                         velocidadPlayer * ejeVertical);
            rigibodyPlayer.MovePosition(rigibodyPlayer.position + nuevaVelocidad * Time.deltaTime);
        }
    }

    private void entroEscalera(GameObject escalera)
    {
        rigibodyPlayer.gravityScale = 0;
        isTocaSuelo = false;
        enEscalera = true;
        rigibodyPlayer.velocity = Vector2.zero;
        rigibodyPlayer.position = new Vector2(escalera.transform.position.x,
                                              rigibodyPlayer.position.y + (ejeVertical * 0.01f));
    }

    private void salgoEscalera()
    {
        enEscalera = false;
        animatorPlayer.Play("quieto");
        rigibodyPlayer.gravityScale = gravedad;
    }

    private void comprobarDir()
    {
        if (isMirandoDerecha) dirX = 1;
        else dirX = -1;

    }

    private void OnDisable()
    {
        GameController.respawn -= Respawn;
    }

    private void ValidaCoyoteTime()
    {
        if (!isTocaSuelo && coyoteTime)
        {
            tiempoCoyote += Time.deltaTime;
            if (tiempoCoyote > tiempoCoyoteTime) coyoteTime = false;
        }
    }

    private void ControlBufferSalto()
    {
        if (Input.GetButtonDown("Jump"))
        {
            tiempoBuffer = tiempoBufferSalto;
        }
        else
        {
            tiempoBuffer -= Time.deltaTime;
        }
    }

    private void checkPolvoPies()
    {
        if (isTocaSuelo && ejeHorizontal != 0)
        {
            emisionPolvoPies.rateOverTime = 40;
        }
        else
        {
            emisionPolvoPies.rateOverTime = 0;
        }
    }


    /* //======================================CONTROL QUIZZ PLAYER-=============================*/
    private void continuarPlayer()
    {
        rigibodyPlayer.gravityScale = gravedad;
        QuizController.finQuizz = false;
    }


}
