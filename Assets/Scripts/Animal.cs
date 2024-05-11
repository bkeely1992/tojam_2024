using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public enum State
    {
        off_screen, walk_in, walk_out, idle, invalid
    }
    private State currentState = State.off_screen;
    private Case currentCase = null;

    [SerializeField]
    private Transform startPosition, idlePosition, exitPosition;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public float speed;
    public float arrivalDistance;

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
                    spriteRenderer.sprite = currentCase.CurrentCaseData.CurrentAnimal.characterSprite;

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
                    //Set the state to idle
                    currentState = State.idle;
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
            case State.idle:
                //Wait until the player hits submit or the time runs out
                break;
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
            currentState = State.walk_out;
        }
    }
}
