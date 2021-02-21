using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool flashlightOn = false;
    Light lightSource;
    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light>();
        lightSource.enabled = false;
        flashlightOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Flashlight"))
        {
            if (flashlightOn)
            {
                TurnFlashlightOff();
            }
            else
            {
                TurnFlashlightOn();
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
        flashlightOn = true;
    }

    public void TurnFlashlightOff ()
    {
        lightSource.enabled = false;
        flashlightOn = false;
    }
}
