using System.Diagnostics;

public static class WurmAPI
{
    /// <summary>
    ///     Used to fire methods for the database in a secure way
    /// </summary>
    /// <param name="type">
    ///     Which method type is used?
    ///     This refers to the request type that will be used
    /// </param>
    /// <param name="data">
    ///     Data passed such as keys, info, etc.
    ///     Basically pass values such as [[key, value],[key, value]],...
    ///     For get requests the value can be empty
    /// </param>
    /// <returns>
    ///     Http status code with data if required
    /// </returns>
    public static string[] MethodHandler<T>(MethodType type, T data)
    {
        string[] response = new string[] { Status.Error1.ToString(), "" };

        response = Database.ProgressData(type, data);

        return response;
    }
}
