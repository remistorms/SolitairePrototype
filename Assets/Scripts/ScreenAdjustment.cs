using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAdjustment : MonoBehaviour
{
    public ScreenOrientation m_currentOrientation;
    Camera m_mainCamera;
    bool isOnMobile = false;

    [Header("Landscape")]
    [SerializeField] private Vector3 m_landscapeCamPosition;
    [SerializeField] private Vector3 m_landscapeCamRotation;
    [Header("Portrait")]             
    [SerializeField] private Vector3 m_portraitCamPosition;
    [SerializeField] private Vector3 m_portraitCamRotation;


    private void Awake()
    {
        m_mainCamera = Camera.main;

        m_currentOrientation = Screen.orientation;

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            isOnMobile = true;
        }

        if (isOnMobile)
            SetCameraPosition();
    }

    void Update()
    {
        if (!isOnMobile)
            return;

        if (Screen.orientation != m_currentOrientation)
        {
            m_currentOrientation = Screen.orientation;

            OrientationChanged(m_currentOrientation);
        }
    }

    void OrientationChanged(ScreenOrientation newOrientation)
    {
        SetCameraPosition();

        EventsManager.Fire_evt_ScreenOrientationChanged(newOrientation);
    }

    private void SetCameraPosition()
    {
        if (m_currentOrientation == ScreenOrientation.Portrait || m_currentOrientation == ScreenOrientation.PortraitUpsideDown)
        {
            m_mainCamera.transform.position = m_portraitCamPosition;
            m_mainCamera.transform.rotation = Quaternion.Euler(m_portraitCamRotation);
        }
        else if (m_currentOrientation == ScreenOrientation.Landscape || m_currentOrientation == ScreenOrientation.LandscapeLeft || m_currentOrientation == ScreenOrientation.LandscapeRight)
        {
            m_mainCamera.transform.position = m_landscapeCamPosition;
            m_mainCamera.transform.rotation = Quaternion.Euler(m_landscapeCamRotation);
        }
    }

}

