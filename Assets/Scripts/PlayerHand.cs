using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHand : MonoBehaviour
{
    public List<Card> m_cardsInHand = new List<Card>();

    private void Start()
    {
        EventsManager.OnCardBeginDrag += OnCardBeginDrag;
        EventsManager.OnCardEndedDrag += OnCardEndedDrag;
    }

    private void Update()
    {

    }

    private void OnCardEndedDrag(Card card)
    {
        m_cardsInHand.Clear();
    }

    private void OnCardBeginDrag(Card card)
    {
        CardPile pile = card.m_cardPile;

        m_cardsInHand = pile.GetAllCardsAboveSelected(card);

        pile.RemoveCardsFromPile(m_cardsInHand);

    }
}
