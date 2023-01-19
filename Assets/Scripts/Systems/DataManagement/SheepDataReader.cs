using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SheepDataReader : MonoBehaviour
{
    public TextAsset sheepDataFile;
    public SheepDataViewer sheepDataViewer;
    public WormDataViewer wormDataViewer;
    public LotDataViewer lotDataViewer;
    public PairCollectionDataViewer pairCollectionDataViewer;

    public GameObject pnlDelete;
    public Button btnDeleteConfirm;
    public Button btnDeleteCancel;

    public bool bConfirmToDelete;
    public Toggle bDeleteToggle;

    public TMP_InputField FarmerUUIDInputField;

    public List<string> receivedAnswers = new List<string>();

    public void OpenFileExplorer()
    {
        var paths = SFB.StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);
    }

    private void OnEnable()
    {
        EventSystem<string>.AddListener(EventType.checkDatabaseResponse, CheckResponse);
        EventSystem.AddListener(EventType.syncedWithDatabase, OnDatabaseLoaded);
    }

    private void OnDisable()
    {
        EventSystem<string>.RemoveListener(EventType.checkDatabaseResponse, CheckResponse);
        EventSystem.RemoveListener(EventType.syncedWithDatabase, OnDatabaseLoaded);
    }

    public void CheckResponse(string methodUUID)
    {
        if (Database.requests.Contains(methodUUID) && DBST.Instance.dataPackages.ContainsKey(methodUUID))
        {
            Debug.Log("Received UUID: " + methodUUID);
            Database.requests.Remove(methodUUID);
            receivedAnswers.Add(methodUUID);
        }
        else
        {
            Debug.LogError("Method UUID not found: " + methodUUID);
        }
    }

    private IEnumerator PostElement(ObjectUUID obj)
    {
        string[] result = Database.ProgressData(MethodType.Post, obj);

        Debug.Log("result length = " + result.Length);

        for (int i = 0; i < result.Length; i++)
        {
            Debug.Log("resultpart " + i + ": " + result[i]);
        }

        Debug.Log(Helpers.CodeToMessage(result[0]));
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator PutElement(ObjectUUID obj)
    {
        string[] result = Database.ProgressData(MethodType.Put, obj);

        Debug.Log("result length = " + result.Length);

        for (int i = 0; i < result.Length; i++)
        {
            Debug.Log("resultpart " + i + ": " + result[i]);
        }

        Debug.Log(Helpers.CodeToMessage(result[0]));
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator PostCollection<T>(List<T> collection) where T : ObjectUUID
    {
        foreach (var obj in collection)
        {
            yield return StartCoroutine(PostElement(obj));
        }
    }

    #region Sheeps
    private IEnumerator PostSheeps()
    {
        yield return PostCollection(Database.GetDatabase().sheeps);
    }

    private IEnumerator PutSheeps()
    {
        foreach (var sheep in Database.GetDatabase().sheeps)
        {
            yield return StartCoroutine(PutElement(sheep));
        }
    }

    private IEnumerator GetSheeps()
    {
        foreach (var sheep in Database.GetDatabase().sheeps)
        {
            yield return StartCoroutine(GetSheep(sheep));
        }
    }

    private IEnumerator GetSheep(SheepObject sheep)
    {
        Debug.Log("Asking for sheep");
        string[] result = Database.ProgressData(MethodType.Get, sheep);
        Debug.Log(Helpers.CodeToMessage(result[0]));
        yield return new WaitForEndOfFrame();

        while (!receivedAnswers.Contains(result[1]))
        {
            yield return new WaitForSeconds(1);
        }

        SheepCollectionJson sheepObj = JsonUtility.FromJson<SheepCollectionJson>("{\"Sheeps\":" + DBST.Instance.dataPackages[result[1]] + "}");
        DBST.Instance.dataPackages.Remove(result[1]);
        receivedAnswers.Remove(result[1]);

        if (sheep.lastModified < long.Parse(sheepObj.Sheeps[0].Last_Modified))
        {
            //update local sheep
            foreach (var sheepChanged in Database.GetDatabase().sheeps)
            {
                if (sheepChanged.UUID == sheepObj.Sheeps[0].Sheep_UUID)
                {
                    sheepChanged.isDeleted = int.Parse(sheepObj.Sheeps[0].Is_Deleted);
                    sheepChanged.lastModified = long.Parse(sheepObj.Sheeps[0].Last_Modified);
                    sheepChanged.sheepTag = sheepObj.Sheeps[0].Sheep_Label;
                    sheepChanged.tsBorn = long.Parse(sheepObj.Sheeps[0].Timestamp_Born);
                }
            }
        }
        else
        {
            //update cloud sheep
            StartCoroutine(PutElement(sheep));
        }
    }

#endregion

    #region Lot

    private IEnumerator PostLots()
    {
        foreach (var lot in Database.GetDatabase().Lots)
        {
            yield return StartCoroutine(PostElement(lot));
        }
    }

    private IEnumerator PutLots()
    {
        foreach (var lot in Database.GetDatabase().Lots)
        {
            yield return StartCoroutine(PutElement(lot));
        }
    }

    private IEnumerator GetLots()
    {
        foreach (var lot in Database.GetDatabase().Lots)
        {
            yield return StartCoroutine(GetLot(lot));
        }
    }

    private IEnumerator GetLot(LotObject lot)
    {
        Debug.Log("Asking for lot");
        string[] result = Database.ProgressData(MethodType.Get, lot);
        Debug.Log(Helpers.CodeToMessage(result[0]));
        yield return new WaitForEndOfFrame();

        while (!receivedAnswers.Contains(result[1]))
        {
            yield return new WaitForSeconds(1);
        }

        LotCollectionJson lotObj = JsonUtility.FromJson<LotCollectionJson>("{\"Lots\":" + DBST.Instance.dataPackages[result[1]] + "}");
        DBST.Instance.dataPackages.Remove(result[1]);
        receivedAnswers.Remove(result[1]);

        if (lot.lastModified < long.Parse(lotObj.Lots[0].Last_Modified))
        {
            //update local lot
            foreach (var lotChanged in Database.GetDatabase().Lots)
            {
                if (lotChanged.UUID == lotObj.Lots[0].Lot_UUID)
                {
                    lotChanged.isDeleted = int.Parse(lotObj.Lots[0].Is_Deleted);
                    lotChanged.lastModified = long.Parse(lotObj.Lots[0].Last_Modified);
                    lotChanged.perceelName = lotObj.Lots[0].Lot_Name;
                    lotChanged.surfaceQuality = float.Parse(lotObj.Lots[0].Lot_Quality);
                    lotChanged.surfaceSqrMtr = int.Parse(lotObj.Lots[0].Lot_Surface);
                    lotChanged.lastMowedTs = long.Parse(lotObj.Lots[0].Lot_Mowed_TS);
                }
            }
        }
        else
        {
            //update cloud lot
            StartCoroutine(PutElement(lot));
        }
    }

    #endregion

    IEnumerator PostDatabase()
    {
        yield return StartCoroutine(PostLots());
        yield return StartCoroutine(PostSheeps());
    }

    IEnumerator PutDatabase()
    {
        yield return StartCoroutine(PutLots());
        yield return StartCoroutine(PutSheeps());
    }

    private void Start()
    {
        btnDeleteCancel.onClick.AddListener(delegate { pnlDelete.SetActive(false); });

        FarmerUUIDInputField.SetTextWithoutNotify(Database.GetDatabase().farmerUUID);

        sheepDataViewer = GetComponent<SheepDataViewer>();
        wormDataViewer = GetComponent<WormDataViewer>();
        lotDataViewer = GetComponent<LotDataViewer>();
        pairCollectionDataViewer = GetComponent<PairCollectionDataViewer>();

        pairCollectionDataViewer.dataReader = this;
        wormDataViewer.dataReader = this;
        sheepDataViewer.sheepDataReader = this;
        lotDataViewer.sheepDataReader = this;

        OnDatabaseLoaded();
    }

    private IEnumerator TestFunc()
    {
        var list = Database.GetDatabase().sheeps;

        foreach (var s in list)
        {
            s.sheepType = (SheepType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(SheepType)).Length - 1); //-1 to exclude 'other'
            yield return StartCoroutine(PutElement(s));
            yield return new WaitForEndOfFrame();
        }

        yield return 0;
    }

    /// <summary>
    /// Creates the buttons for all elements in the database when it has finished loading
    /// </summary>
    public void OnDatabaseLoaded()
    {
        sheepDataViewer.RemoveAllButtons();
        wormDataViewer.RemoveAllButtons();
        pairCollectionDataViewer.RemoveAllButtons();
        lotDataViewer.RemoveAllButtons();
        
        pairCollectionDataViewer.CreateButtonsFromDB(Database.GetDatabase().pairCollection);
        wormDataViewer.CreateWormButtonsFromDB(Database.GetDatabase().worms);
        sheepDataViewer.CreateSheepButtonsFromDB(Database.GetDatabase().sheeps);
        lotDataViewer.CreateSheepButtonsFromDB(Database.GetDatabase().Lots);
        
        EventSystem.InvokeEvent(EventType.onSceneReady);
    }

    public T GetElementByUUID<T>(string uuid, List<T> list) where T : ObjectUUID
    {
        foreach (var element in list)
        {
            if (element.UUID == uuid) return element;
        }

        return null;
    }

    public string GetPairCollectionNameByUUID(string uuid)
    {
        PairCollection pair = GetElementByUUID(uuid, Database.GetDatabase().pairCollection);
        if (pair != null) return pair.pairCollectionName;
        return null;
    }

    public string GetPairCollectionUUIDByName(string name)
    {
        foreach (var pair in Database.GetDatabase().pairCollection)
        {
            if (pair.pairCollectionName == name) return pair.UUID;
        }

        return null;
    }

    public IEnumerator UpdateSheepData(SheepObject sheep)
    {
        Debug.Log("updatesheepdata, tag = " + sheep.sheepTag);
        int nChilds = sheepDataViewer.buttonContainer.childCount;

        // editing existing sheep
        if (sheepDataViewer.panelMode == DetailsPanelMode.EditingElement)
        {
            yield return StartCoroutine(PutElement(sheep));

            // update the visuals representing the data
            for (int i = 0; i < nChilds; i++)
            {
                var obj = sheepDataViewer.buttonContainer.GetChild(i).gameObject.GetComponentInChildren<SheepButton>();
                if (obj.sheep.UUID != sheepDataViewer.selectedSheep.UUID) continue;
                obj.SetInfo(sheep, sheepDataViewer);
                break;
            }
        }

        // Adding a new sheep
        // TODO check if UUID doesnt already exist
        else
        {
            foreach (var tmpSheep in Database.GetDatabase().sheeps)
            {
                if (tmpSheep.UUID == sheep.UUID)
                {
                    Debug.LogError("Cant add sheep! UUID already exists!");
                    yield return 0;
                }
            }

            //TODO get request to check if already exists
            yield return StartCoroutine(PostElement(sheep));
            var obj = sheepDataViewer.CreateNewButton(sheep);
            sheepDataViewer.MoveScrollViewToElement(obj.GetComponent<RectTransform>());
        }

        sheepDataViewer.panelMode = DetailsPanelMode.None;
        yield return 0;
    }

    public void UpdateLotData(LotObject Lot)
    {
        int nChilds = sheepDataViewer.buttonContainer.childCount;

        // editing existing Lot
        if (lotDataViewer.panelMode == DetailsPanelMode.EditingElement)
        {
            // update the actual data
            LotObject wds = GetElementByUUID(lotDataViewer.selectedElement.UUID, Database.GetDatabase().Lots);

            if (wds != null)
            {
                wds.perceelName = Lot.perceelName;
                wds.surfaceQuality = Lot.surfaceQuality;
                wds.surfaceSqrMtr = Lot.surfaceSqrMtr;
            }

            // update the visuals representing the data
            for (int i = 0; i < nChilds; i++)
            {
                var obj = lotDataViewer.LotButtonContainer.GetChild(i).gameObject.GetComponentInChildren<LotButton>();
                if (obj.Lot.UUID != lotDataViewer.selectedElement.UUID) continue;
                obj.SetInfo(Lot, lotDataViewer);
                break;
            }
        }

        // Adding a new Lot
        // TODO check if UUID doesnt already exist
        else
        {
            Database.GetDatabase().Lots.Add(Lot);
            lotDataViewer.CreateNewButton(Lot);
        }

        lotDataViewer.panelMode = DetailsPanelMode.None;
    }

    public IEnumerator DeleteElementCoroutine<T>(T element, DataViewer dataViewer = null) where T : ObjectUUID
    {
        var tmpElement = element;
        tmpElement.isDeleted = 1;
        yield return StartCoroutine(PutElement(tmpElement));

        // Make the dataviewer remove the button associated with the object
        if (dataViewer != null) dataViewer.RemoveButton(element);

        yield return 0;
    }

    public void DeleteButtonClicked(ObjectUUID obj)
    {
        bConfirmToDelete = bDeleteToggle.isOn;

        UnityEngine.Events.UnityAction sheepDeleteEvent = delegate { StartCoroutine(DeleteElementCoroutine(obj as SheepObject, sheepDataViewer)); };
        UnityEngine.Events.UnityAction lotDeleteEvent = delegate { StartCoroutine(DeleteElementCoroutine(obj as LotObject, lotDataViewer)); };
        UnityEngine.Events.UnityAction pairCollectionDeleteEvent = delegate { StartCoroutine(DeleteElementCoroutine(obj as PairCollection, pairCollectionDataViewer)); };

        if (bConfirmToDelete)
        {
            pnlDelete.SetActive(true);
            btnDeleteConfirm.onClick.RemoveAllListeners();

            switch (obj)
            {
                case SheepObject sheepObject:
                    btnDeleteConfirm.onClick.AddListener(sheepDeleteEvent);
                    break;
                case LotObject LotObject:
                    btnDeleteConfirm.onClick.AddListener(lotDeleteEvent);
                    break;
                case PairCollection pairCollection:
                    btnDeleteConfirm.onClick.AddListener(pairCollectionDeleteEvent);
                    break;
            }

            btnDeleteConfirm.onClick.AddListener(delegate { pnlDelete.SetActive(false); });
        }

        else
        {
            switch (obj)
            {
                case SheepObject sheepObject:
                    sheepDeleteEvent.Invoke();
                    break;
                case LotObject LotObject:
                    lotDeleteEvent.Invoke();
                    break;
                case PairCollection sheepKoppel:
                    pairCollectionDeleteEvent.Invoke();
                    break;
            }
        }
    }
}
