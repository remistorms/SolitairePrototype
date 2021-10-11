using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GLOBAL : MonoBehaviour
{
    public static GLOBAL Instance;

    public UnityAdsManager unityAdsManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (unityAdsManager == null)
            unityAdsManager = FindObjectOfType<UnityAdsManager>();

        unityAdsManager.InitializeAds();
    }
}
