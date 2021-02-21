using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public bool powerOn;
    public LightSwitch[] switches;
    public List<Computer> computers;
    // Start is called before the first frame update
    void Start()
    {
        TogglePower(powerOn);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void TogglePower(bool status)
    {
        powerOn = status;
        for (int i = 0; i < switches.Length; i++)
        {
            switches[i].ToggleLights(powerOn);
        }
        foreach(Computer pc in computers)
        {
            if (status)
            {
                pc.TurnOn();
            }
            else
            {
                pc.TurnOff();
            }
        }
    }
}
