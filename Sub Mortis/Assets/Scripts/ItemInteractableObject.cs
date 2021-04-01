using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractableObject : MonoBehaviour, IItemInteraction
{
    [SerializeField]
    public List<ItemInteractionPair> interactions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Interact(Item itemUsed)
    {
        throw new System.NotImplementedException();
    }
}
