using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [TextArea]
    public string text;
    public float time = 3.0f;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter(Collider other)
    {
        Examination.SetExamineTextTimed_static(text, time);
    }
}
