using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class AlphineHelper
{   
    static int UnsignedNumFilter(ref int value)
    {
        return value = value < 0 ? 0 : value;
    }
    static int NaturalNumFilter(ref int value)
    {
        return value = value <= 0 ? 1 : value;
    }
    
    static int OnlyNumber(string value)
    {
        
    }
}