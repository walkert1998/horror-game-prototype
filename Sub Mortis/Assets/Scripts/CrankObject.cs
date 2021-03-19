using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankObject : MonoBehaviour
{
    public WheelCrank connectedCrank;
    public Vector3 minPosition;
    public Vector3 maxPosition;
    public Vector3 targetPosition;
    public float progress;
    public float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        progress = (connectedCrank.RotationCount * 360f + connectedCrank.rotatedAroundX) / (connectedCrank.RotationLimit * 360f);
        targetPosition = Vector3.Lerp(minPosition, maxPosition, progress);
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        //transform.position = Vector3.Lerp(minPosition, maxPosition, progress);
    }
}
