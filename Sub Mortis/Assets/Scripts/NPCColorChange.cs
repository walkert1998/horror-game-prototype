using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCColorChange : MonoBehaviour
{
    public Material mat;
    public Color safe;
    public Color chaseAttack;
    public Color alert;
    public Color searching;
    public Color activeColor;
    // Start is called before the first frame update
    void Start()
    {
        //mat = GetComponent<Renderer>().material;
        //mat.shader = Shader.Find("HDRP/Lit");
    }

    public void SetNormal()
    {
        mat.SetColor("_BaseColor", safe);
        activeColor = safe;
    }

    public void SetAngry()
    {
        mat.SetColor("_BaseColor", chaseAttack);
        Debug.Log("Setting to angry");
        activeColor = chaseAttack;
    }

    public void SetAlert()
    {
        mat.SetColor("_BaseColor", alert);
        activeColor = alert;
    }

    public void SetSearching()
    {
        mat.SetColor("_BaseColor", searching);
        Debug.Log("Setting to search");
        activeColor = searching;
    }
}
