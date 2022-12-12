using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SheepButton : MonoBehaviour
{
    public TextMeshProUGUI SheepUuidText;
    public TextMeshProUGUI SheepGenderText;
    public TextMeshProUGUI SheepSpeciesText;
    public TextMeshProUGUI SheepTSBornText;

    public Button button;
    public Button btnDelete;
    public SheepObject sheep;
    public SheepDataViewer dataViewer;

    // Start is called before the first frame update
    private void Start()
    {
        button.onClick.AddListener(delegate { dataViewer.ShowDetails(sheep); });
        btnDelete.onClick.AddListener(delegate { dataViewer.sheepDataReader.DeleteSheep(sheep); });
    }

    public void SetInfo(SheepObject _sheep, SheepDataViewer _dataReader)
    {
        sheep = _sheep;
        dataViewer = _dataReader;
        SheepUuidText.text = sheep.UUID;
        SheepGenderText.text = sheep.sex.ToString();
        SheepSpeciesText.text = sheep.sheepType.ToString();
        DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(sheep.tsBorn);
        SheepTSBornText.text = date.Day + "-" + date.Month + "-" + date.Year;
    }
}
