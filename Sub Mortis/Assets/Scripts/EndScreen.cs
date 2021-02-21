using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text text3;
    public Image image;
    public bool running = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeItemIn());
    }

    IEnumerator FadeItemIn ()
    {
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            text1.color = new Color(text1.color.r, text1.color.g, text1.color.b, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);
        alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            text2.color = new Color(text2.color.r, text2.color.g, text2.color.b, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);
        alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);
        alpha = 0;
        while (alpha < 1)
        {
            alpha += 0.1f;
            text3.color = new Color(text3.color.r, text3.color.g, text3.color.b, alpha);
            yield return null;
        }
        while(!Input.anyKeyDown)
        {
            yield return null;
        }
        GetComponent<LevelTransition>().change_level("MainMenu");
    }
}
