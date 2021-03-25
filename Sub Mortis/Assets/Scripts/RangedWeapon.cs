using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
//using UnityStandardAssets.Characters.FirstPerson;

[CreateAssetMenu(menuName = "Items/Create Ranged Weapon")]
public class RangedWeapon : Item
{
    public int damage = 0;
    [Header("Timing")]
    public float reloadTime = 1.5f;
    public float timeBetweenShots = 1.0f;
    public float nextShotTime = 1.0f;
    [Header("Recoil")]
    public float recoil;
    public float base_recoil;
    public float recoil_x;
    public float recoil_y;
    [Header("States")]
    public bool reloading = false;
    public bool shooting = false;
    public bool holstered = false;
    public bool aiming = false;
    public bool running = false;
    [Header("Ammo")]
    public int clipSize = 1;
    public int currentAmmo;
    public int ammoReserve;
    public List<Ammo> ammoTypes;
    public Ammo loadedAmmoType;
    public bool canBeReloaded = true;
    [Header("Weapon Modifications")]
    public List<WeaponModification> availableModifications;
    public List<WeaponModification> installedModifications;
    //protected MouseLook mouseLook;
    //protected FirstPersonController controller;
    //MuzzleFlash muzzleFlash;
    [Header("Weapon Model")]
    public bool twoHanded = false;
    public GameObject gameModel;
    public Animator animator;
    public Vector3 aimPosition;
    [TextArea]
    public string effectLightPath;

    private GameObject main_camera;
    [Header("Misc")]
    public Inventory playerInventory;
    [Tooltip("For defining weapons like a flamethrower")]
    public bool streamWeapon = false;
    //public GameSettings gameSettings;

    [Header("Sound Effects")]
    public AudioClip gunshotSound;
    public AudioClip unSilencedSound;
    public AudioClip silencedSound;
    public AudioClip weaponDrawSound;
    public AudioClip noAmmoSound;
    public AudioClip reloadSound;
    AudioSource source;
    //Sleep playerSleep;
    //public AmbienceController ambienceController;
    //public PauseMenu pauseMenu;

    [Header("Impact Textures")]
    public GameObject woodBulletImpact;
    public GameObject metalBulletImpact;
    public GameObject bloodBulletImpact;
    public ParticleSystem gunSmoke;

    IEnumerator Aim()
    {
        aiming = true;
        animator.SetBool("Aiming", true);
        yield return new WaitForSeconds(0.0f);
    }

    IEnumerator UnAim()
    {
        aiming = false;
        animator.SetBool("Aiming", false);
        yield return new WaitForSeconds(0.0f);
    }

    IEnumerator Reload()
    {
        Debug.Log("Reloading");
        reloading = true;
        holstered = false;
        animator.SetBool("Reloading", true);
        //StartCoroutine(PlayAudio(reloadSound));
        yield return new WaitForSeconds(reloadTime);
        int difference = clipSize - currentAmmo;
        
        if (ammoReserve >= difference)
        {
            ammoReserve = ammoReserve - difference;
            currentAmmo += difference;
            //controller.GetComponent<Inventory>().RemoveAmmo(ammoType, difference);
        }

        else
        {
            currentAmmo += ammoReserve;
            //controller.GetComponent<Inventory>().RemoveAmmo(ammoType, ammo_reserve);
            ammoReserve = 0;
        }
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.6f);

