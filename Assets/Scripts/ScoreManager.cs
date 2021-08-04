using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private IntVariable m_scoreVariable;
    [SerializeField] private FloatVariable m_timeVariable;
    [SerializeField] private int m_awardedPointsForGamePileDrop = 5;
    [SerializeField] private int m_awardedPointsForEndPileDrop = 15;
    [SerializeField] private int m_pointsDeductedFromReshuffle = 100;

    //This lists keeps track of cards that awarded points to the player to avoid multiple points
    [SerializeField] private List<Card> m_cardsCountedTowardsScoreFromGamePiles;
    [SerializeField] private List<Card> m_cardsCountedTowardsScoreFromEndPiles;


    private void Awake()
    {
        InitScore();
    }

    private void Start()
    {
        //subscribe to events
        EventsManager.OnCardStackCheck   += OnCardStackCheck;
        EventsManager.OnDeckReshuffled += OnDeckReshuffled;
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
            }
        }
        //Check if card is not on the EndPile dictionary and if so, add the points
        else if (canStack && pile.m_pileType == PileType.EndPile)
        {
            if (!m_cardsCountedTowardsScoreFromEndPiles.Contains(card))
            {
                m_cardsCountedTowardsScoreFromEndPiles.Add(card);
                UpdateScore(m_awardedPointsForEndPileDrop);
            }
        }
        else
        {
            Debug.Log("No points awarded");
        }
    }

    private void OnDeckReshuffled()
    {
        UpdateScore(-m_pointsDeductedFromReshuffle);
    }

    private void OnDisable()
    {
        //Unsubscribe from events
        EventsManager.OnCardStackCheck   -= OnCardStackCheck;
        EventsManager.OnDeckReshuffled -= OnDeckReshuffled;
    }
}
