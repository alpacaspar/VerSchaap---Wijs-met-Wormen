using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WormButton : MonoBehaviour
{
    public TextMeshProUGUI WormUuidText;
    public TextMeshProUGUI WormTypeText;
    //public TextMeshProUGUI SheepSpeciesText;
    //public TextMeshProUGUI SheepTSBornText;

    public Button button;
    public Button btnDelete;
    public WormObject worm;
    public WormDataViewer dataViewer;

    // Start is called before the first frame update
    private void Start()
    {
        button.onClick.AddListener(delegate { dataViewer.ShowDetails(worm); });
        btnDelete.onClick.AddListener(delegate { dataViewer.dataReader.DeleteWorm(worm); });
    }

    public void SetInfo(WormObject _worm, WormDataViewer _dataReader)
    {
        worm = _worm;
        dataViewer = _dataReader;
        WormUuidText.text = worm.UUID;
        WormTypeText.text = worm.wormType.ToString();
    }
}
