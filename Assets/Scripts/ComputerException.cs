using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script used for managing the visualization of a given computer exception
public class ComputerException : MonoBehaviour
{
    [SerializeField]
    private Image animalImage;

    [SerializeField]
    private Image crimeImage;
    
    public void SetImages(Sprite animalSprite, Sprite crimeSprite)
    {
        animalImage.sprite = animalSprite;
        crimeImage.sprite = crimeSprite;
    }
}
