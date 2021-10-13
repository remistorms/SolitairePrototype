using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private int m_currentScore;
    [SerializeField] private int m_previousScore;
    [SerializeField] private IntVariable m_scoreVariable;
    [SerializeField] private int m_awardedPointsForGamePileDrop = 5;
    [SerializeField] private int m_awardedPointsForEndPileDrop = 15;
    [SerializeField] private int m_pointsDeductedFromReshuffle = 100;

    //This lists keeps track of cards that awarded points to the player to avoid multiple points
    [SerializeField] private List<Card> m_cardsCountedTowardsScoreFromGamePiles;
    [SerializeField] private List<Card> m_cardsCountedTowardsScoreFromEndPiles;
    [SerializeField] private List<int> m_scoreHistory;

    private int m_currentScoreIndex;
    //private int lastPointsAwarded;
    [SerializeField] private Card lastCard;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
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

    private void OnDisable()
    {
        //Unsubscribe from events
        EventsManager.OnCardStackCheck   -= OnCardStackCheck;
        EventsManager.OnDeckReshuffled -= OnDeckReshuffled;
        EventsManager.OnUndoMovement -= OnUndoMovement;
        EventsManager.OnTurnSaved -= OnTurnSaved;
    }
}
