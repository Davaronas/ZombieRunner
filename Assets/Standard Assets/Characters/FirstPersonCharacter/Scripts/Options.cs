using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    int difficulty;
    [SerializeField] Text currentDifficultyText;
    [SerializeField] Slider volumeSlider;

    void Awake()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 2);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        switch (difficulty)
        {
            case 1:
                currentDifficultyText.text = "Current difficulty: Easy";
                break;
            case 2:
                currentDifficultyText.text = "Current difficulty: Normal";
                break;
            case 3:
                currentDifficultyText.text = "Current difficulty: Hard";
                break;
            case 4:
                currentDifficultyText.text = "Current difficulty: Masochist";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAudioListenerVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    public void ChangeDifficulty_Easy()
    {
        difficulty = 1;
        PlayerPrefs.SetInt("Difficulty", difficulty);
        currentDifficultyText.text = "Current difficulty: Easy";
    }

    public void ChangeDifficulty_Normal()
    {
        difficulty = 2;
        PlayerPrefs.SetInt("Difficulty", difficulty);
        currentDifficultyText.text = "Current difficulty: Normal";
    }

    public void ChangeDifficulty_Hard()
    {
        difficulty = 3;
        PlayerPrefs.SetInt("Difficulty", difficulty);
        currentDifficultyText.text = "Current difficulty: Hard";
    }

    public void ChangeDifficulty_Masochist()
    {
        difficulty = 4;
        PlayerPrefs.SetInt("Difficulty", difficulty);
        currentDifficultyText.text = "Current difficulty: Masochist";
    }
}
