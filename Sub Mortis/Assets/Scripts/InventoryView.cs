using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Cameras;

public class InventoryView : MonoSOObserver
{
    public RectTransform itemsGrid;
    public Inventory inventory;
    public GameObject itemPrefab;
    public GameObject gridSlotPrefab;
    public Transform itemDropLocation;
    public bool expandWithSlots = false;

    private bool combining = false;

    public WeaponManager weaponManager;

    private FloatPair unitSlot;

    GridHighlight[,] gridSlots;
    List<GridHighlight> allSlots;
    List<GridHighlight> highlightedSlots;
    public AudioSource source;
    public AudioClip removeSound;
    public AudioClip equipSound;
    public AudioClip invalidMoveSound;
    public AudioClip validMoveSound;
    public AudioClip beginMoveSound;

    private GameObject movingObject;
    private StoredItem movingItem;
    private ItemOrientation startingOrientation;

    private void CalcSlotDimensions()
    {
        float gridWidth = itemsGrid.rect.width;
        float gridHeight = itemsGrid.rect.height;

        unitSlot = new FloatPair(gridHeight / inventory.size.x, gridWidth / inventory.size.y);
    }

    private FloatPair GetSlotPosition (int row, int col)
    {
        return new FloatPair(row * -unitSlot.x, col * unitSlot.y);
    }

    private void PositionInGrid (GameObject obj, IntPair position, IntPair size)
    {
        RectTransform trans = obj.transform as RectTransform;
        FloatPair gridPostion = GetSlotPosition(position.x, position.y);
        trans.sizeDelta = new Vector2(unitSlot.y * size.y, unitSlot.x * size.x);
        trans.localPosition = new Vector3(gridPostion.y, gridPostion.x, 0.0f);
    }

    private void DrawGrid()
    {
        GameObject gridCell;
        gridSlots = new GridHighlight[inventory.size.x, inventory.size.y];
        allSlots = new List<GridHighlight>();
        IntPair unitPair = new IntPair(1, 1);
        for (int i = 0; i < inventory.size.x; i++)
        {
            for (int j = 0; j < inventory.size.y; j++)
            {
                gridCell = Instantiate(gridSlotPrefab, itemsGrid);
                gridCell.GetComponent<GridHighlight>().baseColor = gridCell.GetComponent<Image>().color;
                gridCell.GetComponent<GridHighlight>().slotPosition = new IntPair(i, j);
                gridSlots[i, j] = gridCell.GetComponent<GridHighlight>();
                allSlots.Add(gridCell.GetComponent<GridHighlight>());
                //Debug.Log("Draw cell");
                PositionInGrid(gridCell, new IntPair(i, j), unitPair);
                //Debug.Log(gridCell);
            }
        }
        highlightedSlots = new List<GridHighlight>();
    }

    public override void Notify()
    {
        DrawInventory();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DrawInventory();
    }

    private void DrawInventory()
    {
        CleanUpGrid();
        if (expandWithSlots)
        {
            ExpandGridSize();
        }
        CalcSlotDimensions();
        DrawGrid();
        DrawItems();
    }

    private void CleanUpGrid()
    {
        for (int i = 0; i < itemsGrid.childCount; i++)
        {
            if (!itemsGrid.GetChild(i).GetComponent<ItemMenu>())
                Destroy(itemsGrid.GetChild(i).gameObject);
        }
    }

    private void DrawItems()
    {
        foreach (StoredItem item in inventory.items)
        {
            DrawItem(item);
        }
    }

    private void ExpandGridSize()
    {
        itemsGrid.sizeDelta = new Vector2(100 * inventory.size.y, 100 * inventory.size.x);
    }

