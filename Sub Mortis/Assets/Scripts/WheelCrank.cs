using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCrank : MonoBehaviour
{
    public int maxThreshold;
    public int minThreshold;
    public float currentRotation = 0;
    public bool minValueReached = false;
    public bool maxValueReached = false;
    Rigidbody rigidbody;
    float lastAngle = 0;
    int revolutions = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lastAngle = transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float newAngle = transform.localEulerAngles.x;
        if (lastAngle > newAngle)
        {
            DecreaseRotation(Mathf.Abs(newAngle - lastAngle));
            //currentRotation -= newAngle;
            Debug.Log(newAngle);
        }
        else if (lastAngle < newAngle)
        {
            IncreaseRotation(Mathf.Abs(newAngle - lastAngle));
            //currentRotation += newAngle;
            Debug.Log(newAngle);
        }
        lastAngle = transform.localEulerAngles.x;
    }

    public void IncreaseRotation(float amount)
    {
        Debug.Log(amount);
        if (currentRotation + amount <= maxThreshold)
        {
            currentRotation += amount;
        }
        else
        {
            maxValueReached = true;
        }
    }

    public void DecreaseRotation(float amount)
    {
        Debug.Log(amount);
        if (currentRotation - amount >= minThreshold)
        {
            currentRotation -= amount;
        }
        else
        {
            minValueReached = true;
        }
    }
}
