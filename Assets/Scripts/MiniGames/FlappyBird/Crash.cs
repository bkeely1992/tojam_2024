using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    [SerializeField]
    private FlappyBirdManager birdManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        birdManager.GaneOver(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        birdManager.GaneOver(false);
    }
}
