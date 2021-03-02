using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitScreen : MonoBehaviour
{
    Image hit_screen;
    Color invisible;
    // Use this for initialization
    void Start () {
        hit_screen = GetComponent<Image>();
        hit_screen.color = new Color(1, 0, 0, 0);
        invisible = new Color(hit_screen.color.r, hit_screen.color.g, hit_screen.color.b, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
            TookDamage();
        if (hit_screen.color.a > 0)
        {
            hit_screen.color = Color.Lerp(hit_screen.color, invisible, 3 * Time.deltaTime);
        }
	}

    public void TookDamage()
    {
        hit_screen.color = new Color(1, 0, 0, 0.3f);
    }
}
