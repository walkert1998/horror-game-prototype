using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create Weapon Modifcation")]
public class WeaponModification : Item
{
    public ModificationType modificationType;
    public GameObject onWeaponModel;
    public int value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum ModificationType
{
    Silencer,
    AmmoIncrease,
    DamageIncrease,
    Scope
}
