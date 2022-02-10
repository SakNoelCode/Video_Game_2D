using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformasAtravesables : MonoBehaviour
{
    private GameObject        player;
    private CapsuleCollider2D ccPlayer;
    private BoxCollider2D     ccplataforma;
    private Bounds            ccplataformaBounds;
    private float             topPlataforma, piePlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ccPlayer = player.GetComponent<CapsuleCollider2D>();
        ccplataforma = GetComponent<BoxCollider2D>();
        ccplataformaBounds = ccplataforma.bounds;
        topPlataforma = ccplataformaBounds.center.y + ccplataformaBounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        piePlayer = player.transform.position.y - (ccPlayer.size.y / 2);
        comprobarPosPlayer();
    }
        
    private void comprobarPosPlayer()
    {
        if (piePlayer >= topPlataforma)
        {
            ccplataforma.isTrigger = false;
            gameObject.tag = "TerrenoAtravesable";
            gameObject.layer = LayerMask.NameToLayer("Suelo");
        }

        if ((!ccplataforma.isTrigger && (piePlayer < topPlataforma - 0.2f)) || PlayerController2.enEscalera)
        {
            ccplataforma.isTrigger = true;
            gameObject.tag = "Untagged";
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }

    
   /* private void OnDrawGizmosSelected()
    {
        Gizmos.color = colorGizmos;
        Gizmos.DrawSphere(ccplataforma.transform.position, 0.25f);

    }*/
}
