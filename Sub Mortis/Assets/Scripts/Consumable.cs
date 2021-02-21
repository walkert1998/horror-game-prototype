using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create Consumable")]
public class Consumable : Item
{
    public int value;
    public ConsumableType consumableType;
    public float effectTime;


    public override void Use()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (consumableType == ConsumableType.Health)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (effectTime > 1)
                playerHealth.StartCoroutine(playerHealth.HealCharacterOverTime(value, effectTime));
            else
                playerHealth.HealCharacter(value);
        }
    }
}

public enum ConsumableType
{
    Health
}
