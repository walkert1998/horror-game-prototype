using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private float originalHeight;
    [SerializeField]
    private float m_CrouchHeight = 0.7f;
    public bool crouched;
    Coroutine runningRoutine;
    CharacterController m_CharacterController;
    public Transform playerCam;
    Vector3 originalCamPosition;

    public KeyCode crouchKey = KeyCode.LeftControl;
    // Start is called before the first frame update
    void Start()
    {
        crouched = false;
        m_CharacterController = GetComponent<CharacterController>();
        originalHeight = m_CharacterController.height;
        originalCamPosition = playerCam.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(crouchKey) && !PlayerInteraction.interactionBlocked)
        {
            if (crouched)
            {
                RaycastHit hit;
                if (!Physics.Raycast(playerCam.position, transform.TransformDirection(Vector3.up * 1.0f), out hit, 1.0f))
                {
                    crouched = !crouched;
                    ToggleCrouch(crouched);
                }
            }
            else
            {
                crouched = !crouched;
                ToggleCrouch(crouched);
            }
        }
    }

    public void ToggleCrouch(bool crouching)
    {
        if (runningRoutine != null)
        {
            StopCoroutine(runningRoutine);
        }
        runningRoutine = StartCoroutine(CrouchSmooth(crouching));
    }

    IEnumerator CrouchSmooth(bool crouching)
    {
        if (!crouching)
        {
            float cameraDestination = originalCamPosition.y;
            while (m_CharacterController.height < originalHeight)
            {
                m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, originalHeight, Time.deltaTime * 15);
                m_CharacterController.center = Vector3.down * (originalHeight - m_CharacterController.height) / 2.0f;
                Vector3 newCameraPos = new Vector3(playerCam.localPosition.x, cameraDestination, playerCam.localPosition.z);
                playerCam.localPosition = Vector3.Lerp(playerCam.localPosition, newCameraPos, Time.deltaTime * 15);
                yield return null;
            }
        }
        else
        {
            //playerCam.localPosition = newCameraPos;
            float cameraDestination = -0.2f;
            while ((m_CharacterController.height - m_CrouchHeight) > 0.01)
            {
                Vector3 newCameraPos = new Vector3(playerCam.localPosition.x, cameraDestination, playerCam.localPosition.z);
                playerCam.localPosition = Vector3.Lerp(playerCam.localPosition, newCameraPos, Time.deltaTime * 10);
                m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, m_CrouchHeight, Time.deltaTime * 10);
                m_CharacterController.center = Vector3.down * (originalHeight - m_CharacterController.height) / 2.0f;
                yield return null;
            }
            Debug.Log("Finished crouching");
        }
    }
}
