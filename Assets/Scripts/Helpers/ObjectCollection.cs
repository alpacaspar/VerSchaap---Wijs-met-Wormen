using System.Collections.Generic;
using System;

public interface IObject { }

[Serializable]
public class TemporalDatabaseData : IObject
{
    public string farmerName;
    public string farmerUUID;
    public List<WeideObject> weides;
    public List<SheepObject> sheeps;
    public List<WormObject> worms;
}

[Serializable]
public abstract class ObjectUUID : IObject
{
    public string UUID;
}

[Serializable]
public class WeideObject : ObjectUUID
{
    public int surfaceSqrMtr;
    public float surfaceQuality;
    public List<GrassType[]> grassTypes;
    public List<SheepType[]> currentSheeps;
    public List<string> extraRemarks;
}

[Serializable]
public class SheepObject : ObjectUUID
{
    public long tsBorn; // time stamp date of birth
    public List<SheepWeight> weight;        // list is not sorted on timestamps!
    public List<SheepDiseases> diseases;    // list is not sorted on timestamps!
    public Sex sex;
    public SheepType sheepType;
    public List<string> extraRemarks;
}

[Serializable]
public class WormObject : ObjectUUID
{
    public WormType wormType;
    public List<WormMedicines> effectiveMedicines;  // list is not sorted on timestamps!
    public List<WormResistences> resistences;       // list is not sorted on timestamps!
    public List<WormSymptoms> symptoms;             // list is not sorted on timestamps!
    public List<WormFaveConditions> faveConditions; // list is not sorted on timestamps!
    public List<string> extraRemarks;
}

/* * * *
 * All time based structs have to be sorted when retrieved
 * The structs show when a new value was added to the list
 * This time can be used in order to create history graphs
 * * * */
[Serializable]
public struct SheepWeight
{
    // current weight at given time
    public float weight;
    public long timestamp;
}

[Serializable]
public struct SheepDiseases
{
    // active diseases at given time
    public List<Disease> diseases;
    public long timestamp;
}

[Serializable]
public struct WormMedicines
{
    // active diseases at given time
    public List<Medicine> diseases;
    public long timestamp;
}

[Serializable]
public struct WormResistences
{
    // active diseases at given time
    public List<Medicine> diseases;
    public long timestamp;
}

[Serializable]
public struct WormSymptoms
{
    // active diseases at given time
    public List<Symptom> diseases;
    public long timestamp;
}

[Serializable]
public struct WormFaveConditions
{
    // active diseases at given time
    public List<Condition> diseases;
    public long timestamp;
}
