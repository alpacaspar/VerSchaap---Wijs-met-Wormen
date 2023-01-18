using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class PairCollectionDataViewer : DataViewer
{
    [Header("Prefabs")]
    public GameObject PairButtonPrefab;
    public GameObject SheepButtonPrefab;

    [Header("UI Panels")]
    public RectTransform PairButtonContainer;
    public RectTransform PairSheepListButtonContainer;
    public GameObject overviewPanel;
    public GameObject detailsPanel;
    public GameObject sheepListPanel;
    public GameObject addSheepPanel;
    public RectTransform addSheepContainer;

    [Header("Pair variable fields")]
    public TMP_InputField inputName;

    [Header("Element Options")]
    public Button btnCancel;
    public Button btnSave;
    public Button btnAddSheep;
    public Button btnAddPair;
    public Button btnShowAllSheep;

    [HideInInspector]
    public PairCollection selectedElement;
    //[HideInInspector]
    //public bool bAddingElement = false;
    [HideInInspector]
    public SheepDataReader dataReader;

    [Header("Other")]
    public ScrollRect scrollRect;

    public override GameObject CreateNewButton(ObjectUUID objToAdd)
    {
        PairCollection Pair = objToAdd as PairCollection;
        var buttonGameObject = Instantiate(PairButtonPrefab, PairButtonContainer);
        buttonGameObject.GetComponentInChildren<PairCollectionButton>().SetInfo(Pair, this);
        return buttonGameObject;
    }

    public override void RemoveButton(ObjectUUID objToRemove)
    {
        PairCollection Pair = objToRemove as PairCollection;
        if (Pair == null) return;

        for (int i = 0; i < PairButtonContainer.childCount; i++)
        {
            var butObj = PairButtonContainer.GetChild(i).gameObject;
            var but = butObj.GetComponentInChildren<PairCollectionButton>();

            if (but.element.UUID == Pair.UUID)
            {
                Destroy(butObj);
                break;
            }
        }
    }

    private void Start()
    {
        SetupDetailsPanel();
        SetupButtons();
    }

    private void SetupButtons()
    {
        btnShowAllSheep.onClick.AddListener(delegate
        {
            detailsPanel.gameObject.SetActive(false);
            dataReader.sheepDataViewer.overviewPanel.SetActive(true);
        });

        // Button for adding new Pair
        btnAddPair.onClick.AddListener(delegate
        {
            panelMode = DetailsPanelMode.CreatingElement;
            //bAddingElement = true;
            selectedElement = new PairCollection
            {
                pairCollectionName = "Nieuwe Koppel",
                UUID = Helpers.GenerateUUID()
            };

            Database.GetDatabase().pairCollection.Add(selectedElement);
            CreateButtonsFromDB(Database.GetDatabase().pairCollection);
            ShowDetails(selectedElement);
        });

        // Button for adding sheep to Pair
        btnAddSheep.onClick.AddListener(delegate
        {
            bool panelActive = !addSheepPanel.activeSelf;

            sheepListPanel.SetActive(!panelActive);
            addSheepPanel.SetActive(panelActive);

            if (panelActive)
            {
                RemoveAllChildren(addSheepContainer);

                foreach (var sheep in Database.GetDatabase().sheeps)
                {
                    if (sheep.pairCollectionID == "")
                    {
                        var panelGameObject = Instantiate(SheepButtonPrefab, addSheepContainer);
                        var sheepButton = panelGameObject.GetComponentInChildren<SheepButton>();
                        sheepButton.buttonMode = SheepButtonMode.ClickToAddToPairCollection;
                        sheepButton.SetInfo(sheep, FindObjectOfType<SheepDataViewer>());
                    }
                }
            }
        });

        // Pair name input field
        inputName.onEndEdit.AddListener(delegate
        {
            selectedElement.pairCollectionName = inputName.text;

            for (int i = 0; i < PairButtonContainer.childCount; i++)
            {
                PairCollectionButton but = PairButtonContainer.GetChild(i).GetComponentInChildren<PairCollectionButton>();
                if (but.element.UUID == selectedElement.UUID)
                {
                    but.SetInfo(selectedElement, this);
                    break;
                }
            }
        });
    }

    public void CreateButtonsFromDB(List<PairCollection> elementList)
    {
        RemoveAllChildren(PairButtonContainer);

        foreach (PairCollection p in elementList)
        {
            CreateNewButton(p);
        }
    }

    public void RemoveAllChildren(RectTransform obj)
    {
        for (int i = obj.childCount - 1; i >= 0; i--)
        {
            Destroy(obj.GetChild(i).gameObject);
        }
    }

    public GameObject CreateNewSheepButtons()
    {
        RemoveAllChildren(PairSheepListButtonContainer);

        foreach (var sheepuuid in selectedElement.allSheep)
        {
            var foundSheep = dataReader.GetSheepObjectByUUID(sheepuuid);

            // sheep with this uuid was found in the database
            if (foundSheep != null)
            {
                var panelGameObject = Instantiate(SheepButtonPrefab, PairSheepListButtonContainer);
                var sheepButton = panelGameObject.GetComponentInChildren<SheepButton>();
                sheepButton.buttonMode = SheepButtonMode.ClickToRemoveFromPairCollection;
                sheepButton.SetInfo(foundSheep, FindObjectOfType<SheepDataViewer>());
            }
        }
        // get sheep bij UUID
        return null;
    }

    public void MoveScrollViewToElement(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        PairButtonContainer.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(PairButtonContainer.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    public void SetupDetailsPanel()
    {

    }

    public void SetPanelVisibilty(bool showDetails)
    {
        if (overviewPanel != null) overviewPanel.SetActive(!showDetails);
        if (detailsPanel != null) detailsPanel.SetActive(showDetails);
    }

    public void ShowDetails(PairCollection element)
    {
        selectedElement = element;
        inputName.SetTextWithoutNotify(selectedElement.pairCollectionName);
        SetPanelVisibilty(true);
        SetDetailsPanelTitle("Pair");
        CreateNewSheepButtons();
    }
}
