using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private IntVariable m_scoreVariable;
    [SerializeField] private int m_awardedPointsForGamePileDrop = 5;
    [SerializeField] private int m_awardedPointsForEndPileDrop = 15;
    [SerializeField] private int m_pointsDeductedFromReshuffle = 100;

    //This lists keeps track of cards that awarded points to the player to avoid multiple points
    [SerializeField] private List<Card> m_cardsCountedTowardsScoreFromGamePiles;
    [SerializeField] private List<Card> m_cardsCountedTowardsScoreFromEndPiles;

    private int lastPointsAwarded;
    private Card lastCard;


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
    }

    private void OnUndoMovement(PlayerMovement move)
    {
        if (m_cardsCountedTowardsScoreFromEndPiles.Contains(lastCard))
        {
            m_cardsCountedTowardsScoreFromEndPiles.Remove(lastCard);
        }
        if (m_cardsCountedTowardsScoreFromGamePiles.Contains(lastCard))
        {
            m_cardsCountedTowardsScoreFromGamePiles.Remove(lastCard);
        }
        SetScore(move.recordedScore);

    }

    void InitScore()
    {
        m_scoreVariable.value = 0;
        m_cardsCountedTowardsScoreFromGamePiles = new List<Card>();
        m_cardsCountedTowardsScoreFromEndPiles  = new List<Card>();
    }

    void UpdateScore(int score)
    {
        m_scoreVariable.value += score;
        lastPointsAwarded = score;

        if (m_scoreVariable.value <= 0)
            m_scoreVariable.value = 0;
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

    private void OnDeckReshuffled()
    {
        UpdateScore(-m_pointsDeductedFromReshuffle);
    }

    public int GetScore()
    {
        return m_scoreVariable.value;
    }

    void SetScore(int score)
    {
        m_scoreVariable.value = score;
    }

    private void OnDisable()
    {
        //Unsubscribe from events
        EventsManager.OnCardStackCheck   -= OnCardStackCheck;
        EventsManager.OnDeckReshuffled -= OnDeckReshuffled;
        EventsManager.OnUndoMovement -= OnUndoMovement;
    }
}
