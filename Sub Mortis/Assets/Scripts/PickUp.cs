using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUp : MonoBehaviour
{
    public string pickup_name;
    public bool picked_up = false;
    public Item item;
    private GameObject world_item;
    public int quantity;
    Inventory playerInventory;
    GameObject player;
    AudioSource source;
    public AudioClip pickUpSound;
    bool added;
    // Use this for initialization
    void Start ()
    {
        world_item = gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        source = player.GetComponent<AudioSource>();
        playerInventory = Resources.Load<ScriptableObject>("ScriptableObjects/PlayerInventory") as Inventory;
    }

    public void Pickup()
    {
        if (pickUpSound != null)
        {
            source.PlayOneShot(pickUpSound);
        }
        picked_up = true;
        if (playerInventory != null)
        {
            if (pickup_name == "Backpack")
            {
                playerInventory.SetSize(playerInventory.size.x + 4, playerInventory.size.y);
                added = true;
            }
            else if (pickup_name == "Phone")
            {
                player.GetComponent<PhonePickup>().PickupPhone();
                added = true;
            }
            else
            {
                added = playerInventory.AddItem(item, quantity);
            }
            if (added)
                Destroy(world_item);
        }
        else
            Debug.Log("Inventory is null");
    }
}
