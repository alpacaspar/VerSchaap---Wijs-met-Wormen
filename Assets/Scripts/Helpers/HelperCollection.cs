using System.Collections.Generic;

public static class Helpers
{
    public static Dictionary<Status, string> HttpMessage = new Dictionary<Status, string>() {
        {Status.Success0, "Ok - content"},
        {Status.Success1, "Created"},
        {Status.Success2, "Accepted / Updated"}, 
        {Status.Success4, "OK - no content"},    
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

    public static long GetCurrentTimestamp()
    {
        return System.DateTime.UtcNow.Ticks;
    }
}
