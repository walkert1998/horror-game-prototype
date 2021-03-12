using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WheelCrank : MonoBehaviour
{
    public int RotationCount;
    public int RotationLimit = 3;

    public float rotatedAroundX;

    public Vector3 lastUp;

    public UnityEvent OnMaxRotation;

    public AudioClip rotateSound;

    AudioSource source;

    private void Awake()
    {
        rotatedAroundX = 0;
        source = GetComponent<AudioSource>();
        // initialize
        lastUp = transform.up;
    }

    private void Update()
    {
        var rotationDifference = Vector3.SignedAngle(transform.right, lastUp, transform.up);

        rotatedAroundX += rotationDifference;
        Debug.DrawRay(transform.position, transform.up * 3.0f, Color.blue);
        Debug.DrawRay(transform.position, transform.right * 3.0f, Color.red);
        Debug.DrawRay(transform.position, transform.forward * 3.0f, Color.yellow);

        if (rotatedAroundX >= 360.0f)
        {
            Debug.Log("One positive rotation done", this);

            RotationCount++;
            source.PlayOneShot(rotateSound);
            rotatedAroundX -= 360.0f;
        }
        else if (rotatedAroundX <= -360.0f)
        {
            Debug.Log("One negative rotation done", this);

            RotationCount--;
            source.PlayOneShot(rotateSound);

            rotatedAroundX += 360.0f;
        }

        // update last rotation
        lastUp = transform.right;


        // check for fire the event
        if (RotationCount >= RotationLimit)
        {
            OnMaxRotation?.Invoke();
            RotationCount = RotationLimit;
        }
    }
}
