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
    public Camera lightCamera;
    public Transform playerLightObject;
    public Vector3 originalLightObjPos;
    public Vector3 crouchLightObjPos;
    public float originalLightObjHeight;
    public float crouchLightObjHeight;
    Vector3 originalCamPosition;

    public KeyCode crouchKey = KeyCode.LeftControl;
    // Start is called before the first frame update
    void Start()
    {
        crouched = false;
        m_CharacterController = GetComponent<CharacterController>();
        originalHeight = m_CharacterController.height;
        originalCamPosition = playerCam.localPosition;
        originalLightObjHeight = playerLightObject.localScale.y;
        originalLightObjPos = playerLightObject.localPosition;
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
            Vector3 newCameraPos = new Vector3(playerCam.localPosition.x, cameraDestination, playerCam.localPosition.z);
            Vector3 lightScale = new Vector3(playerLightObject.localScale.x, originalLightObjHeight, playerLightObject.localScale.z);
            while (m_CharacterController.height < originalHeight)
            {
                m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, originalHeight, Time.deltaTime * 15);
                m_CharacterController.center = Vector3.down * (originalHeight - m_CharacterController.height) / 2.0f;
                newCameraPos.Set(playerCam.localPosition.x, cameraDestination, playerCam.localPosition.z);
                playerCam.localPosition = Vector3.Lerp(playerCam.localPosition, newCameraPos, Time.deltaTime * 15);
                playerLightObject.localScale = Vector3.Lerp(playerLightObject.localScale, lightScale, Time.deltaTime * 15);
                playerLightObject.localPosition = Vector3.Lerp(playerLightObject.localPosition, originalLightObjPos, Time.deltaTime * 15);
                lightCamera.farClipPlane = Mathf.Lerp(lightCamera.farClipPlane, 0.9f, Time.deltaTime * 15);
                yield return null;
            }
        }
        else
        {
            //playerCam.localPosition = newCameraPos;
            float cameraDestination = -0.2f;
            Vector3 newCameraPos = new Vector3(playerCam.localPosition.x, cameraDestination, playerCam.localPosition.z);
            Vector3 lightScale = new Vector3(playerLightObject.localScale.x, crouchLightObjHeight, playerLightObject.localScale.z);
            while ((m_CharacterController.height - m_CrouchHeight) > 0.01)
            {
                newCameraPos.Set(playerCam.localPosition.x, cameraDestination, playerCam.localPosition.z);
                playerLightObject.localScale = Vector3.Lerp(playerLightObject.localScale, lightScale, Time.deltaTime * 15);
                playerCam.localPosition = Vector3.Lerp(playerCam.localPosition, newCameraPos, Time.deltaTime * 10);
                playerLightObject.localPosition = Vector3.Lerp(playerLightObject.localPosition, crouchLightObjPos, Time.deltaTime * 15);
                m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, m_CrouchHeight, Time.deltaTime * 10);
                m_CharacterController.center = Vector3.down * (originalHeight - m_CharacterController.height) / 2.0f;
                lightCamera.farClipPlane = Mathf.Lerp(lightCamera.farClipPlane, 0.36f, Time.deltaTime * 15);
                yield return null;
            }
        }
    }
}
