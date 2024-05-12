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
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartDay", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == GameState.DAY_IS_RUNNING)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(timeRemaining, 0f);
            if (timeRemaining <= 0f)
            {
                EndGame();
            }
        }
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void EndGame()
    {
        OnGameOver?.Invoke();
    }
    
    public void StartDay()
    {
        currentState = GameState.DAY_IS_RUNNING;
        timeRemaining = timeInDay;
        
        caseManager.GenerateExceptionsForDay(dayIndex);
        caseManager.GenerateCasesForDay(dayIndex);
    }

    public void EndDay()
    {
        currentState = GameState.WAITING_FOR_NEW_DAY;
        OnDayIsOver?.Invoke();
        dayIndex++; // Increase the day.
    }
}
