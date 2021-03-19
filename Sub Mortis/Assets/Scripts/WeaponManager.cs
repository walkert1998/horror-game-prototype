using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponManager : MonoSOObserver
{
    [SerializeField]
    private List<Item> hotkeyItems;
    [SerializeField]
    private List<Item> weapons;
    //private FirstPersonController player;
    private GameObject weapon_name_display;
    private GameObject weapon_ammo_display;
    public GameObject weapon_controller;
    public GameObject currentModel;
    private int index;
    public bool holstered;
    public RangedWeapon currentWeapon;
    public Item currentItem;
    private bool reloading;
    public int ammoReserve;
    public int currentAmmo;
    public Inventory playerInventory;
    public Item item;
    public bool aiming = false;
    [Header("Equipped Item Cursor Attributes")]
    public Image equippedItemCursor;
    public Color canInteractColour;
    public Color cantInteractColour;
    public LayerMask excluded;
    Vector3 originalPosition;
    //PauseMenu pauseMenu;
    //BackPack backPack;
    FirstPersonController controller;
    public AudioSource source;

    // Use this for initialization
    void Start()
    {
        //player = GetComponent<FirstPersonController>();
        //pauseMenu = GetComponent<PauseMenu>();
        //backPack = GetComponent<BackPack>();
        //weapon_name_display = GameObject.Find("WeaponName");
        //weapon_ammo_display = GameObject.Find("WeaponAmmo");
        //initialize_weapons();
        index = 0;
        holstered = true;
        controller = GetComponent<FirstPersonController>();
        originalPosition = weapon_controller.transform.localPosition;
        equippedItemCursor.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon && !holstered)
        {
            //weapon_ammo_display.GetComponent<Text>().text = weapons[index].GetComponent<RangedWeapon>().curr + "/" + weapons[index].GetComponent<RangedWeapon>().reserve_ammo;
        }

        if (!InventoryManager.IsInventoryOpen_Static() && controller.m_CanMove && controller.m_CanLook)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                index++;

                if (index >= hotkeyItems.Count - 1)
                {
                    index = 0;
                }

                SelectHotkeyItem(index);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                index--;

                if (index < 0)
                {
                    index = hotkeyItems.Count - 1;
                }

                SelectHotkeyItem(index);
            }
            if (Input.GetButtonDown("Item1"))
            {
                SelectHotkeyItem(0);
            }
            else if (Input.GetButtonDown("Item2"))
            {
                SelectHotkeyItem(1);
            }
            else if (Input.GetButtonDown("Item3"))
            {
                SelectHotkeyItem(2);
            }
            else if (Input.GetButtonDown("Item4"))
            {
                SelectHotkeyItem(3);
            }
            else if (Input.GetButtonDown("Item5"))
            {
                SelectHotkeyItem(4);
            }
            else if (Input.GetButtonDown("Item6"))
            {
                SelectHotkeyItem(5);
            }
            else if (Input.GetButtonDown("Item7"))
            {
                SelectHotkeyItem(6);
            }
            else if (Input.GetButtonDown("Item8"))
            {
                SelectHotkeyItem(7);
            }
            else if (Input.GetButtonDown("Item9"))
            {
                SelectHotkeyItem(8);
            }
            if (!holstered && currentWeapon != null)
            {
                currentWeapon.nextShotTime += Time.deltaTime;
                if (Input.GetButton("Fire1") && currentWeapon.nextShotTime >= currentWeapon.timeBetweenShots)
                {
                    Fire();
                    currentWeapon.nextShotTime = 0;
                }
                if (Input.GetButtonDown("Reload") && currentWeapon.currentAmmo < currentWeapon.clipSize)
                {
                    if (currentWeapon.ammoReserve > 0 && !holstered && !reloading)
                    {
                        reloading = true;
                        Reload();
                        //StartCoroutine(Reload());
                        return;
                    }
                }
                if (Input.GetButtonDown("ChangeAmmo"))
                {
                    ChangeAmmo();
                }
                if (Input.GetMouseButton(1) && !controller.IsRunning())
                {
                    weapon_controller.transform.localPosition = currentWeapon.aimPosition;
                    weapon_controller.transform.localPosition = Vector3.Lerp(weapon_controller.transform.localPosition, currentWeapon.aimPosition, 0.5f * Time.deltaTime);
                    //Camera.main.fieldOfView = 60;
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, 4f * Time.deltaTime);
                    aiming = true;
                }
                else
                {
                    weapon_controller.transform.localPosition = originalPosition;
                    //Camera.main.fieldOfView = 80;
                    weapon_controller.transform.localPosition = Vector3.Lerp(weapon_controller.transform.localPosition, originalPosition, 0.5f * Time.deltaTime);
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 80, 4f * Time.deltaTime);
                    aiming = false;
                }
            }
            else if (currentItem != null)
            {
                //equippedItemCursor.sprite = currentItem.icon;
                if (Input.GetButtonDown("Fire1"))
                {
                    Use();
                }
            }
        }
    }

    public void Use()
    {
        StoredItem itemToDrop = InventoryManager.GetInventoryView_Static().inventory.FindItem(currentItem);
        if (currentItem is Consumable)
        {
            currentItem.Use();
            source.PlayOneShot(currentItem.onUseSound);
            if (currentItem.destroyOnUse)
            {
                if (itemToDrop.quantity == 1)
                {
                    if (hotkeyItems.Contains(currentItem))
                    {
                        Hotkey.RemoveHotkeyItem_static(currentItem);
                        hotkeyItems[index] = null;
                    }
                    UnEquipItem();
                    equippedItemCursor.gameObject.SetActive(false);
                    InventoryManager.GetInventoryView_Static().inventory.RemoveItem(itemToDrop);
                }
                else
                {
                    InventoryManager.GetInventoryView_Static().inventory.RemoveItemQuantity(itemToDrop.item, 1);
                }
            }
        }
        else
        {
            RaycastHit shot;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out shot, 2, excluded))
            {
                if (currentItem.itemType.Equals(ItemType.Key))
                {
                    if (shot.transform.GetComponent<Lock>() && shot.transform.GetComponent<Lock>().key != null)
                    {
                        if (shot.transform.GetComponent<Lock>().key.Equals(currentItem))
                        {
                            source.PlayOneShot(currentItem.onUseSound);
                            shot.transform.SendMessage("Unlock", shot.transform, SendMessageOptions.DontRequireReceiver);
                            if (currentItem.destroyOnUse)
                            {
                                if (itemToDrop.quantity == 1)
                                {
                                    if (hotkeyItems.Contains(currentItem))
                                    {
                                        Hotkey.RemoveHotkeyItem_static(currentItem);
                                        hotkeyItems[index] = null;
                                    }
                                    UnEquipItem();
                                    equippedItemCursor.gameObject.SetActive(false);
                                    InventoryManager.GetInventoryView_Static().inventory.RemoveItem(itemToDrop);
                                }
                                else
                                {
                                    InventoryManager.GetInventoryView_Static().inventory.RemoveItemQuantity(itemToDrop.item, 1);
                                }
                            }
                        }
                        else
                        {
                            Examination.SetExamineTextUntilClick_static("The key doesn't fit.");
                        }
                    }
                    else
                    {
                        Examination.SetExamineTextUntilClick_static("That didn't work.");
                    }
                }
                else
                {
                    if (shot.transform.GetComponent<PhysicsBlocker>())
                    {
                        if (shot.transform.GetComponent<PhysicsBlocker>().possibleItems.Contains(currentItem))
                        {
                            source.PlayOneShot(currentItem.onUseSound);
                            shot.transform.SendMessage("Interact", currentItem, SendMessageOptions.DontRequireReceiver);
                            if (currentItem.destroyOnUse)
                            {
                                if (itemToDrop.quantity == 1)
                                {
                                    if (hotkeyItems.Contains(currentItem))
                                    {
                                        Hotkey.RemoveHotkeyItem_static(currentItem);
                                        hotkeyItems[index] = null;
                                    }
                                    UnEquipItem();
                                    equippedItemCursor.gameObject.SetActive(false);
                                    InventoryManager.GetInventoryView_Static().inventory.RemoveItem(itemToDrop);
                                }
                                else
                                {
                                    InventoryManager.GetInventoryView_Static().inventory.RemoveItemQuantity(itemToDrop.item, 1);
                                }
                            }
                        }
                        else
                        {
                            Examination.SetExamineTextUntilClick_static("That didn't work.");
                        }
                    }
                    else
                    {
                        Examination.SetExamineTextUntilClick_static("That didn't work.");
                    }
                }
                Debug.Log(shot.transform.gameObject);
                //if (currentWeapon.loadedAmmoType.ammoEffect == AmmoEffect.Fire)
                //{
                //    shot.transform.gameObject.AddComponent<Fire>();
                //}
                /*
                GameObject.Find("Bigfoot").transform.SendMessage("HiddenShot", transform.position, SendMessageOptions.DontRequireReceiver);
                if (shot.transform.tag == "Metal")
                    Instantiate(metalBulletImpact, new Vector3(shot.point.x + 0.01f, shot.point.y + 0.01f, shot.point.z + 0.01f), Quaternion.FromToRotation(Vector3.up, shot.normal));
                else if (shot.transform.tag == "Wood")
                    Instantiate(woodBulletImpact, new Vector3(shot.point.x + 0.01f, shot.point.y + 0.01f, shot.point.z + 0.01f), Quaternion.FromToRotation(Vector3.up, shot.normal));
                else if (shot.transform.gameObject.GetComponent<EnemyController>())
                    Instantiate(bloodBulletImpact, new Vector3(shot.point.x + 0.01f, shot.point.y + 0.01f, shot.point.z + 0.01f), Quaternion.FromToRotation(Vector3.up, shot.normal));
                    */
                //Destroy(shot.transform.gameObject, 50f);
            }
            UnEquipItem();
        }
    }

    public void Fire()
    {
        if (currentWeapon.currentAmmo > 0)
        {
            currentAmmo--;
            currentWeapon.currentAmmo--;
            //source.PlayOneShot(currentWeapon.gunshotSound);
            if (currentModel.GetComponent<Animator>())
            {
                currentModel.GetComponent<Animator>().Play("Shoot");
            }
            RaycastHit shot;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out shot, 100, excluded))
            {
                shot.transform.SendMessage("DamageCharacter", currentWeapon.loadedAmmoType.damage, SendMessageOptions.DontRequireReceiver);
                shot.transform.SendMessage("HiddenShot", controller.transform, SendMessageOptions.DontRequireReceiver);
                if (shot.rigidbody != null)
                {
                    Debug.Log("Added force");
                    //Vector3 direction = shot.transform.position - transform.position;
                    shot.rigidbody.AddForce(-shot.normal * 1000);
                }
                Debug.Log(shot.transform.gameObject);
                //if (currentWeapon.loadedAmmoType.ammoEffect == AmmoEffect.Fire)
                //{
                //    shot.transform.gameObject.AddComponent<Fire>();
                //}
                /*
                GameObject.Find("Bigfoot").transform.SendMessage("HiddenShot", transform.position, SendMessageOptions.DontRequireReceiver);
                if (shot.transform.tag == "Metal")
                    Instantiate(metalBulletImpact, new Vector3(shot.point.x + 0.01f, shot.point.y + 0.01f, shot.point.z + 0.01f), Quaternion.FromToRotation(Vector3.up, shot.normal));
                else if (shot.transform.tag == "Wood")
                    Instantiate(woodBulletImpact, new Vector3(shot.point.x + 0.01f, shot.point.y + 0.01f, shot.point.z + 0.01f), Quaternion.FromToRotation(Vector3.up, shot.normal));
                else if (shot.transform.gameObject.GetComponent<EnemyController>())
                    Instantiate(bloodBulletImpact, new Vector3(shot.point.x + 0.01f, shot.point.y + 0.01f, shot.point.z + 0.01f), Quaternion.FromToRotation(Vector3.up, shot.normal));
                    */
                    //Destroy(shot.transform.gameObject, 50f);
            }
            controller.GetMouseLook().Recoil(currentWeapon.recoil);
        }
    }

    public void Reload ()
    {
        StoredItem initialAmmo = playerInventory.FindItem(currentWeapon.loadedAmmoType);
        Debug.Log(initialAmmo);
        if (initialAmmo != null)
        {
            int difference = currentWeapon.clipSize - currentWeapon.currentAmmo;
            if (currentWeapon.ammoReserve > difference)
            {
                currentWeapon.ammoReserve -= difference;
                currentWeapon.currentAmmo += difference;
                playerInventory.RemoveItemQuantity(initialAmmo.item, difference);
                //controller.GetComponent<Inventory>().RemoveAmmo(ammoType, difference);
            }
            else
            {
                currentWeapon.currentAmmo += currentWeapon.ammoReserve;
                playerInventory.RemoveItemQuantity(initialAmmo.item, currentWeapon.ammoReserve);
                //controller.GetComponent<Inventory>().RemoveAmmo(ammoType, ammo_reserve);
                currentWeapon.ammoReserve = 0;
            }
            source.PlayOneShot(currentWeapon.reloadSound);
            //currentAmmo = Mathf.Clamp(currentWeapon.clipSize - currentAmmo, 0, ammoReserve);
            Debug.Log(currentWeapon.currentAmmo);
        }
        reloading = false;
    }

    public void InitializeAmmo()
    {
        StoredItem initialAmmo = playerInventory.FindItem(currentWeapon.loadedAmmoType);
        if (initialAmmo != null)
        {
            currentWeapon.loadedAmmoType = (Ammo)initialAmmo.item;
            currentWeapon.ammoReserve = initialAmmo.quantity;
            int difference = currentWeapon.clipSize - currentWeapon.currentAmmo;
            playerInventory.RemoveItemQuantity(initialAmmo.item, difference);
            if (currentWeapon.ammoReserve > difference)
            {
                ammoReserve -= difference;
                currentAmmo += difference;
                currentWeapon.ammoReserve -= difference;
                currentWeapon.currentAmmo += difference;
                //controller.GetComponent<Inventory>().RemoveAmmo(ammoType, difference);
            }
            else
            {
                currentWeapon.currentAmmo += currentWeapon.ammoReserve;
                //controller.GetComponent<Inventory>().RemoveAmmo(ammoType, ammo_reserve);
                currentWeapon.ammoReserve = 0;
            }
            //currentAmmo = Mathf.Clamp(currentWeapon.clipSize - currentAmmo, 0, ammoReserve);
            Debug.Log(currentWeapon.currentAmmo);
        }
        else if (currentWeapon.canBeReloaded)
        {
            ammoReserve = 0;
            currentAmmo = 0;
            Debug.Log("Ammo Type could not be found");
        }
        else
        {
            currentAmmo = currentWeapon.currentAmmo;
        }
    }

    public void CheckAmmo()
    {
        if (currentWeapon != null)
        {
            StoredItem initialAmmo = playerInventory.FindItem(currentWeapon.loadedAmmoType);
            if (initialAmmo != null)
            {
                currentWeapon.ammoReserve = initialAmmo.quantity;
            }
            else
            {
                ammoReserve = 0;
            }
        }
    }

    public void ChangeAmmo()
    {
        if (currentWeapon.ammoTypes.Count > 1)
        {
            if (currentWeapon.currentAmmo > 0)
            {
                Item ammoAddedBack = currentWeapon.loadedAmmoType;
                playerInventory.AddItem(ammoAddedBack, currentWeapon.currentAmmo);
            }
            if (currentWeapon.ammoTypes.IndexOf(currentWeapon.loadedAmmoType) + 1 > currentWeapon.ammoTypes.Count - 1)
            {
                currentWeapon.loadedAmmoType = currentWeapon.ammoTypes[0];
                Debug.Log("Loaded ammo type: " + currentWeapon.loadedAmmoType);
            }
            else
            {
                currentWeapon.loadedAmmoType = currentWeapon.ammoTypes[currentWeapon.ammoTypes.IndexOf(currentWeapon.loadedAmmoType) + 1];
                Debug.Log("Loaded ammo type: " + currentWeapon.loadedAmmoType);
            }
            currentWeapon.ammoReserve = 0;
            currentWeapon.currentAmmo = 0;
            source.PlayOneShot(currentWeapon.reloadSound);
            InitializeAmmo();
        }
        else
        {
            return;
        }
    }

    /*
    public void initialize_weapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
                weapons[i].SetActive(false);
        }
        if (weapons[0])
            weapons[0].SetActive(true);
    }
    */

    public void DisplayWeapon()
    {
        GameObject.Destroy(currentModel);
        currentModel = Instantiate(currentWeapon.gameModel, weapon_controller.transform);
        currentModel.transform.localPosition = new Vector3(0,0,0);
        DynamicCursor.ChangeCursor_Static(CursorType.Target);
        if (!InventoryManager.IsInventoryOpen_Static() && currentWeapon.twoHanded)
        {
            InventoryManager.HidePhone_Static();
            InventoryManager.BlockPhone_static();
            PlayerInteraction.LockInteraction();
        }
        holstered = false;
    }

    public void HideWeapon()
    {
        currentModel.SetActive(false);
        //currentModel.transform.localPosition = new Vector3(0, 0, 0);
        holstered = false;
    }

    public void EquipWeapon(Item newWeapon)
    {
        currentWeapon = newWeapon as RangedWeapon;
        InitializeAmmo();
        PlayerInteraction.LockInteraction();
        if (!InventoryManager.IsInventoryOpen_Static() && currentWeapon.twoHanded)
        {
            DisplayWeapon();
        }
        //ammoReserve = currentWeapon.currentAmmo;
    }

    public void UnEquipItem()
    {
        if (currentWeapon.twoHanded)
        {
            InventoryManager.ShowPhone_Static();
            InventoryManager.UnblockPhone_static();
        }
        currentWeapon = null;
        currentItem = null;
        holstered = true;
        equippedItemCursor.gameObject.SetActive(false);
        if (currentModel != null)
        {
            Destroy(currentModel);
            currentModel = null;
        }
        if (!InventoryManager.IsInventoryOpen_Static())
        {
            PlayerInteraction.UnlockInteraction();
        }
    }

    public void SelectHotkeyItem(int selection)
    {
        Debug.Log("Switching to weapon " + (selection + 1));
        if (hotkeyItems[selection] != null)
        {
            //GetCurrentWeapon().StartCoroutine("Holster");
            /*
            for (int i = 0; i < weapons.Length; i++)
            {
                if (weapons[i] != null)
                    weapons[i].SetActive(false);
            }
            */
            //weapons[selection].SetActive(true);
            //UnEquipWeapon();
            if (currentItem == hotkeyItems[selection] && !holstered)
            {
                //Holster();
                UnEquipItem();
            }
            else
            {
                EquipItem(hotkeyItems[selection]);
            }
            //EquipWeapon(hotkeyItems[selection]);
            //weapons[selection].GetComponent<RangedWeapon>().StartCoroutine("Holster");
        }
        else
        {
            UnEquipItem();
        }
        index = selection;
        //if (weapons[selection] != null && currentWeapon != weapons[selection])
        //{
        //    //GetCurrentWeapon().StartCoroutine("Holster");
        //    /*
        //    for (int i = 0; i < weapons.Length; i++)
        //    {
        //        if (weapons[i] != null)
        //            weapons[i].SetActive(false);
        //    }
        //    */
        //    //weapons[selection].SetActive(true);
        //    //UnEquipWeapon();
        //    EquipWeapon(weapons[selection]);
        //    index = selection;
        //    //weapons[selection].GetComponent<RangedWeapon>().StartCoroutine("Holster");
        //}

    }

    public void EquipItem(Item item)
    {
        currentItem = item;
        holstered = false;
        if (item is RangedWeapon)
        {
            EquipWeapon(item);
        }
        else
        {
            equippedItemCursor.sprite = item.horizontalIcon;
            if (!InventoryManager.IsInventoryOpen_Static())
            {
                equippedItemCursor.gameObject.SetActive(true);
            }
            DynamicCursor.HideCursor_Static();
            ToggleHighlightCursor(false);
        }
    }

    public void ToggleHighlightCursor(bool toggle)
    {
        if (toggle)
        {
            equippedItemCursor.color = canInteractColour;
        }
        else
        {
            equippedItemCursor.color = cantInteractColour;
        }
    }

    public Item GetEquippedItem()
    {
        return currentItem;
    }

    public void AssignItemToSlot(int index, Item item)
    {
        if (ItemIsHotkeyed(item))
        {
            hotkeyItems[GetHotkeyIndex(item)] = null;
        }
        hotkeyItems[index] = item;
        Debug.Log("Assigned item " + item.itemName + " to index " + index);
    }

    public bool ItemIsHotkeyed(Item item)
    {
        foreach (Item hkItem in hotkeyItems)
        {
            if (hkItem == item)
            {
                return true;
            }
        }
        return false;
    }

    public int GetHotkeyIndex(Item item)
    {
        for (int i = 0; i < hotkeyItems.Count; i++)
        {
            if (hotkeyItems[i] == item)
            {
                return i;
            }
        }
        return -1;
    }

    public override void Notify()
    {
        if (currentItem is RangedWeapon)
        {
            CheckAmmo();
        }
    }

    public void Holster()
    {
        currentItem = null;
        currentWeapon = null;
        if (currentWeapon.twoHanded)
        {
            InventoryManager.ShowPhone_Static();
            InventoryManager.UnblockPhone_static();
        }
        //ToggleHighlightCursor(false);
    }

    public void Unholster()
    {
        currentItem = hotkeyItems[index];
        if (currentItem is RangedWeapon)
        {
            currentWeapon = currentItem as RangedWeapon;
            if (currentWeapon.twoHanded)
            {
                InventoryManager.HidePhone_Static();
                InventoryManager.BlockPhone_static();
                PlayerInteraction.LockInteraction();
            }
        }
        else
        {
            ToggleHighlightCursor(true);
        }
    }

    public void RemoveItemFromHotkey(Item itemToRemove)
    {
        hotkeyItems.Remove(itemToRemove);
    }
}
