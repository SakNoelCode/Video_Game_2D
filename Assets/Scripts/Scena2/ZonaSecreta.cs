using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZonaSecreta : MonoBehaviour
{
    [Header("Opciones generales")]
    [SerializeField] private Tilemap PlataformaHueco;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(muestraZona());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Color color = PlataformaHueco.color;
            color.a = 1f;
            PlataformaHueco.color = color;
        }
    }

    IEnumerator muestraZona()
    {
        for(float f = 1; f>= 0.5; f-= 0.02f)
        {
            Color color = PlataformaHueco.color;
            color.a = f;
            PlataformaHueco.color = color;
            yield return (0.05f);
        }
    }
}
