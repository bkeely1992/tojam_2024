using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script used for managing the visualization of a given computer exception
public class ComputerException : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text crimeNameText;

    [SerializeField]
    private Image crimeImage;

    [SerializeField]
    private GameObject animalExceptionPrefab;
    [SerializeField]
    private Transform animalExceptionsHolder;
    
    public void Initialize(string crimeName, Sprite crimeSprite, List<Sprite> animalSprites)
    {
        crimeNameText.text = crimeName;
        crimeImage.sprite = crimeSprite;
        foreach(Sprite animalSprite in animalSprites)
        {
            GameObject animalExceptionObject = Instantiate(animalExceptionPrefab, animalExceptionsHolder);
            Image animalExceptionImage = animalExceptionObject.GetComponent<Image>();
            animalExceptionImage.sprite = animalSprite;
        }
    }
}
