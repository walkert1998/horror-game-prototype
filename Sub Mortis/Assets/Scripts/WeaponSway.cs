using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    float amount = 0.02f;
    float maxamount = 0.03f;
    float smooth = 3;
    private Quaternion def;
    private bool Paused = false;
    private Vector3 initial_position;

    void Start()
    {
        initial_position = transform.localPosition;
    }

    void Update()
    {
        if (!InventoryManager.IsInventoryOpen_Static())
        {
            float factorX = (Input.GetAxis("Mouse Y")) * amount;
            float factorY = -(Input.GetAxis("Mouse X")) * amount;
            //float factorZ = -Input.GetAxis("Vertical") * amount;
            factorX = Mathf.Clamp(factorX, -maxamount, maxamount);
            factorY = Mathf.Clamp(factorY, -maxamount, maxamount);
            Vector3 final_position = new Vector3(factorY, -factorX, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, final_position + initial_position, Time.deltaTime * smooth);
        }
        else
        {
            //transform.localPosition = initial_position;
        }
    }
}
