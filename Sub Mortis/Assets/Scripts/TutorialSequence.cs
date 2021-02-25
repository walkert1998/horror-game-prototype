using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSequence : MonoBehaviour
{
    List<KeyCode> movementKeys;
    List<KeyCode> widgetKeys;
    // Start is called before the first frame update
    void Start()
    {
        movementKeys = new List<KeyCode>();
        widgetKeys = new List<KeyCode>();
        movementKeys.Add(KeyCode.W);
        movementKeys.Add(KeyCode.A);
        movementKeys.Add(KeyCode.S);
        movementKeys.Add(KeyCode.D);
        widgetKeys.Add(KeyCode.Q);
        widgetKeys.Add(KeyCode.Mouse0);
        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(1.0f);
        HelpText._DisplayHelpText("Move mouse to look around, and use [W][A][S][D] to move.", KeyCode.W, movementKeys, 1.0f);
        while (HelpText.TextVisible())
        {
            yield return null;
        }
        HelpText._DisplayHelpText("Use [Left-Click] to interact, objects can be moved by holding [Left-Click] and dragging the mouse.", KeyCode.Mouse0, null, 10.0f);
        while (!InventoryManager.hasPhone)
        {
            yield return null;
        }
        HelpText._DisplayHelpText("Press [F] to toggle the phones flashlight.", KeyCode.F, null, 1.0f);
        while (!Input.GetKeyDown(KeyCode.F))
        {
            yield return null;
        }
        HelpText._DisplayHelpText("Press [TAB] to use the smartphone.", KeyCode.Tab, null, 1.0f);
        while (!Input.GetKeyDown(KeyCode.Tab))
        {
            yield return null;
        }
        HelpText._DisplayHelpText("The lock screen displays vital info in the form of 'Widgets', such as your health, current objective, and any messages. You can either click the button at the bottom of the lock screen, or press [Q] to navigate to the home screen.", KeyCode.Q, widgetKeys, 1.0f);
        while (InventoryManager.IsInventoryOpen_Static())
        {
            yield return null;
        }
        HelpText._DisplayHelpText("Use [Right-Click] to examine objects.", KeyCode.Mouse1, null, 1.0f);
        //while (HelpText.TextVisible())
        //{
        //    yield return null;
        //}
        //HelpText._DisplayHelpText("Press [Right-Click] to examine objects.", KeyCode.Mouse1, null, 1.0f);
    }
}
