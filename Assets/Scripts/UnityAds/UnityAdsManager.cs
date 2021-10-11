using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public bool IsUnityAdsManagerInitialized { get; private set; }

    [Header("Unity Ads Initialization")]
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOsGameId;
    [SerializeField] bool _testMode = true;
    [SerializeField] bool _enablePerPlacementMode = true;
    private string _gameId;

    [Header("Rewarded Ad")]
    public bool m_isRewardedAdReady = false;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOsAdUnitId = "Rewarded_iOS";
    string _rewardedAdUnityId;

    public void InitializeAds()
    {
        //Get the GAME ID Based on Platform
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, _enablePerPlacementMode, this);

        // Get the Ad Unit ID for the current platform:
        _rewardedAdUnityId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOsAdUnitId
            : _androidAdUnitId;
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        IsUnityAdsManagerInitialized = true;
        LoadAd();
        UnityAdsEventsManager.Fire_evt_UnityAdsInitialized();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        IsUnityAdsManagerInitialized = false;
        UnityAdsEventsManager.Fire_evt_UnityAdsFailedToInitialize();
    }

    #region Rewarded Video Ads

    // Load content to the Ad Unit:
    public void LoadAd()
    {
        Debug.Log("Loading new ad...");
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Advertisement.Load(_rewardedAdUnityId, this);
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_rewardedAdUnityId))
        {
            m_isRewardedAdReady = true;

            UnityAdsEventsManager.Fire_evt_RewardedAdLoaded();
        }
    }

    // Implement a method to execute when the user clicks the button.
    public void ShowAd()
    {
        // Then show the ad:
        if (m_isRewardedAdReady)
        {
            //m_isRewardedAdReady = false;
            Advertisement.Show(_rewardedAdUnityId, this);
        }
        else
        {
            Debug.Log("Reward Ad Not ready yet.. cant show");
        }
    
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_rewardedAdUnityId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            UnityAdsEventsManager.Fire_evt_RewardedAdCompleted();

            // Load another ad:
            Advertisement.Load(_rewardedAdUnityId, this);
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        m_isRewardedAdReady = false;
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {

    }

    #endregion
}