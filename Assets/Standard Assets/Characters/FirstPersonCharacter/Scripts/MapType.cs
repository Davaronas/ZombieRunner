using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MapType : MonoBehaviour
{
    public bool dark = false;
    [Range(0,50)]public int Level_ID;
    LevelManager levelManager;
    AudioSource audioSource;
    public bool thunder = false;
   [SerializeField] int minTime = 20;
   [SerializeField] int maxTime = 120;
    float timer;
    int nextThunder;
    [SerializeField] AudioClip thunderSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = 0f;
        levelManager = FindObjectOfType<LevelManager>();
        nextThunder = Random.Range(minTime, maxTime + 1);
    }

    private void Update()
    {
        Thunder();

    }

    private void Thunder()
    {
        if (thunder)
        {
            timer = Mathf.Clamp(timer + Time.deltaTime, 0, 600);
            if (timer >= nextThunder)
            {
                UpdateThunder();
            }
        }
    }

    private void UpdateThunder()
    {
        audioSource.PlayOneShot(thunderSound);
        timer = 0f;
        nextThunder = Random.Range(minTime, maxTime + 1);
    }

    public void LoadNextMap()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        /* if(PlayerPrefs.GetInt("UnlockedLevels",1f) < Level_ID + 1)
         {
             PlayerPrefs.SetInt("UnlockedLevels", Level_ID + 1);
         }
         
        levelManager.LoadingLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(1);
        */
        SceneManager.LoadScene(0);
    }
}
