using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] float health = 100f;
    [SerializeField] GameObject healthDisplay;
    [SerializeField] float weaponDropStrenghtForward = 1f;
    [SerializeField] float weaponDropDrag = 10f;
    [SerializeField] float damageTakenFromZombieHeads = 2f;
    [SerializeField] int painkillers = 0;
    [SerializeField] GameObject reticle;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject painkillersTextObject;
    [SerializeField] AudioSource pillsAudio;
    [SerializeField] GameObject redScreen;
    [SerializeField] GameObject pauseMenu;
    //[SerializeField] Texture2D cursorTexture;
    Text painkillersText;

    Text healthText;
    AudioSource audioSource;
    [SerializeField] GameObject DeadUI;
    EnemyAI[] enemyNumber = new EnemyAI[500];
    [HideInInspector] public bool isPlayerAlive = true;
    [HideInInspector] public bool isGamePaused = false;
    RigidbodyFirstPersonController movement;
    Rigidbody movementObjectRigidbody;
    bool canTakeDamageFromEnemyHead = true;
    RectTransform healthBarTransform;
    Vector3 currentHealthBarFill;
    float timeScaleBeforePaused;



    Weapon currentWeapon;

    [SerializeField] AudioClip pain1;
    [SerializeField] AudioClip pain2;
    [SerializeField] AudioClip pain3;
    [SerializeField] AudioClip pain4;
    [SerializeField] AudioClip pain5;
    [SerializeField] AudioClip pain6;
    [SerializeField] AudioClip pain7;
    [SerializeField] AudioClip pain8;
    [SerializeField] AudioClip pain9;
    [SerializeField] AudioClip pain10;
    [SerializeField] AudioClip pain11;
    [SerializeField] AudioClip pain12;

    [SerializeField] AudioClip deathSound;





    void Start()
    {

        switch(PlayerPrefs.GetInt("Difficulty",2))
        {
            case 1:
                damageTakenFromZombieHeads = 2;
                break;
            case 2:
                damageTakenFromZombieHeads = 5;
                break;
            case 3:
                damageTakenFromZombieHeads = 7;
                break;
            case 4:
                damageTakenFromZombieHeads = 10;
                break;
        }
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
       // Cursor.lockState = CursorLockMode.Locked;
        healthText = healthDisplay.GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        enemyNumber = FindObjectsOfType<EnemyAI>();
        movement = FindObjectOfType<RigidbodyFirstPersonController>();
        movementObjectRigidbody = movement.GetComponent<Rigidbody>();
        currentWeapon = FindObjectOfType<Weapon>();
        healthBarTransform = healthBar.GetComponent<RectTransform>();
        painkillersText = painkillersTextObject.GetComponent<Text>();
       // Cursor.SetCursor(cursorTexture, new Vector2(0, 0),CursorMode.Auto);
        
    }

    private void Update()
    {
        DebugKeys();
        if (isPlayerAlive)
        {
            if (isGamePaused == false)
            {
                Painkillers();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
               
            }

            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused == false)
            {
                timeScaleBeforePaused = Time.timeScale;
                isGamePaused = true;
                movement.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
                redScreen.SetActive(false);
                GetComponent<playerBehavior>().runAudio.Pause();
                 Time.timeScale = 0f;
            }
            else if (isGamePaused == true)
            {
                Time.timeScale = timeScaleBeforePaused;
                isGamePaused = false;
                movement.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenu.SetActive(false);
                GetComponent<playerBehavior>().runAudio.Play();
            }

        }
    }

    private void Painkillers()
    {
        if (Input.GetButtonDown("Use pills") && painkillers > 0 && health < 100f)
        {
            painkillers--;
            painkillersText.text = painkillers.ToString();
            pillsAudio.Play();
            PlayerHeal(20f);
        }
    }

    private void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.U) && isPlayerAlive == true && Debug.isDebugBuild)
        {
            PlayerHit(100f,false);
        }

        if (Input.GetKeyDown(KeyCode.L) && Debug.isDebugBuild)
        {
            PlayerHit(1f, false);
        }

        if (Input.GetKeyDown(KeyCode.K) && Debug.isDebugBuild)
        {
            PlayerHeal(1f);
           
        }
    }



    public void PickUpPills()
    {
        painkillers++;
        painkillersText.text = painkillers.ToString();
    }

    public float GetPlayerHealth()
    {
        return health;
    }

    public void SetCurrentWeapon() // Use this when switching weapon, after disabled last
    {
        if(FindObjectOfType<Weapon>() != null)
        currentWeapon = FindObjectOfType<Weapon>();
    }

   public void PlayerHit(float damage, bool isRunner)
    {
        if (health <= damage && isPlayerAlive) // dead
        {
            PlayerDeath();
        }
        else if(isPlayerAlive)
        {
            Hit(damage);
            if(isRunner == true)
            {
               movementObjectRigidbody.velocity = Vector3.zero;
               movementObjectRigidbody.angularVelocity = Vector3.zero;
            }
        }
        currentHealthBarFill = new Vector3(1f, health / 100f, 1f);
        healthBarTransform.localScale = currentHealthBarFill;
        healthText.text = health.ToString();
        redScreen.SetActive(true);
    }

    public void PlayerHeal(float number)
    {
        health = Mathf.Clamp(health + number,0,100);
        currentHealthBarFill = new Vector3(1f, health / 100f, 1f);
        healthBarTransform.localScale = currentHealthBarFill;
        healthText.text = health.ToString();
    }

    private void Hit(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, 100);
        PlayPainSounds();
    }

    private void PlayPainSounds()
    {
        if (audioSource.isPlaying == false)
        {
            switch (Random.Range(1, 13))
            {
                case 1:
                    audioSource.PlayOneShot(pain1);
                    break;
                case 2:
                    audioSource.PlayOneShot(pain2);
                    break;
                case 3:
                    audioSource.PlayOneShot(pain3);
                    break;
                case 4:
                    audioSource.PlayOneShot(pain4);
                    break;
                case 5:
                    audioSource.PlayOneShot(pain5);
                    break;
                case 6:
                    audioSource.PlayOneShot(pain6);
                    break;
                case 7:
                    audioSource.PlayOneShot(pain7);
                    break;
                case 8:
                    audioSource.PlayOneShot(pain8);
                    break;
                case 9:
                    audioSource.PlayOneShot(pain9);
                    break;
                case 10:
                    audioSource.PlayOneShot(pain10);
                    break;
                case 11:
                    audioSource.PlayOneShot(pain11);
                    break;
                case 12:
                    audioSource.PlayOneShot(pain12);
                    break;
            }
        }
    }

    private void PlayerDeath()
    {
        isPlayerAlive = false;
        health = 0;
        healthText.text = health.ToString();
        audioSource.PlayOneShot(deathSound);
        Invoke("PlayDeathMusic",1f);
        DeadUI.SetActive(true);
        DecreaseEnemySound();
        movement.enabled = false;
        movementObjectRigidbody.freezeRotation = false;
        transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z - 3);
        movementObjectRigidbody.velocity = new Vector3(0, 0, 0);
        reticle.SetActive(false);
        Invoke("FreezeRotationAgain", 5);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        movementObjectRigidbody.useGravity = true;
        if (currentWeapon != null)
        {
            currentWeapon.transform.parent = null;
            currentWeapon.gameObject.AddComponent<BoxCollider>();
            var currentWeaponRigidbody = currentWeapon.gameObject.AddComponent<Rigidbody>();
            currentWeaponRigidbody.transform.position = currentWeaponRigidbody.transform.position;
            currentWeaponRigidbody.transform.rotation = currentWeaponRigidbody.transform.rotation;
            currentWeaponRigidbody.mass = 0.01f;
            currentWeaponRigidbody.AddForce(movement.transform.forward * weaponDropStrenghtForward);
            currentWeaponRigidbody.drag = weaponDropDrag;
        }
    }

    private void PlayDeathMusic()
    {
        audioSource.Play();
    }

    private void FreezeRotationAgain()
    {
        movementObjectRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        
    }

    public void DecreaseEnemySound()
    {
        for (int i = 0; i < enemyNumber.Length; i++)
        {
           
                if (enemyNumber[i] == null)
                {

                }
                else
                {
                    enemyNumber[i].DecreaseVolumeAfterPlayerDeath();
                }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyHead" && canTakeDamageFromEnemyHead == true)
        {
            if (health > 0)
            {
                StartCoroutine("TakeDamageFromStayingOnTopOfZombies",collision); 
            }
            else if(isPlayerAlive == true)
            {
                PlayerDeath();
            }
        }
    }

    IEnumerator TakeDamageFromStayingOnTopOfZombies(Collision collision)
    {
        canTakeDamageFromEnemyHead = false;
        collision.transform.GetComponent<BroadcastHit>().ZombieHeadDamage(damageTakenFromZombieHeads);
        yield return new WaitForSeconds(0.5f);
        canTakeDamageFromEnemyHead = true;
    }
}
