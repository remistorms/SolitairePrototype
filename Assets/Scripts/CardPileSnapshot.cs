using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardPileSnapshot : MonoBehaviour
{
    private int m_snapshotTurn;
    private CardPile m_snapshotCardPile;
    private List<Card> m_snapshotCards;
    private List<bool> m_snapshotFlipStates;

    public CardPileSnapshot(CardPile pile, List<Card> cards, List<bool> flipStates)
    {
        m_snapshotCardPile = pile;
        m_snapshotCards = cards;
        m_snapshotFlipStates = flipStates;
    }

    public CardPile GetSnapshotPile()
    {
        return m_snapshotCardPile;
    }

    public List<Card> GetSnapshotCards()
    {
        return m_snapshotCards;
    }

    public List<bool> GetSnapshotCardFlipStates()
    {
        return m_snapshotFlipStates;
    }
}
