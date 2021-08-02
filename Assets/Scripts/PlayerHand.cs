using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public List<Card> m_cardsInHand = new List<Card>();

    private void Start()
    {
        EventsManager.OnCardBeginDrag += OnCardBeginDrag;
        EventsManager.OnCardEndedDrag += OnCardEndedDrag;
    }

    private void OnCardEndedDrag(Card card)
    {
        m_cardsInHand.Clear();
    }

    private void OnCardBeginDrag(Card card)
    {
        m_cardsInHand = card.m_cardPile.GetAllCardsAboveSelected(card);
    }
}
