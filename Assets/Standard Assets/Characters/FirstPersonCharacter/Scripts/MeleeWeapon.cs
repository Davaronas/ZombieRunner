using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MeleeWeapon : MonoBehaviour
{

    Animator animator;
    AudioSource audioSource;
    bool isDealingDamage = false;
    bool readyToAttack = true;
    Quaternion startingRotation;
    PlayerHealth playerHealth;
    RigidbodyFirstPersonController firstPersonController;
    bool addedRigidbody = false;
    bool dealtDamage = false;

    [SerializeField] float meleeWeaponDamage = 100f;
    [SerializeField] float hitForce = 700f;
    [SerializeField] float timeBetweeenAttacks = 0.3f;
    [SerializeField] float smoothTime = 20f;
    [SerializeField] AudioClip hitSound;
    [SerializeField] GameObject reloadTimer;
    [SerializeField] GameObject bloodVFX;
    


    void Awake()
    {
        animator = GetComponent<Animator>();
        firstPersonController = FindObjectOfType<RigidbodyFirstPersonController>();
        audioSource = GetComponent<AudioSource>();
        startingRotation = transform.rotation;
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && isDealingDamage == false && readyToAttack == true && playerHealth.isPlayerAlive)
        {
            animator.SetBool("Cut",true);
            audioSource.Play();
            readyToAttack = false;
        }
        if(playerHealth.isPlayerAlive == false && addedRigidbody == false)
        {
            animator.enabled = false;
            gameObject.AddComponent<Rigidbody>();
            addedRigidbody = true;
        }
    }

    private void OnEnable()
    {
        reloadTimer.SetActive(false);
        animator.SetBool("Cut", false);
        firstPersonController.mouseLook.smoothTime = smoothTime;
    }

    void ActivateMeleeWeaponDamage()
    {
        isDealingDamage = true;
        Invoke("StopDealingDamage", 0.15f);
    }

    void StopDealingDamage()
    {
        isDealingDamage = false;
        dealtDamage = false;
        animator.SetBool("Cut",false);
        Invoke("ReadyToAttack", timeBetweeenAttacks);
    }

    void ReadyToAttack()
    {
        readyToAttack = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDealingDamage == true && dealtDamage == false)
        {
            if (other.transform.tag == "Enemy" || other.transform.tag == "EnemyHead")
            { 
                other.transform.GetComponent<BroadcastHit>().CallTakeDamage(meleeWeaponDamage, hitForce, true,true);
                audioSource.PlayOneShot(hitSound);
                GameObject newBloodVFX = Instantiate(bloodVFX, other.transform.position, other.transform.rotation);
                dealtDamage = true;
            }
          
                
        }
            
        
    }
}
