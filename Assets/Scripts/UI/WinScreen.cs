using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreen : UIScreen
{
    [SerializeField] private IntVariable m_scoreVariable;
    [SerializeField] private TextMeshProUGUI m_scoreLabel;

    private void Update()
    {
        m_scoreLabel.text = m_scoreVariable.value.ToString();
    }
}
