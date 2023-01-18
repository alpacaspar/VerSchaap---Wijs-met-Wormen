using System;
using System.Collections.Generic;

public static class Helpers
{
    /// <summary>
    ///     Has a collection of statusses mapped to human readable messages
    /// </summary>
    public static Dictionary<Status, string> HttpMessage = new Dictionary<Status, string>() {
        {Status.Success0, "Ok - content"},
        {Status.Success1, "Created"},
        {Status.Success2, "Accepted / Updated"},
        {Status.Success4, "OK - no content"},
        {Status.Success5, "OK - waiting"},
        {Status.Failure0, "Bad Request"},    
        {Status.Failure1, "Unauthorized"},   
        {Status.Failure3, "Forbidden"},    
        {Status.Failure4, "Not Found"},    
        {Status.Failure5, "Not Allowed"},     
        {Status.Failure6, "Not Accepted"},     
        {Status.Failure8, "Request Timeout"},    
        {Status.Failure14, "URI too long"},   
        {Status.Error0, "Internal Error"},       
        {Status.Error1, "Not Implemented"},      
        {Status.Error2, "Bad Gateway"},      
        {Status.Error3, "Service Unavailable"},  
        {Status.Error4, "Gateway Timeout"},      
        {Status.Error7, "Insufficient Storage"}, 
        {Status.Error8, "Loop Detected"}     
    };

    /// <summary>
    ///     Get the current timestamp
    /// </summary>
    /// <returns>
    ///     Returns the current timestamp
    /// </returns>
    public static long GetCurrentTimestamp()
    {
        return System.DateTime.UtcNow.Ticks;
    }

    /// <summary>
    ///     Converts list of LotObjects to list of ObjectUUIDs
    /// </summary>
    /// <param name="objects">
    ///     List of LotObjects
    /// </param>
    /// <returns>
    ///     Returns a collection of ObjectUUIDs
    /// </returns>
    public static List<ObjectUUID> LotToUUID(List<LotObject> objects)
    {
        List<ObjectUUID> uuids = new List<ObjectUUID>();
        foreach (LotObject obj in objects)
        {
            uuids.Add(obj);
        }
        return uuids;
    }

    /// <summary>
    ///     Converts list of SheepObjects to list of ObjectUUIDs
    /// </summary>
    /// <param name="objects">
    ///     List of SheepObjects
    /// </param>
    /// <returns>
    ///     Returns a collection of ObjectUUIDs
    /// </returns>
    public static List<ObjectUUID> SheepToUUID(List<SheepObject> objects)
    {
        List<ObjectUUID> uuids = new List<ObjectUUID>();
        foreach (SheepObject obj in objects)
        {
            uuids.Add(obj);
        }

        return uuids;
    }

    /// <summary>
    ///     Converts list of PairCollection to list of ObjectUUIDs
    /// </summary>
    /// <param name="objects">
    ///     List of WormObjects
    /// </param>
    /// <returns>
    ///     Returns a collection of ObjectUUIDs
    /// </returns>
    public static List<ObjectUUID> PairCollectionToUUID(List<PairCollection> objects)
    {
        List<ObjectUUID> uuids = new List<ObjectUUID>();
        foreach (PairCollection obj in objects)
        {
            uuids.Add(obj);
        }

        return uuids;
    }

    /// <summary>
    ///     Converts list of WormObjects to list of ObjectUUIDs
    /// </summary>
    /// <param name="objects">
    ///     List of WormObjects
    /// </param>
    /// <returns>
    ///     Returns a collection of ObjectUUIDs
    /// </returns>
    public static List<ObjectUUID> WormToUUID(List<WormObject> objects)
    {
        List<ObjectUUID> uuids = new List<ObjectUUID>();
        foreach (WormObject obj in objects)
        {
            uuids.Add(obj);
        }
        return uuids;
    }

    /// <summary>
    ///     Generate a new UUID
    /// </summary>
    /// <returns>
    ///     Returns a new UUID
    /// </returns>
    public static string GenerateUUID()
    {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    ///     Translates code to human readable code
    /// </summary>
    /// <param name="response">
    ///     The response body
    /// </param>
    /// <returns>
    ///     Returns human readable code
    /// </returns>
    public static string CodeToMessage(string response)
    {
        return (response + ": " + HttpMessage[(Status)int.Parse(response)]);
    }
}
