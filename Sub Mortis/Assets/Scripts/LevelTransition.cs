using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LevelTransition : MonoBehaviour
{
    public string level_name;
    public VideoPlayer player;
	// Use this for initialization
	void Start () {
        if (player != null)
            StartCoroutine(PlayVideo());
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void change_level(string scene)
    {
        //Debug.Log(player.isPlaying);
        SceneManager.LoadScene("Scenes/" + scene);
    }

    IEnumerator PlayVideo()
    {
        yield return new WaitForSeconds((float)player.clip.length);
        change_level(level_name);
    }

    public string scene_name
    {
        get
        {
            return level_name;
        }
    }
}
