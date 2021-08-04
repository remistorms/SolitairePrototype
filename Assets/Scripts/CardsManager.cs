using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardsManager : MonoBehaviour
{
    public bool m_drawThreeCardMode = false;
    [SerializeField] private GameObject m_cardPrefab;
    public List<Card> m_deck;
    [SerializeField] private CardPile m_deckPile;
    [SerializeField] private CardPile m_drawPile;
    [SerializeField] private CardPile[] m_endPiles;
    [SerializeField] private CardPile[] m_gamePiles;


    private void Start()
    {
        EventsManager.OnRequestDrawCards += DrawCardsFromDeck;

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

                GameObject clonedCard = Instantiate(m_cardPrefab, m_deckPile.transform.position, Quaternion.identity);

                Card card = clonedCard.GetComponent<Card>();

                card.Flip(0);

                card.InitializeCard(v, currentSuit);

                clonedCard.name = currentCardValue.ToString() + " of " + currentSuit.ToString();

                m_deck.Add(card);
            }
        }

        m_deckPile.AddCardsToPile(m_deck);
    }

    void ShuffleDeck()
    {
        m_deckPile.RemoveCardsFromPile(m_deck);

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
            CardPile selectedPile = m_gamePiles[i];

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
            CardPile selectedPile = m_gamePiles[i];

            Card topCard = m_deckPile.GetTopCard();

            m_deckPile.RemoveCardFromPile(topCard);

            selectedPile.AddCardToPile(topCard);

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;

        for (int i = 0; i < m_gamePiles.Length; i++)
        {
            m_gamePiles[i].GetTopCard().Flip();

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DrawCardsFromDeck()
    {
        StartCoroutine(DrawCardsFromDeckRoutine());
    }

    IEnumerator DrawCardsFromDeckRoutine()
    {
        //TODO
        //Check if remaining cards are enought to deal
        int amountOfCardsToDraw = 1;
        if (m_drawThreeCardMode == true)
        {
            amountOfCardsToDraw = 3;
        }

        if (m_deckPile.m_cardsInPile.Count >= amountOfCardsToDraw)
        {
            Debug.Log("Enough cards on deck to draw");
            for (int i = 0; i < amountOfCardsToDraw; i++)
            {
                Card card = m_deckPile.GetTopCard();

                m_deckPile.RemoveCardFromPile(card);

                m_drawPile.AddCardToPile(card);

                card.Flip();

                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            Debug.Log("Not Enough cards on deck to draw");
        }
        //grab the top 3 cards from the deck

        //Move them into the draw pile

        //Flip them
    }

}
