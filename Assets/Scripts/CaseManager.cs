using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Assets.Scripts.Data.AnimalData;

public class CaseManager : MonoBehaviour
{
    public Queue<CaseData> queuedCases = new Queue<CaseData>();
    public int baseNumberOfCases = 4;
    public int numberOfCasesDayIncrement = 1;

    public int baseNumberOfExceptions = 3;
    public int numberOfExceptionsIncrement = 1;

    private int currentNumberOfCases = 0;
    private int currentNumberOfExceptions = 0;

    [SerializeField]
    private Case currentCase;

    [SerializeField]
    private Animal currentAnimal;

    [SerializeField]
    private List<CrimeExceptionData> rulebookExceptions = new List<CrimeExceptionData>();
    private List<CrimeExceptionData> dailyExceptions = new List<CrimeExceptionData>();

    [SerializeField]
    private TMPro.TMP_Text strikesText;

    private int strikes = 0;

    private enum ExceptionType
    {
        animal_class, animal_diet, animal_species, invalid
    }

    [SerializeField]
    private ComputerDatabase computerDatabase;

    [SerializeField]
    private List<AnimalData> allAnimalData = new List<AnimalData>();
    private Dictionary<AnimalData.Animal_species, AnimalData> animalDataMap = new Dictionary<Animal_species, AnimalData>();

    [SerializeField]
    private List<AnimalClassData> allAnimalClassData = new List<AnimalClassData>();
    private Dictionary<Animal_class, AnimalClassData> animalClassMap = new Dictionary<Animal_class, AnimalClassData>();

    [SerializeField]
    private List<AnimalDietData> allAnimalDietData = new List<AnimalDietData>();
    private Dictionary<Animal_diet, AnimalDietData> animalDietMap = new Dictionary<Animal_diet, AnimalDietData>();

    [SerializeField]
    private List<CrimeData> allCrimeData = new List<CrimeData>();
    private Dictionary<CrimeData.Crime, CrimeData> crimeDataMap = new Dictionary<CrimeData.Crime, CrimeData>();



    // Start is called before the first frame update
    void Start()
    {
        currentNumberOfCases = baseNumberOfCases;
        currentNumberOfExceptions = baseNumberOfExceptions;

        foreach(AnimalData animal in allAnimalData)
        {
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
    }

    public void GenerateExceptionsForDay(int dayIndex)
    {
        dailyExceptions = new List<CrimeExceptionData>();
        List<ExceptionType> currentValidExceptionTypes = new List<ExceptionType>() { ExceptionType.animal_class, ExceptionType.animal_diet, ExceptionType.animal_species };  
        for(int i = 0; i < currentNumberOfExceptions; i++)
        {
            //Randomly choose one of the exception types
            ExceptionType currentExceptionType = currentValidExceptionTypes[Random.Range(0, currentValidExceptionTypes.Count)];

            //Randomly choose a crime
            CrimeData.Crime currentCrime = CrimeData.AllCrimes[Random.Range(0, CrimeData.AllCrimes.Count)];
            
            //Tie them together with an exception
            CrimeExceptionData currentException = new CrimeExceptionData();
            currentException.crime = currentCrime;
            currentException.crimeSprite = crimeDataMap[currentException.crime].sprite;
            switch (currentExceptionType)
            {
                case ExceptionType.animal_species:
                    currentException.speciesException = AnimalData.AllAnimalSpecies[Random.Range(0, AnimalData.AllAnimalSpecies.Count)];
                    currentException.animalSprite = animalDataMap[currentException.speciesException].characterSprite;
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
                Debug.Log("Adding exception for crime["+currentException.crime.ToString()+"] for species["+currentException.speciesException.ToString()+"] class["+currentException.classException.ToString()+"] diet["+currentException.dietException.ToString()+"]");
                dailyExceptions.Add(currentException);
            }
        }
        computerDatabase.SetExceptions(dailyExceptions);
        currentNumberOfExceptions += numberOfExceptionsIncrement;
    }

    public void GenerateCasesForDay(int dayIndex)
    {
        queuedCases = new Queue<CaseData>();
        for (int i = 0; i < currentNumberOfCases; i++)
        {
            //Randomly choose an animal
            AnimalData currentAnimalData = animalDataMap[AnimalData.AllAnimalSpecies[Random.Range(0, AllAnimalSpecies.Count)]]; 

            //Randomly choose a crime
            CrimeData currentCrimeData = crimeDataMap[CrimeData.AllCrimes[Random.Range(0, CrimeData.AllCrimes.Count)]];
            queuedCases.Enqueue(new CaseData(dayIndex, currentAnimalData, currentCrimeData));
        }

        currentNumberOfCases += numberOfCasesDayIncrement;

        currentCase.Activate(queuedCases.Dequeue());
        currentAnimal.Generate(currentCase);
    }

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
            //Player guessed incorrectly
            strikes++;
            strikesText.text = "Strikes: " + strikes.ToString();
        }
        else
        {
            //Player guessed correctly
            
        }
        currentCase.Activate(queuedCases.Dequeue());
        currentAnimal.Generate(currentCase);
        currentAnimal.Leave();

    }
}
