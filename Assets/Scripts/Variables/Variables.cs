using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Variables
{
    public static int nivel { get; set; }
    public static int Maxnivel { get; set; }
    public static bool isIniciado { get; set; }

    //Comprobaciones de las posiciones del minimapa
    //estadoPos(0)  = Desactivado
    //estadoPos(1)  = Ya se descubrió
    //estadoPos(2)  = Tiene que aparecer poco a poco
    public static int estadoPosMenu { get; set; }
    public static int estadoPos0 { get; set; }
    public static int estadoPos1 { get; set; }
    public static int estadoPos2 { get; set; }
    public static int estadoPos3 { get; set; }
    public static int estadoPos4 { get; set; }


}
