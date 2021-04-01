using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemInteractionPair
{
    public Item itemRequired;
    public List<UnityEvent> events;
}
