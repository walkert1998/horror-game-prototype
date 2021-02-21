using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBlocker : MonoBehaviour
{
    Rigidbody rb;
    //public Item itemRequired;
    public List<Item> possibleItems;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void Interact(Item itemUsed)
    {
        if (possibleItems.Contains(itemUsed))
        {
            rb.isKinematic = false;
            Destroy(this);
        }
    }
}
