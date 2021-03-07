using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerIllumination : MonoBehaviour
{
    public Flashlight lightSource;
    public float lightLevel;
    public SphericalHarmonicsL2 probe;
    public Renderer r;
    public Slider lightSlider;
    public Image lightIcon;
    public Color currentColor;
    public Color visibleColor;
    public RenderTexture lightTex;
    public Rect rectPic;
    Texture2D temp2D;
    public int interval = 10;
    public GameMenu menu;
    public Material lightIndicator;
    // Start is called before the first frame update
    //void Start()
    //{
    //    rectPic = new Rect(0, 0, lightTex.width, lightTex.height);
    //    temp2D = new Texture2D(lightTex.width, lightTex.height, TextureFormat.RGBA32, false);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Time.frameCount % interval == 0)
    //    {
    //        lightSlider.value = Calc();
    //    }
    //    Debug.Log(lightLevel);
    //}

    public int CalculateLightLevel()
    {
        if (lightSource.flashlightOn)
        {
            lightLevel = 100;
        }
        else
        {
            LightProbes.GetInterpolatedProbe(transform.position, r, out probe);
            lightLevel = Mathf.Max(probe[0, 0], probe[1, 0], probe[2, 0]);
            //currentColor = new Color(probe[0, 0]/255, probe[1, 0]/255, probe[2, 0]/255);
            //Debug.Log(currentColor);
        }
        float value = Mathf.InverseLerp(10f,60f,lightLevel);
        Debug.Log(value);
        currentColor = new Color(1,1,1,value);
        lightIcon.color = currentColor;
        return Mathf.RoundToInt(lightLevel);
    }

    IEnumerator Start()
    {
        rectPic = new Rect(0, 0, lightTex.width, lightTex.height);
        temp2D = new Texture2D(lightTex.width, lightTex.height, TextureFormat.ARGB32, false);
        visibleColor = new Color(1,1,1,1);
        while (true)
        {
            //yield return new WaitForSeconds(1);
            yield return null;

            //var rt = RenderTexture.GetTemporary(lightTex.width, lightTex.height, 0, RenderTextureFormat.ARGB32);
            //ScreenCapture.CaptureScreenshotIntoRenderTexture(rt);
            if (lightSource.flashlightOn && !menu.gamePaused)
            {
                lightLevel = 100;
                //lightSlider.value = 100;
                lightIcon.color = visibleColor;
                //lightIndicator.SetFloat("_EmissiveExposureWeight", 0);
                //lightIndicator.SetFloat("_EmissiveIntensity", 60);
                //Color newColor = lightIndicator.GetColor("_EmissiveColorLDR");
                //lightIndicator.SetColor("_EmissiveColor", newColor*lightIndicator.GetFloat("_EmissiveIntensity"));
            }
            else if (!menu.gamePaused && Time.frameCount % 3 == 0)
            {
                AsyncGPUReadback.Request(lightTex, 0, TextureFormat.ARGB32, OnCompleteReadback);
                //RenderTexture.ReleaseTemporary(rt);
                lightTex.Release();
            }
        }
    }

    void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.Log("GPU readback error detected.");
            Debug.Log(request.ToString());
            return;
        }
        temp2D.LoadRawTextureData(request.GetData<uint>());
        temp2D.Apply();
        Color32[] colors = temp2D.GetPixels32();
        float max = 0;
        float average = 0;
        foreach (Color32 col in colors)
        {
            average = Mathf.Max(col.r, col.g, col.b);
            if (average > max)
            {
                max = average;
            }
            //lightLevel += (0.216f * col.r) + (0.7152f * col.g) + (0.0722f * col.b);
        }
        lightLevel = max;
        float value = Mathf.InverseLerp(10f, 60f, lightLevel);
        //lightIndicator.SetFloat("_EmissiveExposureWeight", 1.0f-value);
        //lightIndicator.SetFloat("_EmissiveIntensity", (value * 60) + 1);
        //Color newColor = lightIndicator.GetColor("_EmissiveColorLDR");
        //lightIndicator.SetColor("_EmissiveColor", newColor * lightIndicator.GetFloat("_EmissiveIntensity"));
        //currentColor = new Color(1, 1, 1, value);
        lightIcon.color = new Color(1, 1, 1, value);
        //lightSlider.value = Mathf.RoundToInt(lightLevel);
    }

    public int Calc()
    {
        if (lightSource.flashlightOn)
        {
            lightLevel = 100;
        }
        else
        {
            //RenderTexture temp = RenderTexture.GetTemporary(lightTex.width, lightTex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            //Graphics.Blit(lightTex, temp);
            //RenderTexture previous = RenderTexture.active;
            RenderTexture.active = lightTex;

            Graphics.CopyTexture(lightTex, temp2D);
            //temp2D.ReadPixels(rectPic, 0, 0);
            temp2D.Apply();

            //RenderTexture.active = previous;
            //RenderTexture.ReleaseTemporary(temp);

            //RenderTexture.active = lightTex;
            lightTex.Release();
            Color32[] colors = temp2D.GetPixels32();
            float max = 0;
            foreach (Color32 col in colors)
            {
                float average = Mathf.Max(col.r, col.g, col.b);
                if (average > max)
                {
                    max = average;
                }
                //lightLevel += (0.216f * col.r) + (0.7152f * col.g) + (0.0722f * col.b);
            }
            lightLevel = max;
        }
        //Debug.Log(value);
        float value = Mathf.InverseLerp(10f, 60f, lightLevel);
        currentColor = new Color(1, 1, 1, value);
        lightIcon.color = currentColor;
        return Mathf.RoundToInt(lightLevel);
    }
}
