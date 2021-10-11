using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public UIScreen[] m_allScreens;
    [SerializeField] private float m_screenTransitionTime = 0.2f;
    [SerializeField] private UIScreen m_currentScreen;

    private void Start()
    {
        for (int i = 0; i < m_allScreens.Length; i++)
        {
            m_allScreens[i].HideScreen(0);
        }

        SwitchScreens(1);
    }

    public void SwitchScreens(int screenIndex)
    {
        //Check if index is not from the curren screen
        if ( m_currentScreen != m_allScreens[screenIndex])
        {
            StartCoroutine(SwitchScreensRoutine(screenIndex));
        }
    }

    IEnumerator SwitchScreensRoutine(int screenIndex)
    {
        m_currentScreen.HideScreen(m_screenTransitionTime);

        m_allScreens[screenIndex].ShowScreen(m_screenTransitionTime);

        yield return new WaitForSeconds(m_screenTransitionTime);

        m_currentScreen = m_allScreens[screenIndex];
    }
}
