using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Computer : MonoBehaviour
{
    public List<Note> textFiles;
    public List<Lock> locks;
    public string password;
    public bool locked = false;
    public bool powerOn;
    public GameObject screen;
    public Light computerLight;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    public void TurnOn()
    {
        powerOn = true;
        computerLight.enabled = true;
        screen.SetActive(true);
    }

    public void TurnOff()
    {
        powerOn = false;
        computerLight.enabled = false;
        screen.SetActive(false);
    }
}
