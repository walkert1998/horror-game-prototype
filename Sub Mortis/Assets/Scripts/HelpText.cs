using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HelpText : MonoBehaviour
{
    public TMP_Text helpTextDisplay;
    private static HelpText instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        helpTextDisplay.text = "";
    }

    private void DisplayHelpText(string helpMessage)
    {
        helpTextDisplay.text = helpMessage;
    }

    public static void _DisplayHelpText(string helpMessage)
    {
        instance.DisplayHelpText(helpMessage);
    }

    public static void HideHelpText()
    {
        instance.DisplayHelpText("");
    }

    public static bool TextVisible()
    {
        return !instance.helpTextDisplay.Equals("");
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.0f);
        _DisplayHelpText("Press [W] [A] [S] [D] to move");
    }
}
