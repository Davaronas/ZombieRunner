using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryPickUp : MonoBehaviour
{
    [SerializeField] GameObject flashlight;
    Light playerFlashlight;
    Flashlight flashlightScript;
    GameObject mainCamera;
    GameObject player;
   
    [SerializeField] string interactString = "";
    [SerializeField] float interactableRange = 1f;
    [SerializeField] GameObject interactTextObject;
    Text interactText;
    InteractText interactTextScript;
    [SerializeField] AudioSource audioSource_PickUp;

    // Start is called before the first frame update
    void Awake()
    {

        playerFlashlight = flashlight.GetComponent<Light>();
        flashlightScript = flashlight.GetComponent<Flashlight>();
        player = FindObjectOfType<PlayerHealth>().gameObject;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        interactText = interactTextObject.GetComponent<Text>();
        interactTextScript = interactTextObject.GetComponent<InteractText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerHealth>().isPlayerAlive == true)
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
                            audioSource_PickUp.Play();
                            flashlightScript.PickUpBattery();
                            interactTextObject.SetActive(false);
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }
}
