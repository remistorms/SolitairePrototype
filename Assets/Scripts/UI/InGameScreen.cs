using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameScreen : UIScreen
{
    [Header("Score Objects")]
    [SerializeField] private TextMeshProUGUI m_scoreLabel;
    [SerializeField] private IntVariable m_scoreVariable;
    [Header("Timer Objects")]
    [SerializeField] private TextMeshProUGUI m_timeLabel;
    [SerializeField] private FloatVariable m_timeVariable;
    [Header("Move Objects")]
    [SerializeField] private TextMeshProUGUI m_moveLabel;
    [SerializeField] private IntVariable m_movesVariable;

    private void Update()
    {
        m_scoreLabel.text = m_scoreVariable.value.ToString() + " pts";
        m_timeLabel.text = m_timeVariable.value.ToString("F2");
        m_moveLabel.text = m_movesVariable.value.ToString();
    }
}
