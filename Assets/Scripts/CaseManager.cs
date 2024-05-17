using System;
using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static Assets.Scripts.Data.AnimalData;
using Random = UnityEngine.Random;

//Main script used for managing a ton of stuff (probably could stand for some refactoring because there's probably too much stuff in here)
public class CaseManager : MonoBehaviour
{
    //The cases that are currently queued for the current day
    public Queue<CaseData> queuedCases = new Queue<CaseData>();

    public int MaxStrikeCount = 10;
    //Properties used for dictating how many cases and exceptions there are per day
    public int baseNumberOfCases = 4;
    public int numberOfCasesDayIncrement = 1;

    public int baseNumberOfExceptions = 3;
    public int numberOfExceptionsIncrement = 1;

    private int currentNumberOfCases = 0;
    private int currentNumberOfExceptions = 0;

    //Script used for managing the visualization of the case in-game
    [SerializeField]
    private Case currentCase;

    [SerializeField]
    private TMPro.TMP_Text casesRemainingText;

    //Script used for managing the visualization of the animal in-game
    [SerializeField]
    private Animal currentAnimal;

    public AuthoredCaseDatabase AuthoredCaseDb;
    
    //The exceptions that are used to determine what animals are guilty or innocent
    [SerializeField]
    private List<CrimeExceptionData> rulebookExceptions = new List<CrimeExceptionData>();
    private List<CrimeExceptionData> dailyExceptions = new List<CrimeExceptionData>();

    //Text field used to show how many strikes the player has gotten
    [SerializeField]
    private TMPro.TMP_Text strikesText;

    private int strikes = 0;

    private enum ExceptionType
    {
        animal_class, animal_diet, animal_species, invalid
    }

    //Script used for managing the computer database which contains the daily exceptions
    [SerializeField]
    private ComputerDatabase computerDatabase;
    
    //Data map of all of the possible animal species
    [SerializeField]
    private List<AnimalData> allAnimalData = new List<AnimalData>();
    private Dictionary<AnimalData.Animal_species, AnimalData> animalDataMap = new Dictionary<Animal_species, AnimalData>();

    //Data map of all of the possible animal classes
    [SerializeField]
    private List<AnimalClassData> allAnimalClassData = new List<AnimalClassData>();
    private Dictionary<Animal_class, AnimalClassData> animalClassMap = new Dictionary<Animal_class, AnimalClassData>();

    //Data map of all of the possible animal diets
    [SerializeField]
    private List<AnimalDietData> allAnimalDietData = new List<AnimalDietData>();
    private Dictionary<Animal_diet, AnimalDietData> animalDietMap = new Dictionary<Animal_diet, AnimalDietData>();

    //Data map of all of the possible crimes
    [SerializeField]
    private List<CrimeData> allCrimeData = new List<CrimeData>();
    private Dictionary<CrimeData.Crime, CrimeData> crimeDataMap = new Dictionary<CrimeData.Crime, CrimeData>();

    [SerializeField]
    private GameObject decisionButtonsHolder;

    [SerializeField]
    private List<DialogueData> basePossibleGreetings;
    [SerializeField]
    private List<DialogueData> basePossibleCorrectGuiltyReactions;
    [SerializeField]
    private List<DialogueData> basePossibleIncorrectGuiltyReactions;
    [SerializeField]
    private List<DialogueData> basePossibleCorrectInnocentReactions;
    [SerializeField]
    private List<DialogueData> basePossibleIncorrectInnocentReactions;

    [SerializeField]
    private float chanceToShowReaction = 0.5f;

    [SerializeField]
    private Rulebook rulebook;

    [SerializeField]
    private List<Strike> strikeObjects;

    [SerializeField]
    private List<ExceptionType> currentValidExceptionTypes;
    [SerializeField]
    private List<CrimeData.Crime> currentValidCrimeTypes;
    [SerializeField]
    private List<Animal_species> currentValidAnimalTypes;

    [SerializeField]
    private ProgressionManager progressionManager;

    [SerializeField]
    private int maximumCasesPerDay;


    private Dictionary<int, Strike> strikeObjectMap = new Dictionary<int, Strike>();


