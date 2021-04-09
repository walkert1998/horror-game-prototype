using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    public float musicMaxVolume = 0.1f;
    public float fadeInSpeed;
    public List<AudioClip> combatMusic;
    //public AudioClip cabinMusic;
    public AudioClip chaseMusic;
    public AudioClip searchMusic;
    public AudioClip ambientMusic;
    public List<AudioClip> genericMusic;
    //AmbienceController ambienceController;
    // Start is called before the first frame update
    void Start()
    {
        musicSource.volume = musicMaxVolume;
        //ambienceController = GetComponent<AmbienceController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            CombatMusic();
    }

    public void StopMusic()
    {
        StopAllCoroutines();
    }

    public IEnumerator FadeOutMusic ()
    {
        Debug.Log("Stopping music");
        while (musicSource.volume > 0)
        {
            musicSource.volume -= fadeInSpeed;
            yield return new WaitForSeconds(0.1f);
        }
        musicSource.Stop();
    }

    public void CombatMusic()
    {
        //ambienceController.StopAmbience();
        int n = Random.Range(1, combatMusic.Count);
        musicSource.clip = combatMusic[n];
        musicSource.Play();
        // move picked sound to index 0 so it's not picked next time
        combatMusic[n] = combatMusic[0];
        combatMusic[0] = musicSource.clip;
        StartCoroutine(FadeInMusic());
    }

    public void CabinMusic()
    {
        //musicSource.clip = cabinMusic;
        StartCoroutine(FadeInMusic());
    }

    public void GenericMusic()
    {
        StartCoroutine(FadeOutMusic());
        int n = Random.Range(1, genericMusic.Count);
        musicSource.clip = genericMusic[n];
        musicSource.Play();
        // move picked sound to index 0 so it's not picked next time
        genericMusic[n] = genericMusic[0];
        genericMusic[0] = musicSource.clip;
        StartCoroutine(FadeInMusic());
    }



    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
        StartCoroutine(FadeInMusic());
    }

    public IEnumerator PlaySongTransition(AudioClip transition)
    {
        musicSource.Stop();
        musicSource.clip = transition;
        musicSource.Play();
        yield return new WaitForSeconds(transition.length);
    }

    public IEnumerator FadeInMusic()
    {
        musicSource.volume = 0;
        musicSource.Play();
        while (musicSource.volume < musicMaxVolume)
        {
            musicSource.volume += fadeInSpeed;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
