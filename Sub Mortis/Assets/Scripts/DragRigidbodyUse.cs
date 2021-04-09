using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
DragRigidbodyUse.cs ver. 11.1.16 - wirted by ThunderWire Games * Script for Drag & Drop & Throw Objects & Draggable Door & PickupObjects
*/

[System.Serializable]
public class GrabObjectClass
{
	public bool m_FreezeRotation;
	public float m_PickupRange = 3f;
	public float m_ThrowStrength = 50f;
	public float m_distance = 3f;
	public float m_maxDistanceGrab = 4f;
}

[System.Serializable]
public class ItemGrabClass
{
	public bool m_FreezeRotation;
	public float m_ItemPickupRange = 2f;
	public float m_ItemThrow = 45f;
	public float m_ItemDistance = 1f;
	public float m_ItemMaxGrab = 2.5f;
}

[System.Serializable]
public class DoorGrabClass
{
	public float m_DoorPickupRange = 2f;
	public float m_DoorThrow = 10f;
	public float m_DoorDistance = 2f;
	public float m_DoorMaxGrab = 3f;
}

[System.Serializable]
public class TagsClass
{
	public string m_InteractTag = "Interact";
	public string m_InteractItemsTag = "InteractItem";
	public string m_DoorsTag = "Door";
}

public class DragRigidbodyUse : MonoBehaviour
{

	public GameObject playerCam;

	public string GrabButton = "Grab";
	public string ThrowButton = "Throw";
	public string UseButton = "Use";
	public GrabObjectClass ObjectGrab = new GrabObjectClass();
	public ItemGrabClass ItemGrab = new ItemGrabClass();
	public DoorGrabClass DoorGrab = new DoorGrabClass();
	public TagsClass Tags = new TagsClass();
	public LayerMask included;

	public List<KeyCode> hints;

	public FirstPersonController controller;
	GameObject dragPoint;

	private float PickupRange = 3f;
	private float ThrowStrength = 50f;
	private float distance = 3f;
	private float maxDistanceGrab = 4f;

	public float maxSpeed = 3.0f;

	private Ray playerAim;
	public GameObject objectHeld;
	public bool isObjectHeld;
	private bool tryPickupObject;

	private bool neverPickedUpBefore = true;

	private bool rotating;

	float lastDist;

	public CursorFollowObject cursor;

	public HingeJoint hinge;
	public ConfigurableJoint configJoint;
	public WheelCrank wheelCrank;
	public Lever lever;

	Vector3 originalScreenTargetPosition;
	Vector3 originalRigidbodyPos;

	void Start()
	{
		hints = new List<KeyCode>();
		hints.Add(KeyCode.Mouse1);
		hints.Add(KeyCode.R);
		isObjectHeld = false;
		tryPickupObject = false;
		rotating = false;
		objectHeld = null;
	}

