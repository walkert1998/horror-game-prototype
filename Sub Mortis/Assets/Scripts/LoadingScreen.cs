using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingBar;
    public TMP_Text loadingText;
    public string levelToLoad;
    // Start is called before the first frame update
    void Start()
    {
        //loadingText.text = "Loading...";
        LoadLevel(levelToLoad);
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadAsync(levelName));
    }

    IEnumerator LoadAsync (string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.value = progress;

            yield return null;
        }
        //loadingText.text = "Click To Continue";
    }
}
