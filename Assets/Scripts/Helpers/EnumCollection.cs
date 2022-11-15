public enum EventType
{
    locationDataReceived,
    weatherDataReceived,
}

public enum MethodType
{
    Get,                // get existing entry
    Post,               // create new entry
    Put,                // update entry
    Remove,             // remove entry
}

public enum Status
{
    // success codes
    Success0 = 200,     // OK - content
    Success1 = 201,     // Created
    Success2 = 202,     // Accepted / Updated
    Success4 = 204,     // OK - no content
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

public enum GrassType
{
    Grass,              // Just your normal average grass
}

public enum SheepType
{
    Texelaar,           // Texelaar
}

public enum Sex
{
    Female,             // Ooi
    Male,               // Ram
}

public enum Decease
{
    Myasis,             // idk
    Leverbot,           // idk
    Fever,              // idk
}

public enum WormType
{
    Myasis,             // worm
    Leverbot,           // worm
}

public enum Medicine
{
    Benzodyne,          // probably spelled wrong
}

public enum Symptom
{
    RedEyes,            // red eyes
}

public enum Conditions
{
    Dry,                // dry weather
    Warm,               // warm weather
}
