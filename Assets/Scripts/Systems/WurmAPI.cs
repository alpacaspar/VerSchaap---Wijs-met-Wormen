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
        string[] test = new string[] { Status.Error1.ToString(), "" };

        switch (type)
        {
            case MethodType.Get:
                test = GetWurmInfo(data);
                break;

            case MethodType.Post:
                test = PostWurmInfo(data);
                break;

            case MethodType.Put:
                test = PutWurmInfo(data);
                break;
        }

        return test;
    }

    /// <summary>
    ///     Get request for the database
    /// </summary>
    /// <param name="data">
    ///     Required information
    /// </param>
    /// <returns>
    ///     Http status code with data if required
    /// </returns>
    private static string[] GetWurmInfo<T>(T data)
    {
        // return highest level based on specification (depending on nesting)
        // returns json
        string[] test = Database.GetData(data);
        return test;
    }

    /// <summary>
    ///     Post request for the database
    /// </summary>
    /// <param name="data">
    ///     Required information
    /// </param>
    /// <returns>
    ///     Http status code with data if required
    /// </returns>
    private static string[] PostWurmInfo<T>(T data)
    {
        // add new data
        string[] test = Database.PostData(data);
        return test;
    }

    /// <summary>
    ///     Put request for the database
    /// </summary>
    /// <param name="data">
    ///     Required information
    /// </param>
    /// <returns>
    ///     Http status code with data if required
    /// </returns>
    private static string[] PutWurmInfo<T>(T data)
    {
        // update data
        string[] test = Database.PutData(data);
        return test;
    }
}
