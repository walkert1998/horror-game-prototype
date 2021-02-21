using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCQuip : MonoBehaviour
{

    [Header("Vocal Quips")]
    public AudioClip targetSightedQuip;
    public AudioClip attackQuip;
    public AudioClip outOfRangeQuip;
    public AudioClip painQuip;
    public AudioClip deathQuip;
    public AudioClip targetDownQuip;
    public AudioClip allClearQuip;
    public AudioClip lastPlayedQuip;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quip(AudioClip quip, bool overrideQuip=false)
    {
        if (overrideQuip)
        {
            StartCoroutine(PlaySound(quip));
        }
        else if (!source.isPlaying && lastPlayedQuip != quip)
        {
            StartCoroutine(PlaySound(quip));
        }
    }

    IEnumerator PlaySound(AudioClip clip)
    {
        source.Stop();
        source.clip = clip;
        lastPlayedQuip = clip;
        source.Play();
        yield return new WaitForSeconds(clip.length);
    }
}
