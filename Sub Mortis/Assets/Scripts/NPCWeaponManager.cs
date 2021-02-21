using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWeaponManager : MonoBehaviour
{
    public int weaponDamage;
    public float timeBetweenShots;
    public int clipSize;
    public float reloadTime;
    public AudioClip gunshotSound;
    public int currentAmmo;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = clipSize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireWeapon()
    {
        GetComponent<NPCAI>().source.PlayOneShot(gunshotSound);
        GetComponent<NPCAI>().animator.Play("Shoot");
        RaycastHit hit;
        if (Physics.Raycast(GetComponent<NPCAI>().visionTransform.position, GetComponent<NPCAI>().visionTransform.forward * GetComponent<NPCAI>().attackRange, out hit))
        {
            Debug.DrawRay(GetComponent<NPCAI>().visionTransform.position, GetComponent<NPCAI>().visionTransform.forward * 10, Color.blue);
            if (!GetComponent<NPCColliders>().otherColliders.Contains(hit.collider))
            {
                hit.transform.SendMessage("DamageCharacter", weaponDamage, SendMessageOptions.DontRequireReceiver);
            }
        }
        currentAmmo--;
    }

    public void Reload()
    {
        GetComponent<NPCAI>().animator.Play("Reload");
        currentAmmo = clipSize;
    }
}
