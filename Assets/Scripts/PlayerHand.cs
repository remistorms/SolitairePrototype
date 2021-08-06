using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerHand : MonoBehaviour
{
    public List<Card> m_cardsInHand = new List<Card>();
    public List<Card> m_lastCardsInHand = new List<Card>();
    public CardPile m_originPile = null;
    public CardPile m_destinationPile = null;
    //bool m_cardsInHandSwitchedPiles = false;
    Vector3 offset = new Vector3(0, -0.2f, -0.05f);
    bool m_hasCheckedStackRule = false;

    private void Start()
    {
        EventsManager.OnCardDragStarted += OnCardDragStarted;
        EventsManager.OnCardDragUpdate += OnCardDragUpdate;
        EventsManager.OnCardDragEnded += OnCardDragEnded;
        EventsManager.OnCardStackCheck += OnCardStackCheck;
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

        for (int i = 0; i < m_cardsInHand.Count; i++)
        {
            m_cardsInHand[i].SetCanvasGroupState(false);
        }
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

    private void OnCardStackCheck(Card card, CardPile cardPile, bool canStack)
    {
        m_lastCardsInHand = m_cardsInHand;

        //Debug.Log("PlayerHand: OnCardStackCheck result: " + canStack );
        if (canStack)
        {
            m_destinationPile = cardPile;
        }
        else
        {
            m_destinationPile = m_originPile;
        }

        m_hasCheckedStackRule = true;
    }

    private void OnCardDragEnded(Card card, PointerEventData pointerEventData)
    {
        //Debug.Log("Save the card to previous here");

        if (!m_hasCheckedStackRule)
        {
            m_destinationPile = m_originPile;
        }

        if (m_destinationPile == null)
        {
            m_destinationPile = m_originPile;
        }
        m_destinationPile.AddCardsToPile(m_cardsInHand);
        m_originPile = null;
        m_destinationPile = null;
        m_cardsInHand.Clear();
        m_hasCheckedStackRule = false;

        for (int i = 0; i < m_cardsInHand.Count; i++)
        {
            m_cardsInHand[i].SetCanvasGroupState(true);
        }

    }

}
