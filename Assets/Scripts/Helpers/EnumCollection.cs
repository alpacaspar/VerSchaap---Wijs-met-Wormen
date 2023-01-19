using System;
using System.Runtime.Serialization;

public enum EventType
{
    locationDataReceived,
    performWeatherUpdate,
    onAdviceValueCalculated,
    performAdviceUpdate,
    checkDatabaseResponse,
    onSceneReady,
    onAlarmRang,
    syncedWithDatabase,
    performSync, // this is a heavy method!!
    hardSetFarmUUID,
    updateFarmBtn,
}

public enum MethodType
{
    Get,                // get existing entry
    Post,               // create new entry
    Put,                // update entry
    Remove,             // remove entry
}

public enum GetRequest
{
    GetSheep,
    GetLot,
    GetPair,
    GetSheepWeight,
    GetSheepBreed,
    GetSheepFamily,
    GetSheepInfection,
    GetSheepPair,
    GetSheepMedication,
    GetWorm,
    GetWormResistence,
    GetWormWeakness,
    GetLotState,
    GetLotPlant,
    GetLotLivestock,
}

public enum PutRequest
{
    UpdateSheep,
    UpdateLot,
    UpdatePair,
    UpdateSheepWeight,
    UpdateSheepBreed,
    UpdateSheepFamily,
    UpdateSheepInfection,
    UpdateSheepPair,
    UpdateSheepMedication,
    UpdateWormResistence,
    UpdateWormWeakness,
    UpdateLotPlant,
    UpdateLotLivestock,
}

public enum PostRequest
{
    AddSheep,
    AddLot,
    AddPair,
    AddSheepWeight,
    AddSheepBreed,
    AddSheepFamily,
    AddSheepInfection,
    AddSheepPair,
    AddSheepMedication,
    AddWormResistence,
    AddWormWeakness,
    AddLotState,
    AddLotPlant,
    AddLotLivestock,
    AddNewBreed,
    AddNewFamilyType,
    AddNewInfection,
    AddNewMedicine,
    AddNewLotState,
    AddNewPlant,
    AddNewLivestock,
    AddNewWorm,
}

public enum Status
{
    // success codes
    Success0 = 200,     // OK - content
    Success1 = 201,     // Created
    Success2 = 202,     // Accepted / Updated
    Success4 = 204,     // OK - no content
    Success5 = 205,     // OK - waiting
    // failure codes
    Failure0 = 400,     // Bad Request
    Failure1 = 401,     // Unauthorized
    Failure3 = 403,     // Forbidden
    Failure4 = 404,     // Not Found
    Failure5 = 405,     // Not Allowed
    Failure6 = 406,     // Not Accepted
    Failure8 = 408,     // Request Timeout
    Failure14 = 414,    // URI too long
    // error codes
    Error0 = 500,       // Internal Error
    Error1 = 501,       // Not Implemented
    Error2 = 502,       // Bad Gateway
    Error3 = 503,       // Service Unavailable
    Error4 = 504,       // Gateway Timeout
    Error7 = 507,       // Insufficient Storage
    Error8 = 508,       // Loop Detected
}

[Flags]
public enum GrassType
{   // increment with times 2 - eg. 1, 2, 4, 8, etc.
    Other = 1,              // Just your normal average grass
}

public enum SheepType
{                               //afkortingen die in stallijsten worden gebruikt. Zijn erg lastig te vinden
    ArdenseVoskop,              
    BarbadosBlackBelly,
    BelgischMelkschaap,
    Bergschaap,
    BlackWelshMountainsheep,
    BlauweTexelaar,
    BleuDuMaine,                //BM
    BorderLeicester,
    BruinHaarschaap,
    Cambridge,
    CharmoiseSchaap,
    Cheviot,
    ClunForest,
    CoburgerFuchs,
    DuitsWitkopschaap,          //DW
    Flevolander,                //FL
    FriesMelkschaap,            //FZ (net als zeeuws melkschaap)
    GotlandPels,
    HampshireDown,              //HD
    Hebridian,
    Herdwick,
    Jacobschaap,
    Kameroen,
    Karakul,
    KarntnerBrilschaap,
    KerryHill,
    LeicesterLongwool,
    Maasduinenschaap,
    Merino,                     //ME
    Moeflon,
    NederlandsBonteSchaap,      //BS
    NolanaSchaap,
    Noordhollander,             //NH
    NorfolkHorn,
    Ouessant,
    OxfordDown,
    PersianBlackhead,
    PollDorset,                 //PD
    PommersLandschaap,
    RackaSchaap,
    Romanov,                    //RO
    RougeDeLOuest,              //RL
    Ryeland,
    ScottishBlackface,
    Shetland,
    Shropshire,
    SkuddeSchaap,
    Soayschaap,
    Solognote,
    Southdown,
    Suffolk,                    //SU
    Swifter,                    //SW
    Texelaar,                   //TE
    WalliserSchwarznase,
    WelshHillSpeckledFace,
    WensleydaleLongwool,
    WiltshireHorn,
    ZeeuwsMelkschaap,           //FZ (net als fries melkschaap)
    Zwartblesschaap,            //ZB
    Kruising,                   //KS
    Overig,                     //OV
}

public enum Sex
{
    Male,               // Ram
    Female,             // Ooi
}

public enum Disease
{
    Myasis,             // idk
    Leverbot,           // idk
    Fever,              // idk
}

public enum WormType
{
    FasciolaHepatica,       // leverbot, platworm
    HaemonchusContortus,    // rode lebmaagworm
    NematodirusBattus,      // voorjaarsworm
    //Myasis,               // worm
}

public enum Medicine
{
    Benzodyne,          // probably spelled wrong
}

public enum Symptom
{
    RedEyes,            // red eyes
}

public enum Condition
{
    Dry,                // dry weather
    Warm,               // warm weather
}

public enum WeatherType
{
    ClearSky = 0,
    MainlyClear = 1,
    PartlyCloudy = 2,
    Overcast = 3,
    Fog = 45,
    DepositingRimeFog = 48,
    LightDrizzle = 51,
    ModerateDrizzle = 53,
    DenseDrizzle = 55,
    LightFreezingDrizzle = 56,
    DenseFreezingDrizzle = 57,
    SlightRain = 61,
    ModerateRain = 63,
    HeavyRain = 65,
    LightFreezingRain = 66,
    HeavyFreezingRain = 67,
    SlightSnowfall = 71,
    ModerateSnowfall = 73,
    HeavySnowFall = 75,
    SnowGrains = 77,
    SlightRainShower = 80,
    ModerateRainShower = 81,
    ViolentRainShower = 82,
    SlightSnowShower = 85,
    HeavySnowShower = 86,
    Thunderstorm = 95,  // Only in Central Europe.
    ThunderstormSlightHail = 96,    // Only in Central Europe.
    ThunderstormHeavyHail = 99, // Only in Central Europe.
}

public enum Quality
{
    Excellent,
    VeryGood,
    Good,
    Mediocre,
    Poor,
    VeryPoor,
    Appalling
}

public enum SheepAge
{
    Lamb,
    Adult,
    Elder
}
