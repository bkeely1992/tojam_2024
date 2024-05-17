using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenuObject.activeSelf)
            {
                Time.timeScale = 1.0f;
                pauseMenuObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0.0f;
                pauseMenuObject.SetActive(true);
            }
            
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        pauseMenuObject.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
