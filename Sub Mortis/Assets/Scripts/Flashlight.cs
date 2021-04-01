using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool flashlightOn = false;
    Light lightSource;
    public Camera lightCamera;
    public GameObject lightDetectionObject;
    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light>();
        lightSource.enabled = false;
        flashlightOn = false;
        lightCamera.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Flashlight"))
        {
            if (flashlightOn)
            {
                TurnFlashlightOff();
                lightCamera.gameObject.SetActive(true);
            }
            else
            {
                TurnFlashlightOn();
                lightCamera.gameObject.SetActive(false);
                //if (HelpText.TextVisible())
                //{
                //    HelpText.HideHelpText();
                //    HelpText._DisplayHelpText("Press [TAB] to use phone");
                //}
            }
        }
    }

    public void TurnFlashlightOn ()
    {
        lightSource.enabled = true;
        AIDirector.SetPlayerFlashight(true);
        lightDetectionObject.SetActive(false);
        flashlightOn = true;
    }

    public void TurnFlashlightOff ()
    {
        lightSource.enabled = false;
        AIDirector.SetPlayerFlashight(false);
        lightDetectionObject.SetActive(true);
        flashlightOn = false;
    }
}
