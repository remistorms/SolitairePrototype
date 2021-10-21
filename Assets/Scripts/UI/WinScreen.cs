using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WinScreen : UIScreen
{
    [SerializeField] TextMeshProUGUI m_baseScoreLabel;
    [SerializeField] TextMeshProUGUI m_timeScoreLabel;
    [SerializeField] TextMeshProUGUI m_movesScoreLabel;
    [SerializeField] TextMeshProUGUI m_totalScoreLabel;
    [SerializeField] Button m_claimButton;
    [SerializeField] Button m_doubleScoreButton;

    float animTime = 0.25f;

    public override void Awake()
    {
        base.Awake();

        m_doubleScoreButton.onClick.AddListener(OnDoubleScoreButtonPressed);
        m_claimButton.onClick.AddListener(OnClaimedButtonPressed);

    }

    protected override void OnScreenShown()
    {
        base.OnScreenShown();

        ResetLabelValues();

        m_doubleScoreButton.gameObject.SetActive(false);
        m_claimButton.gameObject.SetActive(false);

        //Only do the corotuine if game is actually won
        if (!GameManager.Instance.hasWon)
            return;

        StartCoroutine(DisplayBaseScoreRoutine());
    }

    IEnumerator DisplayBaseScoreRoutine()
    {
        yield return null;

        //Base Score
        int initialValue = 0;
        int endValue = ScoreManager.Instance.finalBaseScore;
        int lerpValue = 0;
        float percent = 0f;
        float currentTime = 0f;

        while (currentTime < animTime)
        {
            lerpValue = (int)Mathf.Lerp(initialValue, endValue, percent);

            m_baseScoreLabel.text = lerpValue.ToString("F0");

            percent = currentTime / animTime;

            currentTime += Time.deltaTime;

            yield return null;
        }

        m_baseScoreLabel.text = ScoreManager.Instance.finalBaseScore.ToString();

        //Time Bonus Score
         initialValue = 0;
         endValue = ScoreManager.Instance.finalTimeScore;
         lerpValue = 0;
         percent = 0f;
         currentTime = 0f;

        while (currentTime < animTime)
        {
            lerpValue = (int)Mathf.Lerp(initialValue, endValue, percent);

            m_timeScoreLabel.text = lerpValue.ToString("F0");

            percent = currentTime / animTime;

            currentTime += Time.deltaTime;

            yield return null;
        }

        m_timeScoreLabel.text = ScoreManager.Instance.finalTimeScore.ToString();

        //Moves Bonus Score
        initialValue = 0;
        endValue = ScoreManager.Instance.finalMovesScore;
        lerpValue = 0;
        percent = 0f;
        currentTime = 0f;

        while (currentTime < animTime)
        {
            lerpValue = (int)Mathf.Lerp(initialValue, endValue, percent);

            m_movesScoreLabel.text = lerpValue.ToString("F0");

            percent = currentTime / animTime;

            currentTime += Time.deltaTime;

            yield return null;
        }

        m_movesScoreLabel.text = ScoreManager.Instance.finalMovesScore.ToString();

        //final total score
        initialValue = 0;
        endValue = ScoreManager.Instance.finalTotalScore;
        lerpValue = 0;
        percent = 0f;
        currentTime = 0f;

        while (currentTime < animTime)
        {
            lerpValue = (int)Mathf.Lerp(initialValue, endValue, percent);

            m_totalScoreLabel.text = lerpValue.ToString("F0");

            percent = currentTime / animTime;

            currentTime += Time.deltaTime;

            yield return null;
        }

        m_totalScoreLabel.text = ScoreManager.Instance.finalTotalScore.ToString();

        yield return null;

        ShowButtons();
    }

    private void ShowButtons()
    {
        //If ad is available
        if (GLOBAL.Instance.unityAdsManager.m_isRewardedAdReady)
        {
            m_doubleScoreButton.gameObject.SetActive(true);
        }

        m_claimButton.gameObject.SetActive(true);
    }

    public void OnClaimedButtonPressed()
    {
        DataManager.Instance.m_currentPlayerData.m_coins += ScoreManager.Instance.finalTotalScore;

        //DataManager.Instance.UpdatePlayerData();

        DataManager.Instance.SaveData();

        InGameUI.Instance.SwitchScreens(7);
    }

    public void OnDoubleScoreButtonPressed()
    {
        ScoreManager.Instance.ApplyRewardedAdMultiplier();

        DataManager.Instance.m_currentPlayerData.m_coins += ScoreManager.Instance.finalTotalScore;

        //DataManager.Instance.UpdatePlayerData();

        DataManager.Instance.SaveData();

        InGameUI.Instance.SwitchScreens(7);
    }

    private void ResetLabelValues()
    {
        m_baseScoreLabel.text = "0";
        m_timeScoreLabel.text = "0";
        m_movesScoreLabel.text = "0";
        m_totalScoreLabel.text = "0";
    }
}
