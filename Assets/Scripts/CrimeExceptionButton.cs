using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CrimeExceptionButton : MonoBehaviour
{
    public Image crimeButtonImage;
    public CrimeData.Crime crime;
    public UnityEvent<CrimeData.Crime> onPress;

    public void OnButtonPressed()
    {
        onPress.Invoke(crime);
    }
}
