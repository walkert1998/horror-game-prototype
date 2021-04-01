using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    FilmGrain filmGrain = null;
    public AudioMixer audioMixer;
    public Volume postProcessingObject;
    private bool headbobEnabled;
    private bool gunSmokeEnabled;
    private bool healthBarEnabled;
    private static bool filmGrainOn;
    private static CursorSetting cursorSetting;
    private static VisibilityMeterSetting visibilityMeterSetting;
    private bool interactionPromptsEnabled;
    public Slider volumeSlider;
    private static int fov;
    private static int cursorSettingOption = 0;
    private static int visibilitySettingOption = 0;
    private static int resolutionIndex = 0;
    private static float volume;
    public Resolution[] resolutions;
    public TMP_Dropdown resolutionDropDown;
    public TMP_Dropdown cursorSettingDropDown;
    public TMP_Dropdown visibilityMeterSettingDropDown;
    public Slider visibilityBar;
    public Image visibilityIcon;
    public Material visibilityLight;
    public Toggle filmGrainToggle;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        if (resolutionDropDown != null)
        {
            resolutionDropDown.ClearOptions();
            PopulateResolutionOptions();
            ChangeResolution();
        }
        if (cursorSettingDropDown != null)
        {
            cursorSettingDropDown.ClearOptions();
            PopulateCursorSettingDropdown();
            cursorSettingDropDown.value = cursorSettingOption;
        }
        if (visibilityMeterSettingDropDown != null)
        {
            visibilityMeterSettingDropDown.ClearOptions();
            PopulateVisibilitySettingDropdown();
            visibilityMeterSettingDropDown.value = visibilitySettingOption;
        }
        headbobEnabled = true;
        gunSmokeEnabled = true;
        healthBarEnabled = true;
        interactionPromptsEnabled = true;
        audioMixer.GetFloat("Volume", out volume);
        FilmGrain grain;
        if (postProcessingObject != null)
        {
            if (postProcessingObject.profile.TryGet<FilmGrain>(out grain))
            {
                filmGrain = grain;
                filmGrain.active = filmGrainOn;
            }
        }
        filmGrainToggle.isOn = filmGrainOn;
        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    public void PopulateResolutionOptions()
    {
        List<string> options = new List<string>();
        int i = 0;
        int startResolution = i;
        foreach (Resolution res in resolutions)
        {
            string option = res.width + "x" + res.height;
            if (res.width == Screen.width && res.height == Screen.height)
            {
                startResolution = i;
            }
            options.Add(option);
            i++;
        }
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = startResolution;
    }

    public void PopulateCursorSettingDropdown()
    {
        List<string> options = new List<string>();
        options.Add("Always On");
        options.Add("Interact Only");
        options.Add("Never");
        cursorSettingDropDown.AddOptions(options);
        cursorSettingDropDown.value = cursorSettingOption;
        SetCursorSetting();
    }

    public void PopulateVisibilitySettingDropdown()
    {
        List<string> options = new List<string>();
        options.Add("Lightbulb Icon");
        options.Add("Small Light Indicator");
        options.Add("Visibility Bar");
        visibilityMeterSettingDropDown.AddOptions(options);
        visibilityMeterSettingDropDown.value = visibilitySettingOption;
        SetVisibilityMeterSetting();
    }

    public void SetCursorSetting()
    {
        switch (cursorSettingDropDown.value)
        {
            case 0:
                cursorSetting = CursorSetting.Always;
                cursorSettingOption = 0;
                break;
            case 1:
                cursorSetting = CursorSetting.InteractOnly;
                cursorSettingOption = 1;
                break;
            case 2:
                cursorSetting = CursorSetting.Never;
                cursorSettingOption = 2;
                break;
        }
    }

    public void SetVisibilityMeterSetting()
    {
        switch (visibilityMeterSettingDropDown.value)
        {
            case 0:
                visibilityMeterSetting = VisibilityMeterSetting.Icon;
                if (visibilityIcon)
                {
                    visibilityIcon.gameObject.SetActive(true);
                }
                if (visibilityBar)
                {
                    visibilityBar.gameObject.SetActive(false);
                }
                if (visibilityLight)
                {
                    visibilityLight.SetFloat("_EmissiveExposureWeight", 1.0f);
                    visibilityLight.SetFloat("_EmissiveIntensity", 1);
                    Color newColor = visibilityLight.GetColor("_EmissiveColorLDR");
                    visibilityLight.SetColor("_EmissiveColor", newColor * visibilityLight.GetFloat("_EmissiveIntensity"));
                }
                visibilitySettingOption = 0;
                Debug.Log(visibilitySettingOption);
                break;
            case 1:
                visibilityMeterSetting = VisibilityMeterSetting.Light;
                if (visibilityIcon)
                    visibilityIcon.gameObject.SetActive(false);
                if (visibilityBar)
                    visibilityBar.gameObject.SetActive(false);
                visibilitySettingOption = 1;
                Debug.Log(visibilitySettingOption);
                break;
            case 2:
                visibilityMeterSetting = VisibilityMeterSetting.Bar;
                if (visibilityBar)
                    visibilityBar.gameObject.SetActive(true);
                if (visibilityIcon)
                    visibilityIcon.gameObject.SetActive(false);
                if (visibilityLight)
                {
                    visibilityLight.SetFloat("_EmissiveExposureWeight", 1.0f);
                    visibilityLight.SetFloat("_EmissiveIntensity", 1);
                    Color newColor = visibilityLight.GetColor("_EmissiveColorLDR");
                    visibilityLight.SetColor("_EmissiveColor", newColor * visibilityLight.GetFloat("_EmissiveIntensity"));
                }
                visibilitySettingOption = 2;
                Debug.Log(visibilitySettingOption);
                break;
        }
    }

    public void ToggleFilmGrain()
    {
        if (filmGrainToggle.isOn)
        {
            filmGrainOn = true;
        }
        else
        {
            filmGrainOn = false;
        }
        if (filmGrain != null)
        {
            filmGrain.active = filmGrainOn;
        }
    }

    public void SetFOV(int amount)
    {
        fov = amount;
    }

    public int GetFOV()
    {
        return fov;
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetVolume()
    {
        audioMixer.SetFloat("Volume", volumeSlider.value);
        volume = volumeSlider.value;
    }

    public void ToggleHeadbob ()
    {
        headbobEnabled = !headbobEnabled;
    }

    public void ToggleGunSmoke()
    {
        gunSmokeEnabled = !gunSmokeEnabled;
    }

    public void ToggleHealthBar()
    {
        healthBarEnabled = !healthBarEnabled;
    }

    public void ToggleInteractionPrompts()
    {
        interactionPromptsEnabled = !interactionPromptsEnabled;
    }

    public bool GetGunSmoke()
    {
        return gunSmokeEnabled;
    }

    public bool GetHealthBar()
    {
        return healthBarEnabled;
    }

    public bool GetInteractionPrompts()
    {
        return interactionPromptsEnabled;
    }

    public bool GetHeadbob()
    {
        return headbobEnabled;
    }

    public void ChangeResolution()
    {
        int index = resolutionDropDown.value;
        int width = resolutions[index].width;
        int height = resolutions[index].height;
        Screen.SetResolution(width, height, true);
    }

    public static CursorSetting GetCursorSetting()
    {
        return cursorSetting;
    }

    public static VisibilityMeterSetting GetVisibilityMeterSetting()
    {
        return visibilityMeterSetting;
    }
}

public enum CursorSetting
{
    Always,
    InteractOnly,
    Never
}
public enum VisibilityMeterSetting
{
    Bar,
    Icon,
    Light
}
