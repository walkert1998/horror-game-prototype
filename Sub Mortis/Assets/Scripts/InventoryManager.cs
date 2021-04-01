using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public bool inventoryOpen;
    public static bool hasPhone;
    public bool phoneBlocked;
    [SerializeField]
    private InventoryView playerInventory;
    public FirstPersonController firstPersonController;
    public GameObject phone;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerInventory.gameObject.SetActive(false);
        playerInventory.inventory.firstTimePickup = true;
        inventoryOpen = false;
        hasPhone = false;
        phoneBlocked = false;
        playerInventory.ClearInventory();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory") && hasPhone && !phoneBlocked)
        {
            if (IsInventoryOpen())
                HideInventory();
            else if (!PlayerInteraction.interactionBlocked)
                ShowInventory();
        }
    }

    private void ShowInventory()
    {
        //playerInventory.gameObject.SetActive(true);
        playerInventory.OpenView();
        inventoryOpen = true;
        //PlayerInteraction.LockInteraction();
        //phone.GetComponent<WeaponSway>().enabled = false;
        //Tooltip.HideToolTip_Static();
        //firstPersonController.m_CanMove = false;
        //firstPersonController.m_CanLook = false;
        //firstPersonController.GetMouseLook().SetCursorLock(false);
    }

    private void HideInventory()
    {
        //playerInventory.gameObject.SetActive(false);
        playerInventory.CloseView();
        inventoryOpen = false;
        //PlayerInteraction.UnlockInteraction();
        //firstPersonController.GetMouseLook().SetCursorLock(true);
        //Tooltip.HideToolTip_Static();
        //firstPersonController.m_CanMove = true;
        //firstPersonController.m_CanLook = true;
        //phone.GetComponent<WeaponSway>().enabled = true;
    }

    private void ActivatePhone()
    {
        hasPhone = true;
    }

    private bool IsInventoryOpen()
    {
        return inventoryOpen;
    }

    private InventoryView GetInventoryView()
    {
        return playerInventory;
    }

    private void BlockPhone()
    {
        phoneBlocked = true;
    }

    private void UnblockPhone()
    {
        phoneBlocked = false;
    }

    private void HidePhone()
    {
        phone.GetComponent<Phone>().UnFocus();
        phone.SetActive(false);
        Debug.Log("Hiding phone");
    }

    private void ShowPhone()
    {
        phone.gameObject.SetActive(true);
    }

    public static void BlockPhone_static()
    {
        instance.BlockPhone();
    }

    public static void UnblockPhone_static()
    {
        instance.UnblockPhone();
    }

    public static void ShowInventory_Static()
    {
        instance.ShowInventory();
    }

    public static void HideInventory_Static()
    {
        instance.HideInventory();
    }

    public static bool IsInventoryOpen_Static()
    {
        return instance.IsInventoryOpen();
    }

    public static InventoryView GetInventoryView_Static()
    {
        return instance.GetInventoryView();
    }

    public static void ActivatePhone_Static()
    {
        instance.ActivatePhone();
    }

    public static void HidePhone_Static()
    {
        instance.HidePhone();
    }

    public static void ShowPhone_Static()
    {
        instance.ShowPhone();
    }
}
