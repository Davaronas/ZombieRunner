using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPickUp : MonoBehaviour
{
    WeaponSwitchManager weaponSwitchManager;
    GameObject player;
    GameObject mainCamera;
    [SerializeField] [Range(1, 9)] int whichWeapon = 0;
    [SerializeField] string interactString = "";
    [SerializeField] float interactableRange = 1f;
    [SerializeField] GameObject interactTextObject;
    Text interactText;
    InteractText interactTextScript;

    // Start is called before the first frame update
    void Awake()
    {
        weaponSwitchManager = FindObjectOfType<WeaponSwitchManager>();
        player = FindObjectOfType<PlayerHealth>().gameObject;
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
                        if (Input.GetButtonDown("Interact"))
                        {
                            interactTextObject.SetActive(false);
                            weaponSwitchManager.AmmoPickup(whichWeapon);
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }
}
