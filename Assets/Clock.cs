using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private bool isActive = false;
    private float timeWaiting = 0.0f;
    private float timerTotal = 0.0f;
    [SerializeField]
    private Transform clockHandTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            timeWaiting += Time.deltaTime;
            clockHandTransform.eulerAngles = new Vector3(0, 0, 360-timeWaiting / timerTotal * 360);
            if(timeWaiting > timerTotal)
            {
                timeWaiting = 0.0f;
                isActive = false;
            }
        }
    }

    public void StartTimer(float inTime)
    {
        timerTotal = inTime;
        timeWaiting = 0f;
        isActive = true;
    }
    public void DisableClock()
    {
        isActive = false;
    }
}
