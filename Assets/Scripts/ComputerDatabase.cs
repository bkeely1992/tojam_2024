using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerDatabase : MonoBehaviour
{
    [SerializeField]
    private Transform computerExceptionsParent;

    [SerializeField]
    private GameObject computerExceptionPrefab;

    public void SetExceptions(List<CrimeExceptionData> exceptions)
    {
        while (computerExceptionsParent.childCount > 0)
        {
            DestroyImmediate(computerExceptionsParent.GetChild(0).gameObject);
        }

        foreach (CrimeExceptionData exception in exceptions)
        {

            GameObject spawnedExceptionObject = GameObject.Instantiate(computerExceptionPrefab, computerExceptionsParent);
            ComputerException spawnedException = spawnedExceptionObject.GetComponent<ComputerException>();
            spawnedException.SetImages(exception.animalSprite, exception.crimeSprite);
        }
    }
}
