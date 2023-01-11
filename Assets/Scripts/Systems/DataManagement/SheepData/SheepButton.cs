using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum SheepButtonMode
{
    Nothing,
    ClickToEditOrRemove,
    ClickToRemoveFromKoppel
}

public class SheepButton : MonoBehaviour
{
    public TextMeshProUGUI txtSheepTag;
    public TextMeshProUGUI txtSheepGender;

    public Button button;
    public Button btnDelete;
    public SheepObject sheep;
    public SheepDataViewer dataViewer;

    public SheepButtonMode buttonMode = SheepButtonMode.Nothing;

    // Start is called before the first frame update
    private void Start()
    {
        switch (buttonMode)
        {
            default:
                btnDelete.gameObject.SetActive(false);
                break;
            case SheepButtonMode.ClickToEditOrRemove:
                button.onClick.AddListener(delegate { dataViewer.ShowDetails(sheep); });
                btnDelete.onClick.AddListener(delegate { dataViewer.sheepDataReader.DeleteSheep(sheep); });
                break;
            case SheepButtonMode.ClickToRemoveFromKoppel:
                break;

        }
    }

    public void SetInfo(SheepObject _sheep, SheepDataViewer _dataReader)
    {
        sheep = _sheep;
        dataViewer = _dataReader;
        txtSheepTag.text = sheep.sheepTag;
        txtSheepGender.text = Dictionaries.SheepGenderNames[sheep.sex];
    }
}
