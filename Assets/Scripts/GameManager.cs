using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//Script used for managing the high level game execution in terms of days
public class GameManager : MonoBehaviour
{
    public int dayIndex = 0;
    public int totalLoginCount = 0;
    public float timeInDay = 60;

    public UnityEvent OnDayIsOver;
    public UnityEvent OnGameOver;
    
    [SerializeField]
    private List<GameObject> objectsToHideOnDayStart = new List<GameObject>();

    [SerializeField]
    private List<GameObject> objectsToShowOnDayStart = new List<GameObject>();

    private float timeRemaining = 0f;

    private enum GameState
    {
        WAITING_FOR_NEW_DAY,
        DAY_IS_RUNNING
    }
    private GameState currentState = GameState.WAITING_FOR_NEW_DAY;

    [SerializeField]
    private CaseManager caseManager;

    [SerializeField]
    private ComputerDatabase computer;

    [SerializeField]
    private Clock clock;

    [SerializeField]
    private TutorialManager tutorialManager;

    [SerializeField]
    private GameObject tutorialPromptObject;

    [SerializeField]
    private ProgressionManager progressionManager;

    [SerializeField]
    private GameObject gameOverParent;

    [SerializeField]
    private GameObject nextDayParent;
    [SerializeField]
    private GameObject nextDayMessageParent;
    [SerializeField]
    private TMPro.TMP_Text nextDayMessageText;

    private bool hasWarnedOnTime = false;
    private bool tutorialIsOn = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorialIsOn)
        {
            if(!tutorialManager.HasStarted)
            {
                tutorialIsOn = false;
                AudioManager.Instance.PlaySound("music");
                Invoke("StartDay", 1f);
            }
        }

        if (currentState == GameState.DAY_IS_RUNNING)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(timeRemaining, 0f);
            
            if (timeRemaining <= 0f)
            {
                EndGame();
            }
            else if(!hasWarnedOnTime && timeRemaining <= 10f)
            {
                hasWarnedOnTime = true;
                AudioManager.Instance.PlaySound("time_expiring");
            }
        }
    }

    public void SkipTutorial()
    {
        tutorialPromptObject.SetActive(false);
        AudioManager.Instance.PlaySound("music");
        Invoke("StartDay", 1f);
    }

    public void StartTutorial()
    {
        tutorialPromptObject.SetActive(false);
        tutorialIsOn = true;
        tutorialManager.Initialize();
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public int IncreaseLoginCountAndReturnTheResult() => ++totalLoginCount;

    public void EndGame()
    {
        clock.DisableClock();
        OnGameOver?.Invoke();
    }
    
    public void StartDay()
    {
        hasWarnedOnTime = false;
        clock.StartTimer(timeInDay);
        currentState = GameState.DAY_IS_RUNNING;
        timeRemaining = timeInDay;

        caseManager.UpdateCasePossibilities(dayIndex);
        caseManager.GenerateExceptionsForDay(dayIndex);
        caseManager.GenerateCasesForDay(dayIndex);
    }

    public void EndDay()
    {
        clock.DisableClock();
        currentState = GameState.WAITING_FOR_NEW_DAY;
        OnDayIsOver?.Invoke();
        dayIndex++; // Increase the day.

    }

    public void ShowNextDayPage()
    {
        nextDayParent.SetActive(true);
        string dayMessage = progressionManager.GetDayMessage(dayIndex);
        nextDayMessageParent.SetActive(dayMessage != "");
        nextDayMessageText.text = dayMessage;
    }

    public void ShowGameOverPage()
    {
        gameOverParent.SetActive(true);
    }
}
