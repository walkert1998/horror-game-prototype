using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldInteraction
{
    public List<Item> interactableItems;

    public virtual bool Interact(Item interact)
    {
        return interactableItems.Contains(interact);
    }
}
