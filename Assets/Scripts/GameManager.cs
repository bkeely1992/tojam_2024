using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script used for managing the high level game execution in terms of days
public class GameManager : MonoBehaviour
{
    public int dayIndex = 0;
    public float timeInDay = 60;

    [SerializeField]
    private List<GameObject> objectsToHideOnDayStart = new List<GameObject>();

    [SerializeField]
    private List<GameObject> objectsToShowOnDayStart = new List<GameObject>();

    private float timeRemaining = 0f;

    private enum GameState
    {
        wait, day, invalid
    }
    private GameState currentState = GameState.wait;

    [SerializeField]
    private CaseManager caseManager;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartDay", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDay()
    {
        timeRemaining = timeInDay;
        
        caseManager.GenerateExceptionsForDay(dayIndex);
        caseManager.GenerateCasesForDay(dayIndex);
    }
}
