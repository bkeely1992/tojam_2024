using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private List<TutorialMessage> tutorialMessages = new List<TutorialMessage>();
    private int currentTutorialMessageIndex = 0;
    public bool HasStarted => hasStarted;
    private bool hasStarted = false;

    public void Initialize()
    {
        currentTutorialMessageIndex = 0;
        foreach (TutorialMessage message in tutorialMessages)
        {
            message.onConfirm.AddListener(ContinueTutorial);
        }
        tutorialMessages[currentTutorialMessageIndex].gameObject.SetActive(true);
        hasStarted = true;
    }

    private void ContinueTutorial()
    {
        tutorialMessages[currentTutorialMessageIndex].gameObject.SetActive(false);
        currentTutorialMessageIndex++;
       
        if(tutorialMessages.Count <= currentTutorialMessageIndex)
        {
            foreach (TutorialMessage message in tutorialMessages)
            {
                message.onConfirm.RemoveListener(ContinueTutorial);
            }
            hasStarted = false;
        }
        else
        {
            tutorialMessages[currentTutorialMessageIndex].gameObject.SetActive(true);
        }
    }
}
