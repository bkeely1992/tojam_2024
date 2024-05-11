using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> obstacles = new List<GameObject>();
    [SerializeField]
    private GameObject obstaclesParent;

    [SerializeField]
    private float spawnRate;
    private float lastSpawnTime;
    private List<Vector2> Spawns;

    [SerializeField]
    private GameObject characterPrefab;

    [SerializeField]
    private float duration;
    private float elapsedTime;

    [SerializeField]
    private GameObject screen;
    [SerializeField]
    private GameObject gameOverText;

    private Dictionary<Lane, float> laneCoords;

    private Lane currentLane = Lane.middle;

    private bool running = true;

    enum Lane
    {
        top, middle, bottom
    }

    enum Direction
    {
        up, down
    }

    void Awake()
    {
        var screentransform = (RectTransform)screen.transform;
        laneCoords = new Dictionary<Lane, float>()
        {
            { Lane.top, screentransform.position.y + screentransform.rect.height / 3 },
            { Lane.middle, screentransform.position.y },
            { Lane.bottom, screentransform.position.y - screentransform.rect.height / 3 }
        };
        Spawns = new List<Vector2>()
        {
            new Vector2(screentransform.position.x + screentransform.rect.width / 2, laneCoords[Lane.top]),
            new Vector2(screentransform.position.x + screentransform.rect.width / 2, laneCoords[Lane.middle]),
            new Vector2(screentransform.position.x + screentransform.rect.width / 2, laneCoords[Lane.bottom])
        };

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
                if (Input.GetKeyUp(KeyCode.UpArrow))
                    Move(Direction.up);
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                    Move(Direction.down);

                lastSpawnTime += Time.deltaTime;

                if (obstacles.Any() && lastSpawnTime > spawnRate)
                {
                    lastSpawnTime = 0;
                    SpawnOpbstacle();
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
        running = false;
        Destroy(characterPrefab);
        Destroy(obstaclesParent);

        if (won)
        {
            gameOverText.SetActive(true);
            //gameOverText.GetComponent<TextMeshPro>().text = "You Win!";
        }
        else
        {
            gameOverText.SetActive(true);
            //gameOverText.GetComponent<TextMeshPro>().text = "Game Over";
        }
    }

    private void Move(Direction direction)
    {
        switch (currentLane)
        {
            case Lane.top:
                if (direction == Direction.down)
                    currentLane = Lane.middle;
                break;
            case Lane.middle:
                if (direction == Direction.up)
                    currentLane = Lane.top;
                else
                    currentLane = Lane.bottom;
                break;
            case Lane.bottom:
                if (direction == Direction.up)
                    currentLane = Lane.middle;
                break;
        }

        SetPosition();
    }

    private void SetPosition()
    {
        var characterPosition = characterPrefab.transform.position;
        characterPrefab.transform.position = new Vector3(characterPosition.x, laneCoords[currentLane], 0);
    }

    private void SpawnOpbstacle()
    {
        var obstacle = obstacles[Random.Range(0, obstacles.Count)];
        var spawn = Spawns[Random.Range(0, Spawns.Count)];
        Instantiate(obstacle, (Vector3)spawn, Quaternion.identity, obstaclesParent.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
