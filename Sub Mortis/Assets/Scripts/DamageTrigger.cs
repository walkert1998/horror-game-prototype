using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    public int damageValue;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>())
        {
            other.SendMessage("DamageCharacter", damageValue, SendMessageOptions.DontRequireReceiver);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Health>())
        {
            other.SendMessage("DamageCharacter", damageValue, SendMessageOptions.DontRequireReceiver);
        }
    }
}
