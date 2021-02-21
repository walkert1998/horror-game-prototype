using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextMessage
{
    public bool fromPlayer;
    [TextArea]
    public string messageContent;
}
