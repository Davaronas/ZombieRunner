using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    GameObject player;
    GameObject mainCamera;
    [SerializeField] public bool locked = false;
    [SerializeField] string interactString = "";
    [SerializeField] float interactableRange = 1f;
    [SerializeField] GameObject interactTextObject;
    Text interactText;
    InteractText interactTextScript;
    PlayerHealth player_health;
    AudioSource audioSource;
    Animator animator;
    bool alreadyOpened = false;
    [SerializeField] AudioClip lockedSound;

    // Start is called before the first frame update
    void Awake()
    {
        player_health = FindObjectOfType<PlayerHealth>();
        player = player_health.gameObject;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        interactText = interactTextObject.GetComponent<Text>();
        interactTextScript = interactTextObject.GetComponent<InteractText>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerHealth>().isPlayerAlive == true && alreadyOpened == false && player.GetComponent<PlayerHealth>().isGamePaused == false)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= interactableRange)
            {
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactableRange))
                {
                    if (hit.transform == transform)
                    {
                        interactTextObject.SetActive(true);
                        interactText.text = interactString;
                        interactTextScript.timer = 0;
                        if (Input.GetButtonDown("Interact"))
                        {
                            if (locked == false)
                            {
                                animator.SetTrigger("Open");
                                interactTextObject.SetActive(false);
                                alreadyOpened = true;
                            }
                            else if (locked == true)
                            { audioSource.PlayOneShot(lockedSound); }
                        }
                    }
                }
            }
        }
    }

    public void DoorSound()
    {
        audioSource.Play();
    }
}
