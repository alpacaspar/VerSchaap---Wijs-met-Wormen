using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DBST : MonoBehaviour
{
    public Dictionary<string, string> dataPackages = new Dictionary<string, string>();

    private bool pullSheep, pullWorm, pullPair, pullLot, pullingData;

    public static DBST Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        EventSystem.AddListener(EventType.performSync, UpdateFromCloud);
    }

    private void OnDisable()
    {
        EventSystem.RemoveListener(EventType.performSync, UpdateFromCloud);
    }

    /// <summary>
    ///     This method is used in order to synchronize local files with database files
    ///     Warning, this method is very heavy!!
    /// </summary>
    public void UpdateFromCloud()
    {
        if (!pullingData)
        {
            StartCoroutine(PullFromDatabase());
        }
    }

    private IEnumerator PullFromDatabase()
    {
        string baseUrl = "https://studenthome.hku.nl/~tjaard.vanverseveld/content/vakken/jaar4/context3/VerweidklokGetRequests.php";
        pullSheep = false;
        pullWorm = false;
        pullPair = false;
        pullLot = false;
        pullingData = true;

        StartCoroutine(PullAllSheeps(baseUrl));
        StartCoroutine(PullAllWorms(baseUrl));
        StartCoroutine(PullAllPairs(baseUrl));
        StartCoroutine(PullAllLots(baseUrl));

        while(!pullSheep || !pullWorm || !pullPair || !pullLot)
        {
            yield return new WaitForEndOfFrame();
        }

        pullingData = false;

        EventSystem.InvokeEvent(EventType.syncedWithDatabase);
    }

    private IEnumerator PullAllSheeps(string url)
    {
        string uri = "?request=GetAllSheep&Farmer_UUID=" + Database.GetDatabase().farmerUUID;
        
        Debug.Log(url + uri);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url + uri))
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
                    string data = webRequest.downloadHandler.text;

                    Debug.Log(data);
                    
                    SheepCollectionJson sheepCollection = JsonUtility.FromJson<SheepCollectionJson>("{\"Sheeps\":" + data + "}");
                    foreach (SheepJSON shp in sheepCollection.Sheeps)
                    {
                        bool foundHit = false;
                        foreach (SheepObject sheepObj in Database.GetSheepCollection())
                        {
                            if (sheepObj.UUID == shp.Sheep_UUID)
                            {
                                long lastModified = Helpers.StringToTimestamp(shp.Last_Modified);
                                if (sheepObj.lastModified < lastModified)
                                {
                                    sheepObj.sheepTag = shp.Sheep_Label;
                                    sheepObj.isDeleted = int.Parse(shp.Is_Deleted);
                                    sheepObj.lastModified = lastModified;
                                    sheepObj.sex = (int.Parse(shp.Sheep_Female) == 0) ? Sex.Male : Sex.Female;
                                    sheepObj.tsBorn = Helpers.StringToTimestamp(shp.Timestamp_Born);
                                    Debug.Log("Locally updating sheep with UUID: " + sheepObj.UUID);
                                }
                                else if (sheepObj.lastModified != lastModified)
                                {
                                    Database.ProgressData(MethodType.Put, sheepObj);
                                    Debug.Log("Cloud updating sheep with UUID: " + sheepObj.UUID);
                                }

                                foundHit = true;
                                break;
                            }
                        }
                        if (!foundHit)
                        {
                            SheepObject newSheep = new SheepObject
                            {
                                sheepTag = shp.Sheep_Label,
                                UUID = shp.Sheep_UUID,
                                isDeleted = int.Parse(shp.Is_Deleted),
                                lastModified = Helpers.StringToTimestamp(shp.Last_Modified),
                                sex = (int.Parse(shp.Sheep_Female) == 0) ? Sex.Male : Sex.Female,
                                tsBorn = Helpers.StringToTimestamp(shp.Timestamp_Born)
                            };
                            Database.GetSheepCollection().Add(newSheep);
                            Debug.Log("Added sheep to local db with UUID: " + newSheep.UUID);
                        }
                    }
                    break;
            }
        }

        pullSheep = true;

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator PullAllWorms(string url)
    {
        string uri = "?request=GetAllWorm";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url + uri))
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
                    string data = webRequest.downloadHandler.text;

                    WormCollectionJson wormCollection = JsonUtility.FromJson<WormCollectionJson>("{\"Worms\":" + data + "}");
                    foreach (WormJSON wrm in wormCollection.Worms)
                    {
                        bool foundHit = false;
                        foreach (WormObject wormObj in Database.GetWormCollection())
                        {
                            if (wormObj.UUID == wrm.Worm_UUID)
                            {
                                wormObj.nonScienceName = wrm.Worm_Normal_Name;
                                wormObj.scientificName = wrm.Worm_Latin_Name;
                                wormObj.eggDescription = wrm.Worm_Egg_Description;
                                wormObj.EPGDanger = float.Parse(wrm.Worm_EPG_Danger);
                                Debug.Log("Locally updating worm with UUID: " + wormObj.UUID);

                                foundHit = true;
                                break;
                            }
                        }
                        if (!foundHit)
                        {
                            WormObject newWorm = new WormObject
                            {
                                UUID = wrm.Worm_UUID,
                                nonScienceName = wrm.Worm_Normal_Name,
                                scientificName = wrm.Worm_Latin_Name,
                                eggDescription = wrm.Worm_Egg_Description,
                                EPGDanger = float.Parse(wrm.Worm_EPG_Danger)
                            };
                            Database.GetWormCollection().Add(newWorm);
                            Debug.Log("Added worm to local db with UUID: " + newWorm.UUID);
                        }
                    }
                    break;
            }
        }

        pullWorm = true;

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator PullAllPairs(string url)
    {
        string uri = "?request=GetAllPair&Farmer_UUID=" + Database.GetDatabase().farmerUUID;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url + uri))
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
                    string data = webRequest.downloadHandler.text;

                    PairCollectionJson pairCollection = JsonUtility.FromJson<PairCollectionJson>("{\"Pairs\":" + data + "}");
                    foreach (PairJSON pr in pairCollection.Pairs)
                    {
                        bool foundHit = false;
                        foreach (PairCollection pairObj in Database.GetPairCollection())
                        {
                            if (pairObj.UUID == pr.Pair_UUID)
                            {
                                long lastModified = Helpers.StringToTimestamp(pr.Last_Modified);
                                if (pairObj.lastModified < lastModified)
                                {
                                    pairObj.pairCollectionName = pr.Pair_Name;
                                    pairObj.tsRemoved = Helpers.StringToTimestamp(pr.TS_Removed);
                                    pairObj.lastModified = lastModified;
                                    pairObj.tsFormed = Helpers.StringToTimestamp(pr.TS_Formed);
                                    Debug.Log("Locally updating pair with UUID: " + pairObj.UUID);
                                }
                                else if (pairObj.lastModified != lastModified)
                                {
                                    Database.ProgressData(MethodType.Put, pairObj);
                                    Debug.Log("Cloud updating pair with UUID: " + pairObj.UUID);
                                }

                                foundHit = true;
                                break;
                            }
                        }
                        if (!foundHit)
                        {
                            PairCollection newPair = new PairCollection
                            {
                                UUID = pr.Pair_UUID,
                                pairCollectionName = pr.Pair_Name,
                                tsRemoved = Helpers.StringToTimestamp(pr.TS_Removed),
                                lastModified = Helpers.StringToTimestamp(pr.Last_Modified),
                                tsFormed = Helpers.StringToTimestamp(pr.TS_Formed)
                            };
                            Database.GetPairCollection().Add(newPair);
                            Debug.Log("Added pair to local db with UUID: " + newPair.UUID);
                        }
                    }
                    break;
            }
        }

        pullPair = true;

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator PullAllLots(string url)
    {
        string uri = "?request=GetAllLot&Farmer_UUID=" + Database.GetDatabase().farmerUUID;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url + uri))
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
                    string data = webRequest.downloadHandler.text;

                    LotCollectionJson lotCollection = JsonUtility.FromJson<LotCollectionJson>("{\"Lots\":" + data + "}");
                    foreach (LotJSON lt in lotCollection.Lots)
                    {
                        bool foundHit = false;
                        foreach (LotObject lotObj in Database.GetLotCollection())
                        {
                            if (lotObj.UUID == lt.Lot_UUID)
                            {
                                long lastModified = Helpers.StringToTimestamp(lt.Last_Modified);
                                if (lotObj.lastModified < lastModified)
                                {
                                    lotObj.surfaceSqrMtr = int.Parse(lt.Lot_Surface);
                                    lotObj.surfaceQuality = float.Parse(lt.Lot_Quality);
                                    lotObj.isDeleted = int.Parse(lt.Is_Deleted);
                                    lotObj.lastModified = lastModified;
                                    lotObj.lastMowedTs = Helpers.StringToTimestamp(lt.Lot_Mowed_TS);
                                    lotObj.state = lt.Lot_State_ID;
                                    lotObj.perceelName = lt.Lot_Name;
                                    Debug.Log("Locally updating lot with UUID: " + lotObj.UUID);
                                }
                                else if (lotObj.lastModified != lastModified)
                                {
                                    Database.ProgressData(MethodType.Put, lotObj);
                                    Debug.Log("Cloud updating lot with UUID: " + lotObj.UUID);
                                }

                                foundHit = true;
                                break;
                            }
                        }
                        if (!foundHit)
                        {
                            LotObject newLot = new LotObject
                            {
                                UUID = lt.Lot_UUID,
                                surfaceSqrMtr = int.Parse(lt.Lot_Surface),
                                surfaceQuality = float.Parse(lt.Lot_Quality),
                                isDeleted = int.Parse(lt.Is_Deleted),
                                lastModified = Helpers.StringToTimestamp(lt.Last_Modified),
                                lastMowedTs = Helpers.StringToTimestamp(lt.Lot_Mowed_TS),
                                state = lt.Lot_State_ID,
                                perceelName = lt.Lot_Name
                            };
                            Database.GetLotCollection().Add(newLot);
                            Debug.Log("Added lot to local db with UUID: " + newLot.UUID);
                        }
                    }
                    break;
            }
        }

        pullLot = true;

        yield return new WaitForEndOfFrame();
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
    public void FireURI(string[] fieldCollection, string[] dataCollection, MethodType type, string request, string methodUUID)
    {
        StartCoroutine(FireURIRoutine(fieldCollection, dataCollection, type, request, methodUUID));
    }

    // TODO error handling for missing fields etc
    private IEnumerator FireURIRoutine(string[] fieldCollection, string[] dataCollection, MethodType type, string request, string methodUUID)
    {
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
            case MethodType.Put:
                pageUrl = "VerweidklokPutRequests.php";
                break;
        }

        Debug.Log(baseUrl + pageUrl + uri);

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
                            //Debug.Log(pages[page]; // URI
                            string data = webRequest.downloadHandler.text;
                            dataPackages.Add(methodUUID, data);
                            EventSystem<string>.InvokeEvent(EventType.checkDatabaseResponse, methodUUID);
                            break;
                    }
                }
                //Debug.Log("Fired: " + baseUrl + pageUrl + uri);

                // TODO return response through event system and set caller on "waiting"
                break;

            case MethodType.Put:
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
        }
    }
}