    private void DrawItem(StoredItem storedItem)
    {
        Item item = storedItem.item;
        GameObject itemView = Instantiate(itemPrefab, itemsGrid);
        PositionInGrid(itemView, storedItem.position, item.size);
        itemView.transform.SetParent(itemsGrid, false);
        if (item.stackable)
        {
            Text quantityText = itemView.transform.Find("QuantityText").GetComponent<Text>();
            quantityText.text = "x" + storedItem.quantity;
        }
        if (item is RangedWeapon)
        {
            RangedWeapon wep = item as RangedWeapon;
            if (!wep.canBeReloaded)
            {
                Text ammoText = itemView.transform.Find("QuantityText").GetComponent<Text>();
                ammoText.text = wep.currentAmmo + "%";
            }
        }
        Image img = itemView.GetComponent<Image>();
        if (storedItem.orientation.Equals(ItemOrientation.Horizontal))
        {
            img.sprite = item.horizontalIcon;
        }
        else
        {
            img.sprite = item.verticalIcon;
        }
        UpdateHoveredItem hovUpdater = itemView.GetComponent<UpdateHoveredItem>();
        hovUpdater.item = item;
        UIClickNotifier itemClicks = itemView.GetComponent<UIClickNotifier>();
        itemView.transform.Find("EquippedIcon").gameObject.SetActive(item == weaponManager.GetEquippedItem());
        if (item is Ammo && weaponManager.currentWeapon != null && item == weaponManager.currentWeapon.loadedAmmoType)
        {
            weaponManager.CheckAmmo();
        }
        /*
        
        */
        //if (item is RangedWeapon)
        //{
            itemView.transform.Find("HotkeyNumber").gameObject.SetActive(weaponManager.ItemIsHotkeyed(item));
            if (weaponManager.ItemIsHotkeyed(item))
            {
                itemView.transform.Find("HotkeyNumber").GetComponentInChildren<Text>().text = (weaponManager.GetHotkeyIndex(item) + 1).ToString();
            }
        //}
        //else
        //{
        //    itemView.transform.Find("HotkeyNumber").gameObject.SetActive(false);
        //}
        itemView.transform.Find("ItemHighlight").GetComponent<RectTransform>().sizeDelta = itemView.GetComponent<RectTransform>().sizeDelta;
        itemView.transform.Find("ItemHighlight").gameObject.SetActive(hovUpdater.hovering);
        if (storedItem.orientation == ItemOrientation.Vertical)
        {
            if (storedItem.item.verticalIcon != null)
            {
                itemView.GetComponent<Image>().sprite = storedItem.item.verticalIcon;
            }
        }
        itemClicks.onLeft.AddListener(
            () =>
            {
                if (movingItem == null)
                {
                    Debug.Log("Attempting to move");
                    SelectedSlot.SetSelectedSlot_Static(gridSlots[storedItem.position.x, storedItem.position.y]);
                    source.PlayOneShot(beginMoveSound);
                    MoveItem(itemView, storedItem);
                }
            }
        );
        itemClicks.onRight.AddListener(
            () =>
            {
                if (movingItem == null)
                {
                    ItemMenu.HideMenu_Static();
                    ItemMenu.MoveToFront();
                    ItemMenu.SetSelectedItem_static(storedItem);
                    ItemMenu.PopulateButtons_Static(storedItem);
                    ItemMenu.SetPosition_Static(new IntPair((int)itemView.GetComponent<RectTransform>().localPosition.x + (int)itemView.GetComponent<RectTransform>().rect.width, (int)itemView.GetComponent<RectTransform>().localPosition.y));
                    List<Button> buttons = ItemMenu.GetMenuOptions_Static();
                    if (item.itemCombinationsPossible.Count > 0)
                    {
                        buttons[buttons.Count - 2].onClick.AddListener(
                            () =>
                            {
                                source.PlayOneShot(removeSound);
                                HighlightCombined(storedItem);
                                ItemMenu.HideMenu_Static();
                            }
                        );
                    }
                    if (item.itemType == ItemType.Weapon)
                    {
                        RangedWeapon weapon = item as RangedWeapon;
                        int iterator = 1;
                        buttons[0].onClick.AddListener(
                            () =>
                            {
                                weaponManager.EquipWeapon(weapon);
                                ItemMenu.HideMenu_Static();
                                Notify();
                            }
                        );
                        if (weapon.currentAmmo > 0 && weapon.canBeReloaded)
                        {
                            buttons[1].onClick.AddListener(
                                () =>
                                {
                                    source.PlayOneShot(removeSound);
                                    Ammo removedAmmo = weapon.loadedAmmoType;
                                    inventory.AddItem(removedAmmo, weapon.currentAmmo);
                                    ItemMenu.HideMenu_Static();
                                    weapon.currentAmmo = 0;
                                }
                            );
                            iterator = 2;
                        }
                        if (weapon.installedModifications.Count > 0)
                        {
                            for (int i = iterator; i < buttons.Count - 1; i++)
                            {
                                buttons[i].onClick.AddListener(
                                    () =>
                                    {
                                        source.PlayOneShot(removeSound);
                                        WeaponModification removedMod = weapon.UnInstallMod(weapon.installedModifications[0]);
                                        inventory.AddItem(removedMod);
                                        ItemMenu.HideMenu_Static();
                                    }
                                );
                            }
                        }
                    }
                    else if (item.itemType == ItemType.Consumable)
                    {
                        buttons[0].onClick.AddListener(
                            () =>
                            {
                                item.Use();
                                source.PlayOneShot(item.onUseSound);
                                Notify();
                                if (storedItem.quantity > 1)
                                {
                                    inventory.RemoveItemQuantity(item, 1);
                                }
                                else
                                {
                                    RemoveItem(storedItem);
                                }
                                ItemMenu.HideMenu_Static();
                            }
                        );
                    }
                    else if (item.itemType == ItemType.Misc || item.itemType == ItemType.Key)
                    {
                        buttons[0].onClick.AddListener(
                            () =>
                            {
                                weaponManager.EquipItem(item);
                                ItemMenu.HideMenu_Static();
                                Notify();
                            }
                        );
                    }
                    else if (item is WeaponModification)
                    {
                        if (weaponManager.currentWeapon != null && !weaponManager.currentWeapon.installedModifications.Contains((WeaponModification)item))
                        {
                            buttons[0].onClick.AddListener(
                                () =>
                                {
                                    weaponManager.currentWeapon.InstallMod((WeaponModification)item);
                                    source.PlayOneShot(removeSound);
                                    RemoveItem(storedItem);
                                    ItemMenu.HideMenu_Static();
                                }
                            );
                        }
                    }
                    buttons[buttons.Count - 1].onClick.AddListener(
                            () =>
                            {
                                if (item == weaponManager.GetEquippedItem())
                                    weaponManager.UnEquipItem();
                                source.PlayOneShot(removeSound);
                                RemoveItem(storedItem, true);
                                ItemMenu.HideMenu_Static();
                            }
                        );
                    ItemMenu.DisplayMenu_Static();
                }
            }
        );
    }

