using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public PowerSource powerSource;
    public bool switchOn;
    public Light[] lights;
    public AudioSource source;
    public AudioClip useSound;
    // Start is called before the first frame update
    void Start()
    {
        switchOn = powerSource.powerOn;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void ToggleLights(bool status)
    {
        switchOn = status;
        Debug.Log("Lights " + lights.Length);
        source.PlayOneShot(useSound);
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = switchOn;
            Debug.Log("Lights are now " + lights[i].enabled);
        }
    }
}
