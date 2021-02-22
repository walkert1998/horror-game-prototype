using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhonePickup : MonoBehaviour
{
    public GameObject phone;
    public AudioSource source;
    public AudioClip vibrateSound;
    // Start is called before the first frame update
    void Start()
    {
        phone.SetActive(false);
        PlayVibrate();
    }

    //void Update()
    //{
    //}

    public void PickupPhone()
    {
        phone.SetActive(true);
        InventoryManager.ActivatePhone_Static();
        Destroy(this);
    }

    public void PlayVibrate()
    {
        StartCoroutine(Vibrate());
    }

    IEnumerator Vibrate()
    {
        while (!phone.activeSelf)
        {
            yield return new WaitForSeconds(1.0f);
            source.PlayOneShot(vibrateSound);
        }
    }
}
