using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour, IItemInteraction
{
    public bool locked = true;
    public Item key;

    public void Interact(Item itemUsed = null)
    {
        if (itemUsed == key || itemUsed == null)
        {
            locked = false;
            Destroy(this);
        }
    }
}
