using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerHand : MonoBehaviour
{
    public List<Card> m_cardsInHand = new List<Card>();
    public CardPile m_originPile = null;
    public CardPile m_destinationPile = null;
    bool m_cardsInHandSwitchedPiles = false;
    Vector3 offset = new Vector3(0, -0.2f, -0.05f);

    private void Start()
    {
        EventsManager.OnCardDragStarted += OnCardDragStarted;
        EventsManager.OnCardDragUpdate += OnCardDragUpdate;
        EventsManager.OnCardDragEnded += OnCardDragEnded;
        EventsManager.OnCardDroppedOnPile += OnCardDropped;
    }

    private void OnCardDragStarted(Card card, PointerEventData pointerEventData)
    {
        if (!card.m_isFaceUp)
        {
            return;
        }

        m_originPile = card.m_cardPile;

        m_cardsInHand = m_originPile.GetAllCardsAboveSelected(card);

        m_originPile.RemoveCardsFromPile(m_cardsInHand);
    }

    private void OnCardDragUpdate(Card card, PointerEventData pointerEventData)
    {
        //card.transform.position = worldPoint;
        Vector3 eventPos = new Vector3(pointerEventData.position.x, pointerEventData.position.y, (-Camera.main.transform.position.z - 0.5f));
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(eventPos);
        //card.transform.position = screenPos;
        for (int i = 0; i < m_cardsInHand.Count; i++)
        {
            m_cardsInHand[i].transform.position = new Vector3(
                screenPos.x + offset.x * i,
                screenPos.y + offset.y * i,
                screenPos.z + offset.z * i
                );
        }
    }

    private void OnCardDragEnded(Card card, PointerEventData pointerEventData)
    {
        if (!m_cardsInHandSwitchedPiles && m_originPile != null)
        {
            m_originPile.AddCardsToPile(m_cardsInHand);
        }
        else if(m_cardsInHandSwitchedPiles && m_destinationPile != null)
        {
            m_destinationPile.AddCardsToPile(m_cardsInHand);
        }

        m_originPile = null;
        m_destinationPile = null;
        m_cardsInHand.Clear();
    }



    private void OnCardDropped(Card card, CardPile pile)
    {

    }
}
