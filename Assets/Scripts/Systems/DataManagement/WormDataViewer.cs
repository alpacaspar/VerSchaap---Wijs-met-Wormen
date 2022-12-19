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

    public TextMeshProUGUI detailsPanelTitle;

    public TMP_InputField inputUUID; //string UUID
    public TMP_Dropdown inputWormType; //WormType wormType
    public TMP_InputField inputNonScienceName; //Non-science name
    //List<WormMedicines> effectiveMedicines
    //List<WormResistences> resistences
    //List<WormSymptoms> symptoms
    //List<WormFaveConditions> faveConditions
    //List<string> extraRemarks

    public Button btnCancel;
    public Button btnSave;

    public WormObject selectedWorm;
    public bool bAddingSheep = false;
    public SheepDataReader dataReader;

    public Image wormImg;
    public Dictionary<string, Sprite> wormImages = new Dictionary<string, Sprite>();

    private void LoadImages()
    {
        var textures = Resources.LoadAll("WormImages/Eggs", typeof(Sprite));

        foreach (var t in textures)
        {
            wormImages.Add(t.name, (Sprite)t);
        }
    }

    public void UpdateImage(string sheepName)
    {
        Sprite spr = null;
        wormImages.TryGetValue(sheepName, out spr);
        wormImg.sprite = spr;
    }

    private void Start()
    {
        SetupDetailsPanel();
        SetupButtons();
        LoadImages();
    }

    private void SetupButtons()
    {
        btnCancel.onClick.AddListener(delegate
        {
            overviewPanel.SetActive(true);
            detailsPanel.SetActive(false);
            bAddingSheep = false;
        });

        btnSave.onClick.AddListener(delegate
        {
            Enum.TryParse<WormType>(inputWormType.GetComponentInChildren<TextMeshProUGUI>().text, out WormType wormType);

            WormObject tmpWorm = new WormObject
            {
                UUID = inputUUID.text,
                wormType = wormType,
                nonScienceName = inputNonScienceName.text
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

        inputWormType.onValueChanged.AddListener(delegate { UpdateImage(inputWormType.captionText.text); });
    }

    public void ShowDetails(WormObject worm)
    {
        selectedWorm = worm;
        overviewPanel.SetActive(false);
        detailsPanel.SetActive(true);
        detailsPanelTitle.text = worm.nonScienceName;
        inputUUID.SetTextWithoutNotify(worm.UUID);
        inputNonScienceName.SetTextWithoutNotify(worm.nonScienceName);
        UpdateImage(selectedWorm.wormType.ToString());

        // Set the worm type input dropdown to the correct value
        for (int i = 0; i < inputWormType.options.Count; i++)
        {
            if (!string.Equals(inputWormType.options[i].text, worm.wormType.ToString(), StringComparison.CurrentCultureIgnoreCase)) continue;
            inputWormType.value = i;
            break;
        }
    }
}
