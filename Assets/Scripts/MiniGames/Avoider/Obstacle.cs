using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
    }
}
