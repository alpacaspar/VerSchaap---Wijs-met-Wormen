using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.IO;

public static class WurmFileHandler
{
    /// <summary>
    /// Get the number of lines from a file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static int GetNumberOfLines(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return -1;
        }

        StreamReader reader = new StreamReader(filePath);
        int i = 0;
            
        while (reader.ReadLine() != null)
        {
            i++;
        }

        reader.Close();
        return i;
    }

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
        int numberOfPropertiesInClass = typeof(T).GetProperties().Length;

        // Stop loading if there are missing properties.
        if (numberOfPropertiesInInput < 1 || numberOfPropertiesInInput != numberOfPropertiesInClass)
        {
            Debug.LogError("Properties missing! Aborting data loading!");
            return outList;
        }

        // Go through the properties of the file and check if they are correct.
        foreach (string propName in propertyNames)
        {
            string propertyName = StringTransformer.MakeFirstLetterUpper(propName);
            PropertyInfo prop = typeof(T).GetProperty(propertyName);

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
                bool bIsArray = propertyType.IsArray;
                // Get the type of the property
                Type propType = bIsArray ? propertyType.GetElementType() : propertyType;

                string[] lineElements = propertyValueString.Split(";"[0]);
                int nElements = lineElements.Length;

                if (nElements > 1 && !bIsArray)
                {
                    Debug.LogWarning("The property '" + propertyName + "' is not an array but had multiple values defined. Only the first value will be used");
                }

                Array tmpArr = Array.CreateInstance(propType, nElements);

                for (int k = 0; k < nElements; k++)
                {
                    object tmpArrObj = lineElements[k];
                    // Parse the string if the property value shouldn't be one
                    if (propType == typeof(Single))
                    {
                        tmpArrObj = Convert.ToSingle(lineElements[k]);
                    }
                    else if (propType == typeof(int))
                    {
                        tmpArrObj = Convert.ToInt32(lineElements[k]);
                    }
                    else if (propType == typeof(Sex))
                    {
                        Enum.TryParse<Sex>(lineElements[k], true, out Sex sex);
                        tmpArrObj = sex;
                    }
                    else if (propType == typeof(SheepType))
                    {
                        Enum.TryParse<SheepType>(lineElements[k], true, out SheepType sheepType);
                        tmpArrObj = sheepType;
                    }
                    else if (propType == typeof(Disease))
                    {
                        Enum.TryParse<Disease>(lineElements[k], true, out Disease disease);
                        tmpArrObj = disease;
                    }

                    tmpArr.SetValue(tmpArrObj, k);
                }

                propertyValue = bIsArray ? tmpArr : tmpArr.GetValue(0);
                prop.SetValue(element, propertyValue, null);
            }

            outList.Add(element);
        }

        return outList;
    }

    public static void WriteDataToCsvFile<T>(string fileName, T[] data, bool append = false)
    {
        PropertyInfo[] props = typeof(T).GetProperties();
        int nProps = props.Length;
        int i = 1;

        string path = Application.persistentDataPath + "/" + fileName + ".csv";
        int numberOfLines = GetNumberOfLines(path);
        if (numberOfLines < 2)
        {
            append = false;
        }
        StreamWriter writer = new StreamWriter(path, append);

        //TODO append needs to check if the file is not empty
        if (!append)
        {
            foreach (var prop in props)
            {
                writer.Write(prop.Name.ToLower());

                if (i < nProps)
                {
                    writer.Write(",");
                }
                else
                {
                    writer.Write('\n');
                }

                i++;
            }
        }

        // Iterate elements from input data array
        foreach (T element in data)
        {
            i = 1;
            // Iterate class properties
            foreach (var prop in props)
            {
                object propertyValue = element.GetType().GetProperty(prop.Name).GetValue(element, null);
                writer.Write(propertyValue.ToString());
                
                if (i < nProps)
                {
                    writer.Write(",");
                }
                else
                {
                    writer.Write('\n');
                }

                i++;
            }
        }

        writer.Close();
    }
}
