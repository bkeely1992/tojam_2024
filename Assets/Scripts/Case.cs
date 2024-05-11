using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The current Case in the scene
public class Case : MonoBehaviour
{
    //The data for the current case used for driving the visuals and gameplay
    public CaseData CurrentCaseData => currentCaseData;
    private CaseData currentCaseData;
    private AnimalData animalData;

    //Visualization of the animal on the case
    [SerializeField]
    private Image animalImage;

    //Text field used for showing the animal's name
    [SerializeField]
    private TMPro.TMP_Text animalNameText;

    //Visualization of the crime that the animal is accused of committing
    [SerializeField]
    private Image crimeImage;

    //Text field used for showing the name of the crime
    [SerializeField]
    private TMPro.TMP_Text crimeNameText;

    //Activates the case and shows it
    public void Activate(CaseData inCaseData)
    {
        currentCaseData = inCaseData;
        animalData = currentCaseData.CurrentAnimal;
        Debug.Log("Animal["+animalData.Species.ToString()+"] did crime["+currentCaseData.CurrentCrime.crimeValue.ToString()+"]?");

        //Set the images on the case
        animalImage.sprite = animalData.characterSprite;
        animalNameText.text = animalData.CurrentName;
        crimeImage.sprite = currentCaseData.CurrentCrime.sprite;
        crimeNameText.text = currentCaseData.CurrentCrime.crimeText;
        
        
        //Play a papers shuffling sound

    }
}
