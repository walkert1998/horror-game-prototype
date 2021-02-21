using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageLocation : MonoBehaviour
{
    public float damageMultiplier = 1.0f;
    Health characterHealth;
    // Start is called before the first frame update
    void Start()
    {
        characterHealth = GetComponentInParent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageCharacter(int amount)
    {
        //characterHealth.DamageCharacter();
        transform.root.SendMessage("DamageCharacter", (int)(amount * damageMultiplier), SendMessageOptions.DontRequireReceiver);
    }

    public void HiddenShot(Transform source)
    {
        Debug.Log("New target is " + source.name);
        //transform.root.GetComponent<NPCAI>().lastKnownPosition = source.position;
        transform.root.GetComponent<NPCAI>().SetTarget(source);
    }
}

public enum DamageLocationType
{
    HEAD,
    TORSO,
    ARM,
    LEG
}
