using UnityEngine;

[System.Serializable]
public class Sheep
{
    // These variables are case sensitive and must match the strings in the JSON file.
    public string name;
    public float age;
    public string gender;
    public string description;

    // Properties are currently only required for CSV file importing.
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

    public string Gender
    {
        get { return gender; }
        set { gender = value; }
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