using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class WormDataViewer : MonoBehaviour
{
    public RectTransform wormUIPanel;
    public GameObject wormUIObject;

    public GameObject overviewPanel;
    public GameObject detailsPanel;

    public TMP_InputField inputUUID; //string UUID
    public TMP_Dropdown inputWormType; //WormType wormType
    //List<WormMedicines> effectiveMedicines
    //List<WormResistences> resistences
    //List<WormSymptoms> symptoms
    //List<WormFaveConditions> faveConditions
    //List<string> extraRemarks

    public Button btnCancel;
    public Button btnSave;
    public Button btnAddWorm;

    public WormObject selectedWorm;

    // dirty var to fix the calendar
    public DateTimeOffset tmpTime = new DateTimeOffset(DateTime.UtcNow);
    public bool bAddingSheep = false;
    public SheepDataReader dataReader;

    private void Start()
    {
        //calendarWidget.sheepDataReader = this;
        SetupDetailsPanel();
        SetupButtons();
    }

    private void SetupButtons()
    {
        btnCancel.onClick.AddListener(delegate
        {
            overviewPanel.SetActive(true);
            detailsPanel.SetActive(false);
            bAddingSheep = false;
        });

        btnAddWorm.onClick.AddListener(delegate
        {
            bAddingSheep = true;
            selectedWorm = new WormObject();
            ShowDetails(selectedWorm);
        });

        btnSave.onClick.AddListener(delegate
        {
            Enum.TryParse<WormType>(inputWormType.GetComponentInChildren<TextMeshProUGUI>().text, out WormType wormType);

            WormObject tmpWorm = new WormObject
            {
                UUID = inputUUID.text,
                wormType = wormType
            };

            dataReader.UpdateWormData(tmpWorm);
            overviewPanel.SetActive(true);
            detailsPanel.SetActive(false);
        });
    }

    public void CreateWormButtonsFromDB(List<WormObject> wormDatabase)
    {
        foreach (WormObject w in wormDatabase)
        {
            CreateNewWormButton(w);
        }
    }

    public void CreateNewWormButton(WormObject w)
    {
        var wormPanelGameObject = Instantiate(wormUIObject, wormUIPanel);
        wormPanelGameObject.GetComponentInChildren<WormButton>().SetInfo(w, this);
    }

    public void SetupDetailsPanel()
    {
        WormType[] valsWormType = (WormType[])Enum.GetValues(typeof(WormType));
        List<TMP_Dropdown.OptionData> options = valsWormType.Select(val => new TMP_Dropdown.OptionData(val.ToString())).ToList();
        inputWormType.AddOptions(options);
        options = new List<TMP_Dropdown.OptionData>();

        foreach (var val in valsWormType)
        {
            options.Add(new TMP_Dropdown.OptionData(val.ToString()));
        }
    }

    public void ShowDetails(WormObject worm)
    {
        selectedWorm = worm;
        overviewPanel.SetActive(false);
        detailsPanel.SetActive(true);
        inputUUID.SetTextWithoutNotify(worm.UUID);

        // Set the worm type input dropdown to the correct value
        for (int i = 0; i < inputWormType.options.Count; i++)
        {
            if (!string.Equals(inputWormType.options[i].text, worm.wormType.ToString(), StringComparison.CurrentCultureIgnoreCase)) continue;
            inputWormType.value = i;
            break;
        }
    }
}
