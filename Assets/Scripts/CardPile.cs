﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardPile : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public PileType m_pileType;
    public List<Card> m_cardsInPile;
    //public int m_topIndex;
    public Vector3 m_pilingOffset;
    //public bool m_isFinalPile = false;
    public CardSuit m_finalPileSuit;
    private CardsManager m_cardsManager;

    private void Awake()
    {
        m_cardsManager = FindObjectOfType<CardsManager>();
        //m_topIndex = -1;
        m_cardsInPile = new List<Card>();
    }

    public void AddCardToPile(Card cardToAdd)
    {
        if (m_cardsInPile.Contains(cardToAdd))
        {
            return;
        }

        m_cardsInPile.Add(cardToAdd);

        //cardToAdd.GetComponent<Draggable>().SetReturningPosition(this.transform.position);

        StartCoroutine(UpdatePositions());
    }

    public void AddCardsToPile(List<Card> cardsToAdd)
    {
        for (int i = 0; i < cardsToAdd.Count; i++)
        {
            if (m_cardsInPile.Contains(cardsToAdd[i]))
            {
                return;
            }
            else
            {
                m_cardsInPile.Add(cardsToAdd[i]);
            }
        }
        StartCoroutine(UpdatePositions());
    }

    public void RemoveCardFromPile(Card cardToRemove)
    {
        if (m_cardsInPile.Contains(cardToRemove))
        {
            m_cardsInPile.Remove(cardToRemove);
        }

        StartCoroutine(UpdatePositions());
    }

    public void RemoveCardsFromPile(List<Card> cardsToRemove)
    {
        for (int i = 0; i < cardsToRemove.Count; i++)
        {
            if (m_cardsInPile.Contains(cardsToRemove[i]))
            {
                m_cardsInPile.Remove(cardsToRemove[i]);

                cardsToRemove[i].transform.SetParent(null);
            }
        }

        StartCoroutine( UpdatePositions());
    }

    IEnumerator UpdatePositions()
    {
        yield return null;

        for (int i = 0; i < m_cardsInPile.Count; i++)
        {
            m_cardsInPile[i].transform.SetParent(this.transform);

            //Only offset the top three cards if is pile type draw
            if (m_pileType == PileType.DrawPile)
            {
                Debug.Log("Update Draw Pile Positions");
                if (i >= m_cardsInPile.Count - 3)
                {
                    int difference =  m_cardsInPile.Count - i - 1;
                    Debug.Log("Difference: " + difference);
                    m_cardsInPile[i].transform.DOLocalMove(new Vector3(difference * m_pilingOffset.x, difference * m_pilingOffset.y, difference * m_pilingOffset.z), 0.1f);
                }
                else
                {
                    m_cardsInPile[i].transform.DOLocalMove(Vector3.zero, 0.1f);
                }
            }
            else
            {
                m_cardsInPile[i].transform.DOLocalMove(new Vector3(i * m_pilingOffset.x, i * m_pilingOffset.y, i * m_pilingOffset.z), 0.1f);
            }


            m_cardsInPile[i].m_cardPile = this;

            m_cardsInPile[i].m_isTopCard = false;

            m_cardsInPile[i].SetCanvasGroupState(true);

        }

        m_cardsInPile[m_cardsInPile.Count - 1].m_isTopCard = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Card>() != null)
        {
            DropCardOnPile(eventData.pointerDrag.GetComponent<Card>());
        }
    }

    //TO DO CLEAN UP THIS
    public virtual void DropCardOnPile(Card card)
    {
        EventsManager.Fire_evt_OnCardDroppedOnPile(card, this);

        bool canStack = CardHelper.Instance.CheckIfCanStack(card, this);

        EventsManager.Fire_evt_OnCardStackCheck(card, this, canStack);
    }

    //Events

    public List<Card> GetAllCardsAboveSelected(Card selectedCard)
    {
        List<Card> cardsAbove = new List<Card>();

        if (m_cardsInPile.Contains(selectedCard))
        {
            for (int i =  m_cardsInPile.IndexOf(selectedCard) ; i < m_cardsInPile.Count ; i++)
            {
                cardsAbove.Add(m_cardsInPile[i]);
                Debug.Log("card at i index" + i + " was added");
            }
        }

        //Debug.Log("CardPile" + this.gameObject.name + ": GetAllCardsAvobeSelected(...)");

        return cardsAbove;
    }

    public Card GetTopCard()
    {
        return m_cardsInPile[m_cardsInPile.Count - 1];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked on empty Deck requesting new cards");
        if (m_pileType == PileType.DeckPile && m_cardsInPile.Count == 0)
        {
            EventsManager.Fire_evt_RequestDrawCards();
        }
    }
}
