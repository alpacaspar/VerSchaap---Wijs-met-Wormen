public static class Database
{
    /// <summary>
    ///     Load database
    /// </summary>
    public static void InitializeDatabase()
    {
        // load json file as local database to modify values
        // eventually create database connection for queries
    }

    /// <summary>
    ///     Get request
    /// </summary>
    /// <param name="data">
    ///     Generic type that contains keys and info
    /// </param>
    /// <returns>
    ///     Http status type and data content
    /// </returns>
    public static string[] GetData<T>(T data)
    {
        string[] newData = new string[] { Status.Failure0.ToString(), "" };

        return newData;
    }

    /// <summary>
    ///     Post request
    /// </summary>
    /// <param name="data">
    ///     Generic type that contains keys and info
    /// </param>
    /// <returns>
    ///     Http status type and data content
    /// </returns>
    public static string[] PostData<T>(T data)
    {
        string[] newData = new string[] { Status.Failure0.ToString(), "" };

        return newData;
    }

    /// <summary>
    ///     Put request
    /// </summary>
    /// <param name="data">
    ///     Generic type that contains keys and info
    /// </param>
    /// <returns>
    ///     Http status type and data content
    /// </returns>
    public static string[] PutData<T>(T data)
    {
        string[] newData = new string[] { Status.Failure0.ToString(), "" };

        return newData;
    }
}
