using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create Throwable Weapon")]
public class ThrowableWeapon : Item
{
    [Header("Damage")]
    public DamageEffect effectType;
    public int damage = 0;

    [Header("Timing")]
    public float maxHoldTime = 1.5f;
    public float timeBetweenShots = 1.0f;
    public float nextShotTime = 1.0f;
    [Header("Weapon Model")]
    public GameObject gameModel;
    public GameObject thrownModel;
    public Animator animator;
    public bool recoverable = false;

    [Header("Sound Effects")]
    public AudioClip impactSound;
    public AudioClip weaponDrawSound;
    public AudioClip noAmmoSound;
    public AudioClip reloadSound;
    AudioSource source;
}
