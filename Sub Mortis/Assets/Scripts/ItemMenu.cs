using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour
{
    private static ItemMenu instance;
    private bool menuOpen;
    private StoredItem selectedItem;
    private Button dropbutton;
    private Button useButton;
    private Button unloadAmmoButton;
    private Button removeSilencerButton;
    private Text useText;
    public GameObject buttonPrefab;
    private GameObject equippedIcon;
    private List<Button> menuButtons;

    void Start()
    {
        instance = this;
        instance.HideMenu();
        menuButtons = new List<Button>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            instance.HideMenu();
        }
    }

    private void DisplayMenu()
    {
        gameObject.SetActive(true);
        menuOpen = true;
    }

    private void HideMenu()
    {
        gameObject.SetActive(false);
        menuOpen = false;
    }

    private List<Button> GetMenuOptions()
    {
        return menuButtons;
    }

    private void PopulateButtons(StoredItem storedItem)
    {
        selectedItem = storedItem;
        if (menuButtons.Count > 0)
        {
            menuButtons.Clear();
        }
        int position = 0;
        foreach (Button button in transform.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }
        if (selectedItem.item.itemType == ItemType.Weapon || selectedItem.item.itemType == ItemType.Clothing || (selectedItem.item is WeaponModification && InventoryManager.GetInventoryView_Static().weaponManager.currentWeapon != null && !InventoryManager.GetInventoryView_Static().weaponManager.currentWeapon.installedModifications.Contains((WeaponModification)storedItem.item)))
        {
            GameObject prefab = Instantiate(buttonPrefab, transform);
            prefab.transform.localPosition = new Vector3(0, position, 0.0f);
            position -= 50;
            prefab.transform.Find("Text").GetComponent<TMP_Text>().text = "Equip";
            menuButtons.Add(prefab.GetComponent<Button>());
        }
        else if (selectedItem.item.itemType == ItemType.Consumable)
        {
            GameObject prefab = Instantiate(buttonPrefab, transform);
            prefab.transform.Find("Text").GetComponent<TMP_Text>().text = "Use";
            prefab.transform.localPosition = new Vector3(0, position, 0.0f);
            position -= 50;
            menuButtons.Add(prefab.GetComponent<Button>());
        }
        else if (selectedItem.item.itemType == ItemType.Misc || selectedItem.item.itemType == ItemType.Key)
        {
            GameObject prefab = Instantiate(buttonPrefab, transform);
            prefab.transform.Find("Text").GetComponent<TMP_Text>().text = "Equip";
            prefab.transform.localPosition = new Vector3(0, position, 0.0f);
            position -= 50;
            menuButtons.Add(prefab.GetComponent<Button>());
        }
        if (selectedItem.item.itemType == ItemType.Weapon)
        {
            RangedWeapon weapon = (RangedWeapon)selectedItem.item;
            if (weapon.currentAmmo > 0)
            {
                GameObject unloadPrefab = Instantiate(buttonPrefab, transform);
                unloadPrefab.transform.Find("Text").GetComponent<TMP_Text>().text = "Unload Ammo";
                unloadPrefab.transform.localPosition = new Vector3(0, position, 0.0f);
                position -= 50;
                menuButtons.Add(unloadPrefab.GetComponent<Button>());
            }
            if (weapon.installedModifications.Count > 0)
            {
                foreach (WeaponModification mod in weapon.installedModifications)
                {
                    GameObject modPrefab = Instantiate(buttonPrefab, transform);
                    modPrefab.transform.Find("Text").GetComponent<TMP_Text>().text = "Remove " + mod.itemName;
                    modPrefab.transform.localPosition = new Vector3(0, position, 0.0f);
                    position -= 50;
                    menuButtons.Add(modPrefab.GetComponent<Button>());
                }
            }
        }
        if (selectedItem.item.itemCombinationsPossible.Count > 0)
        {
            GameObject combineButton = Instantiate(buttonPrefab, transform);
            combineButton.transform.Find("Text").GetComponent<TMP_Text>().text = "Combine";
            combineButton.transform.localPosition = new Vector3(0, position, 0.0f);
            menuButtons.Add(combineButton.GetComponent<Button>());
        }
        GameObject dropButton = Instantiate(buttonPrefab, transform);
        dropButton.transform.Find("Text").GetComponent<TMP_Text>().text = "Drop";
        dropButton.transform.localPosition = new Vector3(0, position, 0.0f);
        menuButtons.Add(dropButton.GetComponent<Button>());
    }

    private void SetSelectedItem(StoredItem storedItem)
    {
        selectedItem = storedItem;
    }

    private void SetPosition(IntPair position)
    {
        transform.localPosition = new Vector2(position.x, position.y);
    }

    private Button GetDropButton()
    {
        return dropbutton;
    }

    private Button GetUseButton()
    {
        return useButton;
    }

    private Button GetUnloadAmmoButton()
    {
        return unloadAmmoButton;
    }

    private Button GetRemoveModButton()
    {
        return removeSilencerButton;
    }

    private bool IsOpen()
    {
        return menuOpen;
    }

    public static void DisplayMenu_Static()
    {
        instance.DisplayMenu();
        Debug.Log("Open context mnenu");
    }

    public static void HideMenu_Static()
    {
        instance.HideMenu();
    }

    public static void SetSelectedItem_static(StoredItem item)
    {
        instance.SetSelectedItem(item);
    }

    public static Button GetDropButton_Static()
    {
        return instance.GetDropButton();
    }

    public static Button GetUseButton_Static()
    {
        return instance.GetUseButton();
    }

    public static Button GetUnloadAmmoButton_Static()
    {
        return instance.GetUnloadAmmoButton();
    }

    public static Button GetRemoveModButton_Static()
    {
        return instance.GetRemoveModButton();
    }

    public static void PopulateButtons_Static(StoredItem storedItem)
    {
        instance.PopulateButtons(storedItem);
    }

    public static List<Button> GetMenuOptions_Static()
    {
        return instance.GetMenuOptions();
    }

    public static void SetPosition_Static(IntPair position)
    {
        instance.SetPosition(position);
    }

    public static bool IsOpen_Static()
    {
        return instance.IsOpen();
    }

    public static void MoveToFront()
    {
        instance.transform.SetAsLastSibling();
    }
}
