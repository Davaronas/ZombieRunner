using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpFirstAid : MonoBehaviour
{
    GameObject player;
    GameObject mainCamera;
    [SerializeField] string interactString = "";
    [SerializeField] float interactableRange = 1f;
    [SerializeField] GameObject interactTextObject;
    [SerializeField] float firstAidHealNumber = 50f;
    Text interactText;
    InteractText interactTextScript;
    PlayerHealth player_health;
    [SerializeField] AudioSource audioSource_PickUp;

    // Start is called before the first frame update
    void Awake()
    {
        player_health = FindObjectOfType<PlayerHealth>();
        player = player_health.gameObject;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        interactText = interactTextObject.GetComponent<Text>();
        interactTextScript = interactTextObject.GetComponent<InteractText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerHealth>().isPlayerAlive == true && player.GetComponent<PlayerHealth>().isGamePaused == false)
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
                        if (Input.GetButtonDown("Interact") && player_health.GetPlayerHealth() < 100f)
                        {
                            player_health.PlayerHeal(firstAidHealNumber);
                            audioSource_PickUp.Play();
                            interactTextObject.SetActive(false);
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }
}