        reloading = false;
    }

    IEnumerator Run()
    {
        running = true;
        animator.SetBool("Running", true);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator GunSmoke()
    {
        var emission = gunSmoke.emission;
        gunSmoke.Play();
        emission.enabled = true;
        yield return new WaitForSeconds(0.5f);
        emission.enabled = false;
        gunSmoke.Stop();
    }

    public void Shoot()
    {
        shooting = true;
        if (currentAmmo > 0)
        {
            /*
            if (playerSleep.currentSleep <= 25)
                recoil = 30;
            else if (playerSleep.currentSleep <= 50)
                recoil = 20;
            else if (playerSleep.currentSleep <= 75)
                recoil = 10;
            else
                recoil = 5;
                */
            //StartCoroutine(PlayAudio(gunshotSound));
            //ambienceController.StopAmbience();
            //if (gunSmoke != null && gameSettings.GetGunSmoke())
            //    StartCoroutine(GunSmoke());
            RaycastHit shot;
            if (Physics.Raycast(main_camera.transform.position, main_camera.transform.forward, out shot))
            {
                shot.transform.SendMessage("damage_npc", damage, SendMessageOptions.DontRequireReceiver);
                Debug.Log(shot.transform.gameObject);
                /*
                GameObject.Find("Bigfoot").transform.SendMessage("HiddenShot", transform.position, SendMessageOptions.DontRequireReceiver);
                if (shot.transform.tag == "Metal")
                    Instantiate(metalBulletImpact, new Vector3(shot.point.x + 0.01f, shot.point.y + 0.01f, shot.point.z + 0.01f), Quaternion.FromToRotation(Vector3.up, shot.normal));
                else if (shot.transform.tag == "Wood")
                    Instantiate(woodBulletImpact, new Vector3(shot.point.x + 0.01f, shot.point.y + 0.01f, shot.point.z + 0.01f), Quaternion.FromToRotation(Vector3.up, shot.normal));
                else if (shot.transform.gameObject.GetComponent<EnemyController>())
                    Instantiate(bloodBulletImpact, new Vector3(shot.point.x + 0.01f, shot.point.y + 0.01f, shot.point.z + 0.01f), Quaternion.FromToRotation(Vector3.up, shot.normal));
                    */
                //Destroy(shot.transform.gameObject, 50f);
            }
            //animator.SetTrigger("Shoot");
            //weaponAnimator.SetTrigger("Shoot");
            //leftHandAnimator.SetTrigger("Shoot");
            //rightHandAnimator.SetTrigger("Shoot");
            //muzzleFlash.flash_on();
            currentAmmo--;
            //Debug.Log(current_ammo);
            //controller.GetMouseLook().Recoil(recoil);
            //playerSleep.RestoreSleep(5);
            //ambienceController.StartCoroutine(ambienceController.FadeInSounds());
        }
        //else
        //    StartCoroutine(PlayAudio(noAmmoSound));
        shooting = false;
    }

    public void add_ammo(int amount)
    {
        ammoReserve += amount;
    }

    public void InstallMod(WeaponModification mod)
    {
        if (availableModifications.Contains(mod))
        {
            installedModifications.Add(mod);
            if (mod.modificationType == ModificationType.AmmoIncrease)
            {
                clipSize += mod.value;
            }
            else if (mod.modificationType == ModificationType.DamageIncrease)
            {
                damage += mod.value;
            }
            else if (mod.modificationType == ModificationType.Silencer)
            {
                gunshotSound = silencedSound;
            }
        }
    }

    public WeaponModification UnInstallMod(WeaponModification mod)
    {
        if (installedModifications.Contains(mod))
        {
            installedModifications.Remove(mod);
            if (mod.modificationType == ModificationType.AmmoIncrease)
            {
                clipSize -= mod.value;
            }
            else if (mod.modificationType == ModificationType.DamageIncrease)
            {
                damage -= mod.value;
            }
            else if (mod.modificationType == ModificationType.Silencer)
            {
                gunshotSound = unSilencedSound;
            }
            Debug.Log("Uninstalled " + mod.itemName);
            return mod;
        }
        return null;
    }

    IEnumerator PlayAudio(AudioClip soundEffect)
    {
        source.PlayOneShot(soundEffect);
        yield break;
    }

    IEnumerator Holster()
    {
        holstered = true;
        animator.SetBool("Holstered", true);
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator UnHolster()
    {
        holstered = false;
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("Holstered", false);
    }
}
