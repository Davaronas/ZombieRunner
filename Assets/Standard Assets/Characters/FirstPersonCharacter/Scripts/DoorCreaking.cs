using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCreaking : MonoBehaviour
{
    AudioSource audioSource;
    Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
       
    }

    void PlayCreaking()
    {
        audioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
       
    }

  
}
