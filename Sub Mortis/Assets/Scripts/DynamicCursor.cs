using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCursor : MonoBehaviour
{
    public Texture2D targetCursor;
    public Texture2D pickupCursor;
    public Texture2D doorCursor;
    public Texture2D examineCursor;
    public Texture2D dragCursor;
    public Texture2D grabCursor;
    public Texture2D combineCursor;
    public Sprite targetCursorSprite;
    public Sprite pickupCursorSprite;
    public Sprite doorCursorSprite;
    public Sprite dragCursorSprite;
    public Sprite grabCursorSprite;
    public Sprite examineCursorSprite;
    public Sprite combineCursorSprite;
    public Image staticCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot;
    public static DynamicCursor instance;
    // Start is called before the first frame update
    void Start()
    {
        hotSpot = new Vector2(targetCursor.width / 2, targetCursor.height / 2);
        instance = this;
        DynamicCursor.ChangeCursor_Static(CursorType.Target);
        HideCursor_Static();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void ChangeCursor(CursorType type)
    {
        //Sprite newSprite;
        if (type.Equals(CursorType.Target))
        {
            if (GameSettings.GetCursorSetting().Equals(CursorSetting.InteractOnly))
            {
                Cursor.SetCursor(null, hotSpot, cursorMode);
                staticCursor.sprite = null;
                //Debug.Log("Overriding cursor");
                HideCursor();
                return;
            }
            Cursor.SetCursor(targetCursor, new Vector2(targetCursor.width / 2, targetCursor.height / 2), cursorMode);
            //Debug.Log("Setting cursor to target");
            //newSprite = Sprite.Create(targetCursor, staticCursor.gameObject.GetComponent<RectTransform>().rect, new Vector2(0, 0), 1);
            staticCursor.sprite = targetCursorSprite;
            ShowCursor();
        }
        else if (type.Equals(CursorType.Examine))
        {
            Cursor.SetCursor(examineCursor, new Vector2(examineCursor.width / 2, examineCursor.height / 2), cursorMode);
            //Debug.Log("Setting cursor to examine");
            //newSprite = Sprite.Create(targetCursor, staticCursor.gameObject.GetComponent<RectTransform>().rect, new Vector2(0, 0), 1);
            staticCursor.sprite = examineCursorSprite;
            ShowCursor();
        }
        else if (type.Equals(CursorType.Door))
        {
            Cursor.SetCursor(doorCursor, new Vector2(doorCursor.width / 2, doorCursor.height / 2), cursorMode);
            //Debug.Log("Setting cursor to door");
            //newSprite = Sprite.Create(targetCursor, staticCursor.gameObject.GetComponent<RectTransform>().rect, new Vector2(0, 0), 1);
            staticCursor.sprite = doorCursorSprite;
            ShowCursor();
        }
        else if (type.Equals(CursorType.Grab))
        {
            Cursor.SetCursor(grabCursor, new Vector2(grabCursor.width / 2, grabCursor.height / 2), cursorMode);
            //Debug.Log("Setting cursor to grab");
            //newSprite = Sprite.Create(targetCursor, staticCursor.gameObject.GetComponent<RectTransform>().rect, new Vector2(0, 0), 1);
            //Debug.Log("Showing cursor");
            staticCursor.sprite = grabCursorSprite;
            ShowCursor();
        }
        else if (type.Equals(CursorType.Drag))
        {
            Cursor.SetCursor(dragCursor, new Vector2(dragCursor.width / 2, dragCursor.height / 2), cursorMode);
            //Debug.Log("Setting cursor to drag");
            //newSprite = Sprite.Create(targetCursor, staticCursor.gameObject.GetComponent<RectTransform>().rect, new Vector2(0, 0), 1);
            staticCursor.sprite = dragCursorSprite;
            ShowCursor();
        }
        else if (type.Equals(CursorType.Pickup))
        {
            Cursor.SetCursor(pickupCursor, new Vector2(pickupCursor.width / 2, pickupCursor.height / 2), cursorMode);
            //Debug.Log("Setting cursor to pickup");
            //newSprite = Sprite.Create(targetCursor, staticCursor.gameObject.GetComponent<RectTransform>().rect, new Vector2(0, 0), 1);
            staticCursor.sprite = pickupCursorSprite;
            ShowCursor();
        }
        else if (type.Equals(CursorType.Combine))
        {
            Cursor.SetCursor(combineCursor, new Vector2(combineCursor.width / 2, combineCursor.height / 2), cursorMode);
            //Debug.Log("Setting cursor to combine");
            //newSprite = Sprite.Create(targetCursor, staticCursor.gameObject.GetComponent<RectTransform>().rect, new Vector2(0, 0), 1);
            staticCursor.sprite = combineCursorSprite;
            ShowCursor();
        }
        else if (type.Equals(CursorType.None))
        {
            Cursor.SetCursor(null, hotSpot, cursorMode);
            //Debug.Log("Setting cursor to none");
            //newSprite = Sprite.Create(targetCursor, staticCursor.gameObject.GetComponent<RectTransform>().rect, new Vector2(0, 0), 1);
            staticCursor.sprite = null;
            HideCursor();
        }
    }

    public void HideCursor()
    {
        staticCursor.gameObject.SetActive(false);
    }

    public void ShowCursor()
    {
        staticCursor.gameObject.SetActive(true);
    }

    public static void HideCursor_Static()
    {
        instance.HideCursor();
    }

    public static void ShowCursor_Static()
    {
        instance.ShowCursor();
    }

    public static void ChangeCursor_Static(CursorType type)
    {
        if (GameSettings.GetCursorSetting().Equals(CursorSetting.Never))
        {
            instance.ChangeCursor(CursorType.None);
            instance.HideCursor();
        }
        else
        {
            instance.ChangeCursor(type);
        }
    }
}

public enum CursorType
{
    Target,
    Pickup,
    Door,
    Examine,
    Drag,
    Grab,
    Combine,
    None
}
