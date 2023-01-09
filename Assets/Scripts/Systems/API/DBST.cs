using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class DBST : MonoBehaviour
{
    public static DBST Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    /// <summary>
    ///     This method is used to fire a request to the database
    /// </summary>
    /// <param name="fieldCollection">
    ///     All the fields required (must be matching with field name in database)
    /// </param>
    /// <param name="dataCollection">
    ///     All the values of the fields (must be in same order as the fields)
    /// </param>
    /// <param name="type">
    ///     The method type used
    /// </param>
    /// <param name="request">
    ///     The request that will be handled through php so that the right query is invoked
    /// </param>
    /// <returns></returns>
    public void FireURI(string[] fieldCollection, string[] dataCollection, MethodType type, string request)
    {
        StartCoroutine(FireURIRoutine(fieldCollection, dataCollection, type, request));
    }

    private IEnumerator FireURIRoutine(string[] fieldCollection, string[] dataCollection, MethodType type, string request)
    {
        // TODO implement http return codes
        Status response = Status.Failure0;

        if (fieldCollection.Length != dataCollection.Length)
        {
            // return that entries are not matching in length
        }

        string baseUrl = "https://studenthome.hku.nl/~tjaard.vanverseveld/content/vakken/jaar2/kernmodule4gdev/";
        //string baseUrl = "https://studenthome.hku.nl/~tjaard.vanverseveld/content/vakken/jaar4/context3/";
        string pageUrl = ""; // TODO page not found error thingy
        switch (type)
        {
            case MethodType.Get:
                pageUrl = "VerweidklokGetRequests.php?request=" + request;
                break;
            case MethodType.Post:
                pageUrl = "VerweidklokPostRequests.php?request=" + request;
                break;
        }
        string uri = baseUrl + pageUrl;

        for (int i = 0; i < fieldCollection.Length; i++)
        {
            uri += "&" + fieldCollection[i] + "=" + dataCollection[i];
        }

        // TODO fire uri
        Debug.Log("Fired: " + uri);

        // test url:
        // https://studenthome.hku.nl/~tjaard.vanverseveld/content/vakken/jaar2/kernmodule4gdev/get_scores.php?game_id=1&n_scores=10

        switch (type)
        {
            default:
            case MethodType.Get:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
                {
                    // Request and wait for the desired page.
                    yield return webRequest.SendWebRequest();

                    string[] pages = uri.Split('/');
                    int page = pages.Length - 1;

                    switch (webRequest.result)
                    {
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                            break;
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                            break;
                        case UnityWebRequest.Result.Success:
                            Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                            break;
                    }
                }

                // TODO return response through event system and set caller on "waiting"
                break;

            case MethodType.Post:
                //unityWebRequest = UnityWebRequest.Post(uri);
                break;

            case MethodType.Put:
                //unityWebRequest = UnityWebRequest.Put(uri);
                break;
        }
    }
}
