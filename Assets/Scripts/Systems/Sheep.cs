using UnityEngine;

[System.Serializable]
public class Sheep
{
    // These variables are case sensitive and must match the strings in the data file.
    public string uuid;
    public int tsborn;
    public Sex gender;
    public SheepType species;
    public float weigth;
    public Disease[] diseases;
    public string[] extraremarks;
}