    private void Update()
	{
		if (Input.GetButton(GrabButton) && isObjectHeld)
		{
			holdObject();
		}
		else if (Input.GetMouseButtonDown(0))
		{
			if (!PlayerInteraction.interactionBlocked && !InventoryManager.IsInventoryOpen_Static())
			{
				tryPickObject();
				tryPickupObject = true;
			}
		}
		else if (isObjectHeld)
		{
			DropObject();
		}
		if (rotating && isObjectHeld)
		{
			if (Input.GetKey(KeyCode.R))
			{
				RotateHeldObject();
			}
			else
			{
				rotating = false;
				GetComponent<FirstPersonController>().m_CanLook = true;
				if (objectHeld != null)
				{
					if (ObjectGrab.m_FreezeRotation)
					{
						objectHeld.GetComponent<Rigidbody>().freezeRotation = true;
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape) && isObjectHeld)
		{
			DropObject();
		}
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			MoveHeldObjectCloserToPlayer();
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			MoveHeldObjectAwayFromPlayer();
		}

		if (Input.GetKeyDown(KeyCode.R) && objectHeld.GetComponent<ImpactSounds>())
		{
			if (ObjectGrab.m_FreezeRotation)
			{
				objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
			}
			GetComponent<FirstPersonController>().m_CanLook = false;
			rotating = true;
		}


		if (Input.GetButtonDown(ThrowButton) && isObjectHeld)
		{
			isObjectHeld = false;
			objectHeld.GetComponent<Rigidbody>().useGravity = true;
			ThrowObject();
		}
	}

 //   void FixedUpdate()
	//{

	//	//if (Input.GetButton(UseButton))
	//	//{
	//	//	isObjectHeld = false;
	//	//	tryPickObject();
	//	//	tryPickupObject = false;
	//	//	Use();
	//	//}
	//}

    private void RotateHeldObject()
	{
        //objectHeld.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), Space.World);
        objectHeld.transform.Rotate(playerCam.transform.up, Input.GetAxis("Mouse X") * 2.0f, Space.World);
        objectHeld.transform.Rotate(playerCam.transform.right, Input.GetAxis("Mouse Y") * 2.0f, Space.World);
    }

	private void tryPickObject()
	{
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;

		if (Physics.Raycast(playerAim, out hit, PickupRange, included))
		{
			objectHeld = hit.collider.gameObject;
			if (objectHeld.GetComponent<Rigidbody>() == null)
            {
				return;
            }
			if (objectHeld.GetComponent<Lock>())
			{
				if (objectHeld.GetComponent<Lock>().locked)
				{
					return;
				}
			}

			if (objectHeld.GetComponent<PickUp>())
			{
				return;
			}

			if (objectHeld.GetComponent<PhysicsBlocker>())
			{
				return;
			}
			if (hit.collider.tag == Tags.m_InteractTag && tryPickupObject)
			{
				isObjectHeld = true;
				PlayerInteraction.LockInteraction();
				objectHeld.GetComponent<Rigidbody>().useGravity = false;
                if (ObjectGrab.m_FreezeRotation)
                {
                    objectHeld.GetComponent<Rigidbody>().freezeRotation = true;
                }
                if (ObjectGrab.m_FreezeRotation == false)
                {
                    objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
                }
                /**/
                PickupRange = ObjectGrab.m_PickupRange;
				ThrowStrength = ObjectGrab.m_ThrowStrength;
				distance = hit.distance;
				maxDistanceGrab = ObjectGrab.m_maxDistanceGrab;
				originalRigidbodyPos = hit.collider.transform.position;
				originalScreenTargetPosition = playerCam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
				//DynamicCursor.ChangeCursor_Static(CursorType.Drag);
				DynamicCursor.HideCursor_Static();
			}
			if (hit.collider.tag == Tags.m_InteractItemsTag && tryPickupObject)
			{
				isObjectHeld = true;
				PlayerInteraction.LockInteraction();
				objectHeld.GetComponent<Rigidbody>().useGravity = true;
                if (ItemGrab.m_FreezeRotation)
                {
                    objectHeld.GetComponent<Rigidbody>().freezeRotation = true;
                }
                if (ItemGrab.m_FreezeRotation == false)
                {
                    objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
                }
                /**/
                PickupRange = ItemGrab.m_ItemPickupRange;
				ThrowStrength = ItemGrab.m_ItemThrow;
				distance = hit.distance;
				maxDistanceGrab = ItemGrab.m_ItemMaxGrab;
				originalRigidbodyPos = hit.collider.transform.position;
				originalScreenTargetPosition = playerCam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
				//DynamicCursor.ChangeCursor_Static(CursorType.Drag);
				DynamicCursor.HideCursor_Static();
			}
			if (hit.collider.tag == Tags.m_DoorsTag && tryPickupObject)
			{
				isObjectHeld = true;
				PlayerInteraction.LockInteraction();
				objectHeld.GetComponent<Rigidbody>().useGravity = false;
                objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
				hinge = objectHeld.GetComponent<HingeJoint>();
				lever = objectHeld.GetComponent<Lever>();
				wheelCrank = objectHeld.GetComponent<WheelCrank>();
				/**/
				PickupRange = DoorGrab.m_DoorPickupRange;
				ThrowStrength = DoorGrab.m_DoorThrow;
				distance = hit.distance;
				maxDistanceGrab = DoorGrab.m_DoorMaxGrab;
				//DynamicCursor.ChangeCursor_Static(CursorType.Drag);
				DynamicCursor.HideCursor_Static();
				//originalRigidbodyPos = hit.collider.transform.position;
				//originalScreenTargetPosition = playerCam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
			}

			if (objectHeld.GetComponent<HingeJoint>())
			{
				isObjectHeld = true;
				controller.m_CanLook = false;
				PlayerInteraction.LockInteraction();
				hinge = objectHeld.GetComponent<HingeJoint>();
				cursor.AssignTarget(objectHeld.GetComponent<CursorConnectPoint>().connectPoint);
				DynamicCursor.HideCursor_Static();
				wheelCrank = objectHeld.GetComponent<WheelCrank>();
				//controller.GetMouseLook().SetCursorLock(false);
			}
			else if (objectHeld.GetComponent<ConfigurableJoint>())
			{
				isObjectHeld = true;
				controller.m_CanLook = false;
				PlayerInteraction.LockInteraction();
				configJoint = objectHeld.GetComponent<ConfigurableJoint>();
				cursor.AssignTarget(objectHeld.GetComponent<CursorConnectPoint>().connectPoint);
				DynamicCursor.HideCursor_Static();
				//controller.GetMouseLook().SetCursorLock(false);
			}
			else if (neverPickedUpBefore)
            {
				HelpText._DisplayHelpText("[Right-Click] to throw item. Hold [R] and move mouse to rotate. Use [Scrollwheel Up/Down] to move object further away/closer.", KeyCode.Mouse1, hints, 3.0f);
				neverPickedUpBefore = false;
			}
		}
	}

	private void MoveHeldObjectCloserToPlayer()
    {
		if (distance - 0.2f > 0.2f)
			distance -= 0.2f;
		//objectHeld.GetComponent<Rigidbody>().velocity = (objectHeld.transform.position - playerCam.transform.position) * 5;
	}

	private void MoveHeldObjectAwayFromPlayer()
	{
		if (distance < 1.8f)
			distance += 0.2f;
		//objectHeld.GetComponent<Rigidbody>().velocity = (objectHeld.transform.position - playerCam.transform.position) * 5;
	}

	private void RotateWheel()
    {
		Vector3 wheelVelocity = new Vector3(0,0,0);
		objectHeld.GetComponent<Rigidbody>().velocity = wheelVelocity;
	}

	private void SlideObject()
    {
		Vector3 mouseCamDir = playerCam.transform.up * Input.GetAxis("Mouse Y") + playerCam.transform.right * Input.GetAxis("Mouse X");
		mouseCamDir = Vector3.ClampMagnitude(mouseCamDir, maxSpeed);
        objectHeld.GetComponent<Rigidbody>().velocity = mouseCamDir;
        //objectHeld.GetComponent<Rigidbody>().AddForce(mouseCamDir, ForceMode.Force);
    }

	private void RotateHinge()
    {
		if (lever)
		{
			Vector3 center = Vector3.Scale(objectHeld.transform.position, objectHeld.GetComponent<Renderer>().bounds.center);
			Vector3 jointBody = Vector3.Normalize(center - hinge.anchor);
			Vector3 upJointForward = Vector3.Cross(playerCam.transform.up, hinge.axis);
			Vector3 upAdd = upJointForward * Vector3.Dot(playerCam.transform.up, jointBody);
			Vector3 up = Vector3.Normalize(upAdd + playerCam.transform.up);
			Vector3 rightJointForward = Vector3.Cross(playerCam.transform.right, hinge.axis);
			Vector3 rightAdd = rightJointForward * Vector3.Dot(playerCam.transform.right, jointBody);
			Vector3 right = Vector3.Normalize(playerCam.transform.right + rightAdd);
			Vector3 rotVelocity = (up) * Input.GetAxis("Mouse Y") + right * Input.GetAxis("Mouse X");
			Vector3 pushDir = Vector3.Cross(jointBody, rotVelocity);
			Debug.DrawRay(objectHeld.transform.position, upJointForward * distance, Color.green);
			Debug.DrawRay(objectHeld.transform.position, rightJointForward * distance, Color.red);
			Debug.DrawRay(objectHeld.transform.position, pushDir * distance, Color.blue);
			Debug.DrawRay(objectHeld.transform.position, rotVelocity * distance, Color.cyan);
			rotVelocity = Vector3.ClampMagnitude(rotVelocity, maxSpeed);
            objectHeld.GetComponent<Rigidbody>().velocity = rotVelocity;
            //objectHeld.GetComponent<Rigidbody>().AddForce(rotVelocity);
			//objectHeld.GetComponent<Rigidbody>().velocity.Set(Vector3.Dot(pushDir, objectHeld.GetComponent<HingeJoint>().axis), Vector3.Dot(pushDir, objectHeld.GetComponent<HingeJoint>().axis), Vector3.Dot(pushDir, objectHeld.GetComponent<HingeJoint>().axis));
		}
        //else if (true)
        //{
        //	float dirMul = 1;
        //	if (Vector3.Dot(hinge.axis, playerCam.transform.forward) < 0)
        //	{
        //		dirMul = -dirMul;
        //	}
        //	objectHeld.GetComponent<Rigidbody>().velocity = (playerCam.transform.up + playerCam.transform.forward) * (dirMul * 1.0f) * Input.GetAxis("Mouse Y") + playerCam.transform.right * Input.GetAxis("Mouse X"); ;
        //}
        else if (wheelCrank)
		{
			float dist = lastDist - objectHeld.GetComponent<WheelCrank>().rotatedAroundX;
			Vector3 center = Vector3.Scale(objectHeld.transform.position, objectHeld.GetComponent<Renderer>().bounds.center);
            Vector3 jointBody = Vector3.Normalize(center - hinge.anchor);
            Vector3 upJointForward = Vector3.Cross(playerCam.transform.up, hinge.axis);
            Vector3 upAdd = upJointForward * Vector3.Dot(playerCam.transform.up, jointBody);
            Vector3 up = Vector3.Normalize(upAdd + playerCam.transform.up);
            Vector3 rightJointForward = Vector3.Cross(playerCam.transform.right, hinge.axis);
            Vector3 rightAdd = rightJointForward * Vector3.Dot(playerCam.transform.right, jointBody);
            Vector3 right = Vector3.Normalize(playerCam.transform.right + rightAdd);
            Vector3 rotVelocity = (up) * Input.GetAxis("Mouse Y") + right * Input.GetAxis("Mouse X");
            Vector3 pushDir = Vector3.Cross(jointBody, rotVelocity);
			Vector3 currentPos = objectHeld.transform.position;
            //Debug.DrawRay(objectHeld.transform.position, upJointForward * distance, Color.green);
            //Debug.DrawRay(objectHeld.transform.position, rightJointForward * distance, Color.red);
            //Debug.DrawRay(objectHeld.transform.position, pushDir * distance, Color.blue);
            //Debug.DrawRay(objectHeld.transform.position, rotVelocity * distance, Color.cyan);
            //Debug.Log(dist);
            // Currently blocks player because the crank drops for some reason
            // Crank should hit max at opposite of rotation limit
            if (wheelCrank.rotatedAroundX + (wheelCrank.RotationCount * 360.0f) <= -(wheelCrank.RotationLimit * 360.0f) - 20.0f
                && lastDist < dist)
            {
				hinge.useLimits = true;
			}
            else if (wheelCrank.rotatedAroundX + (wheelCrank.RotationCount * 360.0f) >= (wheelCrank.RotationLimit * 360.0f)
                && lastDist > dist)
			{
				hinge.useLimits = true;
			}
            else
			{
				hinge.useLimits = false;
			}
			rotVelocity = Vector3.ClampMagnitude(rotVelocity, maxSpeed);
            objectHeld.GetComponent<Rigidbody>().velocity = rotVelocity;
            //objectHeld.GetComponent<Rigidbody>().AddForce(rotVelocity, ForceMode.VelocityChange);
			lastDist = wheelCrank.rotatedAroundX;
		}
        else
		{
			Vector3 rotationVel = (playerCam.transform.up + playerCam.transform.forward) * Input.GetAxis("Mouse Y") + playerCam.transform.right * Input.GetAxis("Mouse X");
			rotationVel = Vector3.ClampMagnitude(rotationVel, maxSpeed * 2);
            objectHeld.GetComponent<Rigidbody>().velocity = rotationVel;
            //objectHeld.GetComponent<Rigidbody>().AddForce(rotationVel, ForceMode.VelocityChange);

        }
    }

	private void holdObject()
	{
        Ray playerAim = playerCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		if (hinge)
        {
            Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
            Vector3 currPos = objectHeld.transform.position;
			//objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;

			RotateHinge();
        }

		else if (configJoint)
        {
			SlideObject();
        }

        else
        {
            //Debug.Log(objectHeld.GetComponent<Rigidbody>().velocity);
			Vector3 mousePositionOffset = playerCam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance)) - originalScreenTargetPosition;
			objectHeld.GetComponent<Rigidbody>().velocity = (originalRigidbodyPos + mousePositionOffset - objectHeld.transform.position) * 10;
        }

