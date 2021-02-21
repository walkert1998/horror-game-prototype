using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Picture
{
    public Texture image;
    public bool horizontal;

    public Picture(Texture image, bool horizontal)
    {
        this.image = image;
        this.horizontal = horizontal;
    }
}
