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
                Debug.Log("starting get");
                for (int i = 0; i < fieldCollection.Length; i++)
                {
                    uri += "&" + fieldCollection[i] + "=" + dataCollection[i];
                }

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
                /*WWWForm form = new WWWForm();
                form.AddField("request", "request");
                for (int i = 0; i < fieldCollection.Length; i++)
                {
                    form.AddField(fieldCollection[i], dataCollection[i]);
                }*/
                // TODO form is not added on web request, so debug!


                for (int i = 0; i < fieldCollection.Length; i++)
                {
                    uri += "&" + fieldCollection[i] + "=" + dataCollection[i];
                }

                //https://studenthome.hku.nl/~tjaard.vanverseveld/content/vakken/jaar4/context3/VerweidklokPostRequests.php?request=AddSheep&Sheep_UUID=fd660fab-0d4f-48e0-93dd-50e7a8d7c740&Sheep_Label=NL-123456-1-12345&Sheep_Female=1&Farmer_UUID=48b5722d-0b82-4b88-8aaf-3934f423110d
                //INSERT INTO `VerweidklokSheepTable` (Sheep_UUID, Sheep_Label, Sheep_Female, Farmer_UUID) VALUES('fd660fab-0d4f-48e0-93dd-50e7a8d7c740','NL-123456-1-12345','1','48b5722d-0b82-4b88-8aaf-3934f423110d');

                UnityWebRequest www = UnityWebRequest.Post(baseUrl + pageUrl, uri);
                yield return www.SendWebRequest();

                Debug.Log(baseUrl + pageUrl + uri);

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                }

                // TODO return response through event system and set caller on "waiting"
                break;

            case MethodType.Put:
                //unityWebRequest = UnityWebRequest.Put(uri);
                break;
        }
    }
}
