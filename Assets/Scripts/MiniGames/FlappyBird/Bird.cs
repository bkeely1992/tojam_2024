using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField]
    private FlappyBirdManager flappyBirdManager;

    [SerializeField]
    private LayerMask colliderMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((colliderMask & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(collision.gameObject);
            flappyBirdManager.GaneOver(false);
        }

    }
}
