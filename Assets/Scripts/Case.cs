using Assets.Scripts.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


//The current Case in the scene
public class Case : MonoBehaviour
{
    //The data for the current case used for driving the visuals and gameplay
    public CaseData CurrentCaseData => currentCaseData;
    private CaseData currentCaseData;
    private AnimalData animalData;

    //Visualization of the animal on the case
    [SerializeField]
    private Image animalImage;

    //Text field used for showing the animal's name
    [SerializeField]
    private TMPro.TMP_Text animalNameText;

    //Visualization of the crime that the animal is accused of committing
    [SerializeField]
    private Image crimeImage;

    //Text field used for showing the name of the crime
    [SerializeField]
    private TMPro.TMP_Text crimeNameText;

    [SerializeField]
    private Transform visiblePosition, inactivePosition;

    [SerializeField]
    private Image speciesIconImage, dietIconImage, classIconImage;

    public float speed;
    public float arrivalDistance;

    public UnityEvent onCaseIsVisible;

    private Vector3 targetPosition;

    private enum State
    {
        visible, sliding_up, sliding_down, inactive, invalid
    }
    [SerializeField]
    private State currentState = State.inactive;


    private void Update()
    {
        float newDistance;
        float previousDistance;
        switch(currentState)
        {
            case State.visible:
                break;
            case State.sliding_up:
                previousDistance = Vector3.Distance(targetPosition, transform.position);
                transform.position += (targetPosition - transform.position).normalized * speed *Time.deltaTime;
                newDistance = Vector3.Distance(targetPosition, transform.position);
                if (newDistance < arrivalDistance || newDistance > previousDistance)
                {
                    currentState = State.visible;
                    onCaseIsVisible.Invoke();
                }
                break;
            case State.sliding_down:
                previousDistance = Vector3.Distance(targetPosition, transform.position);
                transform.position += (targetPosition - transform.position).normalized * speed * Time.deltaTime;
                newDistance = Vector3.Distance(targetPosition, transform.position);
                if (newDistance < arrivalDistance || newDistance > previousDistance)
                {
                    currentState = State.inactive;
                }
                break;
            case State.inactive:
                break;
        }

    }

    //Activates the case and shows it
    public void SetNextCase(CaseData inCaseData)
    {
        currentCaseData = inCaseData;
        animalData = currentCaseData.CurrentAnimal;
    }

    public void ShowCase()
    {
        //Play a papers shuffling sound
        AudioManager.Instance.PlaySound("paper_slide");
        //Set the images on the case
        animalImage.sprite = animalData.characterSprite;
        animalNameText.text = animalData.CurrentName;
        crimeImage.sprite = currentCaseData.CurrentCrime.sprite;
        crimeNameText.text = currentCaseData.CurrentCrime.currentCrimeDescription;
        dietIconImage.sprite = currentCaseData.CurrentAnimal.dietSprite;
        classIconImage.sprite = currentCaseData.CurrentAnimal.classSprite;
        currentState = State.sliding_up;

        targetPosition = visiblePosition.position;
    }

    public void Complete()
    {
        AudioManager.Instance.PlaySound("paper_slide2");
        targetPosition = inactivePosition.position;
        currentState = State.sliding_down;
        
        currentCaseData = null;
        animalData = null;
    }
}