    private void RemoveItem(StoredItem item, bool drop=false)
    {
        inventory.RemoveItem(item, drop);
        if (weaponManager.ItemIsHotkeyed(item.item))
        {
            Hotkey.RemoveHotkeyItem_static(item.item);
            weaponManager.RemoveItemFromHotkey(item.item);
        }
        DrawInventory();
    }

    private void MoveItem(GameObject gridObj, StoredItem item)
    {
        movingObject = gridObj;
        movingItem = item;
        movingObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        movingObject.GetComponent<Image>().color = new Color(movingObject.GetComponent<Image>().color.r, movingObject.GetComponent<Image>().color.b, movingObject.GetComponent<Image>().color.g, 0.5f);
        movingObject.transform.SetAsLastSibling();
        gridObj.transform.Find("QuantityText").gameObject.SetActive(false);
        gridObj.transform.Find("ItemHighlight").gameObject.SetActive(false);
        gridObj.transform.Find("EquippedIcon").gameObject.SetActive(false);
        gridObj.transform.Find("HotkeyNumber").gameObject.SetActive(false);
        foreach (GridHighlight slot in allSlots)
        {
            slot.movingItems = true;
        }
        foreach (HotKeySlotUI slot in Hotkey.GetHotkeySlots_Static())
        {
            slot.movingItems = true;
        }
        for (int i = 0; i < itemsGrid.childCount; i++)
        {
            if (itemsGrid.GetChild(i).GetComponent<UpdateHoveredItem>())
            {
                itemsGrid.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
        SelectedSlot.SetDimensions_Static(item.item.size.x, item.item.size.y);
        StartCoroutine(ItemMouseFollow());
    }

    IEnumerator CombinationHighlights()
    {
        while (!Input.GetMouseButtonDown(1))
        {
            yield return null;
        }
        Notify();
    }

    IEnumerator ItemMouseFollow()
    {
        startingOrientation = movingItem.orientation;
        //yield return new WaitForSeconds(0.5f);
        //Debug.Log("Clicked in");
        //if (Input.GetMouseButton(0))
        //{
        Vector3 mousePosition = Input.mousePosition;
        Tooltip.HideToolTip_Static();
        GridHighlight selectedSlot = SelectedSlot.GetSelectedSlot_static();
        while (!Input.GetMouseButtonDown(0))
        {
            if (Input.GetMouseButtonDown(1) && movingItem.item.size.x != movingItem.item.size.y)
            {
                ToggleOrientation();
            }
            //if (mousePosition != Input.mousePosition)
            //{
                Vector2 pos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, transform.parent.GetComponent<Canvas>().worldCamera, out pos);
                movingObject.transform.position = transform.TransformPoint(pos);
            //}
            //mousePosition = Input.mousePosition;
            //worldPosition.z = itemsGrid.localPosition.z;
            //movingObject.transform.localPosition = new Vector3(mousePosition.x, mousePosition.y, mousePosition.z);
            //movingObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, movingObject.transform.position.z);
            //movingObject.transform.position = new Vector3(mousePosition.x, mousePosition.y, movingObject.transform.position.z);
            if (selectedSlot != SelectedSlot.GetSelectedSlot_static())
            {
                DetectOpenSlots();
            }
            selectedSlot = SelectedSlot.GetSelectedSlot_static();
            yield return null;
        }
        RepositionMovingObject();
        foreach (GridHighlight slot in allSlots)
        {
            slot.movingItems = false;
        }
        foreach (HotKeySlotUI slot in Hotkey.GetHotkeySlots_Static())
        {
            slot.movingItems = false;
        }
        for (int i = 0; i < itemsGrid.childCount; i++)
        {
            if (itemsGrid.GetChild(i).GetComponent<UpdateHoveredItem>())
            {
                itemsGrid.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        //}
        //else
        //{
        //    yield return null;
        //}
    }

    private void DetectOpenSlots()
    {
        GridHighlight selection = SelectedSlot.GetSelectedSlot_static();

        foreach (GridHighlight slot in allSlots)
        {
            if (selection == null)
            {
                slot.UnHighlight();
                continue;
            }
            if (slot.slotPosition.x < (SelectedSlot.GetDimensions_Static().x + selection.slotPosition.x) && slot.slotPosition.x >= selection.slotPosition.x && slot.slotPosition.y < (SelectedSlot.GetDimensions_Static().y + selection.slotPosition.y) && slot.slotPosition.y >= selection.slotPosition.y)
            {
                slot.slotEligible = inventory.IsPositionValid(movingItem.item, selection.slotPosition.x, selection.slotPosition.y, movingItem);
                if (movingItem.item.itemCombinationsPossible.Count > 0)
                {
                    slot.canCombine = inventory.CombinationValid(movingItem.item, selection.slotPosition.x, selection.slotPosition.y, movingItem);
                }
                slot.Highlight();
            }
            else
            {
                slot.UnHighlight();
            }
        }
        /*
        int endPositionX = (selection.slotPosition.x + movingItem.item.size.x);
        if (inventory.size.x < endPositionX)
        {
            endPositionX = inventory.size.x - (selection.slotPosition.x + movingItem.item.size.x);
        }
        int endPositionY = (selection.slotPosition.y + movingItem.item.size.y);
        if (inventory.size .y < endPositionY)
        {
            endPositionY = inventory.size.y - (selection.slotPosition.y + movingItem.item.size.y);
        }

        if (movingObject != null)
        {
            for (int i = selection.slotPosition.x; i < endPositionX; i++)
            {
                for (int j = selection.slotPosition.y; j < endPositionY; j++)
                {
                    
                    Debug.Log(inventory.IsPositionValid(movingItem.item, i, j));
                }
            }
        }
        */
    }

    private void RepositionMovingObject()
    {
        if (movingObject != null)
        {
            int row = (int)(movingObject.transform.localPosition.y / unitSlot.x) * -1;
            int col = (int)(movingObject.transform.localPosition.x / unitSlot.y);

            if (Hotkey.GetHighlightedSlotIndex_Static() != -1)
            {
                source.PlayOneShot(equipSound);
                Hotkey.HotkeyItem_static(Hotkey.GetHighlightedSlotIndex_Static(), movingItem.item);
                if (startingOrientation != movingItem.orientation)
                {
                    ToggleOrientation();
                }
                inventory.MoveItem(movingItem, new IntPair(movingItem.position.x, movingItem.position.y));
            }

            else if (!inventory.MoveItem(movingItem, new IntPair(row, col)))
            {
                source.PlayOneShot(invalidMoveSound);
                if (startingOrientation != movingItem.orientation)
                {
                    ToggleOrientation();
                }
                inventory.MoveItem(movingItem, new IntPair(movingItem.position.x, movingItem.position.y));
            }
            else
            {
                if (inventory.CombinationValid(movingItem.item, row, col, movingItem))
                {
                    Debug.Log("Item combo valid");
                    foreach (ItemCombination combo in movingItem.item.itemCombinationsPossible)
                    {
                        StoredItem itemToFind = inventory.FindItem(combo.otherItemRequired);
                        if (itemToFind.position.x == movingItem.position.x && itemToFind.position.y == movingItem.position.y)
                        {
                            source.PlayOneShot(combo.combinationSound);
                            Combine(movingItem, itemToFind, combo.itemResult);
                            DynamicCursor.ChangeCursor_Static(CursorType.None);
                        }
                    }
                }
                else
                {
                    source.PlayOneShot(validMoveSound);
                }
                if (movingItem.orientation == ItemOrientation.Vertical && movingItem.item.verticalIcon != null)
                {
                    movingObject.GetComponent<Image>().sprite = movingItem.item.verticalIcon;
                }
                else if (startingOrientation != movingItem.orientation && movingItem.orientation == ItemOrientation.Horizontal)
                {
                    movingObject.GetComponent<Image>().sprite = movingItem.item.horizontalIcon;
                }
                movingObject.transform.Find("QuantityText").gameObject.SetActive(true);
                movingObject.transform.Find("EquippedIcon").gameObject.SetActive(true);
                movingObject.transform.Find("HotkeyNumber").gameObject.SetActive(true);
            }

            movingObject = null;
            movingItem = null;
        }
    }

    public void ToggleOrientation()
    {
        if (movingItem.orientation == ItemOrientation.Horizontal)
        {
            movingObject.GetComponent<Image>().sprite = movingItem.item.horizontalIcon;
            movingObject.transform.localEulerAngles = new Vector3(0, 0, -90);
            movingObject.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            movingItem.orientation = ItemOrientation.Vertical;
            movingItem.item.size = new IntPair(movingItem.item.size.y, movingItem.item.size.x);
            SelectedSlot.SetDimensions_Static(movingItem.item.size.x, movingItem.item.size.y);
        }
        else
        {
            movingObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            movingObject.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            movingItem.orientation = ItemOrientation.Horizontal;
            movingItem.item.size = new IntPair(movingItem.item.size.y, movingItem.item.size.x);
            movingObject.GetComponent<RectTransform>().sizeDelta = new Vector2(unitSlot.y * movingItem.item.size.y, unitSlot.x * movingItem.item.size.x);
            SelectedSlot.SetDimensions_Static(movingItem.item.size.x, movingItem.item.size.y);
            movingObject.GetComponent<Image>().sprite = movingItem.item.horizontalIcon;
        }
    }

    private void HighlightCombined(StoredItem storedItem)
    {
        foreach (UpdateHoveredItem itemObject in itemsGrid.GetComponentsInChildren<UpdateHoveredItem>())
        {
            itemObject.gameObject.GetComponent<UIClickNotifier>().onLeft.RemoveAllListeners();
            foreach (ItemCombination combo in storedItem.item.itemCombinationsPossible)
            {
                if (combo.otherItemRequired != itemObject.item)
                {
                    itemObject.gameObject.GetComponent<Image>().color = Color.gray;
                }
                else
                {
                    itemObject.gameObject.GetComponent<UIClickNotifier>().onLeft.AddListener(
                        () =>
                        {
                            StoredItem itemToFind = inventory.FindItem(itemObject.item);
                            source.PlayOneShot(combo.combinationSound);
                            Combine(storedItem, itemToFind, combo.itemResult);
                        }
                    );
                }
            }
        }
        StartCoroutine(CombinationHighlights());
    }

    private void Combine(StoredItem item1, StoredItem item2, Item result)
    {
        RemoveItem(item1);
        RemoveItem(item2);
        inventory.AddItem(result);
        foreach (UpdateHoveredItem itemObject in itemsGrid.GetComponentsInChildren<UpdateHoveredItem>())
        {
            itemObject.gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    public GameObject GetMovingObject()
    {
        return movingObject;
    }

    public void CloseView()
    {
        if (movingObject != null)
        {
            RepositionMovingObject();
        }
        if (weaponManager.currentWeapon != null && weaponManager.currentWeapon.twoHanded)
        {
            weaponManager.DisplayWeapon();
            InventoryManager.HidePhone_Static();
            InventoryManager.BlockPhone_static();
            PlayerInteraction.LockInteraction();
        }
        else if (weaponManager.currentItem != null)
        {
            weaponManager.equippedItemCursor.gameObject.SetActive(true);
        }
    }

    public void OpenView()
    {
        if (weaponManager.currentWeapon != null && weaponManager.currentWeapon.twoHanded)
        {
            weaponManager.HideWeapon();
        }
        else if (weaponManager.currentItem != null)
        {
            weaponManager.equippedItemCursor.gameObject.SetActive(false);
        }
    }

    public void ClearInventory()
    {
        inventory.items = new List<StoredItem>();
    }
}
