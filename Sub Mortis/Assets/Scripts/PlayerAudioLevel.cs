using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAudioLevel : MonoBehaviour
{
    AudioSource source;
    public float audioLevel = 0;
    public Slider audioSlider;
    public int sampleDataLength = 1024;

    float[] clipSampleData;
    float clipLoudness;
    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        clipSampleData = new float[sampleDataLength * 2];
        audioSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        audioSlider.value = Mathf.Lerp(audioSlider.value, audioLevel, 0.8f);
        if (!source.isPlaying || source.clip == null)
        {
            SetAudioLevel(0);
            return;
        }
        //source.clip.GetData(clipSampleData, source.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
        //clipLoudness = 0f;
        //foreach (var sample in clipSampleData)
        //{
        //    clipLoudness += Mathf.Abs(sample);
        //}
        //clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
        //SetAudioLevel(clipLoudness);
    }

    public void SetAudioLevel(float value)
    {
        audioLevel = value * 10;
        //Debug.Log(audioSlider.value);
        //audioSlider.value = audioLevel;
    }
}
