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

	private float PickupRange = 3f;
	private float ThrowStrength = 50f;
	private float distance = 3f;
	private float maxDistanceGrab = 4f;

	private Ray playerAim;
	private GameObject objectHeld;
	public bool isObjectHeld;
	private bool tryPickupObject;

	private bool neverPickedUpBefore = true;

	private bool rotating;

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
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			MoveHeldObjectCloserToPlayer();
		}

		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			MoveHeldObjectAwayFromPlayer();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (ObjectGrab.m_FreezeRotation)
			{
				objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
			}
			GetComponent<FirstPersonController>().m_CanLook = false;
			rotating = true;
		}
	}

    void FixedUpdate()
	{
		if (Input.GetButton(GrabButton))
		{
			if (!isObjectHeld && !PlayerInteraction.interactionBlocked)
			{
				tryPickObject();
				tryPickupObject = true;
			}
			else if (isObjectHeld)
			{
				holdObject();
			}
		}
		else if (isObjectHeld)
		{
			DropObject();
		}


		if (Input.GetButton(ThrowButton) && isObjectHeld)
		{
			isObjectHeld = false;
			objectHeld.GetComponent<Rigidbody>().useGravity = true;
			ThrowObject();
		}

		//if (Input.GetButton(UseButton))
		//{
		//	isObjectHeld = false;
		//	tryPickObject();
		//	tryPickupObject = false;
		//	Use();
		//}
	}

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
				DynamicCursor.ChangeCursor_Static(CursorType.Drag);
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
				DynamicCursor.ChangeCursor_Static(CursorType.Drag);
				DynamicCursor.HideCursor_Static();
			}
			if (hit.collider.tag == Tags.m_DoorsTag && tryPickupObject)
			{
				isObjectHeld = true;
				PlayerInteraction.LockInteraction();
				objectHeld.GetComponent<Rigidbody>().useGravity = true;
                objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
				/**/
				PickupRange = DoorGrab.m_DoorPickupRange;
				ThrowStrength = DoorGrab.m_DoorThrow;
				distance = hit.distance;
				maxDistanceGrab = DoorGrab.m_DoorMaxGrab;
				DynamicCursor.ChangeCursor_Static(CursorType.Drag);
				DynamicCursor.HideCursor_Static();
				//originalRigidbodyPos = hit.collider.transform.position;
				//originalScreenTargetPosition = playerCam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
			}

			if (objectHeld.GetComponent<HingeJoint>() || objectHeld.GetComponent<ConfigurableJoint>())
			{
				controller.m_CanLook = false;
				controller.GetMouseLook().SetCursorLock(false);
			}
			else if (neverPickedUpBefore)
            {
				HelpText._DisplayHelpText("[Right-Click] to throw item. Hold [R] and move mouse to rotate. Use [Scrollwheel Up/Down] to move object further away/closer.", KeyCode.Mouse1, hints, 1.0f);
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

	private void holdObject()
	{
        Ray playerAim = playerCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        if (objectHeld.CompareTag("Door"))
        {
            Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
            Vector3 currPos = objectHeld.transform.position;
            objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;
        }
        else
        {
            //Debug.Log(objectHeld.GetComponent<Rigidbody>().velocity);
			Vector3 mousePositionOffset = playerCam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance)) - originalScreenTargetPosition;
			objectHeld.GetComponent<Rigidbody>().velocity = (originalRigidbodyPos + mousePositionOffset - objectHeld.transform.position) * 10;
        }



        if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
		{
			DropObject();
		}
	}

	private void DropObject()
	{
		isObjectHeld = false;
		if (objectHeld.GetComponent<HingeJoint>() || objectHeld.GetComponent<ConfigurableJoint>())
		{
			controller.m_CanLook = true;
			controller.GetMouseLook().SetCursorLock(true);
		}
		tryPickupObject = false;
        objectHeld.GetComponent<Rigidbody>().useGravity = true;
        objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		objectHeld = null;
		GetComponent<FirstPersonController>().m_CanLook = true;
		DynamicCursor.ChangeCursor_Static(CursorType.Target);
		DynamicCursor.HideCursor_Static();
		PlayerInteraction.UnlockInteraction();
	}

	private void ThrowObject()
	{
		isObjectHeld = false;
		if (objectHeld.GetComponent<HingeJoint>() || objectHeld.GetComponent<ConfigurableJoint>())
		{
			controller.m_CanLook = true;
			controller.GetMouseLook().SetCursorLock(true);
		}
		objectHeld.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * ThrowStrength, ForceMode.Impulse);
        objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
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
