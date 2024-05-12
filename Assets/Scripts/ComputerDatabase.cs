using Assets.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

//Script used for managing the details of the visualization of the computer database in-game
public class ComputerDatabase : MonoBehaviour
{
    private enum State
    {
        live, confirming, screensaver, login, game, invalid
    }
    private State currentState = State.login;
    [SerializeField]
    private GameObject computerExceptionsPageObject;

    [SerializeField]
    private Transform computerExceptionsParent;

    [SerializeField]
    private GameObject computerExceptionPrefab;
    [SerializeField]
    private Transform computerExceptionButtonsParent;
    [SerializeField]
    private GameObject computerExceptionButtonPrefab;

    [SerializeField]
    private Screensaver screensaver;

    [SerializeField]
    private GameObject loginParent;

    [SerializeField]
    private GameObject confirmationParent;

    [SerializeField]
    private Transform computerMiniGameParent;

    [SerializeField]
    private List<GameObject> minigamePrefabs;

    [SerializeField]
    private List<GameObject> confirmationOptions;

    public float timeBeforeConfirmationRequired;
    public float timeBeforeScreensaver;

    public float confirmationMaxHeightVariance;
    public float confirmationMaxWidthVariance;

    [SerializeField]
    private Color highlightButtonColour;
    [SerializeField]
    private Color baseButtonColour;

    private Vector3 confirmationStartingPosition;
    private float timeWaiting = 0.0f;
    private GameObject currentMinigameObject;

    private Dictionary<CrimeData.Crime, GameObject> crimeExceptionCategoryObjectMap = new Dictionary<CrimeData.Crime, GameObject>();
    private Dictionary<CrimeData.Crime, Image> crimeExceptionButtonImageMap = new Dictionary<CrimeData.Crime, Image>();

    private void Start()
    {
        confirmationStartingPosition = confirmationParent.transform.position;
        screensaver.onMouseEnterScreen.AddListener(EndScreensaver);
    }

    private void OnDisable()
    {
        screensaver.onMouseEnterScreen.RemoveListener(EndScreensaver);
    }

    private void Update()
    {
        
        switch(currentState)
        {
            case State.live:
                timeWaiting += Time.deltaTime;
                if(timeWaiting > timeBeforeConfirmationRequired)
                {
                    //confirmationParent.transform.position = confirmationStartingPosition + new Vector3(Random.Range(-confirmationMaxWidthVariance, confirmationMaxWidthVariance), Random.Range(-confirmationMaxHeightVariance, confirmationMaxHeightVariance), 0f);
                    foreach(GameObject confirmationOption in confirmationOptions)
                    {
                        confirmationOption.SetActive(false);
                    }
                    confirmationOptions[UnityEngine.Random.Range(0, confirmationOptions.Count)].SetActive(true);
                    confirmationParent.SetActive(true);
                    currentState = State.confirming;
                }
                break;
            case State.confirming:
                timeWaiting += Time.deltaTime;
                if(timeWaiting > timeBeforeScreensaver)
                {
                    ResetComputer();
                }
                break;
        }
    }

    //Creates all of the daily exceptions in the computer database
    public void SetExceptions(List<CrimeExceptionData> exceptions)
    {
        while (computerExceptionsParent.childCount > 0)
        {
            DestroyImmediate(computerExceptionsParent.GetChild(0).gameObject);
        }
        while (computerExceptionButtonsParent.childCount > 0)
        {
            DestroyImmediate(computerExceptionButtonsParent.GetChild(0).gameObject);
        }
        Dictionary<CrimeData.Crime, List<Sprite>> crimeExceptionMap = new Dictionary<CrimeData.Crime, List<Sprite>>();
        Dictionary<CrimeData.Crime, Sprite> crimeSpriteMap = new Dictionary<CrimeData.Crime, Sprite>();
        Dictionary<CrimeData.Crime, string> crimeNameMap = new Dictionary<CrimeData.Crime, string>();

        foreach (CrimeExceptionData exception in exceptions)
        {
            if(!crimeExceptionMap.ContainsKey(exception.crime))
            {
                crimeExceptionMap.Add(exception.crime, new List<Sprite>());
                crimeSpriteMap.Add(exception.crime, exception.crimeSprite);
                crimeNameMap.Add(exception.crime, exception.crimeName);
            }
            crimeExceptionMap[exception.crime].Add(exception.animalSprite);
        }
        crimeExceptionCategoryObjectMap.Clear();
        crimeExceptionButtonImageMap.Clear();

        bool isFirst = true;
        foreach (KeyValuePair<CrimeData.Crime, List<Sprite>> crime in crimeExceptionMap)
        {

            //For each crime, create a button
            GameObject spawnedCrimeButtonObject = Instantiate(computerExceptionButtonPrefab, computerExceptionButtonsParent);
            CrimeExceptionButton crimeButton = spawnedCrimeButtonObject.GetComponent<CrimeExceptionButton>();

            
            //Add listener for the button
            crimeButton.onPress.AddListener(OnCrimeButtonPress);
            crimeButton.crime = crime.Key;
            crimeButton.crimeButtonImage.sprite = crimeSpriteMap[crime.Key];

            //Then create a page
            GameObject spawnedExceptionObject = GameObject.Instantiate(computerExceptionPrefab, computerExceptionsParent);
            ComputerException spawnedException = spawnedExceptionObject.GetComponent<ComputerException>();
            spawnedException.Initialize(crimeNameMap[crime.Key], crimeSpriteMap[crime.Key], crime.Value);

            if (isFirst)
            {
                spawnedExceptionObject.SetActive(true);
                crimeButton.crimeButtonImage.color = highlightButtonColour;
                isFirst = false;
            }

            //Add to the map
            crimeExceptionCategoryObjectMap.Add(crime.Key, spawnedExceptionObject);
            crimeExceptionButtonImageMap.Add(crime.Key, crimeButton.crimeButtonImage);
        }
    }

