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
    ClickToRemoveFromKoppel,
    ClickToAddToKoppel
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
            // Button is inside koppel details menu
            case SheepButtonMode.ClickToRemoveFromKoppel:
                dataViewer.panelMode = DetailsPanelMode.EditingElement;
                dataViewer.ShowDetails(sheep);
                break;
            // Button is inside add to koppel menu
            case SheepButtonMode.ClickToAddToKoppel:
                // Set koppelID for the sheep
                foreach (var tmpSheep in dataViewer.sheepDataReader.testDatabase.sheeps)
                {
                    if (tmpSheep.UUID == sheep.UUID)
                    {
                        tmpSheep.sheepKoppelID = dataViewer.sheepDataReader.koppelDataViewer.selectedElement.UUID;
                        break;
                    }
                }

                // Add the sheep to the koppel
                foreach (var tmpKoppel in dataViewer.sheepDataReader.testDatabase.sheepKoppels)
                {
                    if (tmpKoppel.UUID == sheep.sheepKoppelID)
                    {
                        tmpKoppel.allSheep.Add(sheep.UUID);
                        dataViewer.sheepDataReader.koppelDataViewer.ShowDetails(tmpKoppel);
                        dataViewer.sheepDataReader.koppelDataViewer.addSheepPanel.gameObject.SetActive(false);
                        dataViewer.sheepDataReader.koppelDataViewer.sheepListPanel.SetActive(true);
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
            case SheepButtonMode.ClickToRemoveFromKoppel:
                string koppelUUID = "";

                // Clear koppelID of the sheep
                foreach (var tmpSheep in dataViewer.sheepDataReader.testDatabase.sheeps)
                {
                    if (tmpSheep.UUID == sheep.UUID)
                    {
                        koppelUUID = tmpSheep.sheepKoppelID;
                        tmpSheep.sheepKoppelID = "";
                        break;
                    }
                }

                // Remove the sheep from the koppel
                foreach (var tmpKoppel in dataViewer.sheepDataReader.testDatabase.sheepKoppels)
                {
                    if (tmpKoppel.UUID == koppelUUID)
                    {
                        tmpKoppel.allSheep.Remove(sheep.UUID);
                        dataViewer.sheepDataReader.koppelDataViewer.ShowDetails(tmpKoppel);
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
            case SheepButtonMode.ClickToAddToKoppel:
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
