using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulebookException : MonoBehaviour
{
    [SerializeField]
    private Image animalImage;

    [SerializeField]
    private Image crimeImage;

    public void Initialize(Sprite animalSprite, Sprite crimeSprite)
    {
        animalImage.sprite = animalSprite;
        crimeImage.sprite = crimeSprite;
    }
}
