using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WurmAPI : MonoBehaviour
{
    /// <summary>
    ///     Used to fire methods for database in a save manner
    /// </summary>
    /// <param name="type">
    ///     Which method type is used?
    /// </param>
    /// <param name="data">
    ///     Data passed such as keys, info, etc.
    /// </param>
    /// <returns>
    ///     Http status code with data if required
    /// </returns>
    public string[] MethodHandler<T>(MethodType type, T data)
    {
        string[] test = new string[] { Status.Failure0.ToString(), "" };

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
    private string[] GetWurmInfo<T>(T data)
    {
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
    private string[] PostWurmInfo<T>(T data)
    {
        // add new data
        string[] test = Database.GetData(data);
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
    private string[] PutWurmInfo<T>(T data)
    {
        // update data
        string[] test = Database.GetData(data);
        return test;
    }
}
