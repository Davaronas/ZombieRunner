using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject MainMenu_Options;
    [SerializeField] GameObject MainMenu_Options_Volume;
    [SerializeField] GameObject MainMenu_Options_Difficulty;
    LevelManager levelManager;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Play()
    {
        levelManager.LoadingLevel = PlayerPrefs.GetInt("UnlockedLevels", 1) + 1;
        SceneManager.LoadScene(1);
        
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Options()
    {
        MainMenu.SetActive(false);
        MainMenu_Options.SetActive(true);
    }

    public void Options_Back()
    {
        MainMenu.SetActive(true);
        MainMenu_Options.SetActive(false);
    }

    public void Options_Volume()
    {
        MainMenu_Options.SetActive(false);
        MainMenu_Options_Volume.SetActive(true);
    }

    public void Options_Volume_Back()
    {
        MainMenu_Options.SetActive(true);
        MainMenu_Options_Volume.SetActive(false);
    }

    public void Options_Difficulty()
    {
        MainMenu_Options.SetActive(false);
        MainMenu_Options_Difficulty.SetActive(true);
    }

    public void Options_Difficulty_Back()
    {
        MainMenu_Options.SetActive(true);
        MainMenu_Options_Difficulty.SetActive(false);
    }
}
