using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponSwitchManager : MonoBehaviour
{
    [SerializeField] GameObject reticle;
    [SerializeField] GameObject bulletsLeft;
    [SerializeField] GameObject ammoLeft;

    [SerializeField] bool canSwitchToMelee = true;
    [SerializeField] bool canSwitchToPistol = false;
    [SerializeField] bool canSwitchToSMG = false;
    [SerializeField] bool canSwitchToSemiAuto = false;
    [SerializeField] bool canSwitchToAK47 = false;
    [SerializeField] bool canSwitchToSniper = false;
    [SerializeField] bool canSwitchToLMG = false;
    [SerializeField] bool canSwitchToShotgun = false;
    [SerializeField] bool canSwitchToEnfield = false;
    [SerializeField] bool canSwitchToAT = false;
    Weapon[] allWeapons = new Weapon[9];
   /* [HideInInspector] */ public bool canUseWeapon = true;

    int currentWeapon = -1;
    Weapon lastUsedWeapon;
    MeleeWeapon lastUsedWep_Sword;

    [SerializeField] GameObject meleeWeapon;
    [SerializeField] GameObject Pistol;
    [SerializeField] GameObject SMG;
    [SerializeField] GameObject SemiAuto;
    [SerializeField] GameObject AK47;
    [SerializeField] GameObject Sniper;
    [SerializeField] GameObject LMG;
    [SerializeField] GameObject Shotgun;
    [SerializeField] GameObject Enfield;
    [SerializeField] GameObject AT;
        

    [SerializeField] GameObject meleeWeaponImage;
    [SerializeField] GameObject Weapon1Image;
    [SerializeField] GameObject Weapon2Image;
    [SerializeField] GameObject Weapon3Image;
    [SerializeField] GameObject Weapon4Image;
    [SerializeField] GameObject Weapon5Image;
    [SerializeField] GameObject Weapon6Image;
    [SerializeField] GameObject Weapon7Image;
    [SerializeField] GameObject Weapon8Image;
    [SerializeField] GameObject Weapon9Image;

    PlayerHealth playerHealth;
    AudioSource audioSource;
    [SerializeField]AudioClip drawSwordSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        allWeapons = Resources.FindObjectsOfTypeAll<Weapon>();
    }

    
    void Update()
    {
        if (playerHealth.isPlayerAlive && playerHealth.isGamePaused == false)
        {
            IconManagement();
            SwitchManagement();
        }
    }

    private void SwitchManagement()
    {
        if (canUseWeapon == true)
        {
            DrawSword();
            EquipPistol();
            EquipSMG();
            EquipBattleRifle();
            EquipAssaultRifle();
            EquipSniperRifle();
            EquipMachineGun();
            EquipShotgun();
            EquipRifle();
            EquipHEGun();
        }
    }

    private void EquipHEGun()
    {
        if (Input.GetButtonDown("HE gun") && canSwitchToAT && currentWeapon != 9)
        {
            currentWeapon = 9;
            meleeWeapon.SetActive(false);
            Pistol.SetActive(false);
            SMG.SetActive(false);
            SemiAuto.SetActive(false);
            AK47.SetActive(false);
            Sniper.SetActive(false);
            LMG.SetActive(false);
            Shotgun.SetActive(false);
            Enfield.SetActive(false);
            AT.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
    }

    private void EquipRifle()
    {
        if (Input.GetButtonDown("Rifle") && canSwitchToEnfield && currentWeapon != 8)
        {
            currentWeapon = 8;
            meleeWeapon.SetActive(false);
            Pistol.SetActive(false);
            SMG.SetActive(false);
            SemiAuto.SetActive(false);
            AK47.SetActive(false);
            Sniper.SetActive(false);
            LMG.SetActive(false);
            Shotgun.SetActive(false);
            AT.SetActive(false);
            Enfield.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
    }

    private void EquipShotgun()
    {
        if (Input.GetButtonDown("Shotgun") && canSwitchToShotgun && currentWeapon != 7)
        {
            currentWeapon = 7;
            meleeWeapon.SetActive(false);
            Pistol.SetActive(false);
            SMG.SetActive(false);
            SemiAuto.SetActive(false);
            AK47.SetActive(false);
            Sniper.SetActive(false);
            LMG.SetActive(false);
            Enfield.SetActive(false);
            AT.SetActive(false);
            Shotgun.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
    }

    private void EquipMachineGun()
    {
        if (Input.GetButtonDown("Machine gun") && canSwitchToLMG && currentWeapon != 6)
        {
            currentWeapon = 6;
            meleeWeapon.SetActive(false);
            Pistol.SetActive(false);
            SMG.SetActive(false);
            SemiAuto.SetActive(false);
            AK47.SetActive(false);
            Sniper.SetActive(false);
            Shotgun.SetActive(false);
            Enfield.SetActive(false);
            AT.SetActive(false);
            LMG.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
    }

    private void EquipSniperRifle()
    {
        if (Input.GetButtonDown("Sniper rifle") && canSwitchToSniper && currentWeapon != 5)
        {
            currentWeapon = 5;
            meleeWeapon.SetActive(false);
            Pistol.SetActive(false);
            SMG.SetActive(false);
            SemiAuto.SetActive(false);
            AK47.SetActive(false);
            LMG.SetActive(false);
            Shotgun.SetActive(false);
            Enfield.SetActive(false);
            AT.SetActive(false);
            Sniper.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
    }

    private void EquipAssaultRifle()
    {
        if (Input.GetButtonDown("Assault rifle") && canSwitchToAK47 && currentWeapon != 4)
        {
            currentWeapon = 4;
            meleeWeapon.SetActive(false);
            Pistol.SetActive(false);
            SMG.SetActive(false);
            SemiAuto.SetActive(false);
            Sniper.SetActive(false);
            LMG.SetActive(false);
            Shotgun.SetActive(false);
            Enfield.SetActive(false);
            AT.SetActive(false);
            AK47.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
    }

    private void EquipBattleRifle()
    {
        if (Input.GetButtonDown("Battle rifle") && canSwitchToSemiAuto && currentWeapon != 3)
        {
            currentWeapon = 3;
            meleeWeapon.SetActive(false);
            Pistol.SetActive(false);
            SMG.SetActive(false);
            AK47.SetActive(false);
            Sniper.SetActive(false);
            LMG.SetActive(false);
            Shotgun.SetActive(false);
            Enfield.SetActive(false);
            AT.SetActive(false);
            SemiAuto.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
    }

    private void EquipSMG()
    {
        if (Input.GetButtonDown("SMG") && canSwitchToSMG && currentWeapon != 2)
        {
            currentWeapon = 2;
            meleeWeapon.SetActive(false);
            Pistol.SetActive(false);
            SemiAuto.SetActive(false);
            AK47.SetActive(false);
            Sniper.SetActive(false);
            LMG.SetActive(false);
            Shotgun.SetActive(false);
            Enfield.SetActive(false);
            AT.SetActive(false);
            SMG.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
    }

    private void EquipPistol()
    {
        if (Input.GetButtonDown("Pistol") && canSwitchToPistol && currentWeapon != 1)
        {
            currentWeapon = 1;
            meleeWeapon.SetActive(false);
            SMG.SetActive(false);
            SemiAuto.SetActive(false);
            AK47.SetActive(false);
            Sniper.SetActive(false);
            LMG.SetActive(false);
            Shotgun.SetActive(false);
            Enfield.SetActive(false);
            AT.SetActive(false);
            Pistol.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
    }

    private void DrawSword()
    {
        if (Input.GetButtonDown("Sword") && canSwitchToMelee && currentWeapon != 0)
        {
            currentWeapon = 0;
            Pistol.SetActive(false);
            SMG.SetActive(false);
            SemiAuto.SetActive(false);
            AK47.SetActive(false);
            Sniper.SetActive(false);
            LMG.SetActive(false);
            Shotgun.SetActive(false);
            Enfield.SetActive(false);
            AT.SetActive(false);
            meleeWeapon.SetActive(true);
            audioSource.PlayOneShot(drawSwordSound);
           // reticle.SetActive(false);
            bulletsLeft.SetActive(false);
            ammoLeft.SetActive(false);
            reticle.SetActive(true);

        }
    }

    void IconManagement()
    {
        if (canSwitchToMelee)
        {
            meleeWeaponImage.SetActive(true);
        }
        else
        {
            meleeWeaponImage.SetActive(false);
        }

        if (canSwitchToPistol)
        {
            Weapon1Image.SetActive(true);
        }
        else
        {
            Weapon1Image.SetActive(false);
        }

        if (canSwitchToSMG)
        {
            Weapon2Image.SetActive(true);
        }
        else
        {
            Weapon2Image.SetActive(false);
        }

        if (canSwitchToSemiAuto)
        {
            Weapon3Image.SetActive(true);
        }
        else
        {
            Weapon3Image.SetActive(false);
        }

        if (canSwitchToAK47)
        {
            Weapon4Image.SetActive(true);
        }
        else
        {
            Weapon4Image.SetActive(false);
        }

        if (canSwitchToSniper)
        {
            Weapon5Image.SetActive(true);
        }
        else
        {
            Weapon5Image.SetActive(false);
        }

        if (canSwitchToLMG)
        {
            Weapon6Image.SetActive(true);
        }
        else
        {
            Weapon6Image.SetActive(false);
        }

        if (canSwitchToShotgun)
        {
            Weapon7Image.SetActive(true);
        }
        else
        {
            Weapon7Image.SetActive(false);
        }

        if (canSwitchToEnfield)
        {
            Weapon8Image.SetActive(true);
        }
        else
        {
            Weapon8Image.SetActive(false);
        }

        if (canSwitchToAT)
        {
            Weapon9Image.SetActive(true);
        }
        else
        {
            Weapon9Image.SetActive(false);
        }
    }

    public void ItemPickup(int itemNumber)
    {
        if (playerHealth.isPlayerAlive == false) return;
        switch(itemNumber)
        {
            case 0:
                canSwitchToMelee = true;
                audioSource.PlayOneShot(drawSwordSound);
                break;
            case 1:
                if(canSwitchToPistol == true)
                {
                    Pistol.GetComponent<Weapon>().ammoLeft += Pistol.GetComponent<Weapon>().magazineSize;
                    if (Pistol.activeSelf == true)
                        Pistol.GetComponent<Weapon>().RefreshMaxAmmo();
                }
                canSwitchToPistol = true;
                audioSource.Play();
                break;
            case 2:
                if (canSwitchToSMG == true)
                {
                    SMG.GetComponent<Weapon>().ammoLeft += SMG.GetComponent<Weapon>().magazineSize;
                    if (SMG.activeSelf == true)
                        SMG.GetComponent<Weapon>().RefreshMaxAmmo();
                }
                canSwitchToSMG = true;
                audioSource.Play();
                break;
            case 3:
                if (canSwitchToSemiAuto == true)
                {
                    SemiAuto.GetComponent<Weapon>().ammoLeft += SemiAuto.GetComponent<Weapon>().magazineSize;
                    if (SemiAuto.activeSelf == true)
                        SemiAuto.GetComponent<Weapon>().RefreshMaxAmmo();
                }
                canSwitchToSemiAuto = true;
                audioSource.Play();
                break;
            case 4:
                if (canSwitchToAK47 == true)
                {
                    AK47.GetComponent<Weapon>().ammoLeft += AK47.GetComponent<Weapon>().magazineSize;
                    if (AK47.activeSelf == true)
                        AK47.GetComponent<Weapon>().RefreshMaxAmmo();
                }
                canSwitchToAK47 = true;
                audioSource.Play();
                break;
            case 5:
                if (canSwitchToSniper == true)
                {
                    Sniper.GetComponent<Weapon>().ammoLeft += Sniper.GetComponent<Weapon>().magazineSize;
                    if (Sniper.activeSelf == true)
                        Sniper.GetComponent<Weapon>().RefreshMaxAmmo();
                }
                canSwitchToSniper = true;
                audioSource.Play();
                break;
            case 6:
                if (canSwitchToLMG == true)
                {
                    LMG.GetComponent<Weapon>().ammoLeft += LMG.GetComponent<Weapon>().magazineSize;
                    if (LMG.activeSelf == true)
                        LMG.GetComponent<Weapon>().RefreshMaxAmmo();
                }
                canSwitchToLMG = true;
                audioSource.Play();
                break;
            case 7:
                if (canSwitchToShotgun == true)
                {
                    Shotgun.GetComponent<Weapon>().ammoLeft += Shotgun.GetComponent<Weapon>().magazineSize;
                    if (Shotgun.activeSelf == true)
                        Shotgun.GetComponent<Weapon>().RefreshMaxAmmo();
                }
                canSwitchToShotgun = true;
                audioSource.Play();
                break;
            case 8:
                if (canSwitchToEnfield == true)
                {
                    Enfield.GetComponent<Weapon>().ammoLeft += Enfield.GetComponent<Weapon>().magazineSize;
                    if (Enfield.activeSelf == true)
                        Enfield.GetComponent<Weapon>().RefreshMaxAmmo();
                }
                canSwitchToEnfield = true;
                audioSource.Play();
                break;
            case 9:
                if (canSwitchToAT == true)
                {
                    AT.GetComponent<Weapon>().ammoLeft += AT.GetComponent<Weapon>().magazineSize;
                    if (AT.activeSelf == true)
                        AT.GetComponent<Weapon>().RefreshMaxAmmo();
                }
                canSwitchToAT = true;
                audioSource.Play();
                break;

        }
    }

    public void AmmoPickup(int itemNumber)
    {
        switch (itemNumber)
        {
            case 1:
                Pistol.GetComponent<Weapon>().ammoLeft += Pistol.GetComponent<Weapon>().magazineSize;
                if(Pistol.activeSelf == true)
                Pistol.GetComponent<Weapon>().RefreshMaxAmmo();
                audioSource.Play();
                break;
            case 2:
                SMG.GetComponent<Weapon>().ammoLeft += SMG.GetComponent<Weapon>().magazineSize;
                if (SMG.activeSelf == true)
                    SMG.GetComponent<Weapon>().RefreshMaxAmmo();
                audioSource.Play();
                break;
            case 3:
                SemiAuto.GetComponent<Weapon>().ammoLeft += SemiAuto.GetComponent<Weapon>().magazineSize;
                if (SemiAuto.activeSelf == true)
                    SemiAuto.GetComponent<Weapon>().RefreshMaxAmmo();
                audioSource.Play();
                break;
            case 4:
                AK47.GetComponent<Weapon>().ammoLeft += AK47.GetComponent<Weapon>().magazineSize;
                if (AK47.activeSelf == true)
                    AK47.GetComponent<Weapon>().RefreshMaxAmmo();
                audioSource.Play();
                break;
            case 5:
                Sniper.GetComponent<Weapon>().ammoLeft += Sniper.GetComponent<Weapon>().magazineSize;
                if (Sniper.activeSelf == true)
                    Sniper.GetComponent<Weapon>().RefreshMaxAmmo();
                audioSource.Play();
                break;
            case 6:
                LMG.GetComponent<Weapon>().ammoLeft += LMG.GetComponent<Weapon>().magazineSize;
                if (LMG.activeSelf == true)
                    LMG.GetComponent<Weapon>().RefreshMaxAmmo();
                audioSource.Play();
                break;
            case 7:
                Shotgun.GetComponent<Weapon>().ammoLeft += Shotgun.GetComponent<Weapon>().magazineSize;
                if (Shotgun.activeSelf == true)
                    Shotgun.GetComponent<Weapon>().RefreshMaxAmmo();
                audioSource.Play();
                break;
            case 8:
                Enfield.GetComponent<Weapon>().ammoLeft += Enfield.GetComponent<Weapon>().magazineSize;
                if (Enfield.activeSelf == true)
                    Enfield.GetComponent<Weapon>().RefreshMaxAmmo();
                audioSource.Play();
                break;
            case 9:
                AT.GetComponent<Weapon>().ammoLeft += AT.GetComponent<Weapon>().magazineSize;
                if (AT.activeSelf == true)
                    AT.GetComponent<Weapon>().RefreshMaxAmmo();
                audioSource.Play();
                break;

        }
    }

    public void DeequipWeapon()
    {
        if (FindObjectOfType<Weapon>() != null)
        {
            lastUsedWeapon = FindObjectOfType<Weapon>();
        }
        else if(FindObjectOfType<MeleeWeapon>() != null)
        {
            lastUsedWep_Sword = FindObjectOfType<MeleeWeapon>();
        }

       foreach (Weapon weapon in allWeapons)
        {
            weapon.gameObject.SetActive(false);
        }
       if(lastUsedWep_Sword != null)
        lastUsedWep_Sword.gameObject.SetActive(false);
        bulletsLeft.SetActive(false);
        ammoLeft.SetActive(false);
        canUseWeapon = false;
    }

    public void ReequipWeapon()
    {
        if (lastUsedWeapon != null)
        {
            lastUsedWeapon.gameObject.SetActive(true);
            playerHealth.SetCurrentWeapon();
            audioSource.Play();
        }
        else if (lastUsedWep_Sword != null)
        {
            audioSource.PlayOneShot(drawSwordSound);
            lastUsedWep_Sword.gameObject.SetActive(true);
        }
       
        canUseWeapon = true;
    }
}
