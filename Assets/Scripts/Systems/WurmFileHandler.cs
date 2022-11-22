using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.IO;

public static class WurmFileHandler
{

    public static bool IsList(object o)
    {
        return o.GetType() == typeof(List<string>);
    }
    /*
    public static bool IsList(object o)
    {
        if (o == null) return false;
        // why doesnt the other thing just work?!
        return o.GetType() == typeof(List<string>);
        //return o is IList && o.GetType().IsGenericType;
        //return o is IList;
        //return o is IList && o.GetType().IsGenericType && o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
    }
    */

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
        string[] fieldNames = (lines[0].Trim()).Split(","[0]);
        // The number of properties in the input file.
        int numberOfFieldsInInput = fieldNames.Length;
        // The number of properties to be expected.
        int numberOfFieldsInClass = typeof(T).GetFields().Length;

        // Stop loading if there are missing properties.
        if (numberOfFieldsInInput < 1 || numberOfFieldsInInput != numberOfFieldsInClass)
        {
            Debug.LogError("Fields missing! Aborting data loading!");
            return outList;
        }

        // Go through the properties of the file and check if they are correct.
        foreach (string fieldName in fieldNames)
        {
            //string fielddName = StringTransformer.MakeFirstLetterUpper(fieldName);
            FieldInfo field = typeof(T).GetField(fieldName);

            // Stop loading if the line contains an invalid property.
            if (field == null)
            {
                Debug.LogError("Property '" + fieldName + "' not found! Aborting data loading!");
                return outList;
            }
        }

        // All properties were provided.
        for (int i = 1; i < lines.Length; i++)
        {
            T element = new T();

            string[] lineProperties = lines[i].Split(","[0]);
            int numberOfArgumentsInLine = lineProperties.Length;

            if (numberOfArgumentsInLine != numberOfFieldsInClass)
            {
                Debug.LogWarning("Line " + i + " has corrupt data and will be skipped");
                continue;
            }

            for (int j = 0; j < lineProperties.Length; j++)
            {
                string fieldName = fieldNames[j];
                string fieldValueString = lineProperties[j];

                FieldInfo field = element.GetType().GetField(fieldName);
                Type fieldType = field.FieldType;

                object fieldValue = fieldValueString;
                bool bIsArray = fieldType.IsArray;
                // Get the type of the property
                fieldType = bIsArray ? fieldType.GetElementType() : fieldType;

                string[] lineElements = fieldValueString.Split(";"[0]);
                int nElements = lineElements.Length;

                if (nElements > 1 && !bIsArray)
                {
                    Debug.LogWarning("The field '" + fieldName + "' is not an array but had multiple values defined. Only the first value will be used");
                }

                Array tmpArr = Array.CreateInstance(fieldType, nElements);

                for (int k = 0; k < nElements; k++)
                {
                    object tmpArrObj = lineElements[k];
                    // Parse the string if the property value shouldn't be one
                    if (fieldType == typeof(Single))
                    {
                        tmpArrObj = Convert.ToSingle(lineElements[k]);
                    }
                    else if (fieldType == typeof(int))
                    {
                        tmpArrObj = Convert.ToInt32(lineElements[k]);
                    }
                    else if (fieldType == typeof(Sex))
                    {
                        Enum.TryParse<Sex>(lineElements[k], true, out Sex sex);
                        tmpArrObj = sex;
                    }
                    else if (fieldType == typeof(SheepType))
                    {
                        Enum.TryParse<SheepType>(lineElements[k], true, out SheepType sheepType);
                        tmpArrObj = sheepType;
                    }
                    else if (fieldType == typeof(Disease))
                    {
                        Enum.TryParse<Disease>(lineElements[k], true, out Disease disease);
                        tmpArrObj = disease;
                    }

                    tmpArr.SetValue(tmpArrObj, k);
                }

                fieldValue = bIsArray ? tmpArr : tmpArr.GetValue(0);
                field.SetValue(element, fieldValue);
            }

            outList.Add(element);
        }

        return outList;
    }

    public static void WriteDataToCsvFile<T>(string fileName, T[] data, bool append = false)
    {
        Debug.Log("writing array to file");
        Debug.Log("class type="+typeof(T).ToString());

        FieldInfo[] fields = typeof(T).GetFields();
        Debug.Log("NProperties=" + fields.Length);
        Debug.Log("Properties");

        foreach (var f in fields)
        {
            Debug.Log("- " + f.Name);
        }

        int nFields = fields.Length;
        int i = 1;

        string path = Application.persistentDataPath + "/" + fileName + ".csv";
        int numberOfLines = GetNumberOfLines(path);

        if (numberOfLines < 2)
        {
            append = false;
        }

        StreamWriter writer = new StreamWriter(path, append);

        if (!append)
        {
            foreach (var field in fields)
            {
                writer.Write(field.Name.ToLower());
                writer.Write(i < nFields ? ',' : '\n');
                i++;
            }
        }

        foreach (T element in data)
        {
            i = 1;
            // Iterate class fields
            foreach (var field in fields)
            {
                bool bIsArray = element.GetType().GetField(field.Name).FieldType.IsArray;
                Debug.Log("objtype=" + element.GetType().GetField(field.Name).FieldType.ToString());

                // TODO: implement list support
                var elemType = element.GetType().GetField(field.Name).FieldType;
                if (elemType == typeof(List<string>) || elemType == typeof(List<SheepWeight>) || elemType == typeof(List<SheepDiseases>))
                {
                    Debug.Log("islist");
                }
                else if (bIsArray)
                {
                    Debug.Log("isarray");
                    Array a = (Array)element.GetType().GetField(field.Name).GetValue(element);
                    int arrayLength = a.Length;
                    for (int k = 0; k < arrayLength; k++)
                    {
                        writer.Write(a.GetValue(k).ToString());
                        if (k < arrayLength - 1)
                        {
                            writer.Write(";");
                        }
                    }
                }
                else
                {
                    Debug.Log("isvar");
                    object propertyValue = element.GetType().GetField(field.Name).GetValue(element);
                    writer.Write(propertyValue.ToString());
                }

                writer.Write(i < nFields ? ',' : '\n');
                i++;
            }
        }

        writer.Close();
    }
}
