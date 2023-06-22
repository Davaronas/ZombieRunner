using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadUIHandler : MonoBehaviour
{
    LevelManager levelManager;

    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void PlayAgainButton() // restart map
    {
        Time.timeScale = 1f;
        levelManager.LoadingLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(1);
    }

    

    public void QuitButton()
    {
        Time.timeScale = 1f;
        levelManager.LoadingLevel = 0;
       SceneManager.LoadScene(1);
    }

   

    public void QuitGameButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

   


}
