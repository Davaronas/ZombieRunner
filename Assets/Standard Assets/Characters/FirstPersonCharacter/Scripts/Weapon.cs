using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;



public class Weapon : MonoBehaviour
{
    // some variables could be set without serialize field


    /*
     [DllImport("user32.dll")]
     static extern bool SetCursorPos(float X, float Y);
     [DllImport("user32.dll")]
     public static extern bool GetCursorPos(out Vector2 something);
     Vector2 currentCursorPosition;
     */



    EnemyHealth enemyHealth;
    ParticleSystem muzzleFlash;
    AudioSource audioSource;
    bool readyToShoot = true;
    MeshRenderer meshRenderer;
    public EnemyAI[] numberOfEnemies = new EnemyAI[500];
    PlayerHealth playerHealth;
    playerBehavior player_behaviour;
    RigidbodyFirstPersonController firstPersonController;
    float baseMouseSensitivityX;
    float baseMouseSensitivityY;
    float zoomedMouseSensitivityX;
    float zoomedMouseSensitivityY;
    bool reloaded = true;
    float reloadTimeSoundLenght;
    float reloadTimeSoundLenghtLeft;
    [HideInInspector] public float bulletsLeftInMagazine;
    bool reloading = false;
    Text magazineText;
    Text ammoLeftText;
    bool auto_Firing = false;
    bool canPlayAutomaticEmptySound = true;
    bool endedCouroutineForShootingAuto = true;
    Vector3 baserotation;
    float reloadTimer = 0f;
    float currentRaycastRecoil = 0f;
    RectTransform reticleRectTransform;
    float oldReticleSizeX;
    float oldReticleSizeY;


