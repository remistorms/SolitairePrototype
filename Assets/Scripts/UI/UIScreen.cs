using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public abstract class UIScreen : MonoBehaviour
{
    private RectTransform m_rect;
    private CanvasGroup m_canvasGroup;

    public virtual void Awake()
    {
        m_rect = GetComponent<RectTransform>();
        m_canvasGroup = GetComponent<CanvasGroup>();
        CenterScreen();
    }

    public virtual void CenterScreen()
    {
        m_rect.offsetMin = Vector2.zero;
        m_rect.offsetMax = Vector2.one;
    }
}
