using Assets.Scripts.Data;
using Assets.Scripts.Data.ProgressionData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    private Dictionary<int, List<AnimalData.Animal_species>> animalProgressionDataMap = new Dictionary<int, List<AnimalData.Animal_species>>();
    private Dictionary<int, List<CrimeData.Crime>> crimeProgressionDataMap = new Dictionary<int, List<CrimeData.Crime>>();
    private Dictionary<int, List<GameObject>> confirmationButtonProgressionMap = new Dictionary<int, List<GameObject>>();
    private Dictionary<int, List<CrimeExceptionData>> rulebookProgressionDataMap = new Dictionary<int, List<CrimeExceptionData>>();
    private Dictionary<int, string> dayMessageProgressionDataMap = new Dictionary<int, string>();

    [SerializeField]
    private List<AnimalProgressionData> allAnimalProgressionData = new List<AnimalProgressionData>();

    [SerializeField]
    private List<CrimeProgressionData> allCrimeProgressionData = new List<CrimeProgressionData>();

    [SerializeField]
    private List<ConfirmationButtonVariantData> allConfirmationButtonData = new List<ConfirmationButtonVariantData>();

    [SerializeField]
    private List<RulebookProgressionData> allRulebookProgressionData = new List<RulebookProgressionData>();

    [SerializeField]
    private List<DayMessageProgressionData> allDayMessageProgressionData = new List<DayMessageProgressionData>();

    public bool isPCActive = false;
    public int dayPCBecomesActive;

    public bool isConfirmationActive = false;
    public int dayConfirmationBecomesActive;

    public bool isDietActive = false;
    public int dayDietBecomesActive;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the maps   
        foreach(AnimalProgressionData animalProgress in allAnimalProgressionData)
        {
            if(!animalProgressionDataMap.ContainsKey(animalProgress.dayToUnlock))
            {
                animalProgressionDataMap.Add(animalProgress.dayToUnlock, new List<AnimalData.Animal_species>());
            }
            animalProgressionDataMap[animalProgress.dayToUnlock].Add(animalProgress.animalToUnlock);
        }

        foreach(CrimeProgressionData crimeProgress in allCrimeProgressionData)
        {
            if(!crimeProgressionDataMap.ContainsKey(crimeProgress.dayToUnlock))
            {
                crimeProgressionDataMap.Add(crimeProgress.dayToUnlock, new List<CrimeData.Crime>());
            }
            crimeProgressionDataMap[crimeProgress.dayToUnlock].Add(crimeProgress.crimeToUnlock);
        }

        foreach(ConfirmationButtonVariantData confirmationButtonProgress in allConfirmationButtonData)
        {
            if(!confirmationButtonProgressionMap.ContainsKey(confirmationButtonProgress.dayUnlocked))
            {
                confirmationButtonProgressionMap.Add(confirmationButtonProgress.dayUnlocked, new List<GameObject>());
            }
            confirmationButtonProgressionMap[confirmationButtonProgress.dayUnlocked].Add(confirmationButtonProgress.confirmationButton);
        }

        foreach(RulebookProgressionData rulebookProgress in allRulebookProgressionData)
        {
            if(!rulebookProgressionDataMap.ContainsKey(rulebookProgress.dayUnlocked))
            {
                rulebookProgressionDataMap.Add(rulebookProgress.dayUnlocked, new List<CrimeExceptionData>());
            }
            rulebookProgressionDataMap[rulebookProgress.dayUnlocked].Add(rulebookProgress.exception);
        }

        foreach(DayMessageProgressionData dayMessageProgress in allDayMessageProgressionData)
        {
            if(!dayMessageProgressionDataMap.ContainsKey(dayMessageProgress.day))
            {
                dayMessageProgressionDataMap.Add(dayMessageProgress.day, dayMessageProgress.message);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Create the dictionary accessors
    public List<AnimalData.Animal_species> AddAnimalProgression(int dayIndex)
    {
        List<AnimalData.Animal_species> animalProgress = new List<AnimalData.Animal_species>();
        if(animalProgressionDataMap.ContainsKey(dayIndex))
        {
            animalProgress = animalProgressionDataMap[dayIndex];
        }
        return animalProgress;
    }

    public List<CrimeData.Crime> AddCrimeProgressionData(int dayIndex)
    {
        List<CrimeData.Crime> crimeProgress = new List<CrimeData.Crime>();
        if(crimeProgressionDataMap.ContainsKey(dayIndex))
        {
            crimeProgress = crimeProgressionDataMap[dayIndex];
        }
        return crimeProgress;
    }

    public List<GameObject> AddConfirmationButtonProgression(int dayIndex)
    {
        List<GameObject> confirmationButtonProgress = new List<GameObject>();
        if(confirmationButtonProgressionMap.ContainsKey(dayIndex))
        {
            confirmationButtonProgress = confirmationButtonProgressionMap[dayIndex];
        }
        return confirmationButtonProgress;
    }

    public List<CrimeExceptionData> AddRulebookProgressionData(int dayIndex)
    {
        List<CrimeExceptionData> rulebookProgress = new List<CrimeExceptionData>();
        if(rulebookProgressionDataMap.ContainsKey(dayIndex))
        {
            rulebookProgress = rulebookProgressionDataMap[dayIndex];
        }
        return rulebookProgress;
    }

    public string GetDayMessage(int dayIndex)
    {
        if(dayMessageProgressionDataMap.ContainsKey(dayIndex))
        {
            return dayMessageProgressionDataMap[dayIndex];
        }
        return "";
    }
}
