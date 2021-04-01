using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemInteraction
{
    //List<ItemInteractionPair> itemsRequired;
    void Interact(Item itemUsed);
}
