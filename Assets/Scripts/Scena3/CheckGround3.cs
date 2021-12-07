using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---------------------SCRIPT COMPROBAR TIERRA----------------------------
public class CheckGround3 : MonoBehaviour
{
    public static bool isColisionPies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Terreno") isColisionPies = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Terreno") isColisionPies = false;
    }
}
