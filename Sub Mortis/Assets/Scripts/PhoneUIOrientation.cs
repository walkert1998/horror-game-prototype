using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneUIOrientation : MonoBehaviour
{
    public Vector3 horizontalFocusedPosition;
    public Quaternion horizontalFocusedRotation;
    public Vector3 verticalFocusedPosition;
    public Quaternion verticalFocusedRotation;
    public bool vertical = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetToVertical()
    {
        vertical = true;
        transform.localPosition = verticalFocusedPosition;
        transform.localRotation = verticalFocusedRotation;
        //StartCoroutine(SetOrientation(horizontalFocusedPosition, horizontalFocusedRotation));
    }

    public void SetToHorizontal()
    {
        vertical = false;
        transform.localPosition = horizontalFocusedPosition;
        transform.localRotation = horizontalFocusedRotation;
        //StartCoroutine(SetOrientation(verticalFocusedPosition, verticalFocusedRotation));
    }

    IEnumerator SetOrientation(Vector3 targetPosition, Quaternion targetRotation)
    {
        while (transform.localPosition != targetPosition && transform.localRotation != targetRotation)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 2f * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, 2f * Time.deltaTime);
            yield return null;
        }
    }
}
