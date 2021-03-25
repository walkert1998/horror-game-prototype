using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Examination : MonoBehaviour
{
    public TMP_Text examineText;
    public TMP_Text popupText;
    public FirstPersonController controller;
    private static Examination instance;
    public GameObject blurUI;
    public bool neverExaminedBefore;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        blurUI.SetActive(false);
        examineText.text = "";
        popupText.text = "";
        neverExaminedBefore = true;
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
        if (neverExaminedBefore)
        {
            text += "\n[Press any key to close the examine view]";
        }
        StartCoroutine(DisplayTextUntilClick(text));
    }

    public static void SetExamineTextUntilClick_static(string text)
    {
        instance.SetExamineTextUntilClick(text);
    }

    private IEnumerator DisplayTextOverTime(string text, float seconds)
    {
        popupText.text = text;
        while (popupText.alpha < 1.0f)
        {
            popupText.alpha = Mathf.Lerp(popupText.alpha, popupText.alpha + 1.0f, Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(seconds);
        while (popupText.alpha > 0)
        {
            popupText.alpha = Mathf.Lerp(popupText.alpha, popupText.alpha - 1.0f, Time.deltaTime);
            yield return null;
        }
        popupText.text = "";
    }

    private IEnumerator DisplayTextUntilClick(string text)
    {
        //Debug.Log(text);
        DynamicCursor.HideCursor_Static();
        controller.m_CanMove = false;
        controller.m_CanLook = false;
        //blurUI.SetActive(true);
        PlayerInteraction.LockInteraction();
        examineText.text = text;
        //while (examineText.alpha < 1.0f)
        //{
        //    examineText.alpha = Mathf.Lerp(examineText.alpha, examineText.alpha + 1.0f, 3 * Time.deltaTime);
        //    Debug.Log(examineText.alpha);
        //    yield return null;
        //}
        yield return new WaitForSeconds(0.1f);
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        neverExaminedBefore = false;
        //blurUI.SetActive(false);
        PlayerInteraction.UnlockInteraction();
        DynamicCursor.ShowCursor_Static();
        //while (examineText.alpha > 0)
        //{
        //    examineText.alpha = Mathf.Lerp(examineText.alpha, examineText.alpha - 1.0f, 3 * Time.deltaTime);
        //    yield return null;
        //}
        examineText.text = "";
        controller.m_CanMove = true;
        controller.m_CanLook = true;
    }
}
