using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCColliders : MonoBehaviour
{
    public Collider head;
    public List<Collider> otherColliders;
    public List<Rigidbody> rigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Collider col in GetComponentsInChildren<Collider>())
        {
            otherColliders.Add(col);
        }
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rigidbodies.Add(rb);
        }
        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableRagdoll()
    {
        foreach(Rigidbody body in rigidbodies)
        {
            body.isKinematic = true;
        }
    }

    public void EnableRagdoll()
    {
        foreach (Rigidbody body in rigidbodies)
        {
            body.isKinematic = false;
        }
    }

    public void DisableDamageSensors()
    {
        foreach (Collider col in otherColliders)
        {
            Destroy(col.GetComponent<DamageLocation>());
        }
    }
}
