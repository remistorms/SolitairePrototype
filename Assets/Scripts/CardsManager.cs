using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private GameObject m_cardPrefab;
    [SerializeField] private CardPile m_deckPile;
    [SerializeField] private CardPile[] m_bottomPiles;
    [SerializeField] private CardPile[] m_topPiles;

    private void Start()
    {
        GenerateDeck();
    }

    void GenerateDeck()
    {
        for (int s = 0; s < 4; s++)
        {
            CardSuit currentSuit = CardHelper.Instance.GetCardSuitFromIndex(s);

            for (int v = 0; v < 13; v++)
            {
                CardValue currentCardValue = CardHelper.Instance.GetCardValueFromInt(v);

                GameObject clonedCard = Instantiate(m_cardPrefab, transform.position, Quaternion.identity);

                Card card = clonedCard.GetComponent<Card>();

                card.InitializeCard(v, currentSuit);

                clonedCard.name = currentCardValue.ToString() + " of " + currentSuit.ToString();

                //card.Flip(0);

                m_deckPile.AddCardToPile(card);


            }
        }
    }
}
