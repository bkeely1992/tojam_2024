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

    [SerializeField]
    private Transform computerMiniGameParent;

    [SerializeField]
    private GameObject runner;

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

    public enum screen
    {
        exceptions,
        runner
    }

    public void ChangeScreen(screen selectedScreen)
    {
        switch (selectedScreen)
        {
            case screen.runner:
                computerExceptionsParent.gameObject.SetActive(false);
                Instantiate(runner, computerMiniGameParent);
                break;
            case screen.exceptions:
                computerExceptionsParent.gameObject.SetActive(true);
                if (computerMiniGameParent.childCount > 0)
                    DestroyImmediate(computerMiniGameParent.GetChild(0).gameObject);
                break;
        }
    }
}
