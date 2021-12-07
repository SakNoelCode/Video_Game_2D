using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3 : MonoBehaviour
{
    [Header("Valores del Personaje")]
    [SerializeField] private float velocidadPlayer;
    [SerializeField] private float velocidadMaxPlayer;
    [SerializeField] private float fuerzaSaltoPlayer;
    [SerializeField] private bool isColisionPies = false;
    [SerializeField] private float friccionSuelo;


    //Variables para acceder a las propiedades del personaje
    private Rigidbody2D rigibodyPlayer; //rPlayer
    private float ejeHorizontal;        //h
    private bool isMirandoDerecha = true;


    //Variables para las animaciones
    private Animator animatorPlayer;   //aPlayer

    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        rigibodyPlayer = GetComponent<Rigidbody2D>();
        animatorPlayer = GetComponent<Animator>();
    }


    //------------------------------------------METODO UPDATE-----------------------------------
    void Update()
    {
        girarPlayer(ejeHorizontal);
        asignarValoresAnimaciones();
        saltarPlayer();
    }


    //------------------------------------------METODO FIXED UPDATE-----------------------------------
    void FixedUpdate()
    {
        moverPlayer();
        asignarFriccionPlayer();
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
        ejeHorizontal = Input.GetAxisRaw("Horizontal");
        rigibodyPlayer.AddForce(Vector2.right * velocidadPlayer * ejeHorizontal);

        //Aplicar velocidad máxima
        float limiteVelocidad = Mathf.Clamp(rigibodyPlayer.velocity.x, -velocidadMaxPlayer, velocidadMaxPlayer);
        rigibodyPlayer.velocity = new Vector2(limiteVelocidad, rigibodyPlayer.velocity.y);
    }

    private void saltarPlayer()
    {

        isColisionPies = CheckGround3.isColisionPies;

        if (Input.GetButtonDown("Jump") && isColisionPies)
        {
            rigibodyPlayer.velocity = new Vector2(rigibodyPlayer.velocity.x, 0f);//Anular cualquier velocidad en el ejer Y
            rigibodyPlayer.AddForce(new Vector2(0, fuerzaSaltoPlayer), ForceMode2D.Impulse);
        }
    }

    private void asignarValoresAnimaciones()
    {
        animatorPlayer.SetFloat("velocidadX", Mathf.Abs(rigibodyPlayer.velocity.x));
        animatorPlayer.SetFloat("velocidadY", rigibodyPlayer.velocity.y);
        animatorPlayer.SetBool("isTocaSuelo", isColisionPies);
    }

    private void asignarFriccionPlayer()
    {
        if (ejeHorizontal == 0 && isColisionPies)
        {
            Vector3 velocidadConFriccion = rigibodyPlayer.velocity;
            velocidadConFriccion.x *= friccionSuelo;
            rigibodyPlayer.velocity = velocidadConFriccion;
        }
    }  
}
