using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverTipManager : MonoBehaviour
{
    public TextMeshProUGUI TipText;
    public RectTransform TipWindow;
    public float MaxWindowWidth = 160.0f;

    public static Action<string, Vector2, float> OnMouseHover;
    public static Action OnMouseLeave;

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLeave += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLeave -= HideTip;
    }

    void Start()
    {
        HideTip();
    }

    void ShowTip(string tip, Vector2 buttonPos, float buttonHeight)
    {
        TipText.text = tip;
        TipWindow.sizeDelta = new Vector2(TipText.preferredWidth > MaxWindowWidth ? MaxWindowWidth : TipText.preferredWidth, TipText.preferredHeight);

        TipWindow.gameObject.SetActive(true);
        TipWindow.transform.position = new Vector2(buttonPos.x, buttonPos.y + buttonHeight + 5.0f + (TipWindow.sizeDelta.y * 0.5f));    // 5.0f is the size of the border (build menu background)
    }

    void HideTip()
    {
        TipText.text = default;
        TipWindow.gameObject.SetActive(false);
    }

}
