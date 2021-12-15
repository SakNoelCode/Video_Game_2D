using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
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


    [Header("Valores informativos del Personaje")] 
    [SerializeField] private bool isSaltando = false;
    [SerializeField] private bool isPuedoSaltar = false;
    [SerializeField] private bool isTocaSuelo = false;


    //Variables auxiliares
   // private bool       inPlataforma = false;
    private Vector2    nuevaVelocidad;
    private Color      colorInicialPlayer;

    //Variables para acceder a las propiedades del personaje
    private SpriteRenderer spritePlayer;  //sPlayer
    private Rigidbody2D    rigibodyPlayer; //rPlayer
    private float          ejeHorizontal;        //h
    private bool           isMirandoDerecha = true;
    private Vector3        posInicialPlayer;   

    //Variables para las animaciones
    private Animator animatorPlayer;   //aPlayer

    //Variables para colision con el enemigo
    private bool isTocado = false;

    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        posInicialPlayer = transform.position;
        rigibodyPlayer = GetComponent<Rigidbody2D>();
        animatorPlayer = GetComponent<Animator>();
        spritePlayer = GetComponent<SpriteRenderer>();
        colorInicialPlayer = spritePlayer.color;
    }


    //------------------------------------------METODO UPDATE-----------------------------------
    void Update()
    {
        recibePulsaciones();
        asignarValoresAnimaciones();
    }


    //------------------------------------------METODO FIXED UPDATE-----------------------------------
    void FixedUpdate()
    {
        comprobarSiTocamosSuelo();

        if(!isTocado) moverPlayer();
    }


    private void recibePulsaciones()
    {
        if (Input.GetKey(KeyCode.R)) reaparecePlayer(); 

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
        if (Input.GetButtonDown("Jump") && isPuedoSaltar && isTocaSuelo)
        {
            isSaltando = true;
            isPuedoSaltar = false;
            rigibodyPlayer.velocity = new Vector2(rigibodyPlayer.velocity.x, 0f);//Anular cualquier velocidad en el ejer Y
            rigibodyPlayer.AddForce(new Vector2(0, fuerzaSaltoPlayer), ForceMode2D.Impulse);
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
        if(collision.gameObject.tag == "Pinchos")  //DETECCION PINCHOS
        {
            pierdeVida();
        }

        if(collision.gameObject.tag == "CaidaVacio") //DETECCION CAIDA VACIO
        {
            pierdeVida();
        }
    }

    private void pierdeVida()
    {
        reaparecePlayer();
    }

    private void reaparecePlayer()
    {
        rigibodyPlayer.velocity = Vector3.zero;
        transform.position = posInicialPlayer;
    }

    private void tocarEnemigo(float posX)
    {
        if (!isTocado)
        {
            //Cambiar color
            Color nuevoColor = new Color(255f/255, 100f/255, 100f/255);
            spritePlayer.color = nuevoColor;

            isTocado = true;
            float lado = Mathf.Sign(posX - transform.position.x);
            rigibodyPlayer.velocity = Vector2.zero;
            rigibodyPlayer.AddForce(new Vector2(fuerzaToqueEnemigo * -lado, fuerzaToqueEnemigo), ForceMode2D.Impulse);
        }
    }

}
