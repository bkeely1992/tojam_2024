using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FlappyBirdManager : Minigame
{
    [SerializeField]
    private Rigidbody2D bird;

    [SerializeField]
    private float flapForce;

    [SerializeField]
    private List<GameObject> pipes = new List<GameObject>();
    // used to control how often the pipes spawn
    [SerializeField]
    private float spawnRate;
    private float lastSpawnTime;
    [SerializeField]
    private GameObject pipeParent;

    // properties to control the mini game length
    [SerializeField]
    private float duration;
    private float elapsedTime;
    private bool running = true;

    // The computer screen object.
    [SerializeField]
    private GameObject screen;

    // The text object that will be displayed when the game is over
    [SerializeField]
    private GameObject gameOverText;

    private void Awake()
    {
        gameOverText.transform.position = screen.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (running)
        {
            if (elapsedTime < duration)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetMouseButtonDown(0))
                {
                    AudioManager.Instance.PlaySound("boost", true);
                    Flap();
                }
                    

                lastSpawnTime += Time.deltaTime;

                if (pipes.Any() && lastSpawnTime > spawnRate)
                {
                    lastSpawnTime = 0;
                    SpawnPipe();
                }
            }
            else
            {
                GaneOver(true);
            }
        }
    }

    public void GaneOver(bool won)
    {
        if (!running) return;
        running = false;

        if (won)
        {
            
            gameOverText.SetActive(true);
            gameOverText.GetComponent<TMP_Text>().text = "You Win!";
            onWin.Invoke();
        }
        else
        {
            
            gameOverText.SetActive(true);
            gameOverText.GetComponent<TMP_Text>().text = "Game Over";
            onLose.Invoke();
        }
        
    }

    private void Flap()
    {
        bird.AddForce(new Vector2(0, flapForce), ForceMode2D.Impulse);
    }

    private void SpawnPipe()
    {
        var screentransform = (RectTransform)screen.transform;
        var obstacle = pipes[Random.Range(0, pipes.Count)];
        Instantiate(obstacle, pipeParent.transform.position, Quaternion.identity, pipeParent.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
