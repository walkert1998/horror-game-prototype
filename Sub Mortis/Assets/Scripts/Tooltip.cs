using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;

    private Text toolTipText;
    private RectTransform toolTipBackgroundRect;
    [SerializeField]
    private Camera uiCamera;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        toolTipBackgroundRect = transform.Find("ToolTipBackground").GetComponent<RectTransform>();
        toolTipText = transform.Find("ToolTipText").GetComponent<Text>();
        instance.HideToolTip();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), new Vector3(Input.mousePosition.x + 40, Input.mousePosition.y - 40, 0), uiCamera, out localPoint);
        transform.localPosition = localPoint;
    }

    public void DisplayToolTip(string ToolTipText)
    {
        toolTipText.text = ToolTipText;
        float textPadding = 4f;
        Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + textPadding * 2f, toolTipText.preferredHeight + textPadding * 2f);
        toolTipBackgroundRect.sizeDelta = backgroundSize;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), new Vector3(Input.mousePosition.x + 40, Input.mousePosition.y - 40, 0), uiCamera, out localPoint);
        transform.localPosition = localPoint;
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        toolTipText.text = "";
    }

    public static void DisplayToolTip_Static(string ToolTip)
    {
        instance.DisplayToolTip(ToolTip);
    }

    public static void HideToolTip_Static()
    {
        //Debug.Log("Hiding tooltip");
        instance.HideToolTip();
    }
}
