using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used for managing the details of the visualization of the computer database in-game
public class ComputerDatabase : MonoBehaviour
{
    private enum State
    {
        live, confirming, screensaver, login, game, invalid
    }
    private State currentState = State.login;

    [SerializeField]
    private Transform computerExceptionsParent;

    [SerializeField]
    private GameObject computerExceptionPrefab;

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

    private Vector3 confirmationStartingPosition;
    private float timeWaiting = 0.0f;
    private GameObject currentMinigameObject;

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
                    confirmationOptions[Random.Range(0, confirmationOptions.Count)].SetActive(true);
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

        foreach (CrimeExceptionData exception in exceptions)
        {
            GameObject spawnedExceptionObject = GameObject.Instantiate(computerExceptionPrefab, computerExceptionsParent);
            ComputerException spawnedException = spawnedExceptionObject.GetComponent<ComputerException>();
            spawnedException.SetImages(exception.animalSprite, exception.crimeSprite);
        }
    }

    public void PressLogin()
    {
        loginParent.SetActive(false);
        currentMinigameObject = Instantiate(minigamePrefabs[Random.Range(0, minigamePrefabs.Count)], computerMiniGameParent);
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
        computerExceptionsParent.gameObject.SetActive(false);
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
        computerExceptionsParent.gameObject.SetActive(true);
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
        computerExceptionsParent.gameObject.SetActive(false);
        confirmationParent.SetActive(false);
        loginParent.SetActive(false);
        
        screensaver.gameObject.SetActive(true);
        currentState = State.screensaver;
    }
}
