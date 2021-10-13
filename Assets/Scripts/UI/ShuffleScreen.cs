using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShuffleScreen : UIScreen
{
    [SerializeField] Button _closePopUpButton;
    [SerializeField] Button _showAdButton;

    public override void Awake()
    {
        base.Awake();

        DisableAdButton();
        _closePopUpButton.onClick.AddListener(ClosePopUp);
        _showAdButton.onClick.AddListener(ShowAdAndShuffle);
    }

    public void ClosePopUp()
    {
        EventsManager.Fire_evt_ShuffleWithAd(false);
    }

    public void ShowAdAndShuffle()
    {
        EventsManager.Fire_evt_ShuffleWithAd(true);
    }

    private void DisableAdButton()
    {
        _showAdButton.interactable = false;
        _showAdButton.GetComponentInChildren<TextMeshProUGUI>().text = "Ad not ready, sorry";
    }

    private void EnableAdButton()
    {
        _showAdButton.interactable = true;
        _showAdButton.GetComponentInChildren<TextMeshProUGUI>().text = "Keep my points";
    }

    protected override void OnScreenShown()
    {
        base.OnScreenHide();

        if (GLOBAL.Instance.unityAdsManager.m_isRewardedAdReady)
        {
            EnableAdButton();
        }
        else
        {
            DisableAdButton();
        }
    }

    protected override void OnScreenHide()
    {
        base.OnScreenHide();
    }
}