    // Start is called before the first frame update
    void Start()
    {
        currentAnimal.onArrival.AddListener(StartNextCase);
        currentCase.onCaseIsVisible.AddListener(ShowDecisionButtons);
        currentNumberOfCases = baseNumberOfCases;
        currentNumberOfExceptions = baseNumberOfExceptions;

        rulebook.PopulateExceptions(rulebookExceptions);

        foreach(AnimalData animal in allAnimalData)
        {
            animal.AddToReactionPool(basePossibleGreetings, basePossibleCorrectGuiltyReactions, basePossibleIncorrectGuiltyReactions, basePossibleCorrectInnocentReactions, basePossibleIncorrectInnocentReactions);
            animalDataMap.Add(animal.Species, animal);
        }
        foreach(CrimeData crime in allCrimeData)
        {
            crimeDataMap.Add(crime.crimeValue, crime);
        }
        foreach(AnimalDietData animalDiet in allAnimalDietData)
        {
            animalDietMap.Add(animalDiet.animalDiet, animalDiet);
        }
        foreach(AnimalClassData animalClass in allAnimalClassData)
        {
            animalClassMap.Add(animalClass.animalClass, animalClass);
        }
        foreach(Strike strikeObject in strikeObjects)
        {
            strikeObjectMap.Add(strikeObject.Index, strikeObject);
        }
    }

    private void OnDestroy()
    {
        currentAnimal.onArrival.RemoveListener(StartNextCase);
        currentCase.onCaseIsVisible.RemoveListener(ShowDecisionButtons);
    }

    public void UpdateCasePossibilities(int dayIndex)
    {
        //Update the currentValidExceptionTypes based on the day
        if(!progressionManager.isDietActive && progressionManager.dayDietBecomesActive == dayIndex)
        {
            progressionManager.isDietActive = true;
            currentValidExceptionTypes.Add(ExceptionType.animal_diet);
            rulebook.ActivateDietTabObject();
        }

        //Update the currentValidCrimeTypes based on the day
        currentValidCrimeTypes.AddRange(progressionManager.AddCrimeProgressionData(dayIndex));

        //Update the currentValidAnimalTypes based on the day
        currentValidAnimalTypes.AddRange(progressionManager.AddAnimalProgression(dayIndex));

        //Update if the PC is on
        if(!progressionManager.isPCActive && progressionManager.dayPCBecomesActive == dayIndex)
        {
            progressionManager.isPCActive = true;
            computerDatabase.Activate();
        }

        //Update if confirmations are on
        if(!progressionManager.isConfirmationActive && progressionManager.dayConfirmationBecomesActive == dayIndex)
        {
            progressionManager.isConfirmationActive = true;
        }

        //Update the currentValidConfirmationButtonVariants
        computerDatabase.AddConfirmationButtons(progressionManager.AddConfirmationButtonProgression(dayIndex));

        List<CrimeExceptionData> newRulebookExceptions = progressionManager.AddRulebookProgressionData(dayIndex);
        rulebookExceptions.AddRange(newRulebookExceptions);
        rulebook.AddExceptions(newRulebookExceptions);
    }
    public void GenerateExceptionsForDay(int dayIndex)
    {
        if(!progressionManager.isPCActive)
        {
            return;
        }
        dailyExceptions = new List<CrimeExceptionData>();
        for(int i = 0; i < currentNumberOfExceptions; i++)
        {
            //Randomly choose one of the exception types
            ExceptionType currentExceptionType = currentValidExceptionTypes[Random.Range(0, currentValidExceptionTypes.Count)];

            //Randomly choose a crime
            CrimeData.Crime currentCrime = currentValidCrimeTypes[Random.Range(0, currentValidCrimeTypes.Count)];
            
            //Tie them together with an exception
            CrimeExceptionData currentException = new CrimeExceptionData();
            currentException.crime = currentCrime;
            currentException.crimeSprite = crimeDataMap[currentException.crime].sprite;
            currentException.crimeName = crimeDataMap[currentException.crime].crimeText;
            switch (currentExceptionType)
            {
                case ExceptionType.animal_species:
                    currentException.speciesException = AnimalData.AllAnimalSpecies[Random.Range(0, AnimalData.AllAnimalSpecies.Count)];
                    currentException.animalSprite = animalDataMap[currentException.speciesException].caseFileSprite;
                    break;
                case ExceptionType.animal_class:
                    currentException.classException = AnimalData.AllAnimalClasses[Random.Range(0, AnimalData.AllAnimalClasses.Count)];
                    currentException.animalSprite = animalClassMap[currentException.classException].sprite;
                    break;
                case ExceptionType.animal_diet:
                    currentException.dietException = AnimalData.AllAnimalDiets[Random.Range(0, AnimalData.AllAnimalDiets.Count)];
                    currentException.animalSprite = animalDietMap[currentException.dietException].sprite;
                    break;
            }
            //Double check that the exception doesn't already exist
            bool exceptionExists = false;
            foreach(CrimeExceptionData exception in dailyExceptions)
            {
                if(exception.crime == currentException.crime && exception.dietException == currentException.dietException &&
                    exception.speciesException == currentException.speciesException && exception.classException == currentException.classException)
                {
                    //Skip because this exception already exists
                    exceptionExists = true;
                    break;
                }
                
            }
            if(!exceptionExists)
            {
                dailyExceptions.Add(currentException);
            }
        }
        computerDatabase.SetExceptions(dailyExceptions);
        currentNumberOfExceptions += numberOfExceptionsIncrement;
    }

