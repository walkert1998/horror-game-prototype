using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotKeySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image slot;
    public Sprite defaultIcon;
    public bool movingItems;
    public bool hovered;
    public int index;
    public Color normalColor;
    public Color highlightColor;

    public void AssignSlotIcon(Sprite icon)
    {
        slot.sprite = icon;
    }

    public void ClearSlotIcon()
    {
        slot.sprite = defaultIcon;
    }

    public void Highlight()
    {
        GetComponent<Image>().color = highlightColor;
    }

    public void UnHighlight()
    {
        GetComponent<Image>().color = normalColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("pointer enters");
        if (movingItems)
        {
            Highlight();
            hovered = true;
            Hotkey.SlotHighlighted_Static(index);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //SelectedSlot.ClearSelectedSlot_Static();
        UnHighlight();
        hovered = false;
        Hotkey.SlotHighlighted_Static(-1);
    }
}
