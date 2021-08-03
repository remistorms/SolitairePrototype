using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private GameObject m_cardPrefab;
    public List<Card> m_deck;
    [SerializeField] private CardPile m_deckPile;
    [SerializeField] private CardPile[] m_bottomPiles;
    [SerializeField] private CardPile[] m_topPiles;

    private void Start()
    {
        GenerateDeck();

        ShuffleDeck();

        StartCoroutine(DealCards());
    }

    void GenerateDeck()
    {
        m_deck = new List<Card>();

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

                m_deck.Add(card);
            }
        }
    }

    void ShuffleDeck()
    {
        List<Card> tempDeck = new List<Card>();

        while (m_deck.Count > 0)
        {
            int randomInt = Random.Range(0, m_deck.Count);

            tempDeck.Add(m_deck[randomInt]);

            m_deck.Remove(m_deck[randomInt]);
        }

        m_deck = tempDeck;

        foreach (Card card in m_deck)
        {
            m_deckPile.AddCardToPile(card);

            card.Flip(0);
        }

        Debug.Log("Deck Shuffled");
    }

    IEnumerator DealCards()
    {

        //Face downCards
        for (int i = 0; i < 7; i++)
        {
            CardPile selectedPile = m_bottomPiles[i];

            for (int j = i; j > 0; j--)
            {
                Card topCard = m_deckPile.GetTopCard();

                m_deckPile.RemoveCardFromPile(topCard);

                selectedPile.AddCardToPile(topCard);

                yield return new WaitForSeconds(0.01f);
            }
     
        }

        for (int i = 0; i < 7; i++)
        {
            CardPile selectedPile = m_bottomPiles[i];

            Card topCard = m_deckPile.GetTopCard();

            m_deckPile.RemoveCardFromPile(topCard);

            selectedPile.AddCardToPile(topCard);

            yield return new WaitForSeconds(0.01f);
        }

        yield return null;

        for (int i = 0; i < m_bottomPiles.Length; i++)
        {
            m_bottomPiles[i].GetTopCard().Flip();

            yield return new WaitForSeconds(0.01f);
        }


    }
 
}