    public void GenerateCasesForDay(int dayIndex)
    {
        queuedCases = new Queue<CaseData>();

        remainingCasesForTheDay = currentNumberOfCases;
        addedAuthoredCasesToCurrentDay = new List<string>();
        for (int i = 0; i < currentNumberOfCases; i++)
        {
            var generatedCaseForTheDay = GenerateNewCaseData(dayIndex);
            queuedCases.Enqueue(generatedCaseForTheDay);
        }
        casesRemainingText.text = queuedCases.Count.ToString();
        currentNumberOfCases += numberOfCasesDayIncrement;
        if(currentNumberOfCases > maximumCasesPerDay)
        {
            currentNumberOfCases = maximumCasesPerDay;
        }
        currentCase.SetNextCase(queuedCases.Dequeue());
        currentAnimal.Generate(currentCase);
    }

    // AUTHORING SYSTEM START
    #region Authoring System
    private List<string> addedAuthoredCasesToCurrentDay;
    private int remainingCasesForTheDay = 0;
    private CaseData GenerateNewCaseData(int dayIndex)
    {
        CaseData pickedCase = null;
        // Evaluate if there will be any authored case has to be taken during the target day
        var validAuthoredCasesForTheDay = HowManyAuthoredCasesWeNeedToTake(dayIndex);
        // Filter only the non-added authored cases
        validAuthoredCasesForTheDay = validAuthoredCasesForTheDay
            .Where(x => !addedAuthoredCasesToCurrentDay.Contains(x.GimmeYourGuid())).ToList();
        if (remainingCasesForTheDay - validAuthoredCasesForTheDay.Count > 0)
        {
            // We have room for randomCases
            var shallWeGenerateRandom = Random.Range(0, 2); // if 0 => random, 1=>authored
            if (shallWeGenerateRandom == 0) // Generate random
            {
                pickedCase = GenerateRandomCase(dayIndex);
            }
            else // Non random picked by random
            {
                // If we have any authored in the list for today
                if (validAuthoredCasesForTheDay.Count > 0)
                {
                    pickedCase = GenerateAuthoredCase(dayIndex);
                }
                else // if there are no authored case left, just gen rnd
                {
                    pickedCase = GenerateRandomCase(dayIndex);
                }
            }
        }
        else
        {
            // Generate authored case because we have no room for random
            pickedCase = GenerateAuthoredCase(dayIndex);
        }
        // If so, see if we have any space for random cases
        // If there are slots for random cases hit them in, if not just get the authored ones\
        remainingCasesForTheDay--;
        
        return pickedCase;
    }

    private List<AuthoredCase> HowManyAuthoredCasesWeNeedToTake(int dayIndex)
    {
        if (AuthoredCaseDb.Cases == null || AuthoredCaseDb.Cases.Count == 0) return new List<AuthoredCase>();

        return AuthoredCaseDb.Cases.Where(x => (x.DayRanges.x <= dayIndex && x.DayRanges.y >= dayIndex) || x.NoDayRange).ToList();
    }
    
    private CaseData GenerateRandomCase(int dayIndex)
    {
        //Randomly choose an animal
        AnimalData currentAnimalData = animalDataMap[currentValidAnimalTypes[Random.Range(0, currentValidAnimalTypes.Count)]];
        currentAnimalData.SetName();
        //Randomly choose a crime
        CrimeData currentCrimeData = crimeDataMap[currentValidCrimeTypes[Random.Range(0, currentValidCrimeTypes.Count)]];
        currentCrimeData.currentCrimeDescription = currentCrimeData.possibleCrimeDescriptions[Random.Range(0, currentCrimeData.possibleCrimeDescriptions.Count)];
        return new CaseData(dayIndex, currentAnimalData, currentCrimeData);
    }

