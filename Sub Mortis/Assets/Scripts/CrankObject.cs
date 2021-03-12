using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrankObject : MonoBehaviour
{
    public WheelCrank connectedCrank;
    public Vector3 minPosition;
    public Vector3 maxPosition;
    public float progress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        progress = (connectedCrank.RotationCount * 360f + connectedCrank.rotatedAroundX) / (connectedCrank.RotationLimit * 360f);
        transform.position = Vector3.Lerp(minPosition, maxPosition, progress);
    }
}
