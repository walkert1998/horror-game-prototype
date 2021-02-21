using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerDragInteraction : MonoBehaviour
{
    const float k_Spring = 1000.0f;
    const float k_Damper = 0.0f;
    const float k_Drag = 3.0f;
    const float k_AngularDrag = 3f;
    const float k_Distance = 0.0f;
    const bool k_AttachToCenterOfMass = false;
    public bool dragging = false;
    public float rayDistance;
    public bool canInteract = true;
    private SpringJoint m_SpringJoint;
    public ConfigurableJoint configurableJoint;
    public float ThrowStrength = 50f;
    public LayerMask included;
    private FirstPersonController controller;
    private Vector3 startingPos;

    private void Start()
    {
        controller = GetComponent<FirstPersonController>();
    }

    private void Update()
    {
        if (InventoryManager.IsInventoryOpen_Static())
        {
            return;
        }

        if(!canInteract)
        {
            return;
        }
        // Make sure the user pressed the mouse down
        if (!Input.GetMouseButtonDown(0))
        {
            //DynamicCursor.ChangeCursor_Static(CursorType.Target);
            return;
        }

        var mainCamera = FindCamera();

        // We need to actually hit an object
        RaycastHit hit = new RaycastHit();
        if (
            !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                             mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, rayDistance,
                             included))
        {
            //DynamicCursor.ChangeCursor_Static(CursorType.Target);
            return;
        }

        if (hit.transform.GetComponent<Lock>())
        {
            if (hit.transform.GetComponent<Lock>().locked)
            {
                return;
            }
        }

        if (hit.transform.GetComponent<PickUp>())
        {
            return;
        }

        // We need to hit a rigidbody that is not kinematic
        if (!hit.rigidbody || hit.rigidbody.isKinematic)
        {
            //DynamicCursor.ChangeCursor_Static(CursorType.Target);
            return;
        }
        //DynamicCursor.ChangeCursor_Static(CursorType.Grab);

        if (!m_SpringJoint)
        {
            var go = new GameObject("Rigidbody dragger");
            Rigidbody body = go.AddComponent<Rigidbody>();
            m_SpringJoint = go.AddComponent<SpringJoint>();
            body.isKinematic = true;
            startingPos = hit.rigidbody.transform.position;
            //if (hit.rigidbody.GetComponent<ConfigurableJoint>())
            //{
            //    ClampRange(hit.rigidbody.GetComponent<ConfigurableJoint>());
            //}
        }
        m_SpringJoint.transform.position = hit.point;
        m_SpringJoint.anchor = Vector3.zero;

        m_SpringJoint.spring = k_Spring;
        m_SpringJoint.damper = k_Damper;
        m_SpringJoint.maxDistance = k_Distance;
        m_SpringJoint.connectedBody = hit.rigidbody;

        StartCoroutine("DragObject", hit.distance);
    }

    //private IEnumerator holdObject()
    //{
    //    while(!Input.GetMouseButton(1))
    //    {
    //        Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

    //        Vector3 nextPos = playerCam.transform.position + playerCam.transform.forward * distance;
    //        Vector3 currPos = objectHeld.transform.position;

    //        objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

    //        if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
    //        {
    //            DropObject();
    //        }
    //    }
    //}

    private void ClampRange(ConfigurableJoint joint)
    {
        configurableJoint = joint;
        if (configurableJoint.xMotion.Equals(ConfigurableJointMotion.Limited))
        {
            m_SpringJoint.transform.position = new Vector3(Mathf.Clamp(startingPos.x, -configurableJoint.linearLimit.limit, configurableJoint.linearLimit.limit), 0, 0);
        }
        else if (configurableJoint.yMotion.Equals(ConfigurableJointMotion.Limited))
        {
            m_SpringJoint.transform.position = new Vector3(0, Mathf.Clamp(startingPos.y, -configurableJoint.linearLimit.limit, configurableJoint.linearLimit.limit), 0);
        }
        else if (configurableJoint.zMotion.Equals(ConfigurableJointMotion.Limited))
        {
            m_SpringJoint.transform.position = new Vector3(0, 0, Mathf.Clamp(startingPos.z, -configurableJoint.linearLimit.limit, configurableJoint.linearLimit.limit));
        }
    }


    private IEnumerator DragObject(float distance)
    {
        dragging = true;
        if (m_SpringJoint.connectedBody.transform.GetComponent<HingeJoint>() != null || m_SpringJoint.connectedBody.transform.GetComponent<ConfigurableJoint>() != null)
        {
            controller.m_CanLook = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            m_SpringJoint.connectedBody.freezeRotation = true;
            //m_SpringJoint.connectedBody.isKinematic = true;
        }
        Cursor.visible = true;
        var oldDrag = m_SpringJoint.connectedBody.drag;
        var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
        m_SpringJoint.connectedBody.drag = k_Drag;
        m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
        var mainCamera = FindCamera();
        DynamicCursor.ChangeCursor_Static(CursorType.Drag);
        DynamicCursor.HideCursor_Static();
        while (Input.GetMouseButton(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            m_SpringJoint.transform.position = ray.GetPoint(distance);
            //Vector3 nextPos = ray.GetPoint(distance);
            //Vector3 currPos = m_SpringJoint.transform.position;
            Debug.DrawLine(mainCamera.transform.position, ray.direction, Color.red, 0);
            //m_SpringJoint.connectedBody.velocity = (nextPos - currPos) * 10;
            if (Vector3.Distance(m_SpringJoint.connectedBody.transform.position, mainCamera.transform.position) > rayDistance)
            {
                break;
            }
            if (Input.GetMouseButtonDown(1))
            {
                ThrowObject();
                break;
            }
            yield return null;
        }
        dragging = false;
        //DynamicCursor.ChangeCursor_Static(CursorType.Grab);
        if (m_SpringJoint.connectedBody)
        {
            m_SpringJoint.connectedBody.freezeRotation = false;
            m_SpringJoint.connectedBody.drag = oldDrag;
            m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
            m_SpringJoint.connectedBody = null;
        }
        controller.m_CanLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        DynamicCursor.ShowCursor_Static();
    }

    private void ThrowObject()
    {
        m_SpringJoint.connectedBody.AddForce(Camera.main.transform.forward * ThrowStrength);
        //m_SpringJoint.connectedBody.isKinematic = false;
        //objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
        dragging = false;
    }


    private Camera FindCamera()
    {
        if (GetComponent<Camera>())
        {
            return GetComponent<Camera>();
        }

        return Camera.main;
    }
}
