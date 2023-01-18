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
    ClickToRemoveFromPairCollection,
    ClickToAddToPairCollection
}

public class SheepButton : ObjectUUIDButton
{
    public TextMeshProUGUI txtSheepTag;
    public TextMeshProUGUI txtSheepGender;

    public SheepObject sheep;
    public SheepDataViewer dataViewer;

    public SheepButtonMode buttonMode = SheepButtonMode.Nothing;

    public override void OnButtonClicked()
    {
        switch (buttonMode)
        {
            default:
                break;
            // Button is inside sheep menu
            case SheepButtonMode.ClickToEditOrRemove:
                dataViewer.panelMode = DetailsPanelMode.EditingElement;
                dataViewer.ShowDetails(sheep);
                break;
            // Button is inside PairCollection details menu
            case SheepButtonMode.ClickToRemoveFromPairCollection:
                dataViewer.panelMode = DetailsPanelMode.EditingElement;
                dataViewer.ShowDetails(sheep);
                break;
            // Button is inside add to PairCollection menu
            case SheepButtonMode.ClickToAddToPairCollection:
                // Set PairCollectionID for the sheep
                foreach (var tmpSheep in Database.GetDatabase().sheeps)
                {
                    if (tmpSheep.UUID == sheep.UUID)
                    {
                        tmpSheep.pairCollectionID = dataViewer.sheepDataReader.PairCollectionDataViewer.selectedElement.UUID;
                        break;
                    }
                }

                // Add the sheep to the PairCollection
                foreach (var tmpPairCollection in Database.GetDatabase().pairCollection)
                {
                    if (tmpPairCollection.UUID == sheep.pairCollectionID)
                    {
                        tmpPairCollection.allSheep.Add(sheep.UUID);
                        dataViewer.sheepDataReader.PairCollectionDataViewer.ShowDetails(tmpPairCollection);
                        dataViewer.sheepDataReader.PairCollectionDataViewer.addSheepPanel.gameObject.SetActive(false);
                        dataViewer.sheepDataReader.PairCollectionDataViewer.sheepListPanel.SetActive(true);
                        break;
                    }
                }
                break;
        }
    }

    public override void OnDeleteButtonClicked()
    {
        switch (buttonMode)
        {
            default:
                break;
            case SheepButtonMode.ClickToEditOrRemove:
                dataViewer.sheepDataReader.DeleteButtonClicked(sheep);
                //dataViewer.sheepDataReader.DeleteSheep(sheep);
                break;
            case SheepButtonMode.ClickToRemoveFromPairCollection:
                string PairCollectionUUID = "";

                // Clear PairCollectionID of the sheep
                foreach (var tmpSheep in Database.GetDatabase().sheeps)
                {
                    if (tmpSheep.UUID == sheep.UUID)
                    {
                        PairCollectionUUID = tmpSheep.pairCollectionID;
                        tmpSheep.pairCollectionID = "";
                        break;
                    }
                }

                // Remove the sheep from the PairCollection
                foreach (var tmpPairCollection in Database.GetDatabase().pairCollection)
                {
                    if (tmpPairCollection.UUID == PairCollectionUUID)
                    {
                        tmpPairCollection.allSheep.Remove(sheep.UUID);
                        dataViewer.sheepDataReader.PairCollectionDataViewer.ShowDetails(tmpPairCollection);
                        break;
                    }
                }
                break;
        }
    }

    // Start is called before the first frame update
    public override void OnStart()
    {
        base.OnStart();
        switch (buttonMode)
        {
            case SheepButtonMode.Nothing:
                btnDelete.gameObject.SetActive(false);
                break;
            case SheepButtonMode.ClickToAddToPairCollection:
                btnDelete.gameObject.SetActive(false);
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
