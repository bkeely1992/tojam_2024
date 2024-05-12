using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour
{
    public int Index => index;
    [SerializeField]
    private int index;

    [SerializeField]
    private GameObject onObject;

    public void SetStrikeObjectVisibility(bool isOn)
    {
        onObject.SetActive(isOn);
    }
}
