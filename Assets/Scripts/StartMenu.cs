using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuContainerObject;

    [SerializeField]
    private GameObject creditsContainerObject;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowCredits()
    {
        menuContainerObject.SetActive(false); 
        creditsContainerObject.SetActive(true);
    }

    public void ShowMenu()
    {
        menuContainerObject.SetActive(true);
        creditsContainerObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
