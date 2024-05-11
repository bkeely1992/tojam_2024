using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used for managing the details of the visualization of the computer database in-game
public class ComputerDatabase : MonoBehaviour
{
    [SerializeField]
    private Transform computerExceptionsParent;

    [SerializeField]
    private GameObject computerExceptionPrefab;

    //Creates all of the daily exceptions in the computer database
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
