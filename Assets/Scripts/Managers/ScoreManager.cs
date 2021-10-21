using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private int m_currentScore;
    [SerializeField] private int m_previousScore;
    [SerializeField] private IntVariable m_scoreVariable;

    [Header("Point Calculation")]
    public int m_awardedPointsForGamePileDrop = 50;
    public int m_awardedPointsForEndPileDrop = 150;
    public int m_pointsDeductedFromReshuffle = 1000;
    [SerializeField] private int m_maxTimeBonus = 3000;
    [SerializeField] private int m_maxMoveBonus = 3000;

    //This lists keeps track of cards that awarded points to the player to avoid multiple points
    [SerializeField] private List<Card> m_cardsCountedTowardsScoreFromGamePiles;
    [SerializeField] private List<Card> m_cardsCountedTowardsScoreFromEndPiles;
    [SerializeField] private List<int> m_scoreHistory;

    private int m_currentScoreIndex;
    //private int lastPointsAwarded;
    [SerializeField] private Card lastCard;

    [Header("Final Score")]
    [SerializeField] private int m_pointsDeductedByTime = 10;
    [SerializeField] private int m_pointsDeductedByMove = 50;
    [SerializeField] private int m_rewardedAdMultiplier = 2;

    public int finalBaseScore = 0;
    public int finalTimeScore = 0;
    public int finalMovesScore = 0;
    public int finalTotalScore = 0;


    public override void Awake()
    {
        base.Awake();

        InitScore();
    }

    private void Start()
    {
        //subscribe to events
        EventsManager.OnCardStackCheck   += OnCardStackCheck;
        EventsManager.OnDeckReshuffled += OnDeckReshuffled;
        EventsManager.OnUndoMovement += OnUndoMovement;
        EventsManager.OnTurnSaved += OnTurnSaved;

        m_scoreHistory = new List<int>();

        //m_scoreHistory.Add(0);
        m_currentScoreIndex = -1;
    }

    private void OnTurnSaved(Turn turn)
    {
        m_scoreHistory.Add(m_currentScore);
        m_currentScoreIndex++;
    }

    private void OnUndoMovement(Turn move)
    {
        if (m_cardsCountedTowardsScoreFromEndPiles.Contains(lastCard))
        {
            m_cardsCountedTowardsScoreFromEndPiles.Remove(lastCard);
            lastCard = m_cardsCountedTowardsScoreFromEndPiles[m_cardsCountedTowardsScoreFromEndPiles.Count - 1];
        }
        if (m_cardsCountedTowardsScoreFromGamePiles.Contains(lastCard))
        {
            m_cardsCountedTowardsScoreFromGamePiles.Remove(lastCard);
            lastCard = m_cardsCountedTowardsScoreFromGamePiles[m_cardsCountedTowardsScoreFromGamePiles.Count - 1];
        }
        m_currentScoreIndex--;
        SetScore(m_scoreHistory[m_currentScoreIndex]);

        m_scoreHistory.Remove(m_scoreHistory[m_scoreHistory.Count -1]);
    }

    public void InitScore()
    {
        //m_scoreVariable.value = 0;
        m_currentScore = 0;
        m_previousScore = 0;
        m_scoreVariable.value = m_currentScore;
        m_cardsCountedTowardsScoreFromGamePiles = new List<Card>();
        m_cardsCountedTowardsScoreFromEndPiles  = new List<Card>();

        finalBaseScore = 0;
        finalMovesScore = 0;
        finalTimeScore = 0;
        finalTotalScore = 0;
    }

    void UpdateScore(int score)
    {
        //m_scoreVariable.value += score;
        m_previousScore = m_currentScore;
        m_currentScore += score;
        //lastPointsAwarded = score;

        if (m_currentScore <= 0)
            m_currentScore = 0;

        m_scoreVariable.value = m_currentScore;
    }

    //Events callbacks
    private void OnCardStackCheck(Card card, CardPile pile, bool canStack)
    {
        //Check if card is not on the GamePile dictionary and if so, add the points
        if (canStack && pile.m_pileType == PileType.GamePile)
        {
            if (!m_cardsCountedTowardsScoreFromGamePiles.Contains(card))
            {
                m_cardsCountedTowardsScoreFromGamePiles.Add(card);
                UpdateScore(m_awardedPointsForGamePileDrop);
                lastCard = card;
            }
        }
        //Check if card is not on the EndPile dictionary and if so, add the points
        else if (canStack && pile.m_pileType == PileType.EndPile)
        {
            if (!m_cardsCountedTowardsScoreFromEndPiles.Contains(card))
            {
                m_cardsCountedTowardsScoreFromEndPiles.Add(card);
                UpdateScore(m_awardedPointsForEndPileDrop);
                lastCard = card;
            }
        }
        else
        {
            //Debug.Log("No points awarded");
        }
    }

    private void OnDeckReshuffled(bool watchedAd)
    {
        if (watchedAd)
        {
            Debug.Log("No penalty of points");
        }
        else
        {
            UpdateScore(-m_pointsDeductedFromReshuffle);
        }
      
    }

    public int GetCurrentScore()
    {
        //return m_scoreVariable.value;
        return m_currentScore;
    }

    public int GetPreviousScore()
    {
        //return m_scoreVariable.value;
        return m_previousScore;
    }

    public void SetScore(int score)
    {
        //m_scoreVariable.value = score;
        m_currentScore = score;
        m_scoreVariable.value = m_currentScore;
    }

    public void SetPreviousScore(int prevScore)
    {
        m_previousScore = prevScore;
    }

    public void CalculateFinalScore()
    {
        finalBaseScore = m_currentScore;

        finalMovesScore = m_maxMoveBonus - TurnsManager.Instance.m_currentTurn * m_pointsDeductedByMove;

        finalMovesScore = Mathf.Clamp(finalMovesScore, 0, m_maxMoveBonus);

        finalTimeScore = m_maxTimeBonus -  Mathf.RoundToInt(GameManager.Instance.m_timer.value * m_pointsDeductedByTime);

        finalTimeScore = Mathf.Clamp(finalTimeScore, 0, m_maxTimeBonus);

        finalTotalScore = finalBaseScore + finalMovesScore + finalTimeScore;
    }

    public void ApplyRewardedAdMultiplier()
    {
        finalTotalScore *= m_rewardedAdMultiplier;
    }

    private void OnDisable()
    {
        //Unsubscribe from events
        EventsManager.OnCardStackCheck   -= OnCardStackCheck;
        EventsManager.OnDeckReshuffled -= OnDeckReshuffled;
        EventsManager.OnUndoMovement -= OnUndoMovement;
        EventsManager.OnTurnSaved -= OnTurnSaved;
    }
}
