using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public float onAngle;
    public float offAngle;
    public bool on;
    HingeJoint joint;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void FixedUpdate()
    {
        if (on)
        {
            return;
        }
        if (Mathf.Abs(onAngle - joint.angle) <= 3.0f)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
        //Debug.Log(joint.angle + " " + onAngle);
        //Debug.Log(Mathf.Abs(onAngle - joint.angle));
    }

    public void TurnOn()
    {
        on = true;
        rb.isKinematic = true;
        if (GetComponent<PowerSource>())
        {
            GetComponent<PowerSource>().TogglePower(true);
        }
        this.enabled = false;
    }

    public void TurnOff()
    {
        on = false;
        //rb.isKinematic = false;
    }
}
