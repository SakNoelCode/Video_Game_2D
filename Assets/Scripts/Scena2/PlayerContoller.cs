using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{

    [Header("Valores del Personaje")]
    [SerializeField] private float velocidadPlayer;
    [SerializeField] private float velocidadMaxPlayer;
    [SerializeField] private float fuerzaSaltoPlayer;


    //Variables para acceder a las propiedades del personaje
    private Rigidbody2D rigibodyPlayer; //rPlayer
    private float       ejeHorizontal;        //h
    private bool        isMirandoDerecha = true;


    //Variable para colision Pies
    public bool isColisionPies = false;


    //------------------------------------------METODO START-----------------------------------
    void Start()
    {
        rigibodyPlayer = GetComponent<Rigidbody2D>();
    }


    //------------------------------------------METODO UPDATE-----------------------------------
    void Update()
    {
        girarPlayer(ejeHorizontal);
        saltarPlayer();
    }

     
    //------------------------------------------METODO FIXED UPDATE-----------------------------------
    void FixedUpdate()
    {
        moverPlayer();
    }

    
    private void girarPlayer(float ejeHorizontal)
    {
        if( (ejeHorizontal > 0 && !isMirandoDerecha) || (ejeHorizontal < 0 && isMirandoDerecha))
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

        isColisionPies = CheckGround.isColisionPies;

        if (Input.GetButtonDown("Jump") && isColisionPies)
        {
            rigibodyPlayer.velocity = new Vector2(rigibodyPlayer.velocity.x, 0f);//Anular cualquier velocidad en el ejer Y
            rigibodyPlayer.AddForce(new Vector2(0, fuerzaSaltoPlayer), ForceMode2D.Impulse);
        }
    }
}
