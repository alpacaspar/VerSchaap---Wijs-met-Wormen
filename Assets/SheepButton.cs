using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SheepButton : MonoBehaviour
{
    public TextMeshProUGUI SheepUuidText;
    public TextMeshProUGUI SheepGenderText;
    public TextMeshProUGUI SheepSpeciesText;

    // Start is called before the first frame update
    public void SetInfo(SheepObject sheep)
    {
        SheepUuidText.text = sheep.UUID;
        SheepGenderText.text = sheep.sex.ToString();
        SheepSpeciesText.text = sheep.sheepType.ToString();
    }
}
