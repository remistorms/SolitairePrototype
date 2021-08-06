using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public abstract class UIScreen : MonoBehaviour
{
    bool m_isShown = true;
    public bool m_isTransitioning;
    private RectTransform m_rect;
    private CanvasGroup m_canvasGroup;
    private Vector2 m_hiddenPosition;

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

    public void ToggleScreen(float animTime = 0.2f)
    {
        if (m_isShown)
        {
            HideScreen(animTime);
        }
        else
        {
            ShowScreen(animTime);
        }
    }

    public void ShowScreen(float animTime = 0.2f)
    {
        if (m_isTransitioning || m_isShown)
            return;

        StartCoroutine(ShowScreenRoutine(animTime));
    }

    IEnumerator ShowScreenRoutine(float animTime = 0.2f)
    {
        m_isTransitioning = true;
        float currentTime = 0;
        float percent = 0;

        while (percent < animTime)
        {
            percent = currentTime / animTime;

            m_canvasGroup.alpha = Mathf.Lerp( 0f, 1.0f, percent );

            currentTime += Time.deltaTime;

            yield return null;
        }

        m_canvasGroup.alpha = 1.0f;
        m_canvasGroup.blocksRaycasts = true;
        m_canvasGroup.interactable = true;
        m_isTransitioning = false;
        m_isShown = true;
    }

    public void HideScreen(float animTime = 0.2f)
    {
        if (m_isTransitioning || !m_isShown)
            return;

        StartCoroutine(HideScreenRoutine(animTime));
    }

    IEnumerator HideScreenRoutine(float animTime = 0.2f)
    {
        m_isTransitioning = true;
        float currentTime = 0;
        float percent = 0;

        while (percent < animTime)
        {
            percent = currentTime / animTime;

            m_canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, percent);

            currentTime += Time.deltaTime;

            yield return null;
        }

        m_canvasGroup.alpha = 0.0f;
        m_canvasGroup.blocksRaycasts = false;
        m_canvasGroup.interactable = false;
        m_isTransitioning = false;
        m_isShown = false;
    }
}