        if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
		{
			Destroy(dragPoint);
			dragPoint = null;
			DropObject();
		}
	}

	private void DropObject()
	{
		isObjectHeld = false;
		tryPickupObject = false;
		if (!wheelCrank)
		{
			objectHeld.GetComponent<Rigidbody>().useGravity = true;
		}
        objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		if (objectHeld.GetComponent<HingeJoint>() || objectHeld.GetComponent<ConfigurableJoint>())
		{
			controller.m_CanLook = true;
			controller.GetMouseLook().SetCursorLock(true);
			hinge = null;
			configJoint = null;
			if (wheelCrank)
			{
				objectHeld.GetComponent<Rigidbody>().velocity = Vector3.zero;
				wheelCrank = null;
			}
			else if (lever)
			{
				lever = null;
			}
		}
		objectHeld = null;
		GetComponent<FirstPersonController>().m_CanLook = true;
		cursor.AssignTarget(null);
		DynamicCursor.ChangeCursor_Static(CursorType.Target);
		DynamicCursor.HideCursor_Static();
		PlayerInteraction.UnlockInteraction();
	}

	private void ThrowObject()
	{
		isObjectHeld = false;
		if (!wheelCrank)
		{
			objectHeld.GetComponent<Rigidbody>().useGravity = true;
		}
		if (objectHeld.GetComponent<HingeJoint>() || objectHeld.GetComponent<ConfigurableJoint>())
		{
			controller.m_CanLook = true;
			Destroy(dragPoint);
			dragPoint = null;
			hinge = null;
			configJoint = null;
			controller.GetMouseLook().SetCursorLock(true);
			if (wheelCrank)
			{
				objectHeld.GetComponent<Rigidbody>().velocity = Vector3.zero;
				wheelCrank = null;
			}
			else if (lever)
			{
				lever = null;
			}
		}
		objectHeld.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * ThrowStrength, ForceMode.Impulse);
        objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		cursor.AssignTarget(null);
		objectHeld = null;
		GetComponent<FirstPersonController>().m_CanLook = true;
		DynamicCursor.ChangeCursor_Static(CursorType.Target);
		DynamicCursor.HideCursor_Static();
        PlayerInteraction.UnlockInteraction();
	}

	private void Use()
	{
		objectHeld.SendMessage("UseObject", SendMessageOptions.DontRequireReceiver); //Every script attached to the PickupObject that has a UseObject function will be called.
		objectHeld = null;
	}
}
