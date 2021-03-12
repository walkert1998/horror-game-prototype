using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollowObject : MonoBehaviour
{
    public Transform target;
    public Camera playerCam;
    public bool cursorOn;
    // Start is called before the first frame update
    void Start()
    {
        cursorOn = false;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCam.WorldToScreenPoint(target.position);
    }

    public void AssignTarget(Transform obj)
    {
        target = obj;
        if (target != null)
        {
            cursorOn = true;
            gameObject.SetActive(true);
        }
        else
        {
            cursorOn = false;
            gameObject.SetActive(false);
        }
    }
}
