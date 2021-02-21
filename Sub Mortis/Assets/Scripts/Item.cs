using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Create Item")]
public class Item : ScriptableObject
{
    [Header("Item Info")]
    public ItemType itemType;
    public string itemName;
    [TextArea]
    public string description;
    [Header("Icons")]
    public Sprite horizontalIcon;
    public Sprite verticalIcon;
    public Sprite img;
    [Header("Inventory Properties")]
    public IntPair size;
    public bool stackable;
    public List<ItemCombination> itemCombinationsPossible;
    [Header("Item Model")]
    public GameObject droppedItem;
    [Header("Sounds")]
    public AudioClip onUseSound;

    public virtual void Use() { }

    public bool CanCombineWith(Item item)
    {
        foreach (ItemCombination combo in itemCombinationsPossible)
        {
            if (combo.otherItemRequired == item)
            {
                return true;
            }
        }
        return false;
    }
}
public enum ItemType
{
    Weapon,
    Consumable,
    Clothing,
    Currency,
    Key,
    Note,
    Misc,
    Ammo
}
