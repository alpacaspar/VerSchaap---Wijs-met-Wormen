using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeideButton : MonoBehaviour
{
    public TextMeshProUGUI txtPerceelName;

    public Button button;
    public Button btnDelete;
    public WeideObject weide;
    public WeideDataViewer dataViewer;
    public QuickAdviceDisplay adviceDisplay;

    // Start is called before the first frame update
    private void Start()
    {
        button.onClick.AddListener(delegate
        {
            dataViewer.panelMode = DetailsPanelMode.EditingElement;
            dataViewer.ShowDetails(weide); 
        });

        btnDelete.onClick.AddListener(delegate { dataViewer.sheepDataReader.DeleteWeide(weide); });
    }

    public void SetInfo(WeideObject _weide, WeideDataViewer _dataViewer)
    {
        weide = _weide;
        dataViewer = _dataViewer;
        txtPerceelName.text = weide.perceelName;
        adviceDisplay.UpdateDotsBasedOnQuality(weide.surfaceQuality);
    }
}
