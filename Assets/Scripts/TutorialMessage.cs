using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialMessage : MonoBehaviour
{
    public UnityEvent onConfirm;
    public void OnConfirmPressed()
    {
        onConfirm.Invoke();
    }
}
