using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//The current animal in the scene.
public class Animal : MonoBehaviour
{
    //State machine logic used for managing the behaviour of the animal
    public enum State
    {
        off_screen, walk_in, walk_out, react, guilty, idle, invalid
    }
    private State currentState = State.off_screen;
    private Case currentCase = null;

    //Transforms used for determining where the animal navigates in the scene
    [SerializeField]
    private Transform startPosition, idlePosition, exitPosition;

    [SerializeField]
    private Image bodyImage;

    //Variables used for specifying the properties of the motion of the animal
    public float speed;
    public float arrivalDistance;

    public UnityEvent onArrival;

    [SerializeField]
    private GameObject dialogueContainerObject;

    [SerializeField]
    private TMPro.TMP_Text dialogueText;

    [SerializeField]
    private float waitingTime = 0.0f;

    [SerializeField]
    private float chanceToShowGreeting = 0.5f;

    [SerializeField]
    private float chanceToShowReaction = 0.5f;

    private float timeRemainingToShowDialogue = 0f;
    private float timeRemainingToWait = 0.0f;
    public bool guiltyChosen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.off_screen:
                if(currentCase != null)
                {
                    //Set the sprite
                    bodyImage.sprite = currentCase.CurrentCaseData.CurrentAnimal.characterSprite;

                    //Set the starting position
                    transform.position = startPosition.position;

                    //Set the state to walk_in
                    currentState = State.walk_in;
                }
                break;
            case State.walk_in:
                //Move them towards the idle position
                transform.position += (idlePosition.position - transform.position).normalized * speed * Time.deltaTime;
                //if close enough to idle position then
                if(Vector3.Distance(transform.position, idlePosition.position) < arrivalDistance)
                {
                    if(Random.Range(0f, 1.0f) <= chanceToShowGreeting && currentCase.CurrentCaseData.CurrentAnimal.PossibleGreetings.Count>0)
                    {
                        ShowDialogue(GetGreeting());
                    }
                    
                    //Set the state to idle
                    currentState = State.idle;
                    onArrival.Invoke();
                }

                break;
            case State.walk_out:
                //Move them towards the exit position
                transform.position += (exitPosition.position - transform.position).normalized * speed * Time.deltaTime;
                //if close enough to exit position then
                if(Vector3.Distance(transform.position, exitPosition.position) < arrivalDistance)
                {
                    //Set the state to off_screen
                    currentState = State.off_screen;
                }

                break;
            case State.react:
                timeRemainingToWait -= Time.deltaTime;
                if(timeRemainingToWait <= 0)
                {
                    if (guiltyChosen)
                    {
                        currentState = State.guilty;
                    }
                    else
                    {
                        currentState = State.walk_out;
                    }
                }
                break;
            case State.guilty:
                //Move them towards the exit position
                transform.position += (startPosition.position - transform.position).normalized * speed * Time.deltaTime;
                //if close enough to exit position then
                if (Vector3.Distance(transform.position, startPosition.position) < arrivalDistance)
                {
                    timeRemainingToShowDialogue = 0f;
                    transform.position = startPosition.position;
                    //Set the state to off_screen
                    currentState = State.off_screen;
                }

                break;
            case State.idle:
                //Wait until the player hits submit or the time runs out
                break;
        }

        if(timeRemainingToShowDialogue >= 0)
        {
            timeRemainingToShowDialogue -= Time.deltaTime;
            if(timeRemainingToShowDialogue < 0)
            {
                dialogueContainerObject.SetActive(false);
            }
        }
    }

    public void Generate(Case inCase)
    {
        currentCase = inCase;
    }

    public void Leave()
    {
        if(currentState == State.idle)
        {
            currentState = State.react;
            timeRemainingToWait = waitingTime;
        }
    }

    public void ShowDialogue(DialogueData inDialogue)
    {
        if (inDialogue == null) return; // Early return if there are no valid dialogue
        dialogueText.text = inDialogue.textValue;
        dialogueContainerObject.SetActive(true);
        timeRemainingToShowDialogue = inDialogue.duration;
    }

    public DialogueData GetGreeting()
    {
        List<DialogueData> possibleGreetings = currentCase.CurrentCaseData.CurrentAnimal.PossibleGreetings;
        return possibleGreetings[Random.Range(0, possibleGreetings.Count)];
    }
    public DialogueData GetGuiltyReaction(bool wasCorrect)
    {
        List<DialogueData> possibleResponses;
        if(wasCorrect)
        {
            possibleResponses = currentCase.CurrentCaseData.CurrentAnimal.PossibleCorrectGuiltyReactions;
        }
        else
        {
            possibleResponses = currentCase.CurrentCaseData.CurrentAnimal.PossibleIncorrectGuiltyReactions;
        }

        if (possibleResponses.Count == 0) return null;
        return possibleResponses[Random.Range(0, possibleResponses.Count)];
    }
    public DialogueData GetInnocentReaction(bool wasCorrect)
    {
        List<DialogueData> possibleResponses;
        if (wasCorrect)
        {
            possibleResponses = currentCase.CurrentCaseData.CurrentAnimal.PossibleCorrectInnocentReactions;
        }
        else
        {
            possibleResponses = currentCase.CurrentCaseData.CurrentAnimal.PossibleIncorrectInnocentReactions;
        }
        if(possibleResponses.Count==0) return null; // null is handled in show dialogue function
        return possibleResponses[Random.Range(0, possibleResponses.Count)];
    }

    

}
