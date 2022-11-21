using UnityEngine;

[System.Serializable]
public class Sheep
{
    // These variables are case sensitive and must match the strings in the JSON file.
    public string uuid;
    public int tsborn;
    public Sex gender;
    public SheepType species;
    public float weight;
    public Disease[] diseases;
    public string[] extraRemarks;

    // Properties are currently only required for CSV file importing.
    public string Uuid
    {
        get { return uuid; }
        set { uuid = value; }
    }

    public int Tsborn
    {
        get { return tsborn; }
        set { tsborn = value; }
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

    public Disease[] Diseases
    {
        get { return diseases; }
        set { diseases = value; }
    }

    public string[] Extraremarks
    {
        get { return extraRemarks; }
        set { extraRemarks = value; }
    }
}