using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager Instance;
    
    [SerializeField] private int m_currentScore = 0;

    private void Awake()
    {
        //Init instance
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        EventsManager.OnCardStackCheck += OnCardStackCheck;
    }

    //Here we can modify the score per card or pile
    private void OnCardStackCheck(Card card, CardPile pile, bool canStack, bool hasSwitchedPiles)
    {
        if (canStack && hasSwitchedPiles)
        {
            switch (pile.m_pileType)
            {
                case PileType.None:
                    break;

                case PileType.DeckPile:
                    break;

                case PileType.GamePile:
                    UpdateScore(10);
                    break;

                case PileType.EndPile:
                    UpdateScore(10);
                    break;

                case PileType.DrawPile:
                    break;

                default:
                    break;
            }
        }
    }

    private void UpdateScore(int newScore)
    {
        m_currentScore += newScore;

        if (m_currentScore < 0)
        {
            m_currentScore = 0;
        }
    }

    private void ResetScore()
    {
        m_currentScore = 0;
    }

    private void OnDisable()
    {
        EventsManager.OnCardStackCheck -= OnCardStackCheck;
    }
}
