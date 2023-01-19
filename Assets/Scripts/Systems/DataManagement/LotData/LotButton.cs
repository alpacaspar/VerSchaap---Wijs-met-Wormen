using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LotButton : ObjectUUIDButton
{
    public TextMeshProUGUI txtPerceelName;

    public LotObject Lot;
    public LotDataViewer dataViewer;
    public QuickAdviceDisplay adviceDisplay;

    public override void OnButtonClicked()
    {
        dataViewer.panelMode = DetailsPanelMode.EditingElement;
        dataViewer.ShowDetails(Lot);
    }

    public override void OnDeleteButtonClicked()
    {
        dataViewer.sheepDataReader.DeleteButtonClicked(Lot);
        //dataViewer.sheepDataReader.DeleteLot(Lot);
    }

    public void SetInfo(LotObject _Lot, LotDataViewer _dataViewer)
    {
        Lot = _Lot;
        dataViewer = _dataViewer;
        txtPerceelName.text = Lot.perceelName;
        adviceDisplay.UpdateDots((float)VerweidAdvisor.CalcValue(null, Lot.surfaceSqrMtr, Lot.currentSheeps.Count));
    }
}
