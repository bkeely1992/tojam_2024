using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Screensaver : MonoBehaviour, IPointerEnterHandler
{
    public UnityEvent onMouseEnterScreen;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        onMouseEnterScreen.Invoke();
    }
}
