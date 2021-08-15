using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAdjustment : MonoBehaviour
{
    [SerializeField] GameBounds m_gameBounds;
    [SerializeField] Camera m_camera;

    private void Start()
    {
        m_camera = Camera.main;
        m_gameBounds = FindObjectOfType<GameBounds>();
    }

    private void Update()
    {
        
    }

    void UpdateScreenBounds()
    {
       
    }
}

