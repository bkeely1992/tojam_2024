using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Avoider avoider;

    [SerializeField]
    private LayerMask colliderMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((colliderMask & (1 << collision.gameObject.layer)) != 0)
        {
            Destroy(collision.gameObject);
            avoider.GaneOver(false);
        }
            
    }
}
