using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;


public class playerBehavior : MonoBehaviour
{
    [SerializeField] GameObject staminaIndicator;
    [SerializeField] GameObject staminaBar;
    [SerializeField] GameObject weaponCamera;
    Text staminaText;
    [SerializeField] public float maxStamina = 100f;
    float currentStamina;
    [SerializeField] float staminaDecreaseTime = 0.2f;
    [SerializeField] float staminaRegenTime = 2f;
    [SerializeField] float staminaStartToRegenTime = 0.5f;
    [SerializeField] bool jumpStopsStaminaRegen = true;
    [SerializeField] bool jumpCostsStamina = true;
    [SerializeField] float jumpStaminaCost = 10f;
    [SerializeField] float baseFieldOfView = 65f;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject backCamera;
    [SerializeField] GameObject sniperScope;
    [SerializeField]public AudioSource runAudio;
    [SerializeField] AudioSource outOfBreathAudio;
    [SerializeField] AudioSource flashlightAudio;
    [SerializeField] AudioSource needleAudio;
    [SerializeField] public int injections = 0;
    [SerializeField] float timeSlowAmountPercent = 75f;
    [SerializeField] float adrenalineWoreOffTime = 5f;
    [SerializeField] GameObject injectionTextObject;
    bool verticalAxisUsed = false;
    bool horizontalAxisUsed = false;
    RectTransform staminaBarTransform;
    Vector3 currentStaminaBarFill;
    float timerForAudioClips;
    Text injectionText;
    bool canDecreaseStamina = true;
    bool canUseInjection = true;
    [HideInInspector] public bool crouching = false;
    bool canCrouch = true;
    [HideInInspector] public bool notAbleToCrouch = false;
    [SerializeField] GameObject crouchImage;
   
    
    [HideInInspector]public bool running = false;
    [HideInInspector]public bool staminaKeyPressed = false;
    [HideInInspector]public bool regening = false;
    int moving = 0;
    float timePassedSinceLastStaminaUse = 0f;
    int numberOfStaminaKeyPressed = 0;
    PlayerHealth player_health;
    Rigidbody playerRigidbody;
    [HideInInspector] public bool isJumping = false;
    [SerializeField] GameObject flashlight;
    bool flashlightTurnedOn = false;
    
    
    void Start()
    {
        Time.timeScale = 1f;
        staminaText = staminaIndicator.GetComponent<Text>();
        currentStamina = maxStamina;
        player_health = FindObjectOfType<PlayerHealth>();
        playerRigidbody = GetComponent<Rigidbody>();
        staminaBarTransform = staminaBar.GetComponent<RectTransform>();
        injectionText = injectionTextObject.GetComponent<Text>();
       
    }

    private void FixedUpdate()
    {
        timePassedSinceLastStaminaUse += Time.deltaTime;
        if (timePassedSinceLastStaminaUse > 10000f) // avoid getting too high numbers
        {
            timePassedSinceLastStaminaUse /= 2; 
        }

        if (moving == 0 && isJumping == false) // this helps with the 0 friction in the game
        {
            playerRigidbody.velocity = new Vector3(0, playerRigidbody.velocity.y, 0);
        }
        timerForAudioClips += Time.deltaTime;
        if (timerForAudioClips > 10000) timerForAudioClips /= 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (player_health.isPlayerAlive == true && player_health.isGamePaused == false)
        {
            if (crouching == false)
                StaminaManagement();
            else
                CheckIfMoving();
            BackCameraManagement();
            Flashlight();
            if(notAbleToCrouch == false)
            Crouching();
            AdrenalineShot();
           
        }
        else if(player_health.isPlayerAlive == false)
        {
            runAudio.Stop();
            transform.GetComponent<CapsuleCollider>().height = 1.6f;
            transform.GetComponent<CapsuleCollider>().radius = 0.3f;
            currentStamina = 0;
            staminaText.text = 0.ToString();
            currentStaminaBarFill = new Vector3(0, 1, 1);
            staminaBarTransform.localScale = currentStaminaBarFill;
            backCamera.SetActive(false);
            weaponCamera.SetActive(true);
            crouchImage.SetActive(false);
        }
        else if(player_health.isGamePaused == true)
        {
            
        }
       

    }

