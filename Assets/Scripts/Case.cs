using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Case : MonoBehaviour
{
    public CaseData CurrentCaseData => currentCaseData;
    private CaseData currentCaseData;

    private AnimalData animalData;

    [SerializeField]
    private Image animalImage;

    [SerializeField]
    private TMPro.TMP_Text animalNameText;

    [SerializeField]
    private Image crimeImage;

    [SerializeField]
    private TMPro.TMP_Text crimeNameText;

    public void Activate(CaseData inCaseData)
    {
        currentCaseData = inCaseData;
        animalData = currentCaseData.CurrentAnimal;
        Debug.Log("Animal["+animalData.Species.ToString()+"] did crime["+currentCaseData.CurrentCrime.crimeValue.ToString()+"]?");

        //Set the images on the case

        //Play a papers shuffling sound
    }
}
