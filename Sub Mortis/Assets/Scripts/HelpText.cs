using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpText : MonoBehaviour
{
    public TMP_Text helpTextDisplay;
    public List<KeyCode> keyPressesRequired;
    public KeyCode keyPressRequired;
    private static HelpText instance;
    Coroutine routine;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        helpTextDisplay.text = "";
    }

    private void DisplayHelpText(string helpMessage, KeyCode keyRequired, List<KeyCode> keysRequired = null, float time = 3.0f)
    {
        helpTextDisplay.text = helpMessage;
        keyPressesRequired = keysRequired;
        keyPressRequired = keyRequired;
        if (routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(DisplayTextUntilInput(time));
    }

    public static void _DisplayHelpText(string helpMessage, KeyCode keyRequired, List<KeyCode> keysRequired = null, float time = 3.0f)
    {
        instance.DisplayHelpText(helpMessage, keyRequired, keysRequired, time);
    }

    public static void HideHelpText()
    {
        Debug.Log("Hiding help text: " + instance.helpTextDisplay.text);
        instance.helpTextDisplay.text = "";
    }

    public static bool TextVisible()
    {
        return !instance.helpTextDisplay.text.Equals("");
    }

    IEnumerator DisplayTextUntilInput(float time)
    {
        if (keyPressesRequired != null)
        {
            while (!KeyPressed())
            {
                yield return null;
            }
            Debug.Log("A key has been pressed");
        }
        else
        {
            while (!Input.GetKeyDown(keyPressRequired))
            {
                yield return null;
            }
            Debug.Log(keyPressRequired + " pressed");
        }
        Debug.Log("Input detected");
        yield return new WaitForSeconds(time);
        HideHelpText();
        routine = null;
    }

    public bool KeyPressed()
    {
        foreach(KeyCode code in keyPressesRequired)
        {
            if (Input.GetKeyDown(code))
            {
                Debug.Log(code.ToString() + " pressed");
                return true;
            }
        }
        return false;
    }
}
