using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Variables
{
    //ESTA CLASE GUARDARA LAS VARIABLES DE NUESTRO MINIMAPA

    public static int nivel { get; set; } //0,1,2,3,4,5
    public static int Maxnivel { get; set; } //5 => Nivel máximo al que podemos acceder
    public static bool isIniciado { get; set; }

    //public static int isCloseApp { get; set; } //Controlar cuando salgamos del videojuego

    //Comprobaciones de las posiciones del minimapa
    //estadoPos == 0  Desactivado
    //estadoPos == 1  Ya se descubrió
    //estadoPos == 2  Tiene que aparecer poco a poco
    public static int estadoPosMenu { get; set; }
    public static int estadoPos0 { get; set; }
    public static int estadoPos1 { get; set; }
    public static int estadoPos2 { get; set; }
    public static int estadoPos3 { get; set; }
    public static int estadoPos4 { get; set; }


}
