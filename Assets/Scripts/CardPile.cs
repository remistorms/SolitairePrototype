using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPile : MonoBehaviour, IDropHandler
{
    public List<Card> m_cardsInPile;
    public int m_topIndex;
    public Vector3 m_pilingOffset;

    private void Awake()
    {
        m_topIndex = -1;
        m_cardsInPile = new List<Card>();
    }

    public void AddToPile(Card cardToAdd)
    {
        if (m_cardsInPile.Contains(cardToAdd))
            return;

        m_cardsInPile.Add(cardToAdd);

        cardToAdd.GetComponent<Draggable>().SetReturningPosition(this.transform.position);

       // UpdatePositions();
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

        UpdatePositions();
    }

    private void UpdatePositions()
    {
        for (int i = 0; i < m_cardsInPile.Count; i++)
        {
            m_cardsInPile[i].transform.SetParent(this.gameObject.transform);

            m_cardsInPile[i].transform.localPosition = new Vector3( i * m_pilingOffset.x, i * m_pilingOffset.y, i * m_pilingOffset.z ) ;

            m_cardsInPile[i].m_cardPile = this;
        }

        m_cardsInPile[m_cardsInPile.Count - 1].m_isTopCard = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Card>() != null)
        {
            Card card = eventData.pointerDrag.GetComponent<Card>();

            Debug.Log("droping card: " + card.name + " onto " + this.gameObject.name);

            AddToPile(card);

            //UpdatePositions();
        }
    }

    private void Update()
    {
        if (m_cardsInPile.Count > 0)
        {
            UpdatePositions();
        }
    }
}