    [SerializeField] Camera FPSCamera;
    [SerializeField] bool automatic = false;
    [SerializeField] bool hasScope = false;
    [SerializeField] bool shotgun = false;
    [SerializeField] bool explosiveShots = false;
    [SerializeField] bool canDismember = false;
    [SerializeField] float range = 100f;
    [SerializeField] float timeBetweenShots = 0.5f;
    [SerializeField] float recoil = 20f;
    [SerializeField] float raycastRecoil = 0.01f;
    [SerializeField] float maxRaycastRecoil = 0.2f;
    [SerializeField] public float magazineSize = 9f;
    [SerializeField] bool alterStartingBullets = false;
    [SerializeField] [Range(0, 100)] [Tooltip("Must not be higher than magazine size")] int startingBulletsLeftInMagazine;
    [SerializeField] public float ammoLeft;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] float decalTimeToLive = 120f;
    [SerializeField] float fieldOfViewZoomLevel = 40f;
    [SerializeField] float zoomedMouseSensitivity = 1f;
    [SerializeField] float rotateWeaponWhenReloadingX = 45f;
    [SerializeField] float rotateWeaponWhenReloadingY = 0f;
    [SerializeField] float rotateWeaponWhenReloadingZ = 0f;
    [SerializeField] public float currentWeaponDamage = 20f;
    [SerializeField] float headShotMultiplier = 2f;
    [SerializeField] public float zombieHearDistance = 20f;
    [SerializeField] float hitForceOnEnemy = 200f;
    [SerializeField] float innerShotgunDifference = 0.02f;
    [SerializeField] float outerShotgunDifference = 0.04f;
    [SerializeField] int baseFieldOfView = 65;
    [SerializeField] float smoothTime = 10f;
    [SerializeField] float recoilSpeed = 1f;
    [SerializeField] float timerForAudio;
    [SerializeField] AudioClip fireSound;
    [SerializeField] AudioClip reloadWeaponSound;
    [SerializeField] AudioClip emptyGunShotSound;
    [SerializeField] Transform mainCamera;
    [SerializeField] GameObject muzzleFlashLight;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject bulletHole;
    [SerializeField] GameObject magazineTextObject;
    [SerializeField] GameObject maxMagazineTextObject;
    [SerializeField] GameObject reloadTimerObject;
    RectTransform reloadTimerTransform;
    [SerializeField] GameObject reticle;
    [SerializeField] GameObject sniperScope;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] GameObject bloodVFX;



    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<MeshRenderer>() != null)
            meshRenderer = GetComponent<MeshRenderer>();
        muzzleFlash = GameObject.Find("Muzzle_Flash").GetComponent<ParticleSystem>();
        reticleRectTransform = reticle.GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
        numberOfEnemies = FindObjectsOfType<EnemyAI>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        player_behaviour = FindObjectOfType<playerBehavior>();
        firstPersonController = transform.parent.parent.GetComponent<RigidbodyFirstPersonController>();
        baseMouseSensitivityX = firstPersonController.mouseLook.XSensitivity;
        baseMouseSensitivityY = firstPersonController.mouseLook.YSensitivity;
        zoomedMouseSensitivityX = 1f;
        zoomedMouseSensitivityY = 1f;
        reloadTimeSoundLenght = reloadTime - reloadWeaponSound.length;
        if (alterStartingBullets == true)
            bulletsLeftInMagazine = startingBulletsLeftInMagazine;
        else
            bulletsLeftInMagazine = magazineSize;
        magazineText = magazineTextObject.GetComponent<Text>();
        magazineText.text = bulletsLeftInMagazine.ToString();
        ammoLeftText = maxMagazineTextObject.GetComponent<Text>();
        ammoLeftText.text = "\\" + ammoLeft;
        reloadTimerTransform = reloadTimerObject.GetComponent<RectTransform>();
        if (hasScope)
        {
            reticle.SetActive(false);
        }
        oldReticleSizeX = 1f;
        oldReticleSizeY = 1f;
        // GetCursorPos(out currentCursorPosition);

    }


    // Update is called once per frame
    void Update()
    {
        reloadTimer = Mathf.Clamp(reloadTimer - Time.deltaTime, 0, 30);
        if (reloadTimerObject.activeSelf == true)
        {
            reloadTimerTransform.transform.localScale = new Vector3(reloadTimer / reloadTime, 1f, 1f);
        }
        timerForAudio += Time.deltaTime;
        if (timerForAudio > 10000f)
            timerForAudio /= 5f;

        if (playerHealth.isPlayerAlive && playerHealth.isGamePaused == false)
        {
            if (Input.GetButtonDown("Shoot/Cut") && readyToShoot == true && reloaded == true && automatic == false)
            {
                Fire();
                StartCoroutine("waitTime");
            }
            else if (Input.GetButtonDown("Shoot/Cut") && reloaded == false && reloading == false && timerForAudio > 0.05f)
            {
                audioSource.PlayOneShot(emptyGunShotSound);
            }




            if (Input.GetButton("Shoot/Cut") && readyToShoot == true && reloaded == true && automatic == true && auto_Firing == false && endedCouroutineForShootingAuto == true)
            {
                auto_Firing = true;
                StartCoroutine("AutoFire");
            }
            else if (Input.GetButtonDown("Shoot/Cut") && bulletsLeftInMagazine <= 0 && automatic == true && canPlayAutomaticEmptySound == true && reloading == false)
            {
                if (timerForAudio > 0.05f)
                    audioSource.PlayOneShot(emptyGunShotSound);
                StartCoroutine("Wait_Automatic");
            }

            if (Input.GetButtonUp("Shoot/Cut"))
            {
                auto_Firing = false;
            }

            Zoom();

            if (Input.GetButtonDown("Reload") && bulletsLeftInMagazine < magazineSize && reloading == false && auto_Firing == false && ammoLeft > 0)
            {
                StartCoroutine("ReloadWeapon");
            }
        }
        else if (playerHealth.isPlayerAlive == false || playerHealth.isGamePaused == true)
        {
            FPSCamera.fieldOfView = baseFieldOfView;
            sniperScope.SetActive(false);
        }
    }


    private void Zoom()
    {
        if (Input.GetButtonDown("Aim"))
        {
            FPSCamera.fieldOfView = fieldOfViewZoomLevel;
            firstPersonController.mouseLook.XSensitivity = zoomedMouseSensitivityX;
            firstPersonController.mouseLook.YSensitivity = zoomedMouseSensitivityY;
            if (hasScope)
            {
                sniperScope.SetActive(true);
                meshRenderer.enabled = false;
            }
        }
        if (Input.GetButtonUp("Aim"))
        {
            FPSCamera.fieldOfView = baseFieldOfView;
            firstPersonController.mouseLook.XSensitivity = baseMouseSensitivityX;
            firstPersonController.mouseLook.YSensitivity = baseMouseSensitivityY;
            if (hasScope)
            {
                sniperScope.SetActive(false);
                meshRenderer.enabled = true;
            }
        }
    }

    public void RefreshMaxAmmo()
    {
        ammoLeftText.text = "\\" + ammoLeft;
    }

    private void OnEnable()
    {
        if (meshRenderer != null)
            meshRenderer.enabled = true;
        reticleRectTransform.localScale = new Vector3(oldReticleSizeX, oldReticleSizeY, 1f);
        reloadTimerObject.SetActive(false);
        magazineTextObject.SetActive(true);
        maxMagazineTextObject.SetActive(true);
        if (reloaded == false && ammoLeft > 0)
        {
            StartCoroutine("ReloadWeapon");
        }
        if (readyToShoot == false)
        {
            Invoke("MakeReadyToShootAfterDelay", timeBetweenShots);
        }
        FPSCamera.fieldOfView = baseFieldOfView;

        if (hasScope)
        {
            reticle.SetActive(false);
        }
        else
        {
            reticle.SetActive(true);
        }
        if (automatic && auto_Firing == true)
        {
            auto_Firing = false;
            endedCouroutineForShootingAuto = true;
        }
        magazineTextObject.GetComponent<Text>().text = bulletsLeftInMagazine.ToString();
        ammoLeftText.text = "\\" + ammoLeft;
        sniperScope.SetActive(false);
        firstPersonController.mouseLook.smoothTime = smoothTime;
    }

    void MakeReadyToShootAfterDelay()
    {
        readyToShoot = true;
    }

    private void Fire()
    {
        /*
        GetCursorPos(out currentCursorPosition);
        SetCursorPos(currentCursorPosition.x, currentCursorPosition.y + recoil);
        */

        RaycastHit hit;
        muzzleFlash.Play();
        if (timerForAudio > 0.04f)
            audioSource.PlayOneShot(fireSound);
        muzzleFlashLight.SetActive(true);
        Invoke("DisableLight", 0.05f);
        numberOfEnemies = FindObjectsOfType<EnemyAI>();
        for (int i = 0; i < numberOfEnemies.Length; i++)
        {
            numberOfEnemies[i].ifHeardShot(zombieHearDistance);
        }
        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward + new Vector3(Random.Range(0f, currentRaycastRecoil / 2), currentRaycastRecoil, Random.Range(0f, currentRaycastRecoil / 2)), out hit, range) && shotgun == false)
        {
            //  print("we hit " + hit.transform.name);

            if (hit.transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hit.transform.tag == "Enemy" || hit.transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hit.point, Quaternion.LookRotation(hit.normal));
                    if (hit.transform.gameObject.tag == "EnemyHead")
                        hit.transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hit.transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else if (shotgun == false)
                {
                    if (hit.transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                        GameObject newBulletHole = Instantiate(bulletHole, hit.point, hitRotation);
                        GameObject newHitVFX = Instantiate(hitVFX, hit.point, Quaternion.LookRotation(hit.normal));
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                }
            }


            /*if (hit.transform.GetComponent<EnemyHealth>() != null)
            {
                enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                if (hit.transform.gameObject.tag == "EnemyHead")
                {
                    enemyHealth.TakeDamage(currentWeaponDamage * headShotMultiplier);
                }
                else
                {
                    enemyHealth.TakeDamage(currentWeaponDamage);
                }
            }
            else if (hit.transform.parent != null)
            {
                if (hit.transform.parent.GetComponent<EnemyHealth>() != null)
                {
                    enemyHealth = hit.transform.parent.GetComponent<EnemyHealth>();
                    if (hit.transform.gameObject.tag == "EnemyHead")
                    {
                        enemyHealth.TakeDamage(currentWeaponDamage * headShotMultiplier);
                    }
                    else
                    {
                        enemyHealth.TakeDamage(currentWeaponDamage);
                    }
                }
                else if (shotgun == false)
                {
                    var hitRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                    GameObject newBulletHole = Instantiate(bulletHole, hit.point, hitRotation);
                    Destroy(newBulletHole, decalTimeToLive);
                }

            }
            */


            // print(enemyHealth.hitPoints);



            if (explosiveShots)
            {
                GameObject explosion = Instantiate(explosionVFX, hit.point, Quaternion.LookRotation(hit.normal));
            }

        }
        else
        {

        }


        if (shotgun)
        {
            ShotgunFire();
        }

        mainCamera.Rotate(transform.rotation.x - recoil / 4, transform.rotation.y, transform.rotation.z);

        // NOT WORKING mainCamera.localRotation = Quaternion.RotateTowards(mainCamera.transform.localRotation, Quaternion.Euler(transform.localRotation.x - recoil/10, transform.localRotation.y, transform.localRotation.z),90f);


        bulletsLeftInMagazine--;
        magazineText.text = bulletsLeftInMagazine.ToString();
        if (bulletsLeftInMagazine <= 0)
        {
            magazineText.text = bulletsLeftInMagazine.ToString();
            reloaded = false;
        }

    }



    void ShotgunFire()
    {
        RaycastHit[] hits = new RaycastHit[17];

        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward, out hits[0], range))
        {
            //print("we hit " + hit.transform.name);
            if (hits[0].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[0].transform.tag == "Enemy" || hits[0].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[0].point, Quaternion.LookRotation(hits[0].normal));
                    if (hits[0].transform.gameObject.tag == "EnemyHead")
                        hits[0].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[0].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[0].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[0].point, Quaternion.LookRotation(hits[0].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[0].normal);
                        GameObject newHitVFX = Instantiate(hitVFX, hits[0].point, Quaternion.LookRotation(hits[0].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[0].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                }
            }
        }

        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward - new Vector3(innerShotgunDifference, 0, innerShotgunDifference), out hits[1], range)) // LEFT FIRST
        {
            //  print("we hit " + hit2.transform.name);
            if (hits[1].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[1].transform.tag == "Enemy" || hits[1].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[1].point, Quaternion.LookRotation(hits[1].normal));
                    if (hits[1].transform.gameObject.tag == "EnemyHead")
                        hits[1].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[1].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[1].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[1].point, Quaternion.LookRotation(hits[1].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[0].normal);
                        GameObject newHitVFX2 = Instantiate(hitVFX, hits[1].point, Quaternion.LookRotation(hits[1].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[1].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                }
            }
            // print(enemyHealth.hitPoints);
        }


        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward - new Vector3(outerShotgunDifference, 0, outerShotgunDifference), out hits[2], range))
        {
            //  print("we hit " + hit3.transform.name);
            if (hits[2].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[2].transform.tag == "Enemy" || hits[2].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[2].point, Quaternion.LookRotation(hits[2].normal));
                    if (hits[2].transform.gameObject.tag == "EnemyHead")
                        hits[2].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[2].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[2].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[2].point, Quaternion.LookRotation(hits[2].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[2].normal);
                        GameObject newHitVFX3 = Instantiate(hitVFX, hits[2].point, Quaternion.LookRotation(hits[2].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[2].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                }
            }
            // print(enemyHealth.hitPoints);
        }


        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward + new Vector3(innerShotgunDifference, 0, innerShotgunDifference), out hits[3], range))
        {
            //  print("we hit " + hit4.transform.name);
            if (hits[3].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[3].transform.tag == "Enemy" || hits[3].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[3].point, Quaternion.LookRotation(hits[3].normal));
                    if (hits[3].transform.gameObject.tag == "EnemyHead")
                        hits[3].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[3].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[3].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[3].point, Quaternion.LookRotation(hits[3].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[3].normal);
                        GameObject newHitVFX4 = Instantiate(hitVFX, hits[3].point, Quaternion.LookRotation(hits[3].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[3].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                }
            }

        }



        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward + new Vector3(outerShotgunDifference, 0, outerShotgunDifference), out hits[4], range))
        {
            // print("we hit " + hit5.transform.name);
            if (hits[4].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[4].transform.tag == "Enemy" || hits[4].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[4].point, Quaternion.LookRotation(hits[4].normal));
                    if (hits[4].transform.gameObject.tag == "EnemyHead")
                        hits[4].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[4].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[4].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[4].point, Quaternion.LookRotation(hits[4].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[4].normal);
                        GameObject newHitVFX5 = Instantiate(hitVFX, hits[4].point, Quaternion.LookRotation(hits[4].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[4].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                }
                // print(enemyHealth.hitPoints); 
            }

        }


        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward - new Vector3(0, innerShotgunDifference, innerShotgunDifference), out hits[5], range))
        {
            //  print("we hit " + hit6.transform.name);
            if (hits[5].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[5].transform.tag == "Enemy" || hits[5].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[5].point, Quaternion.LookRotation(hits[5].normal));
                    if (hits[5].transform.gameObject.tag == "EnemyHead")
                        hits[5].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[5].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[5].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[5].point, Quaternion.LookRotation(hits[5].normal));
                    }
                    else
                    {
                        if (hits[5].transform.CompareTag("Bodyparts"))
                        {
                            GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[5].point, Quaternion.LookRotation(hits[5].normal));
                        }
                        else
                        {
                            var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[5].normal);
                            GameObject newHitVFX6 = Instantiate(hitVFX, hits[5].point, Quaternion.LookRotation(hits[5].normal));
                            GameObject newBulletHole = Instantiate(bulletHole, hits[5].point, hitRotation);
                            Destroy(newBulletHole, decalTimeToLive);
                        }
                    }
                }


            }
        }

        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward - new Vector3(0, outerShotgunDifference, outerShotgunDifference), out hits[6], range))
        {

            if (hits[6].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[6].transform.tag == "Enemy" || hits[6].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[6].point, Quaternion.LookRotation(hits[6].normal));
                    if (hits[6].transform.gameObject.tag == "EnemyHead")
                        hits[6].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[6].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[6].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[6].point, Quaternion.LookRotation(hits[6].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[6].normal);
                        GameObject newHitVFX7 = Instantiate(hitVFX, hits[6].point, Quaternion.LookRotation(hits[6].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[6].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                    //  print(enemyHealth.hitPoints);
                }
            }
        }

        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward + new Vector3(0, innerShotgunDifference, innerShotgunDifference), out hits[7], range))
        {
            //print("we hit " + hit8.transform.name);
            if (hits[7].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[7].transform.tag == "Enemy" || hits[7].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[7].point, Quaternion.LookRotation(hits[7].normal));
                    if (hits[7].transform.gameObject.tag == "EnemyHead")
                        hits[7].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[7].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[7].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[7].point, Quaternion.LookRotation(hits[7].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[7].normal);
                        GameObject newHitVFX8 = Instantiate(hitVFX, hits[7].point, Quaternion.LookRotation(hits[7].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[7].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                    //  print(enemyHealth.hitPoints);
                }
            }

        }

        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward + new Vector3(0, outerShotgunDifference, outerShotgunDifference), out hits[8], range))
        {
            //  print("we hit " + hit9.transform.name);
            if (hits[8].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[8].transform.tag == "Enemy" || hits[8].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[8].point, Quaternion.LookRotation(hits[8].normal));
                    if (hits[8].transform.gameObject.tag == "EnemyHead")
                        hits[8].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[8].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[8].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[8].point, Quaternion.LookRotation(hits[8].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[8].normal);
                        GameObject newHitVFX9 = Instantiate(hitVFX, hits[8].point, Quaternion.LookRotation(hits[8].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[8].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                }
                //    print(enemyHealth.hitPoints);

            }
        }

        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward - new Vector3(innerShotgunDifference, innerShotgunDifference, innerShotgunDifference), out hits[9], range)) // LEFT FIRST
        {
            //  print("we hit " + hit10.transform.name);
            if (hits[9].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[9].transform.tag == "Enemy" || hits[9].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[9].point, Quaternion.LookRotation(hits[9].normal));
                    if (hits[9].transform.gameObject.tag == "EnemyHead")
                        hits[9].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[9].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[9].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[9].point, Quaternion.LookRotation(hits[9].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[9].normal);
                        GameObject newHitVFX10 = Instantiate(hitVFX, hits[9].point, Quaternion.LookRotation(hits[9].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[9].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                }
            }
        }
        if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward - new Vector3(outerShotgunDifference, outerShotgunDifference, outerShotgunDifference), out hits[10], range))
        {

            if (hits[10].transform.gameObject.layer == 15 /* Ammo layer */) { }
            else
            {
                if (hits[10].transform.tag == "Enemy" || hits[10].transform.tag == "EnemyHead")
                {
                    GameObject newBloodVFX = Instantiate(bloodVFX, hits[10].point, Quaternion.LookRotation(hits[10].normal));
                    if (hits[10].transform.gameObject.tag == "EnemyHead")
                        hits[10].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                    else
                    {
                        hits[10].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                    }
                }
                else
                {
                    if (hits[10].transform.CompareTag("Bodyparts"))
                    {
                        GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[10].point, Quaternion.LookRotation(hits[10].normal));
                    }
                    else
                    {
                        var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[10].normal);
                        GameObject newHitVFX11 = Instantiate(hitVFX, hits[10].point, Quaternion.LookRotation(hits[10].normal));
                        GameObject newBulletHole = Instantiate(bulletHole, hits[10].point, hitRotation);
                        Destroy(newBulletHole, decalTimeToLive);
                    }
                }
                //   print(enemyHealth.hitPoints);


            }

            if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward + new Vector3(innerShotgunDifference, innerShotgunDifference, innerShotgunDifference), out hits[11], range))
            {
                //  print("we hit " + hit12.transform.name);

                if (hits[11].transform.gameObject.layer == 15 /* Ammo layer */) { }
                else
                {
                    if (hits[11].transform.tag == "Enemy" || hits[11].transform.tag == "EnemyHead")
                    {
                        GameObject newBloodVFX = Instantiate(bloodVFX, hits[11].point, Quaternion.LookRotation(hits[11].normal));
                        if (hits[11].transform.gameObject.tag == "EnemyHead")
                            hits[11].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                        else
                        {
                            hits[11].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                        }
                    }
                    else
                    {
                        if (hits[11].transform.CompareTag("Bodyparts"))
                        {
                            GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[11].point, Quaternion.LookRotation(hits[11].normal));
                        }
                        else
                        {
                            var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[11].normal);
                            GameObject newHitVFX12 = Instantiate(hitVFX, hits[11].point, Quaternion.LookRotation(hits[11].normal));
                            GameObject newBulletHole = Instantiate(bulletHole, hits[11].point, hitRotation);
                            Destroy(newBulletHole, decalTimeToLive);
                        }
                    }
                    //    print(enemyHealth.hitPoints);
                }

            }

            if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward + new Vector3(outerShotgunDifference, outerShotgunDifference, outerShotgunDifference), out hits[12], range))
            {
                // print("we hit " + hit13.transform.name);

                if (hits[12].transform.gameObject.layer == 15 /* Ammo layer */) { }
                else
                {
                    if (hits[12].transform.tag == "Enemy" || hits[12].transform.tag == "EnemyHead")
                    {
                        GameObject newBloodVFX = Instantiate(bloodVFX, hits[12].point, Quaternion.LookRotation(hits[12].normal));
                        if (hits[12].transform.gameObject.tag == "EnemyHead")
                            hits[12].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                        else
                        {
                            hits[12].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                        }
                    }
                    else
                    {
                        if (hits[12].transform.CompareTag("Bodyparts"))
                        {
                            GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[12].point, Quaternion.LookRotation(hits[12].normal));
                        }
                        else
                        {
                            var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[12].normal);
                            GameObject newHitVFX13 = Instantiate(hitVFX, hits[12].point, Quaternion.LookRotation(hits[12].normal));
                            GameObject newBulletHole = Instantiate(bulletHole, hits[12].point, hitRotation);
                            Destroy(newBulletHole, decalTimeToLive);
                        }
                        // print(enemyHealth.hitPoints);
                    }
                }

            }

            if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward - new Vector3(innerShotgunDifference, 0, innerShotgunDifference) + new Vector3(0, innerShotgunDifference, innerShotgunDifference), out hits[13], range))
            {
                //  print("we hit " + hit14.transform.name);

                if (hits[13].transform.gameObject.layer == 15 /* Ammo layer */) { }
                else
                {
                    if (hits[13].transform.tag == "Enemy" || hits[13].transform.tag == "EnemyHead")
                    {
                        GameObject newBloodVFX = Instantiate(bloodVFX, hits[13].point, Quaternion.LookRotation(hits[13].normal));
                        if (hits[13].transform.gameObject.tag == "EnemyHead")
                            hits[13].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                        else
                        {
                            hits[13].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                        }
                    }
                    else
                    {
                        if (hits[13].transform.CompareTag("Bodyparts"))
                        {
                            GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[13].point, Quaternion.LookRotation(hits[13].normal));
                        }
                        else
                        {
                            var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[13].normal);
                            GameObject newHitVFX14 = Instantiate(hitVFX, hits[13].point, Quaternion.LookRotation(hits[13].normal));
                            GameObject newBulletHole = Instantiate(bulletHole, hits[13].point, hitRotation);
                            Destroy(newBulletHole, decalTimeToLive);
                        }
                    }
                    //    print(enemyHealth.hitPoints);
                }

            }

            if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward - new Vector3(outerShotgunDifference, 0, outerShotgunDifference) + new Vector3(0, outerShotgunDifference, outerShotgunDifference), out hits[14], range))
            {
                //  print("we hit " + hit15.transform.name);

                if (hits[14].transform.gameObject.layer == 15 /* Ammo layer */) { }
                else
                {
                    if (hits[14].transform.tag == "Enemy" || hits[14].transform.tag == "EnemyHead")
                    {
                        GameObject newBloodVFX = Instantiate(bloodVFX, hits[14].point, Quaternion.LookRotation(hits[14].normal));
                        if (hits[14].transform.gameObject.tag == "EnemyHead")
                            hits[14].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                        else
                        {
                            hits[14].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                        }
                    }
                    else
                    {
                        if (hits[14].transform.CompareTag("Bodyparts"))
                        {
                            GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[14].point, Quaternion.LookRotation(hits[14].normal));
                        }
                        else
                        {
                            var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[14].normal);
                            GameObject newHitVFX15 = Instantiate(hitVFX, hits[14].point, Quaternion.LookRotation(hits[14].normal));
                            GameObject newBulletHole = Instantiate(bulletHole, hits[14].point, hitRotation);
                            Destroy(newBulletHole, decalTimeToLive);
                        }
                        //   print(enemyHealth.hitPoints);
                    }
                }

            }

            if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward + new Vector3(innerShotgunDifference, 0, innerShotgunDifference) - new Vector3(0, innerShotgunDifference, innerShotgunDifference), out hits[15], range))
            {
                // print("we hit " + hit16.transform.name);

                if (hits[15].transform.gameObject.layer == 15 /* Ammo layer */) { }
                else
                {
                    if (hits[15].transform.tag == "Enemy" || hits[15].transform.tag == "EnemyHead")
                    {
                        GameObject newBloodVFX = Instantiate(bloodVFX, hits[15].point, Quaternion.LookRotation(hits[15].normal));
                        if (hits[15].transform.gameObject.tag == "EnemyHead")
                            hits[15].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                        else
                        {
                            hits[15].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                        }
                    }
                    else
                    {
                        if (hits[15].transform.CompareTag("Bodyparts"))
                        {
                            GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[15].point, Quaternion.LookRotation(hits[15].normal));
                        }
                        else
                        {
                            var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[15].normal);
                            GameObject newHitVFX16 = Instantiate(hitVFX, hits[15].point, Quaternion.LookRotation(hits[15].normal));
                            GameObject newBulletHole = Instantiate(bulletHole, hits[15].point, hitRotation);
                            Destroy(newBulletHole, decalTimeToLive);
                        }
                    }
                }

                //  print(enemyHealth.hitPoints);


            }

            if (Physics.Raycast(FPSCamera.transform.position, FPSCamera.transform.forward + new Vector3(outerShotgunDifference, 0, outerShotgunDifference) - new Vector3(0, outerShotgunDifference, outerShotgunDifference), out hits[16], range))
            {
                // print("we hit " + hit17.transform.name);

                if (hits[16].transform.gameObject.layer == 15 /* Ammo layer */) { }
                else
                {
                    if (hits[16].transform.tag == "Enemy" || hits[16].transform.tag == "EnemyHead")
                    {
                        GameObject newBloodVFX = Instantiate(bloodVFX, hits[16].point, Quaternion.LookRotation(hits[16].normal));
                        if (hits[16].transform.gameObject.tag == "EnemyHead")
                            hits[16].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage * headShotMultiplier, hitForceOnEnemy, canDismember);
                        else
                        {
                            hits[16].transform.GetComponent<BroadcastHit>().CallTakeDamage(currentWeaponDamage, hitForceOnEnemy, canDismember);
                        }
                    }
                    else
                    {
                        if (hits[16].transform.CompareTag("Bodyparts"))
                        {
                            GameObject newBloodVFX2 = Instantiate(bloodVFX, hits[16].point, Quaternion.LookRotation(hits[16].normal));
                        }
                        else
                        {
                            var hitRotation = Quaternion.FromToRotation(Vector3.forward, hits[16].normal);
                            GameObject newHitVFX17 = Instantiate(hitVFX, hits[16].point, Quaternion.LookRotation(hits[16].normal));
                            GameObject newBulletHole = Instantiate(bulletHole, hits[16].point, hitRotation);
                            Destroy(newBulletHole, decalTimeToLive);
                        }
                        //   print(enemyHealth.hitPoints);
                    }
                }

            }


        }
    }



        IEnumerator waitTime()
        {
            readyToShoot = false;
            yield return new WaitForSeconds(timeBetweenShots);
            readyToShoot = true;
        }

        IEnumerator Wait_Automatic()
        {
            canPlayAutomaticEmptySound = false;
            yield return new WaitForSeconds(timeBetweenShots);
            canPlayAutomaticEmptySound = true;
        }

        IEnumerator AutoFire()
        {


            while (auto_Firing == true && bulletsLeftInMagazine > 0 && playerHealth.isPlayerAlive)
            {
                if (currentRaycastRecoil < maxRaycastRecoil)
                {
                    if (player_behaviour.crouching == true)
                        currentRaycastRecoil += raycastRecoil * 0.5f;
                    else
                    if (player_behaviour.running == true)
                        currentRaycastRecoil += raycastRecoil * 2f;
                    else
                        currentRaycastRecoil += raycastRecoil;
                }

                reticleRectTransform.localScale = new Vector3(oldReticleSizeX + currentRaycastRecoil * 10, oldReticleSizeY + currentRaycastRecoil * 10, 1f);
                auto_Firing = true;
                endedCouroutineForShootingAuto = false;
                Fire();
                yield return new WaitForSeconds(timeBetweenShots);
                endedCouroutineForShootingAuto = true;

            }
            currentRaycastRecoil = 0f;
            reticleRectTransform.localScale = new Vector3(oldReticleSizeX, oldReticleSizeY, 1f);
        }

        IEnumerator ReloadWeapon()
        {
            reloadTimerObject.SetActive(true);
            reloadTimer = reloadTime;
            if (reloading == false)
                transform.Rotate(transform.rotation.x - rotateWeaponWhenReloadingX, transform.rotation.y - rotateWeaponWhenReloadingY, transform.rotation.z - rotateWeaponWhenReloadingZ);
            reloaded = false;
            reloading = true;
            yield return new WaitForSeconds(reloadTimeSoundLenght);
            if (timerForAudio > 0.2f)
                audioSource.PlayOneShot(reloadWeaponSound);
            yield return new WaitForSeconds(reloadWeaponSound.length);
            if (ammoLeft >= magazineSize)
            {
                ammoLeft -= Mathf.Abs(bulletsLeftInMagazine - magazineSize);
                bulletsLeftInMagazine = magazineSize;
                magazineText.text = bulletsLeftInMagazine.ToString();
                ammoLeftText.text = "\\" + ammoLeft;
            }
            else
            {
                if (ammoLeft >= (magazineSize - bulletsLeftInMagazine))
                {
                    float usedBullets = magazineSize - bulletsLeftInMagazine;
                    bulletsLeftInMagazine += usedBullets;
                    ammoLeft -= usedBullets;
                }
                else
                {
                    float currentAmmoLeft = ammoLeft;
                    bulletsLeftInMagazine += currentAmmoLeft;
                    ammoLeft -= currentAmmoLeft;
                }

                magazineText.text = bulletsLeftInMagazine.ToString();
                ammoLeftText.text = "\\" + ammoLeft;
            }
            reloading = false;
            reloaded = true;
            transform.Rotate(transform.rotation.x + rotateWeaponWhenReloadingX, transform.rotation.y + rotateWeaponWhenReloadingY, transform.rotation.z + rotateWeaponWhenReloadingZ);
            reloadTimerObject.SetActive(false);
        }


        private void DisableLight()
        {
            muzzleFlashLight.SetActive(false);
        }

    
}
                        

                                
