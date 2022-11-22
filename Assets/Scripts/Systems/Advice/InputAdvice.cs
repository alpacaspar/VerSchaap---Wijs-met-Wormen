using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAdvice : Advice
{
    public override void GiveAdvice()
    {
        Debug.Log("Dit is een advies gebaseerd op het bedrijf zelf");
    }
}
