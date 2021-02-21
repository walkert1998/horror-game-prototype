using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {
    public float distance;
    public Text prompt;
    NoteReader noteReader;
    ComputerInteraction computerInteraction;
    public PhoneCamera phoneCamera;
    private WeaponManager weaponManager;
    public LayerMask excluded;
    public DragRigidbodyUse dragInteraction;
    public GameMenu menu;
    public static bool interactionBlocked = false;

    // Use this for initialization
    void Start ()
    {
        noteReader = GetComponent<NoteReader>();
        weaponManager = GetComponent<WeaponManager>();
        computerInteraction = GetComponent<ComputerInteraction>();
        UnlockInteraction();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if (menu.gamePaused)
        //{
        //    return;
        //}
        if (interactionBlocked)
        {
            return;
        }
        //if (dragInteraction.isObjectHeld || InventoryManager.IsInventoryOpen_Static())
        //{
        //    return;
        //}
        RaycastHit hit = new RaycastHit();
        //if (!GetComponent<Stamina>().knockedDown)
        //{
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, distance, excluded))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * distance, Color.green);
            GameObject hitObject = hit.collider.gameObject;
            //Debug.Log(hit.collider.gameObject.name);
            if (phoneCamera.cameraActive && !phoneCamera.scanning)
            {
                if (hitObject.GetComponent<QRCode>() && !hitObject.GetComponent<QRCode>().scanned)
                {
                    phoneCamera.StartScanning(hitObject.GetComponent<QRCode>());
                }
                else
                {
                    phoneCamera.StopScan();
                    return;
                }
            }
            if (hitObject.GetComponent<ExamineText>())
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    Examination.SetExamineTextUntilClick_static(hitObject.GetComponent<ExamineText>().examineInfo);
                }
            }
            if (weaponManager.currentItem != null && weaponManager.currentItem != weaponManager.currentWeapon)
            {
                if (hitObject.GetComponent<Lock>())
                {
                    weaponManager.ToggleHighlightCursor(true);
                }
                else if (hitObject.GetComponent<PhysicsBlocker>())
                {
                    weaponManager.ToggleHighlightCursor(true);
                }
                else
                {
                    weaponManager.ToggleHighlightCursor(false);
                }
            }
            else if (hitObject.GetComponent<PickUp>())
            {
                DynamicCursor.ChangeCursor_Static(CursorType.Pickup);
                //if (hitObject.GetComponent<PickUp>().item.stackable)
                //{
                //    //prompt.text = "Pick Up " + hitObject.GetComponent<PickUp>().pickup_name + "(" + hitObject.GetComponent<PickUp>().quantity + ")";
                //}
                //else
                //{
                //    //prompt.text = "Pick Up " + hitObject.GetComponent<PickUp>().pickup_name;
                //}
                if (Input.GetMouseButtonDown(0))
                {
                    hitObject.GetComponent<PickUp>().Pickup();
                    //prompt.text = "";
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Examination.SetExamineTextUntilClick_static(hitObject.GetComponent<PickUp>().item.description);
                }
            }
            else if (hitObject.GetComponent<Note>())
            {
                //if (gameSettings.GetInteractionPrompts())
                //prompt.text = "Read " + hitObject.GetComponent<Note>().noteName;
                DynamicCursor.ChangeCursor_Static(CursorType.Pickup);
                if (Input.GetMouseButtonDown(0) && !noteReader.readingNote)
                {
                    noteReader.ReadNote(hitObject.GetComponent<Note>());
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    noteReader.ReadNote(hitObject.GetComponent<Note>());
                }
            }
            else if (hitObject.GetComponent<LightSwitch>())
            {
                DynamicCursor.ChangeCursor_Static(CursorType.Grab);
                if (!hitObject.GetComponent<LightSwitch>().powerSource.powerOn)
                {
                    //prompt.text = "Power Is Off";
                    if (Input.GetMouseButtonDown(0))
                    {
                        Examination.SetExamineTextUntilClick_static("Won't work until the power is turned back on.");
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        Examination.SetExamineTextUntilClick_static("Won't work until the power is turned back on.");
                    }
                }
                else
                {
                    if (hitObject.GetComponent<LightSwitch>().switchOn)
                    {
                        //prompt.text = "Turn Off";
                        if (Input.GetMouseButtonDown(0))
                        {
                            hitObject.GetComponent<LightSwitch>().ToggleLights(false);
                        }
                        else if (Input.GetMouseButtonDown(1))
                        {
                            Examination.SetExamineTextUntilClick_static("Light switch, seems like it still works.");
                        }
                    }
                    else
                    {
                        //prompt.text = "Turn On";
                        if (Input.GetMouseButtonDown(0))
                        {
                            hitObject.GetComponent<LightSwitch>().ToggleLights(true);
                        }
                        else if (Input.GetMouseButtonDown(1))
                        {
                            Examination.SetExamineTextUntilClick_static("Light switch, seems like it still works.");
                        }
                    }
                }
            }
            //else if (hitObject.GetComponent<PowerSource>())
            //{
            //    DynamicCursor.ChangeCursor_Static(CursorType.Grab);
            //    if (hitObject.GetComponent<PowerSource>().powerOn)
            //    {
            //        //prompt.text = "Turn Off Power ";
            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            hitObject.GetComponent<PowerSource>().TogglePower(false);
            //        }
            //    }
            //    else
            //    {
            //        //prompt.text = "Turn On Power ";
            //        if (Input.GetMouseButtonDown(0))
            //        {
            //            hitObject.GetComponent<PowerSource>().TogglePower(true);
            //        }
            //    }
            //}

            else if (hitObject.GetComponent<Lock>())
            {
                DynamicCursor.ChangeCursor_Static(CursorType.Grab);
                if (hitObject.GetComponent<Lock>().locked)
                {
                    //prompt.text = "Locked";
                    if (Input.GetMouseButtonDown(0))
                    {
                        Examination.SetExamineTextUntilClick_static("Locked.");
                    }
                }
                //if (GetComponent<Inventory>().ItemInInventory(hit.collider.GetComponent<Door>().keyRequired))
                //{
                //    prompt.text = "[E] Unlock Door";
                //    if (Input.GetKeyDown(KeyCode.E))
                //    {
                //        hit.collider.gameObject.GetComponent<Door>().UnlockDoor();
                //    }
                //}
                //else
                //{
                //    prompt.text = "[Locked] " + hit.collider.GetComponent<Door>().keyRequired + " Required";
                //}
            }
            else if (hitObject.GetComponent<DoorSequence>())
            {
                DynamicCursor.ChangeCursor_Static(CursorType.Door);
                if (Input.GetButtonDown("Fire1"))
                {
                    hitObject.GetComponent<DoorSequence>().LoadLevel();
                }
            }
            else if (hitObject.GetComponent<LevelTransition>())
            {
                DynamicCursor.ChangeCursor_Static(CursorType.Door);
                if (Input.GetButtonDown("Fire1"))
                {
                    hitObject.GetComponent<LevelTransition>().change_level(hitObject.GetComponent<LevelTransition>().level_name);
                }
                //if (GetComponent<Inventory>().ItemInInventory(hit.collider.GetComponent<Door>().keyRequired))
                //{
                //    prompt.text = "[E] Unlock Door";
                //    if (Input.GetKeyDown(KeyCode.E))
                //    {
                //        hit.collider.gameObject.GetComponent<Door>().UnlockDoor();
                //    }
                //}
                //else
                //{
                //    prompt.text = "[Locked] " + hit.collider.GetComponent<Door>().keyRequired + " Required";
                //}
            }
            else if (hitObject.GetComponent<QRCode>())
            {
                if (Input.GetButtonDown("Fire2"))
                {
                    if (!hitObject.GetComponent<QRCode>().scanned)
                    {
                        Examination.SetExamineTextUntilClick_static("QR code, might be something on it.");
                    }
                    else
                    {
                        Examination.SetExamineTextUntilClick_static("Already scanned.");
                    }
                }
            }
            else if (hitObject.GetComponent<Computer>())
            {
                DynamicCursor.ChangeCursor_Static(CursorType.Grab);
                if (Input.GetButtonDown("Fire1"))
                {
                    if (hitObject.GetComponent<Computer>().powerOn)
                    {
                        computerInteraction.DisplayComputerUI(hitObject.GetComponent<Computer>());
                    }
                    else
                    {
                        Examination.SetExamineTextUntilClick_static("There's no power.");
                    }
                }
            }
            else if (hitObject.GetComponent<Rigidbody>())
            {
                //prompt.text = "";
                DynamicCursor.ChangeCursor_Static(CursorType.Grab);
            }
            else
            {
                //prompt.text = "";
                if (!phoneCamera.cameraActive)
                {
                    DynamicCursor.ChangeCursor_Static(CursorType.Target);
                }
            }
            //else if (hitObject.GetComponent<ExamineObject>())
            //{
            //    //if (gameSettings.GetInteractionPrompts())
            //    prompt.text = "[E] Examine " + hitObject.GetComponent<ExamineObject>().objectName;
            //    if (Input.GetKeyDown(KeyCode.E))
            //    {
            //        ExamineText.SetExamineText_static(hitObject.GetComponent<ExamineObject>().examineText, 3.0f);
            //        if (hitObject.GetComponent<ExamineObject>().voiceLine != null)
            //        {
            //            GetComponent<AudioSource>().Stop();
            //            GetComponent<AudioSource>().clip = hitObject.GetComponent<ExamineObject>().voiceLine;
            //            GetComponent<AudioSource>().Play();
            //        }
            //    }
            //}
            //else if (hitObject.GetComponent<AudioLog>())
            //{
            //    //if (gameSettings.GetInteractionPrompts())
            //    prompt.text = "[E] Play " + hitObject.GetComponent<AudioLog>().fileName;
            //    if (Input.GetKeyDown(KeyCode.E))
            //    {
            //        AudioLogReader.PlayAudioLog_static(hitObject.GetComponent<AudioLog>());
            //    }
            //}
            /*
            else if (hitObject.GetComponent<ItemInteraction>())
            {
                //if (gameSettings.GetInteractionPrompts())
                prompt.text = "[E] Play " + hitObject.GetComponent<AudioLog>().fileName;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    AudioLogReader.PlayAudioLog_static(hitObject.GetComponent<AudioLog>());
                }
            }
            /*
            else if (hit.collider.gameObject.tag == "NPC")
            {
                if (hit.collider.gameObject.GetComponent<Conversation>())
                {
                    if (GetComponent<WeaponManager>().holstered)
                    {
                        prompt.text = "[E] Talk To " + hit.collider.gameObject.GetComponent<Conversation>().character_name;
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            gameObject.GetComponent<NPCDialogue>().npcTransform = hit.transform.Find("NPCHead");
                            Debug.Log("Worked in interaction");
                            gameObject.GetComponent<NPCDialogue>().initiate_dialogue(hit.collider.gameObject.GetComponent<Conversation>());
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.gameObject.GetComponent<CombatTaunt>().StartCoroutine("Taunt");
                    }
                }
            }

            else if (hit.collider.gameObject.tag == "Door")
            {
                if (GetComponent<Inventory>().ItemInInventory(hit.collider.GetComponent<Door>().keyRequired))
                {
                    prompt.text = "[E] Unlock Door";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.collider.gameObject.GetComponent<Door>().UnlockDoor();
                    }
                }
                else
                {
                    prompt.text = "[Locked] " + hit.collider.GetComponent<Door>().keyRequired + " Required";
                }
            }

            else if (hit.collider.gameObject.tag == "LevelTransition")
            {
                //prompt.text = "[F] Enter " + hit.collider.gameObject.GetComponent<LevelTransition>().scene_name;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    //hit.collider.gameObject.GetComponent<LevelTransition>().change_level(hit.collider.gameObject.GetComponent<LevelTransition>().scene_name);
                }
            }

            if (hit.collider.gameObject.tag == "Door")
            {
                if (hit.collider.gameObject)
                prompt.text = "[F] Pick Up " + hit.collider.gameObject.GetComponent<PickUp>().pickup_name;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    hit.collider.gameObject.GetComponent<PickUp>().Pickup();
                }
            }
            */
        }
        else
        {
            //prompt.text = "";
            //Debug.Log("Nothing");
            phoneCamera.StopScan();
            if (!phoneCamera.cameraActive && weaponManager.currentItem == null)
            {
                DynamicCursor.ChangeCursor_Static(CursorType.Target);
            }
            //DynamicCursor.HideCursor_Static();
            PlayerInteraction.UnlockInteraction();
            weaponManager.ToggleHighlightCursor(false);
        }
        //}

        //else
        //{
            //prompt.text = "";
        //}
        //if (is character)
        //{
        //    prompt.value = "Press 'F' to talk";
        //}
        //if (is item)
        //{
        //    prompt.value = "Press 'F' to pickup";
        //}
        //if (is vending machine)
        //{
        //    prompt.value = "Press 'F' to buy ($2)";
        //}
        //if (is locked)
        //{
        //    prompt.value = "Hold 'F' to lockpick";
        //}
        //if (is hackable)
        //{
        //    prompt.value = "Hold 'F' to hack";
        //}
        //if (is door)
        //{
        //    prompt.value = "Press 'F' to open";
        //}
    }

    public static void LockInteraction()
    {
        interactionBlocked = true;
        //Debug.Log("Locking");
    }

    public static void UnlockInteraction()
    {
        interactionBlocked = false;
        //Debug.Log("Unlocking");
    }
}
