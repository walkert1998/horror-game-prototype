using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISFX : MonoBehaviour
{
    private static UISFX instance;
    private AudioSource source;
    public AudioClip hoverSound;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        instance = this;
    }

    private void PlayHoverSound()
    {
        source.PlayOneShot(hoverSound, 0.1f);
    }

    public static void PlayHoverSound_Static()
    {
        instance.PlayHoverSound();
    }
}
