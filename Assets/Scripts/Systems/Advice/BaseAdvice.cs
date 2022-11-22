using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAdvice : Advice
{
    public override void GiveAdvice()
    {
        Debug.Log("Dit is een basisadvies");
    }
}
