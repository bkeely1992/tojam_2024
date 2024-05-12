using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class ScreensaverBall : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D rb;

    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void OnEnable()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = (new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0)).normalized * speed;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 normal = ((Vector2)transform.position - collision.ClosestPoint(transform.position)).normalized;
        Vector2 reflectedVector = (Vector2)rb.velocity - 2 * (Vector2.Dot((Vector2)rb.velocity, normal)) * normal;
        rb.velocity = (reflectedVector).normalized * speed;
    }
}
