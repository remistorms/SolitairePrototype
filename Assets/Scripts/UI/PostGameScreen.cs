using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PostGameScreen : UIScreen
{
    [SerializeField] TextMeshProUGUI m_finalRewardLabel;
    [SerializeField] Button m_playAgainButton;
    [SerializeField] Button m_mainMenuButton;

    protected override void OnScreenShown()
    {
        base.OnScreenShown();

        m_finalRewardLabel.text = ScoreManager.Instance.finalTotalScore.ToString();
    }
}
