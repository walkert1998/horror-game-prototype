using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPhysicsObjectPosition : MonoBehaviour
{
    public Vector3 startPosition;
    public void SetPosition()
    {
        transform.localPosition = startPosition;
    }
}