    private void OnCrimeButtonPress(CrimeData.Crime crime)
    {
        foreach(KeyValuePair<CrimeData.Crime, GameObject> crimeCategoryObject in crimeExceptionCategoryObjectMap)
        {
            crimeExceptionButtonImageMap[crimeCategoryObject.Key].color = baseButtonColour;
            crimeCategoryObject.Value.SetActive(false);
        }
        crimeExceptionButtonImageMap[crime].color = highlightButtonColour;
        crimeExceptionCategoryObjectMap[crime].SetActive(true);
    }

    public void PressLogin()
    {
        loginParent.SetActive(false);
        currentMinigameObject = Instantiate(minigamePrefabs[UnityEngine.Random.Range(0, minigamePrefabs.Count)], computerMiniGameParent);
        Minigame minigame = currentMinigameObject.GetComponent<Minigame>();
        minigame.onLose.AddListener(LoseGame);
        minigame.onWin.AddListener(CompleteGame);
        computerMiniGameParent.gameObject.SetActive(true);
        currentState = State.game;
    }

    public void PressConfirmation()
    {
        confirmationParent.SetActive(false);
        timeWaiting = 0.0f;
        currentState = State.live;
    }

    public void PressNo()
    {
        timeWaiting = 0.0f;
        computerExceptionsPageObject.SetActive(false);
        confirmationParent.SetActive(false);
        screensaver.gameObject.SetActive(true);
        currentState = State.screensaver;
    }

    public void LoseGame()
    {
        Minigame minigame = currentMinigameObject.GetComponent<Minigame>();
        minigame.onLose.RemoveListener(LoseGame);
        minigame.onWin.RemoveListener(CompleteGame);
        Destroy(currentMinigameObject);
        
        computerMiniGameParent.gameObject.SetActive(false);
        loginParent.SetActive(true);
        currentState = State.login;
    }

    public void CompleteGame()
    {
        Minigame minigame = currentMinigameObject.GetComponent<Minigame>();
        minigame.onLose.RemoveListener(LoseGame);
        minigame.onWin.RemoveListener(CompleteGame);
        Destroy(currentMinigameObject);
        
        computerMiniGameParent.gameObject.SetActive(false);
        computerExceptionsPageObject.SetActive(true);
        currentState = State.live;
    }

    public void EndScreensaver()
    {
        screensaver.gameObject.SetActive(false);
        loginParent.SetActive(true);
        currentState = State.login;
    }

    public void ResetComputer()
    {
        if(currentMinigameObject != null)
        {
            Minigame minigame = currentMinigameObject.GetComponent<Minigame>();
            minigame.onLose.RemoveListener(LoseGame);
            minigame.onWin.RemoveListener(CompleteGame);
            Destroy(currentMinigameObject);
        }

        timeWaiting = 0.0f;
        computerMiniGameParent.gameObject.SetActive(false);
        computerExceptionsPageObject.SetActive(false);
        confirmationParent.SetActive(false);
        loginParent.SetActive(false);
        
        screensaver.gameObject.SetActive(true);
        currentState = State.screensaver;
    }
}
