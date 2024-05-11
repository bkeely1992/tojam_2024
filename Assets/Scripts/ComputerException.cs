using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
