using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WeideButton : ObjectUUIDButton
{
    public TextMeshProUGUI txtPerceelName;

    public WeideObject weide;
    public WeideDataViewer dataViewer;
    public QuickAdviceDisplay adviceDisplay;

    public override void OnButtonClicked()
    {
        dataViewer.panelMode = DetailsPanelMode.EditingElement;
        dataViewer.ShowDetails(weide);
    }

    public override void OnDeleteButtonClicked()
    {
        dataViewer.sheepDataReader.DeleteButtonClicked(weide);
        //dataViewer.sheepDataReader.DeleteWeide(weide);
    }

    public void SetInfo(WeideObject _weide, WeideDataViewer _dataViewer)
    {
        weide = _weide;
        dataViewer = _dataViewer;
        txtPerceelName.text = weide.perceelName;
        adviceDisplay.UpdateDotsBasedOnQuality(weide.surfaceQuality);
    }
}
