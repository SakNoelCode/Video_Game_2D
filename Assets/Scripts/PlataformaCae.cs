using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaCae : MonoBehaviour
{
    [SerializeField] private float tiempoEspera;
    [SerializeField] private float tiempoReaparicion;
    [SerializeField] private float margenMov;
    [SerializeField] private GameObject sprite1;
    [SerializeField] private GameObject sprite2;

    private Rigidbody2D rbody;
    private Vector3 posIni;
    private SpriteRenderer spr1, spr2;

    private bool estaTemblando = false;
    private float temblandoDerecha = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        posIni = rbody.transform.position;
        spr1 = sprite1.GetComponent<SpriteRenderer>();
        spr2 = sprite2.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (estaTemblando)
        {
            transform.position = new Vector3(transform.position.x + temblandoDerecha,transform.position.y,transform.position.z);

            if(transform.position.x >= posIni.x + margenMov || transform.position.x <= posIni.x - margenMov)
            {
                temblandoDerecha *= -1;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Invoke("Cae", tiempoEspera);
            Invoke("Reaparece", tiempoReaparicion);

            estaTemblando = true;
        }
    }

    private void Cae()
    {
        rbody.isKinematic = false; 
    }

    private void Reaparece()
    {
        estaTemblando = false;
        rbody.velocity = Vector3.zero;
        rbody.isKinematic = true;
        transform.position = posIni;

        //Reaparecer plataforma suavemente
        ReaparecerPlataformaSuave(spr1,0.0f);
        ReaparecerPlataformaSuave(spr2,0.0f);

        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        for(float i =0.0f; i <=1; i+= 0.1f)
        {
            ReaparecerPlataformaSuave(spr1, i);
            ReaparecerPlataformaSuave(spr2, i);
            yield return new WaitForSeconds(0.025f);
        }
        ReaparecerPlataformaSuave(spr1, 1f);
        ReaparecerPlataformaSuave(spr2, 1f);
    }

    private void ReaparecerPlataformaSuave(SpriteRenderer spr, float A)
    {
        Color c = spr.material.color;
        c.a = A;   //Hacer transparente al Sprite
        spr.material.color = c;
    }
}
