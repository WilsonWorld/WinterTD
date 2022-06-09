using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string TipToShow;
    float m_TimeToWait = 0.5f;
    Vector2 m_ButtonPosition;
    float m_ButtonHeight;

    void Start()
    {
        m_ButtonPosition = transform.position;
        m_ButtonHeight = GetComponent<RectTransform>().rect.height * 0.5f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverTipManager.OnMouseLeave();
    }

    void ShowMessage()
    {
        HoverTipManager.OnMouseHover(TipToShow, m_ButtonPosition, m_ButtonHeight);
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(m_TimeToWait);

        ShowMessage();
    }
}
