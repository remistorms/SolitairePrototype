using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardPile : MonoBehaviour, IDropHandler
{
    public List<Card> m_cardsInPile;
    public int m_topIndex;
    public Vector3 m_pilingOffset;
    public bool m_isFinalPile = false;
    public CardSuit m_finalPileSuit;

    private void Awake()
    {
        m_topIndex = -1;
        m_cardsInPile = new List<Card>();
    }

    private void Start()
    {

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

            m_cardsInPile[i].transform.DOLocalMove(new Vector3(i * m_pilingOffset.x, i * m_pilingOffset.y, i * m_pilingOffset.z), 0.1f);
            //m_cardsInPile[i].transform.localPosition = new Vector3( i * m_pilingOffset.x, i * m_pilingOffset.y, i * m_pilingOffset.z) ;
            //m_cardsInPile[i].transform.localPosition = m_pilingOffset;

            m_cardsInPile[i].m_cardPile = this;

            m_cardsInPile[i].m_isTopCard = false;

        }

        m_cardsInPile[m_cardsInPile.Count - 1].m_isTopCard = true;

        m_topIndex = m_cardsInPile.Count - 1;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Card>() != null)
        {
            DropCardOnPile(eventData.pointerDrag.GetComponent<Card>());
        }
    }

    //TO DO CLEAN UP THIS
    public void DropCardOnPile(Card card)
    {
        EventsManager.Fire_evt_OnCardDroppedOnPile(card, this);

        //Check rules here
        Debug.Log( card.name + " is being dropped onto " + this.gameObject.name );

        //Case pile contains at least one card
        if (m_cardsInPile.Count > 0)
        {
            bool canStack = CardHelper.Instance.CheckIfCanStack(card, m_cardsInPile[m_cardsInPile.Count - 1]);
            EventsManager.Fire_evt_OnCardStackCheck(card, this, canStack);

            /*
            if (canStack)
            {
                Debug.Log(card.name + " CAN be stacked on top of " + m_cardsInPile[m_cardsInPile.Count].name);
            }
            else
            {
                Debug.Log(card.name + " CAN NOT be stacked on top of " + m_cardsInPile[m_cardsInPile.Count].name);
            }
            */
        }
        //If pile is empty and is final pile
        //Only accept 
        else if (m_cardsInPile.Count == 0 && m_isFinalPile)
        {
            if (card.m_cardSuit == m_finalPileSuit && card.m_cardValue == CardValue.Ace)
            {
                //Final pile got its first card
                EventsManager.Fire_evt_OnCardStackCheckOnEmptyPile(card, this, true);
            }
        }
        //Only place card inside pile if its a king
        else if (m_cardsInPile.Count == 0 && !m_isFinalPile)
        {
            if (card.m_cardValue == CardValue.King)
            {
                //Final pile got its first card
                EventsManager.Fire_evt_OnCardStackCheckOnEmptyPile(card, this, true);
            }
        }
        else
        {
            EventsManager.Fire_evt_OnCardStackCheckOnEmptyPile(card, this, false);
        }
    }

    //Events
    private void OnCardDragStarted(Card card, PointerEventData pointerEventData)
    {
        if (m_cardsInPile.Contains(card))
        {
            Debug.Log("Here");
        }
    }

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
}
