using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerIllumination : MonoBehaviour
{
    public Flashlight lightSource;
    public int lightLevel;
    public SphericalHarmonicsL2 probe;
    public Renderer r;
    public Slider lightSlider;
    public Image lightIcon;
    public Color currentColor;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        lightSlider.value = CalculateLightLevel();
    }

    public int CalculateLightLevel()
    {
        if (lightSource.flashlightOn)
        {
            lightLevel = 100;
        }
        else
        {
            LightProbes.GetInterpolatedProbe(transform.position, r, out probe);
            lightLevel = Mathf.RoundToInt(Mathf.Max(probe[0, 0], probe[1, 0], probe[2, 0]));
            //currentColor = new Color(probe[0, 0]/255, probe[1, 0]/255, probe[2, 0]/255);
            //Debug.Log(currentColor);
        }
        float value = Mathf.InverseLerp(10f,60f,lightLevel);
        Debug.Log(value);
        currentColor = new Color(1,1,1,value);
        lightIcon.color = currentColor;
        return lightLevel;
    }
}
