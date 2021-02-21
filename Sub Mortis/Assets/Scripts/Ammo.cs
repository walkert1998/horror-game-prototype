using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create Ammo")]
public class Ammo : Item
{
    public int damage;
    public DamageEffect ammoEffect = DamageEffect.None;
}

public enum DamageEffect
{
    None,
    Fire,
    EMP,
    Poison,
    Knockout,
    Explosive
}
