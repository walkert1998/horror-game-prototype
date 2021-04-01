using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Create Inventory")]
public class Inventory : ObservableSO
{
    public List<StoredItem> items;
    public IntPair size;
    public GameObject assignedCharacter;
    public bool firstTimePickup = true;

    public void AddItem(StoredItem item)
    {
        items.Add(item);
        Notify();
    }

    public void RemoveItem(StoredItem item, bool drop=false)
    {
        items.Remove(item);
        if (item.item.droppedItem != null && drop)
        {
            GameObject newDrop = Instantiate(item.item.droppedItem, InventoryManager.GetInventoryView_Static().itemDropLocation.position, Quaternion.identity);
            newDrop.GetComponent<PickUp>().quantity = item.quantity;
        }
        Notify();
    }

    public void AddItemQuantity(Item item, int quantity)
    {
        StoredItem itemChanged = items.Find(StoredItem => StoredItem.item == item);
        itemChanged.quantity += quantity;
        Notify();
    }

    public void RemoveItemQuantity(Item item, int quantity, bool drop=false)
    {
        StoredItem itemChanged = items.Find(StoredItem => StoredItem.item == item);
        if (itemChanged != null)
        {
            Debug.Log("Removing " + quantity + " " + item.itemName + "'s");
            if (quantity >= itemChanged.quantity)
                items.Remove(itemChanged);
            else
                itemChanged.quantity -= quantity;

            if (item.droppedItem != null && drop)
            {
                GameObject newDrop = Instantiate(item.droppedItem, InventoryManager.GetInventoryView_Static().itemDropLocation.position, Quaternion.identity);
                newDrop.GetComponent<PickUp>().quantity = quantity;
            }
            Notify();
        }
    }

    public bool MoveItem(StoredItem toMove, IntPair newPos)
    {
        if (IsPositionValid(toMove.item, newPos.x, newPos.y, toMove) ||
            CombinationValid(toMove.item, newPos.x, newPos.y, toMove))
        {
            toMove.position = newPos;
            Notify();
            return true;
        }
        else
        {
            return false;
        }
    }

    private int FreeSlotsCount()
    {
        int occupied = 0;

        foreach (StoredItem item in items)
        {
            occupied += item.item.size.x * item.item.size.y;
        }

        return size.x * size.y - occupied;
    }

    private Item IsColliding (IntPair itemSize, int row, int col, StoredItem ignoreWith = null)
    {
        foreach (StoredItem item in items)
        {
            if (
                ABBintersectsABB(
                    item.position.y, item.position.x, item.item.size.y, item.item.size.x,
                    col, row, itemSize.y, itemSize.x
                )
                &&
                item != ignoreWith
            )
            {
                return item.item;
            }
        }
        return null;
    }

    private bool ABBintersectsABB(int ax, int ay, float aw, float ah, int bx, int by, float bw, float bh)
    {
        return (ax < bx + bw &&
                ax + aw > bx &&
                ay < by + bh &&
                ah + ay > by);
    }

    public bool IsPositionValid (Item item, int row, int col, StoredItem ignoreWith = null)
    {
        return InBounds(item.size, row, col) && IsColliding(item.size, row, col, ignoreWith) == null;
    }

    public bool CombinationValid(Item item, int row, int col, StoredItem ignoreWith = null)
    {
        return InBounds(item.size, row, col) && item.CanCombineWith(IsColliding(item.size, row, col, ignoreWith));
    }

    private IntPair FindValidPosition (Item item)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (IsPositionValid(item, i, j))
                {
                    return new IntPair(i, j);
                }
            }
        }

        return null;
    }

    public bool AddItem(Item item, int itemQuantity=1)
    {
        int totalSize = item.size.x * item.size.y;
        if (firstTimePickup)
        {
            HelpText._DisplayHelpText("You can interact with items you've collected in the 'Items' app on your phone.", KeyCode.None, null, 4.0f);
            firstTimePickup = false;
        }

        if (FreeSlotsCount() >= totalSize)
        {
            IntPair position = FindValidPosition(item);

            if (position != null)
            {
                StoredItem updatedItem = items.Find(foundItem => foundItem.item.itemName == item.itemName);
                if (item.stackable && items.Contains(updatedItem))
                {
                    Debug.Log("Adding " + itemQuantity + " item: " + item.itemName);
                    updatedItem.quantity += itemQuantity;
                }
                else
                {
                    Debug.Log("Adding " + itemQuantity + " item: " + item.itemName);
                    items.Add(new StoredItem(item, position, Quantity: itemQuantity));
                }
                Notify();
                return true;
            }
            else
            {
                //Debug.Log("Move items to make space");
                Examination.SetExamineTextUntilClick_static("I'll need to make more space for this before I can take it with me.\n[" + item.size.x + "x" + item.size.y + " space required]");
                return false;
            }
        }
        else
        {
            //Debug.Log("Inventory full");
            Examination.SetExamineTextUntilClick_static("I'll need to make more space for this before I can take it with me.\n[" + item.size.x + "x" + item.size.y + " space required]");
            return false;
        }
    }

    public StoredItem FindItem(Item itemToReturn)
    {
        StoredItem item = items.Find(StoredItem => StoredItem.item == itemToReturn);
        if (item != null)
        {
            return item;
        }
        return null;
    }

    public bool InBounds (IntPair itemSize, int row, int col)
    {
        return row >= 0 && row < size.x &&
               row + itemSize.x <= size.x &&
               col >= 0 && col < size.y &&
               col + itemSize.y <= size.y;
    }

    public void SetSize(int rows, int columns)
    {
        size = new IntPair(rows, columns);
        Notify();
    }
}
