using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPile : MonoBehaviour, IDropHandler
{
    public List<Card> m_cardsInPile;
    public int m_topIndex;
    public Vector3 m_pilingOffset;
    public bool m_isFinalPile = false;

    private void Awake()
    {
        m_topIndex = -1;
        m_cardsInPile = new List<Card>();
    }

    private void Start()
    {
        EventsManager.OnCardBeginDrag += OnCardBeginDrag;
    }

    public void AddCardToPile(Card cardToAdd)
    {
        m_cardsInPile.Add(cardToAdd);

        cardToAdd.GetComponent<Draggable>().SetReturningPosition(this.transform.position);

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

    public void RemoveFromPile(List<Card> cardsToRemove)
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

        Transform currentParent = this.transform;

        for (int i = 0; i < m_cardsInPile.Count; i++)
        {
            m_cardsInPile[i].transform.SetParent(this.transform);

            m_cardsInPile[i].transform.localPosition = new Vector3( i * m_pilingOffset.x, i * m_pilingOffset.y, i * m_pilingOffset.z) ;
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
            Card card = eventData.pointerDrag.GetComponent<Card>();

            EventsManager.Fire_evt_OnCardDropped( card, this );

            AddCardToPile(card);
        }
    }

    //Events
    private void OnCardBeginDrag(Card card)
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
            for (int i =  m_cardsInPile.IndexOf(selectedCard) ; i < m_cardsInPile.Count - 1 ; i++)
            {
                cardsAbove.Add(m_cardsInPile[i]);
            }
        }
     

        return cardsAbove;
    }
}
