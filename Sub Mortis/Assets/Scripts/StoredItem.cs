using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class StoredItem
{
    public Item item;
    public int quantity = 1;
    public IntPair position;
    public ItemOrientation orientation;

    public StoredItem(Item Item, IntPair Position, ItemOrientation Orientation = ItemOrientation.Horizontal, int Quantity=1)
    {
        item = Item;
        position = Position;
        orientation = Orientation;
        quantity = Quantity;
    }
}

public enum ItemOrientation
{
    Horizontal,
    Vertical
}
