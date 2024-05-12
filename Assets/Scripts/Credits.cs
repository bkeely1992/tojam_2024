using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private List<string> audioCredits = new List<string>();

    [SerializeField]
    private TMPro.TMP_Text currentAudioCreditText;

    private int currentAudioCreditIndex = 0;

    private void Start()
    {
        //Show audio credit
        currentAudioCreditText.text = audioCredits[0];
    }

    public void ShowNextAudioCredit()
    {
        currentAudioCreditIndex++;
        if(currentAudioCreditIndex >= audioCredits.Count)
        {
            currentAudioCreditIndex = 0;
        }
        currentAudioCreditText.text = audioCredits[currentAudioCreditIndex];
    }

    public void ShowPreviousAudioCredit()
    {
        currentAudioCreditIndex--;
        if(currentAudioCreditIndex < 0)
        {
            currentAudioCreditIndex = audioCredits.Count - 1;
        }
        currentAudioCreditText.text = audioCredits[currentAudioCreditIndex];
    }
}
