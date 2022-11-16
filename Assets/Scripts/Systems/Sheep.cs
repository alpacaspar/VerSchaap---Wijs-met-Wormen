using UnityEngine;

[System.Serializable]
public class Sheep
{
    // These variables are case sensitive and must match the strings in the JSON file.

    //id, tag
    //leeftijd
    //geslacht
    //ras
    //gewicht
    //ziektes
    //opmerkingen

    public int id;
    public string name;
    public float age;
    public Sex gender;
    public SheepType species;
    public float weight;
    public string diseases;
    public string description;

    // Properties are currently only required for CSV file importing.
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public float Age
    {
        get { return age; }
        set { age = value; }
    }

    public Sex Gender
    {
        get { return gender; }
        set { gender = value; }
    }

    public SheepType Species
    {
        get { return species; }
        set { species = value; }
    }

    public float Weigth
    {
        get { return weight; }
        set { weight = value; }
    }

    public string Diseases
    {
        get { return diseases; }
        set { diseases = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public void Print()
    {
        Debug.Log("name:" + name);
    }
}