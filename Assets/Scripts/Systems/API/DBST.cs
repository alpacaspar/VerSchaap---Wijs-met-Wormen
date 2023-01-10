using System.Collections;
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

        string baseUrl = "https://studenthome.hku.nl/~tjaard.vanverseveld/content/vakken/jaar4/context3/";
        string pageUrl = ""; // TODO page not found error thingy
        string uri = "?request=" + request;
        for (int i = 0; i < fieldCollection.Length; i++)
        {
            uri += "&" + fieldCollection[i] + "=" + dataCollection[i];
        }

        switch (type)
        {
            case MethodType.Get:
                pageUrl = "VerweidklokGetRequests.php";
                break;
            case MethodType.Post:
                pageUrl = "VerweidklokPostRequests.php";
                break;
        }

        switch (type)
        {
            default:
            case MethodType.Get:
                using (UnityWebRequest webRequest = UnityWebRequest.Get(baseUrl + pageUrl + uri))
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
                Debug.Log("Fired: " + baseUrl + pageUrl + uri);

                // TODO return response through event system and set caller on "waiting"
                break;

            case MethodType.Post:
                UnityWebRequest www = UnityWebRequest.Get(baseUrl + pageUrl + uri); // any other form of request causes issues
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                }
                Debug.Log("Fired: " + baseUrl + pageUrl + uri);

                // TODO return response through event system and set caller on "waiting"
                break;

            case MethodType.Put:
                //unityWebRequest = UnityWebRequest.Put(uri);
                break;
        }
    }
}
