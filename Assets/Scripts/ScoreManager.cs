using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int m_score = 0;
    private Dictionary<Card, bool> m_cardsCountedTowardsScoreFromGamePiles;
    private Dictionary<Card, bool> m_cardsCountedTowardsScoreFromEndPiles;

    private void Awake()
    {
        InitScore();
    }

    void InitScore()
    {
        m_score = 0;
        m_cardsCountedTowardsScoreFromGamePiles = new Dictionary<Card, bool>();
        m_cardsCountedTowardsScoreFromEndPiles = new Dictionary<Card, bool>();
    }
}