    private CaseData GenerateAuthoredCase(int dayIndex)
    {
        var validAuthoredCases = AuthoredCaseDb.Cases.Where(x => DoesTheAuthoredCaseValid(x,dayIndex)).ToList();
        var pickedCase = validAuthoredCases[Random.Range(0, validAuthoredCases.Count)];
        addedAuthoredCasesToCurrentDay.Add(pickedCase.GimmeYourGuid()); // add the picked case to the authored cases
        pickedCase.Animal.SetName();
        pickedCase.Crime.currentCrimeDescription = pickedCase.Crime.possibleCrimeDescriptions[Random.Range(0, pickedCase.Crime.possibleCrimeDescriptions.Count)];

        return new CaseData(dayIndex, pickedCase.Animal, pickedCase.Crime);
    }

    private bool DoesTheAuthoredCaseValid(AuthoredCase x, int dayIndex)
    {
        return ((x.DayRanges.x <= dayIndex && x.DayRanges.y >= dayIndex) || x.NoDayRange) &&
               !addedAuthoredCasesToCurrentDay.Contains(x.GimmeYourGuid());
    }
    #endregion
    // AUTHORING SYSTEM OVER

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SubmitGuilty()
    {
        SubmitPlayerDecision(true);
    }

    public void SubmitInnocent()
    {
        SubmitPlayerDecision(false);
    }

    private void SubmitPlayerDecision(bool submittedGuilty)
    {
        AudioManager.Instance.PlaySound("stamp");
        CrimeData.Crime currentCrime = currentCase.CurrentCaseData.CurrentCrime.crimeValue;
        AnimalData currentAnimalData = currentCase.CurrentCaseData.CurrentAnimal;
        bool isGuilty = true;
        //Check to see if the animal actually is guilty based on the exceptions that exist
        foreach(CrimeExceptionData crimeException in rulebookExceptions)
        {
            if(crimeException.crime == currentCrime)
            {
                //Check to see if any of the criteria are met for the crime exception
                if(crimeException.classException == currentAnimalData.Class ||
                    crimeException.speciesException == currentAnimalData.Species ||
                    crimeException.dietException == currentAnimalData.Diet)
                {
                    isGuilty = false;
                }
            }
        }
        foreach(CrimeExceptionData crimeException in dailyExceptions)
        {
            if (crimeException.crime == currentCrime)
            {
                //Check to see if any of the criteria are met for the crime exception
                if (crimeException.classException == currentAnimalData.Class ||
                    crimeException.speciesException == currentAnimalData.Species ||
                    crimeException.dietException == currentAnimalData.Diet)
                {
                    isGuilty = false;
                }
            }
        }
        if(isGuilty != submittedGuilty)
        {
            AudioManager.Instance.PlaySound("strike");
            //Player guessed incorrectly
            strikes++;
            strikeObjectMap[strikes].SetStrikeObjectVisibility(true);
            //strikesText.text = "Strikes: " + strikes.ToString();
        }

        if (Random.Range(0f, 1.0f) <= chanceToShowReaction)
        {
            
            if (submittedGuilty)
            {
                currentAnimal.ShowDialogue(currentAnimal.GetGuiltyReaction(isGuilty == submittedGuilty));
            }
            else
            {
                currentAnimal.ShowDialogue(currentAnimal.GetInnocentReaction(isGuilty == submittedGuilty));
            }
        }
        currentAnimal.guiltyChosen = submittedGuilty;

        currentCase.Complete();
        if (strikes >= MaxStrikeCount)
        {
            currentAnimal.Generate(null);
            WeAreInTheEndGameNow();
            
        }
        else if (!queuedCases.Any())
        {
            casesRemainingText.text = queuedCases.Count.ToString();
            // No case for the day, end it
            currentAnimal.Generate(null);
            EndDayByFinishingCases();

        }
        else
        {
            casesRemainingText.text = queuedCases.Count.ToString();
            // Next case
            currentCase.SetNextCase(queuedCases.Dequeue());
            currentAnimal.Generate(currentCase);
        }

        currentAnimal.Leave();
        HideDecisionButtons();
    }

    private void WeAreInTheEndGameNow()
    {
        AudioManager.Instance.PlaySound("game_over");
        FindObjectOfType<GameManager>().EndGame();
    }
    
    // End day if we are out of time or out of cases in hand
    private void EndDayByFinishingCases()
    {
        computerDatabase.ResetComputer();
        FindObjectOfType<GameManager>().EndDay();
        AudioManager.Instance.PlaySound("day_complete");
    }

    public void StartNextCase()
    {
        currentCase.ShowCase();
    }

    public void ShowDecisionButtons()
    {
        decisionButtonsHolder.SetActive(true);
    }
    public void HideDecisionButtons()
    {
        decisionButtonsHolder.SetActive(false);
    }
}
