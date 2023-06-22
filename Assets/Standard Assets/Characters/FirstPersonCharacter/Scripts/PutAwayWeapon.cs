using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutAwayWeapon : MonoBehaviour
{

    WeaponSwitchManager weaponSwitchManager;
    // Start is called before the first frame update
    void Start()
    {
        weaponSwitchManager = FindObjectOfType<WeaponSwitchManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        weaponSwitchManager.DeequipWeapon();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            weaponSwitchManager.ReequipWeapon();
    }
}
