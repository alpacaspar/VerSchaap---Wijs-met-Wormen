using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringTransformer
{
    /// <summary>
    /// Capitalizes the first letter of a string.
    /// </summary>
    /// <param name="inputString"></param>
    /// <returns></returns>
    public static string MakeFirstLetterUpper(string inputString)
    {
        string outputString = "";

        if (inputString.Length == 1)
        {
            outputString = char.ToUpper(inputString[0]).ToString();
        }
        else
        {
            outputString = char.ToUpper(inputString[0]) + inputString.Substring(1);
        }

        return outputString;
    }
}
