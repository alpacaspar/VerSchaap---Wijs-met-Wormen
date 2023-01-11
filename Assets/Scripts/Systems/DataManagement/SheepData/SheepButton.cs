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
                btnDelete.onClick.AddListener(delegate
                {
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
                });
                break;
            case SheepButtonMode.ClickToAddToKoppel:
                btnDelete.gameObject.SetActive(false);
                button.onClick.AddListener(delegate
                {
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
                });
                
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
