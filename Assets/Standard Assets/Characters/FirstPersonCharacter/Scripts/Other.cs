using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other : MonoBehaviour
{

    PlayerHealth playerHealth;
    AudioSource audioSource;
    [SerializeField] float heartbeatStartsAt = 25f;
    [SerializeField] AudioClip heartbeatSound;
    [SerializeField] float speedingRateOfHeartbeatSound = 0.02f;
    [SerializeField] float blood1AppearHealth = 80f;
    [SerializeField] float blood2AppearHealth = 70f;
    [SerializeField] float blood3AppearHealth = 50f;
    [SerializeField] float blood4AppearHealth = 40f;
    [SerializeField] float blood5AppearHealth = 30f;
    [SerializeField] float blood6AppearHealth = 20f;
    [SerializeField] GameObject blood1;
    [SerializeField] GameObject blood2;
    [SerializeField] GameObject blood3;
    [SerializeField] GameObject blood4;
    [SerializeField] GameObject blood5;
    [SerializeField] GameObject blood6;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetPlayerHealth() <= heartbeatStartsAt && audioSource.isPlaying == false && playerHealth.isPlayerAlive)
        {
            audioSource.pitch = 1 + ((heartbeatStartsAt - playerHealth.GetPlayerHealth()) * speedingRateOfHeartbeatSound);
            audioSource.Play();
        }

        if(playerHealth.GetPlayerHealth() <= blood1AppearHealth)
        {
            blood1.SetActive(true);
        }
        else
        {
            blood1.SetActive(false);
        }

        if (playerHealth.GetPlayerHealth() <= blood2AppearHealth)
        {
            blood2.SetActive(true);
        }
        else
        {
            blood2.SetActive(false);
        }

        if (playerHealth.GetPlayerHealth() <= blood3AppearHealth)
        {
            blood3.SetActive(true);
        }
        else
        {
            blood3.SetActive(false);
        }

        if (playerHealth.GetPlayerHealth() <= blood4AppearHealth)
        {
            blood4.SetActive(true);
        }
        else
        {
            blood4.SetActive(false);
        }

        if (playerHealth.GetPlayerHealth() <= blood5AppearHealth)
        {
            blood5.SetActive(true);
        }
        else
        {
            blood5.SetActive(false);
        }

        if (playerHealth.GetPlayerHealth() <= blood6AppearHealth)
        {
            blood6.SetActive(true);
        }
        else
        {
            blood6.SetActive(false);
        }
        
    }
}
