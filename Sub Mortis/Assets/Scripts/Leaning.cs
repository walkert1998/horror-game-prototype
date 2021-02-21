using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Leaning : MonoBehaviour
{
    public FirstPersonController controller;
    public Transform cameraTransform;
    public Vector3 startingPosition;
    public Vector3 leanPosition;
    public Quaternion startingRotation;
    public Quaternion leanRotation;
    public float leanAmount = 10.0f;
    public bool leaningLeft = false;
    public bool leaningRight = false;
    public float leanSpeed = 1.0f;
    public float leanDistance = 0.5f;
    public float leanTimer = 0.0f;
    public LayerMask excluded;

    // Start is called before the first frame update
    void Start()
    {
        //controller = GetComponent<FirstPersonController>();
        cameraTransform = controller.transform.GetChild(0);
        startingPosition = cameraTransform.localPosition;
        startingRotation = cameraTransform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            leaningLeft = true;
            leaningRight = false;
        }
        else
        {
            leaningLeft = false;
        }

        if (Input.GetKey(KeyCode.E))
        {
            leaningRight = true;
            leaningLeft = false;
        }
        else
        {
            leaningRight = false;
        }
        Lean();
    }

    void Lean ()
    {
        if (leaningLeft)
        {
            controller.SetRotateZ(leanAmount, leanSpeed * 4);
            CalculateLeanDistance();
            Vector3 newPos = new Vector3(startingPosition.x - leanDistance, startingPosition.y, startingPosition.z);
            leanPosition = newPos;
            if (leanTimer < leanSpeed)
                leanTimer += Time.deltaTime * leanSpeed;
            cameraTransform.localPosition = Vector3.MoveTowards(cameraTransform.localPosition, leanPosition, leanTimer);
        }
        else if (leaningRight)
        {
            controller.SetRotateZ(-leanAmount, leanSpeed * 4);
            CalculateLeanDistance();
            Vector3 newPos = new Vector3(startingPosition.x + leanDistance, startingPosition.y, startingPosition.z);
            leanPosition = newPos;
            if (leanTimer < leanSpeed)
                leanTimer += Time.deltaTime * leanSpeed;
            cameraTransform.localPosition = Vector3.MoveTowards(cameraTransform.localPosition, leanPosition, leanTimer);
        }
        else
        {
            leanTimer = 0f;
            controller.ResetCamRotation();
            controller.SetRotateZ(startingRotation.eulerAngles.z, leanSpeed * 4);
            cameraTransform.localPosition = Vector3.MoveTowards(cameraTransform.localPosition, startingPosition, Time.deltaTime * leanSpeed);
        }
    }

    public void CalculateLeanDistance ()
    {
        if (leaningLeft)
        {
            RaycastHit hit;
            Debug.DrawRay(cameraTransform.position, cameraTransform.TransformDirection(Vector3.left * 0.5f), Color.red, 0.5f);

            if (Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.left * 0.5f), out hit, 0.5f, excluded))
            {
                leanDistance = hit.distance;
            }
            else
            {
                leanDistance = 0.5f;
            }
        }

        if (leaningRight)
        {
            RaycastHit hit;
            Debug.DrawRay(cameraTransform.position, cameraTransform.TransformDirection(Vector3.right * 0.5f), Color.red, 0.5f);
            if (Physics.Raycast(cameraTransform.position, cameraTransform.TransformDirection(Vector3.right * 0.5f), out hit, 0.5f, excluded))
            {
                leanDistance = hit.distance;
            }
            else
            {
                leanDistance = 0.5f;
            }
        }
    }
}
