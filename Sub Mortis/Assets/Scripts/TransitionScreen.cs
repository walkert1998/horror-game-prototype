﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class TransitionScreen : MonoBehaviour
{
    private static TransitionScreen instance;
    public bool fadingOut;
    public bool fadingIn;
    Image screen;
    Color invisible;
    Color visible;
    public FirstPersonController controller;
    public List<GameObject> uiElements;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        instance.screen = GetComponent<Image>();
        instance.invisible = new Color(screen.color.r, screen.color.g, screen.color.b, 0);
        instance.visible = new Color(screen.color.r, screen.color.g, screen.color.b, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingOut && screen.color.a < 1)
        {
            instance.screen.color = Color.Lerp(screen.color, visible, 3 * Time.deltaTime);
        }
        else if (fadingIn && screen.color.a > 0)
        {
            instance.screen.color = Color.Lerp(screen.color, invisible, 3 * Time.deltaTime);
        }
    }

    public void HideUIElements()
    {
        foreach (GameObject element in uiElements)
        {
            element.SetActive(false);
        }
    }

    public void ShowUIElements()
    {
        foreach (GameObject element in uiElements)
        {
            element.SetActive(true);
        }
    }

    public void LockPlayer()
    {
        controller.m_CanMove = false;
        controller.m_CanLook = false;
    }

    public void UnlockPlayer()
    {
        controller.m_CanMove = true;
        controller.m_CanLook = true;
    }

    public void FadeOut()
    {
        fadingOut = true;
        fadingIn = false;
    }

    public void FadeIn()
    {
        fadingOut = false;
        fadingIn = true;
    }

    public static void FadeOut_Static()
    {
        instance.FadeOut();
    }

    public static void FadeIn_Static()
    {
        instance.FadeIn();
    }

    public static void LockPlayer_Static()
    {
        instance.LockPlayer();
    }

    public static void UnlockPlayer_Static()
    {
        instance.UnlockPlayer();
    }

    public static void HideUIElements_Static()
    {
        instance.HideUIElements();
    }

    public static void ShowUIElements_Static()
    {
        instance.ShowUIElements();
    }
}
