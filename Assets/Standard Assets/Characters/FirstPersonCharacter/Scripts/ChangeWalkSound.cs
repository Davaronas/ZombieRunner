using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWalkSound : MonoBehaviour
{
    AudioSource runningAudio;
    [SerializeField] AudioClip walking_water;
    [SerializeField] AudioClip walking_normal;

    void Start()
    {
        runningAudio = GameObject.Find("Running").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            runningAudio.clip = walking_water;
            runningAudio.Play();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() != null)
        {
            runningAudio.clip = walking_normal;
            runningAudio.Play();
        }
    }
}