    void Crouching()
    {
        
        if(Input.GetButtonDown("Crouch") && canCrouch == true)
        {
            canCrouch = false;
            if (crouching == false)
            {
                transform.GetComponent<CapsuleCollider>().height = 0.8f;
                transform.GetComponent<CapsuleCollider>().radius = 0.35f;
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                crouching = !crouching;
                crouchImage.SetActive(true);
                playerRigidbody.useGravity = true;
                Invoke("AllowCrouchAgain", 1f);
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 0.3f, mainCamera.transform.position.z);
            }
            else if (crouching == true)
            {
                transform.GetComponent<CapsuleCollider>().height = 1.6f;
                transform.GetComponent<CapsuleCollider>().radius = 0.3f;
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
                staminaKeyPressed = false;
                crouching = !crouching;
                crouchImage.SetActive(false);
                Invoke("AllowCrouchAgain", 1f);
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 0.3f, mainCamera.transform.position.z);
            }
            if (running == true)
                runAudio.pitch = 1f;

        }
        
    }

    void AllowCrouchAgain()
    {
        canCrouch = true;
    }

    private void AdrenalineShot()
    {
        if (Input.GetButtonDown("Use adrenaline") && injections > 0 && currentStamina < maxStamina)
        {
            canUseInjection = false;
            RestoreStamina(100f);
            injections--;
            injectionText.text = injections.ToString();
            needleAudio.Play();
            staminaText.text = currentStamina.ToString();
            currentStaminaBarFill = new Vector3(1, currentStamina / 100, 1);
            staminaBarTransform.localScale = currentStaminaBarFill;
            Time.timeScale = timeSlowAmountPercent/100f;
            Invoke("StopAdrenalineEffect", adrenalineWoreOffTime);

        }
    }

    void StopAdrenalineEffect()
    {
        Time.timeScale = 1f;
        canUseInjection = true;
    }

    void Flashlight()
    {
        if(Input.GetButtonDown("Flashlight"))
        {
            flashlightAudio.Play();
            flashlightTurnedOn = !flashlightTurnedOn;
            flashlight.SetActive(flashlightTurnedOn);
        }
    }

    private void BackCameraManagement()
    {
        if (Input.GetButtonDown("Look behind"))
        {
            sniperScope.SetActive(false);
            backCamera.SetActive(true);
            weaponCamera.SetActive(false);
            flashlight.transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + 180, transform.localRotation.z);

        }
        else if (Input.GetButtonUp("Look behind"))
        {
            mainCamera.GetComponent<Camera>().fieldOfView = baseFieldOfView;
            backCamera.SetActive(false);
            weaponCamera.SetActive(true);
            flashlight.transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z);
        }
    }

    private void StaminaManagement()
    {
        if (Input.GetButton("Run") && timePassedSinceLastStaminaUse > 0 && moving > 0)
        {
            timePassedSinceLastStaminaUse = 0;
            regening = false;
        }

        if (Input.GetButtonDown("Run") && canDecreaseStamina)
        {
            staminaKeyPressed = true;
            StartCoroutine("decreaseStamina");
        }

        if (jumpStopsStaminaRegen) if (Input.GetButtonDown("Jump"))
            {
                timePassedSinceLastStaminaUse = -staminaStartToRegenTime;
                if (jumpCostsStamina && currentStamina > jumpStaminaCost && isJumping == false)
                {
                    currentStamina -= jumpStaminaCost;
                    staminaText.text = currentStamina.ToString();
                    currentStaminaBarFill = new Vector3(1, /*(currentStamina - jumpStaminaCost) / 100f*/ currentStamina/100f, 1);
                    staminaBarTransform.localScale = currentStaminaBarFill;
                    
                }
                else if (jumpCostsStamina && currentStamina <= jumpStaminaCost && isJumping == false)
                {
                    currentStamina = 0;
                    if( outOfBreathAudio.isPlaying == false)
                    {
                        outOfBreathAudio.Play();
                    }
                    staminaText.text = currentStamina.ToString();
                    currentStaminaBarFill = new Vector3(1, 0, 1);
                    staminaBarTransform.localScale = currentStaminaBarFill;
                              
                }
            }
        

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        if (Input.GetButtonUp("Run"))
        {
            staminaKeyPressed = false;
            running = false;
        }


        CheckIfMoving();

        if (timePassedSinceLastStaminaUse >= staminaStartToRegenTime && regening == false)
        {
            regening = true;
            StartCoroutine("regenStamina");
           
        }
    }


    private void CheckIfMoving()
    {
        

        if (moving != 0 && isJumping == false && running == false && player_health.isPlayerAlive == true)
        {
            runAudio.pitch = 0.6f;
            if(runAudio.isPlaying == false)
            runAudio.Play();
        }
        else if(moving == 0 || isJumping == true || player_health.isPlayerAlive == false)
        {
            runAudio.Stop();
        }

        if (Input.GetAxisRaw("Vertical") != 0 && verticalAxisUsed == false)
        {
            moving++;
            verticalAxisUsed = true;
        }
        if (Input.GetAxisRaw("Horizontal") != 0 && horizontalAxisUsed == false)
        {
            moving++;
            horizontalAxisUsed = true;
        }

        if (Input.GetAxisRaw("Vertical") == 0 && verticalAxisUsed == true)
        {
            moving--;
            verticalAxisUsed = false;
            
        }
        if (Input.GetAxisRaw("Horizontal") == 0 && horizontalAxisUsed == true)
        {
            moving--;
            horizontalAxisUsed = false;
        }

        /* if (Input.GetKeyDown(KeyCode.W))
             moving++;
         if (Input.GetKeyDown(KeyCode.A))
             moving++;
         if (Input.GetKeyDown(KeyCode.S))
             moving++;
         if (Input.GetKeyDown(KeyCode.D))
             moving++;

         if (Input.GetKeyUp(KeyCode.W))
             moving--;
         if (Input.GetKeyUp(KeyCode.A))
             moving--;
         if (Input.GetKeyUp(KeyCode.S))
             moving--;
         if (Input.GetKeyUp(KeyCode.D))
             moving--;
             */
    }

    private void OnCollisionEnter(Collision collision)
    {
        isJumping = false;
        FallDamage(collision);
    }

    private void FallDamage(Collision collision)
    {
        if (collision.relativeVelocity.y > 10f && collision.transform.CompareTag("Enemy") == false && collision.transform.CompareTag("EnemyHead") == false)
        {
            player_health.PlayerHit((int)collision.relativeVelocity.y * 3, false);
            if (crouching == false)
            {
                transform.GetComponent<CapsuleCollider>().height = 0.8f;
                transform.GetComponent<CapsuleCollider>().radius = 0.35f;
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                crouching = true;
                crouchImage.SetActive(true);
                Invoke("AllowCrouchAgain", 1f);
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 0.3f, mainCamera.transform.position.z);
            }
        }
    }

    public void InjectionPickUp()
    {
        injections++;
        injectionText.text = injections.ToString();
    }

    void RestoreStamina(float number)
    {
        currentStamina = Mathf.Clamp(currentStamina + number, 0, 100);
        currentStaminaBarFill = new Vector3(1, currentStamina / 100, 1);
        staminaBarTransform.localScale = currentStaminaBarFill;
    }

    IEnumerator decreaseStamina()
    {
        canDecreaseStamina = false;
        while (currentStamina > 0 && staminaKeyPressed == true)
        {
            if (isJumping == true)
            {
                running = false;
            }
                if (moving > 0 && isJumping == false)
            {
                runAudio.pitch = 1f;
                currentStamina -= 1;
                staminaText.text = currentStamina.ToString();
                currentStaminaBarFill = new Vector3(1, currentStamina / 100, 1);
                staminaBarTransform.localScale = currentStaminaBarFill;
                running = true;
               

               /* if (runAudio.isPlaying == false)
                {
                    runAudio.Play();
                }*/
               
            }
            if (currentStamina == 0 && outOfBreathAudio.isPlaying == false && timerForAudioClips > 2f)
            {
                outOfBreathAudio.Play();
                timerForAudioClips = 0f;
            }
            if (player_health.isPlayerAlive == false)
            {
                runAudio.Stop();
                break;
            }
            if (crouching == true)
            {
                runAudio.pitch = 0.6f;
                running = false;
                canDecreaseStamina = true;
                break;
            }


            yield return new WaitForSeconds(staminaDecreaseTime);
        }
        running = false;
        runAudio.pitch = 0.6f;
        canDecreaseStamina = true;
        
    }

    IEnumerator regenStamina()
    {

        while (currentStamina < maxStamina)
        {
            if (timePassedSinceLastStaminaUse > staminaStartToRegenTime && staminaKeyPressed == false && regening == true)
            { 
                currentStamina += 1;
                staminaText.text = currentStamina.ToString();
                currentStaminaBarFill = new Vector3(1, currentStamina/100, 1);
                staminaBarTransform.localScale = currentStaminaBarFill;
            }
            if (regening == false || staminaKeyPressed == true || timePassedSinceLastStaminaUse < staminaStartToRegenTime || crouching == true)
            {
                regening = false;
                break;
            }
                yield return new WaitForSeconds(staminaRegenTime);
        }
        regening = false;
    }

    public bool returnRunningState()
    {
        return running;
    }

    public float returnStamina()
    {
        return currentStamina;
    }

}
