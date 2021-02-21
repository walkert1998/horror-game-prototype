using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSequence : MonoBehaviour
{
    public AudioClip doorSound;
    public AudioSource source;
    public void LoadLevel()
    {
        StartCoroutine(PlaySequence());
    }
    IEnumerator PlaySequence()
    {
        PlayerInteraction.LockInteraction();
        TransitionScreen.LockPlayer_Static();
        TransitionScreen.HideUIElements_Static();
        TransitionScreen.FadeOut_Static();
        source.PlayOneShot(doorSound);
        yield return new WaitForSeconds(2.0f);
        GetComponent<LevelTransition>().change_level("EndScreen");
    }
}
