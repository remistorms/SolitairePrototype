using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAdButton : MonoBehaviour
{
    [SerializeField] Button button;

    private void OnEnable()
    {
        Debug.Log("Button has been enabled !!!");
        UnityAdsEventsManager.OnRewardedAdLoaded += EnableButton;
    }

    private void Update()
    {
        button.interactable = GLOBAL.Instance.unityAdsManager.m_isRewardedAdReady;
    }

    public void ShowAd()
    {
        button.interactable = false;
        GLOBAL.Instance.unityAdsManager.ShowAd();
    }

    private void EnableButton()
    {
        button.interactable = true;
    }

    private void OnDisable()
    {
        UnityAdsEventsManager.OnRewardedAdLoaded -= EnableButton;
    }
}
