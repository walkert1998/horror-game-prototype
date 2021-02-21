using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Examination : MonoBehaviour
{
    public TMP_Text examineText;
    public FirstPersonController controller;
    private static Examination instance;
    public GameObject blurUI;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        blurUI.SetActive(false);
        examineText.text = "";
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void SetExamineTextTimed(string text, float seconds)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayTextOverTime(text, seconds));
    }

    public static void SetExamineTextTimed_static(string text, float seconds)
    {
        instance.SetExamineTextTimed(text, seconds);
    }

    private void SetExamineTextUntilClick(string text)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayTextUntilClick(text));
    }

    public static void SetExamineTextUntilClick_static(string text)
    {
        instance.SetExamineTextUntilClick(text);
    }

    private IEnumerator DisplayTextOverTime(string text, float seconds)
    {
        examineText.text = text;
        yield return new WaitForSeconds(seconds);
        examineText.text = "";
    }

    private IEnumerator DisplayTextUntilClick(string text)
    {
        //Debug.Log(text);
        DynamicCursor.HideCursor_Static();
        controller.m_CanMove = false;
        controller.m_CanLook = false;
        //blurUI.SetActive(true);
        examineText.text = text;
        PlayerInteraction.LockInteraction();
        yield return new WaitForSeconds(0.1f);
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        examineText.text = "";
        //blurUI.SetActive(false);
        PlayerInteraction.UnlockInteraction();
        DynamicCursor.ShowCursor_Static();
        controller.m_CanMove = true;
        controller.m_CanLook = true;
    }
}
