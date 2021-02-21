using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotkey : MonoBehaviour
{
    private static Hotkey instance;
    public InventoryView view;
    public int highlightedSlot;
    public WeaponManager weaponManager;
    public List<HotKeySlotUI> weaponSlots;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        highlightedSlot = -1;
    }

    private void HotkeyItem (int index, Item item)
    {
        weaponManager.AssignItemToSlot(index, item);
        view.Notify();
        foreach (HotKeySlotUI slot in weaponSlots)
        {
            if (slot.slot.sprite == item.horizontalIcon)
            {
                slot.ClearSlotIcon();
            }
        }
        weaponSlots[index].AssignSlotIcon(item.horizontalIcon);
    }

    private void RemoveHotkeyItem(Item item)
    {
        int index = weaponManager.GetHotkeyIndex(item);
        weaponSlots[index].ClearSlotIcon();
        //view.Notify();
    }

    public static void HotkeyItem_static(int index, Item item)
    {
        instance.HotkeyItem(index, item);
    }

    public static List<HotKeySlotUI> GetHotkeySlots_Static()
    {
        return instance.weaponSlots;
    }

    public static void RemoveHotkeyItem_static(Item item)
    {
        instance.RemoveHotkeyItem(item);
    }

    public static void SlotHighlighted_Static(int index)
    {
        instance.highlightedSlot = index;
    }

    public static int GetHighlightedSlotIndex_Static()
    {
        return instance.highlightedSlot;
    }
}
