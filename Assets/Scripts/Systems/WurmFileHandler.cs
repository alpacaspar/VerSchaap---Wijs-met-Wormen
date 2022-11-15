using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public static class WurmFileHandler
{
    /// <summary>
    /// Generic function for retrieving a list of objects of type T from a CSV file
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    /// <param name="inputFile">The CSV input file</param>
    /// <returns></returns>
    public static List<T> GetDataFromCsvFile<T>(TextAsset inputFile) where T : class, new()
    {
        List<T> outList = new List<T>();

        if (inputFile == null)
        {
            Debug.LogError("CSV input file not defined!");
            return outList;
        }

        string fileData = inputFile.text.Trim();
        string[] lines = fileData.Split("\n"[0]);

        if (lines.Length < 2)
        {
            Debug.Log("CSV input file is empty!");
            return outList;
        }

        // Get the property names from the input file.
        string[] propertyNames = (lines[0].Trim()).Split(","[0]);
        // The number of properties in the input file.
        int numberOfPropertiesInInput = propertyNames.Length;
        // The number of properties to be expected.
        int numberOfPropertiesInClass = typeof(Sheep).GetProperties().Length;

        // Stop loading if there are missing properties.
        if (numberOfPropertiesInInput < 1 || numberOfPropertiesInInput != numberOfPropertiesInClass)
        {
            Debug.Log("Properties missing! Aborting data loading!");
            return outList;
        }

        // Go through the properties of the file and check if they are correct.
        foreach (string propName in propertyNames)
        {
            string propertyName = StringTransformer.MakeFirstLetterUpper(propName);
            PropertyInfo prop = typeof(Sheep).GetProperty(propertyName);

            // Stop loading if the line contains an invalid property.
            if (prop == null)
            {
                Debug.LogError("Property '" + propertyName + "' not found! Aborting data loading!");
                return outList;
            }
        }

        // All properties were provided.
        for (int i = 1; i < lines.Length; i++)
        {
            T element = new T();

            string[] lineProperties = lines[i].Split(","[0]);
            int numberOfArgumentsInLine = lineProperties.Length;
            if (numberOfArgumentsInLine != numberOfPropertiesInClass)
            {
                Debug.LogWarning("Line " + i + " has corrupt data and will be skipped");
                continue;
            }

            for (int j = 0; j < lineProperties.Length; j++)
            {
                string propertyName = StringTransformer.MakeFirstLetterUpper(propertyNames[j]);
                string propertyValueString = lineProperties[j];

                PropertyInfo prop = element.GetType().GetProperty(propertyName);
                Type propertyType = prop.PropertyType;

                object propertyValue = propertyValueString;

                // Cast the value if it is not supposed to be a string.
                if (propertyType == typeof(Single))
                {
                    propertyValue = Convert.ToSingle(propertyValueString);
                }

                prop.SetValue(element, propertyValue, null);
            }

            outList.Add(element);
        }

        return outList;
    }
}
