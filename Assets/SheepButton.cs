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
    public SheepObject sheep;
    public SheepDataReader dataReader;

    // Start is called before the first frame update
    private void Start()
    {
        button.onClick.AddListener(delegate { dataReader.ShowDetails(sheep); });
    }

    public void SetInfo(SheepObject _sheep, SheepDataReader _dataReader)
    {
        sheep = _sheep;
        dataReader = _dataReader;
        SheepUuidText.text = sheep.UUID;
        SheepGenderText.text = sheep.sex.ToString();
        SheepSpeciesText.text = sheep.sheepType.ToString();
        DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(sheep.tsBorn);
        SheepTSBornText.text = date.Day + "-" + date.Month + "-" + date.Year;
    }
}
