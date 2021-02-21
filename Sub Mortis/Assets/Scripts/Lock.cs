using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public bool locked = true;
    public Item key;
    public void Unlock()
    {
        locked = false;
        Destroy(this);
    }
}
