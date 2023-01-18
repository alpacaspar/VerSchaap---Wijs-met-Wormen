using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PairButton : MonoBehaviour
{
    public TextMeshProUGUI txtPairName;

    public Button button;
    public Button btnDelete;
    public PairCollection element;
    public PairCollectionDataViewer dataViewer;

    // Start is called before the first frame update
    private void Start()
    {
        button.onClick.AddListener(delegate
        {
            dataViewer.panelMode = DetailsPanelMode.EditingElement;
            dataViewer.ShowDetails(element);
        });

        btnDelete.onClick.AddListener(delegate
        {
            dataViewer.dataReader.DeleteButtonClicked(element);
            //dataViewer.dataReader.DeletePair(element); 
        });
    }

    public void SetInfo(PairCollection _element, PairCollectionDataViewer _dataViewer)
    {
        element = _element;
        dataViewer = _dataViewer;
        txtPairName.text = element.pairCollectionName;
    }
}
