using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class KoppelDataViewer : DataViewer
{
    [Header("Prefabs")]
    public GameObject KoppelButtonPrefab;
    public GameObject SheepButtonPrefab;

    [Header("UI Panels")]
    public RectTransform koppelButtonContainer;
    public RectTransform koppelSheepListButtonContainer;
    public GameObject overviewPanel;
    public GameObject detailsPanel;
    public GameObject sheepListPanel;
    public GameObject addSheepPanel;
    public RectTransform addSheepContainer;

    [Header("Koppel variable fields")]
    public TMP_InputField inputName;

    [Header("Element Options")]
    public Button btnCancel;
    public Button btnSave;
    public Button btnAddSheep;
    public Button btnAddKoppel;
    public Button btnShowAllSheep;

    [HideInInspector]
    public SheepKoppel selectedElement;
    //[HideInInspector]
    //public bool bAddingElement = false;
    [HideInInspector]
    public SheepDataReader dataReader;

    [Header("Other")]
    public ScrollRect scrollRect;

    public override GameObject CreateNewButton(ObjectUUID objToAdd)
    {
        SheepKoppel koppel = objToAdd as SheepKoppel;
        var buttonGameObject = Instantiate(KoppelButtonPrefab, koppelButtonContainer);
        buttonGameObject.GetComponentInChildren<KoppelButton>().SetInfo(koppel, this);
        return buttonGameObject;
    }

    public override void RemoveButton(ObjectUUID objToRemove)
    {
        SheepKoppel koppel = objToRemove as SheepKoppel;
        if (koppel == null) return;

        for (int i = 0; i < koppelButtonContainer.childCount; i++)
        {
            var butObj = koppelButtonContainer.GetChild(i).gameObject;
            var but = butObj.GetComponentInChildren<KoppelButton>();

            if (but.element.UUID == koppel.UUID)
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

        // Button for adding new koppel
        btnAddKoppel.onClick.AddListener(delegate
        {
            panelMode = DetailsPanelMode.CreatingElement;
            //bAddingElement = true;
            selectedElement = new SheepKoppel
            {
                koppelName = "Nieuwe koppel",
                UUID = Helpers.GenerateUUID()
            };

            dataReader.testDatabase.sheepKoppels.Add(selectedElement);
            CreateButtonsFromDB(dataReader.testDatabase.sheepKoppels);
            ShowDetails(selectedElement);
        });

        // Button for adding sheep to koppel
        btnAddSheep.onClick.AddListener(delegate
        {
            bool panelActive = !addSheepPanel.activeSelf;

            sheepListPanel.SetActive(!panelActive);
            addSheepPanel.SetActive(panelActive);

            if (panelActive)
            {
                RemoveAllChildren(addSheepContainer);

                foreach (var sheep in dataReader.testDatabase.sheeps)
                {
                    if (sheep.sheepKoppelID == "")
                    {
                        var panelGameObject = Instantiate(SheepButtonPrefab, addSheepContainer);
                        var sheepButton = panelGameObject.GetComponentInChildren<SheepButton>();
                        sheepButton.buttonMode = SheepButtonMode.ClickToAddToKoppel;
                        sheepButton.SetInfo(sheep, FindObjectOfType<SheepDataViewer>());
                    }
                }
            }
        });

        // Koppel name input field
        inputName.onEndEdit.AddListener(delegate
        {
            selectedElement.koppelName = inputName.text;

            for (int i = 0; i < koppelButtonContainer.childCount; i++)
            {
                KoppelButton but = koppelButtonContainer.GetChild(i).GetComponentInChildren<KoppelButton>();
                if (but.element.UUID == selectedElement.UUID)
                {
                    but.SetInfo(selectedElement, this);
                    break;
                }
            }
        });
    }

    public void CreateButtonsFromDB(List<SheepKoppel> elementList)
    {
        RemoveAllChildren(koppelButtonContainer);

        foreach (SheepKoppel s in elementList)
        {
            CreateNewButton(s);
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
        RemoveAllChildren(koppelSheepListButtonContainer);

        foreach (var sheepuuid in selectedElement.allSheep)
        {
            var foundSheep = dataReader.GetSheepObjectByUUID(sheepuuid);

            // sheep with this uuid was found in the database
            if (foundSheep != null)
            {
                var panelGameObject = Instantiate(SheepButtonPrefab, koppelSheepListButtonContainer);
                var sheepButton = panelGameObject.GetComponentInChildren<SheepButton>();
                sheepButton.buttonMode = SheepButtonMode.ClickToRemoveFromKoppel;
                sheepButton.SetInfo(foundSheep, FindObjectOfType<SheepDataViewer>());
            }
        }
        // get sheep bij UUID
        return null;
    }

    public void MoveScrollViewToElement(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        koppelButtonContainer.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(koppelButtonContainer.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    public void SetupDetailsPanel()
    {

    }

    public void SetPanelVisibilty(bool showDetails)
    {
        if (overviewPanel != null) overviewPanel.SetActive(!showDetails);
        if (detailsPanel != null) detailsPanel.SetActive(showDetails);
    }

    public void ShowDetails(SheepKoppel element)
    {
        selectedElement = element;
        inputName.SetTextWithoutNotify(selectedElement.koppelName);
        SetPanelVisibilty(true);
        SetDetailsPanelTitle("Koppel");
        CreateNewSheepButtons();
    }
}
