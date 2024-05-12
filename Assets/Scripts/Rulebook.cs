using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rulebook : MonoBehaviour
{
    [SerializeField]
    private GameObject classExceptionsHolderObject;
    [SerializeField]
    private GameObject dietExceptionsHolderObject;
    [SerializeField]
    private GameObject speciesExceptionsHolderObject;

    [SerializeField]
    private Color classColour;

    [SerializeField]
    private Color speciesColour;

    [SerializeField]
    private Color dietColour;

    [SerializeField]
    private Image sectionImage;

    [SerializeField]
    private GameObject rulebookExceptionPrefab;

    public void PopulateExceptions(List<CrimeExceptionData> exceptions)
    {
        while (classExceptionsHolderObject.transform.childCount > 0)
        {
            DestroyImmediate(classExceptionsHolderObject.transform.GetChild(0).gameObject);
        }
        while (dietExceptionsHolderObject.transform.childCount > 0)
        {
            DestroyImmediate(dietExceptionsHolderObject.transform.GetChild(0).gameObject);
        }
        while (speciesExceptionsHolderObject.transform.childCount > 0)
        {
            DestroyImmediate(speciesExceptionsHolderObject.transform.GetChild(0).gameObject);
        }
        foreach (CrimeExceptionData exception in exceptions)
        {
            Transform spawnParent;
            if(exception.dietException != AnimalData.Animal_diet.invalid)
            {
                spawnParent = dietExceptionsHolderObject.transform;
            }
            else if(exception.speciesException != AnimalData.Animal_species.invalid)
            {
                spawnParent = speciesExceptionsHolderObject.transform;
            }
            else if(exception.classException != AnimalData.Animal_class.invalid)
            {
                spawnParent = classExceptionsHolderObject.transform;
            }
            else
            {
                continue;
            }
            GameObject spawnedExceptionObject = GameObject.Instantiate(rulebookExceptionPrefab, spawnParent);
            RulebookException spawnedException = spawnedExceptionObject.GetComponent<RulebookException>();
            spawnedException.Initialize(exception.animalSprite, exception.crimeSprite);
        }
    }


    public void SetClassTab()
    {
        AudioManager.Instance.PlaySound("page_flip", true);
        dietExceptionsHolderObject.SetActive(false);
        speciesExceptionsHolderObject.SetActive(false);
        classExceptionsHolderObject.SetActive(true);
        sectionImage.color = classColour;
    }
    public void SetDietTab()
    {
        AudioManager.Instance.PlaySound("page_flip", true);
        classExceptionsHolderObject.SetActive(false);
        speciesExceptionsHolderObject.SetActive(false);
        dietExceptionsHolderObject.SetActive(true);
        sectionImage.color = dietColour;
    }

    public void SetSpeciesTab()
    {
        AudioManager.Instance.PlaySound("page_flip", true);
        dietExceptionsHolderObject.SetActive(false);
        classExceptionsHolderObject.SetActive(false);
        speciesExceptionsHolderObject.SetActive(true);
        sectionImage.color = speciesColour;
    }




}
