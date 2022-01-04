using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    private static ToolTip mInstance;
    private RectTransform mBackground;
    private Text mText;

    [SerializeField]
    private float mTextPadding = 4f;
    void Awake()
    {
        mInstance = this;
        gameObject.SetActive(false);


        mBackground = transform.Find("background").GetComponent<RectTransform>();
        mText = transform.Find("text").GetComponent<Text>();
    }

    void Update()
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    private void ShowTooltip(string tooltipString)
    {
        mText.text = tooltipString;
        var size = new Vector2(mText.preferredWidth + mTextPadding * 2, mText.preferredHeight + mTextPadding * 2);
        mBackground.sizeDelta = size;
        transform.position = Mouse.current.position.ReadValue();

        gameObject.SetActive(true);
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string tooltipString)
    {
        mInstance.ShowTooltip(tooltipString);
    }

    public static void HideToolTip_Static()
    {
        mInstance.HideTooltip();
    }
}
