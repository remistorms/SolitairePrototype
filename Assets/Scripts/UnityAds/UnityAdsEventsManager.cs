using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Advertisements;

public static class UnityAdsEventsManager
{
    #region Unity Ads Initialization Events
    //Unity Ads Initialized Event
    public static event Action OnUnityAdsInitialized = delegate { };
    public static void Fire_evt_UnityAdsInitialized()
    {
        Debug.Log("UnityAdsEventsManager.OnUnityAdsInitialized()");
        OnUnityAdsInitialized();
    }

    //Unity Ads FAILE INITIALIZATION Event
    public static event Action OnUnityAdsFailedToInitialize = delegate { };
    public static void Fire_evt_UnityAdsFailedToInitialize()
    {
        Debug.Log("UnityAdsEventsManager.OnUnityAdsFailedToInitialize()");
        OnUnityAdsFailedToInitialize();
    }
    #endregion

    #region Unity Ads - REWARDED AD EVENTS
    //Rewarded Ad Events
    public static event Action OnRewardedAdLoaded = delegate { };
    public static void Fire_evt_RewardedAdLoaded()
    {
        Debug.Log("UnityAdsEventsManager.OnRewardedAdLoaded()");
        OnRewardedAdLoaded();
    }

    public static event Action OnRewardedAdFailedToLoad = delegate { };
    public static void Fire_evt_RewardedAdFailedToLoad()
    {
        Debug.Log("UnityAdsEventsManager.OnRewardedAdFailedToLoad()");
        OnRewardedAdFailedToLoad();
    }

    public static event Action OnRewardedAdCompleted = delegate { };
    public static void Fire_evt_RewardedAdCompleted()
    {
        Debug.Log("UnityAdsEventsManager.OnRewardedAdCompleted()");
        OnRewardedAdCompleted();
    }

    public static event Action OnRewardedAdCanceled = delegate { };
    public static void Fire_evt_RewardedAdCanceled()
    {
        Debug.Log("UnityAdsEventsManager.OnRewardedAdCanceled()");
        OnRewardedAdCanceled();
    }
    #endregion
}
