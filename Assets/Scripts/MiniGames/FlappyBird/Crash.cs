using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    [SerializeField]
    private FlappyBirdManager birdManager;
    [SerializeField]
    private LayerMask colliderMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((colliderMask & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(collision.gameObject);
            birdManager.GaneOver(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        birdManager.GaneOver(false);
    }
}
