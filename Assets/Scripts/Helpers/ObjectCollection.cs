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
    public List<SheepKoppel> sheepKoppels;
}

[Serializable]
public abstract class ObjectUUID : IObject
{
    public string UUID;
}

[Serializable]
public class WeideObject : ObjectUUID
{
    public string perceelName = "Perceel";
    public int surfaceSqrMtr = 0;
    public float surfaceQuality = 0;
    public List<GrassType[]> grassTypes = new List<GrassType[]>();
    public List<SheepType[]> currentSheeps = new List<SheepType[]>();
    public List<string> extraRemarks = new List<string>();
}

[Serializable]
public class SheepObject : ObjectUUID
{
    public string sheepTag = "NL-000000-0-00000";
    public long tsBorn = 0; // time stamp date of birth
    public List<SheepWeight> weight = new List<SheepWeight>();        // list is not sorted on timestamps!
    public List<SheepDiseases> diseases = new List<SheepDiseases>();    // list is not sorted on timestamps!
    public Sex sex = Sex.Female;
    public SheepType sheepType = 0;
    public List<string> extraRemarks = new List<string>();
    public string sheepKoppelID = "";
}

[Serializable]
public class WormObject : ObjectUUID
{
    public WormType wormType = 0;
    public string nonScienceName = "worm";
    public List<WormMedicines> effectiveMedicines = new List<WormMedicines>();  // list is not sorted on timestamps!
    public List<WormResistences> resistences = new List<WormResistences>();       // list is not sorted on timestamps!
    public List<WormSymptoms> symptoms = new List<WormSymptoms>();             // list is not sorted on timestamps!
    public List<WormFaveConditions> faveConditions = new List<WormFaveConditions>(); // list is not sorted on timestamps!
    public List<string> extraRemarks = new List<string>();
}

[Serializable]
public class SheepKoppel : ObjectUUID
{
    public string koppelName = "koppel";
    public List<string> allSheep = new List<string>();
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
public static class Dictionaries
{
    public static Dictionary<WormType, string> wormNonScienceNames = new Dictionary<WormType, string>()
    {
        { WormType.FasciolaHepatica,    "leverbot" },
        { WormType.HaemonchusContortus, "rode lebmaagworm" },
        { WormType.NematodirusBattus,   "voorjaarsworm" }
    };

    public static Dictionary<Sex, string> SheepGenderNames = new Dictionary<Sex, string>
    {
        { Sex.Female,   "Ooi" },
        { Sex.Male,     "Ram" }
    };

    public static Dictionary<SheepAge, string> sheepAges = new Dictionary<SheepAge, string>
    {
        { SheepAge.Lamb,    "Lam"},
        { SheepAge.Adult,   "Volwassenen"},
        { SheepAge.Elder,   "Ouderling"}
    };
}