using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeideButton : MonoBehaviour
{
    public TextMeshProUGUI SheepUuidText;
    public TextMeshProUGUI SheepGenderText;
    public TextMeshProUGUI SheepSpeciesText;
    public TextMeshProUGUI SheepTSBornText;

    public Button button;
    public Button btnDelete;
    public WeideObject weide;
    public WeideDataViewer dataViewer;

    // Start is called before the first frame update
    private void Start()
    {
        button.onClick.AddListener(delegate { dataViewer.ShowDetails(weide); });
        btnDelete.onClick.AddListener(delegate { dataViewer.sheepDataReader.DeleteWeide(weide); });
    }

    public void SetInfo(WeideObject _weide, WeideDataViewer _dataViewer)
    {
        weide = _weide;
        dataViewer = _dataViewer;
        SheepUuidText.text = weide.UUID;
        //SheepGenderText.text = Dictionaries.SheepGenderNames[sheep.sex];
        //SheepGenderText.text = sheep.sex.ToString();
        //SheepSpeciesText.text = sheep.sheepType.ToString();
        //DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(sheep.tsBorn);
        //SheepTSBornText.text = date.Day + "-" + date.Month + "-" + date.Year;
    }
}
