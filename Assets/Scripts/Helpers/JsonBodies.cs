using System;

[Serializable]
public class LotCollectionJson
{
    public LotJSON[] Lots;
}

[Serializable]
public class LotJSON
{
    public string Lot_DB_ID;
    public string Lot_UUID;
    public string Lot_Name;
    public string Lot_Surface;
    public string Lot_Quality;
    public string Lot_Mowed_TS;
    public string Lot_State_ID;
    public string Farmer_UUID;
    public string Last_Modified;
    public string Is_Deleted;
}

[Serializable]
public class SheepCollectionJson
{
    public SheepJSON[] Sheeps;
}

[Serializable]
public class SheepJSON
{
    public string Sheep_DB_ID;
    public string Sheep_UUID;
    public string Sheep_Label;
    public string Sheep_Female;
    public string Timestamp_Born;
    public string Farmer_UUID;
    public string Last_Modified;
    public string Is_Deleted;
}

[Serializable]
public class WormCollectionJson
{
    public WormJSON[] Worms;
}

[Serializable]
public class WormJSON
{
    public string Worm_DB_ID;
    public string Worm_UUID;
    public string Worm_Latin_Name;
    public string Worm_Normal_Name;
    public string Worm_EPG_Danger;
    public string Worm_Egg_Description;
}

[Serializable]
public class PairCollectionJson
{
    public PairJSON[] Pairs;
}

[Serializable]
public class PairJSON
{
    public string Pair_DB_ID;
    public string Pair_UUID;
    public string Pair_Name;
    public string TS_Formed;
    public string TS_Removed;
    public string Farmer_UUID;
    public string Last_Modified;
}
