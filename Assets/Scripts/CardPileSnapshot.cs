using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardPileSnapshot
{
    public int m_snapshotTurn;
    public CardPile m_snapshotCardPile;
    public List<Card> m_snapshotCards;
    public List<bool> m_snapshotFlipStates;
}
