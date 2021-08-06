using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardsManager : MonoBehaviour
{
    public static CardsManager Instance;

    public bool m_drawThreeCardMode = false;
    [SerializeField] private GameObject m_cardPrefab;
    public List<Card> m_deck;
    [SerializeField] private List<Card> m_allCardsInPlay;
    [SerializeField] private CardPile m_deckPile;
    [SerializeField] private CardPile m_drawPile;
    [SerializeField] private CardPile[] m_endPiles;
    [SerializeField] private CardPile[] m_gamePiles;
    TurnsManager m_turnsManager;

    public bool resetCompleted;
    public bool deckGenerated;
    public bool dealCompleted;
    public bool deckShuffled;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void Start()
    {
        m_turnsManager = FindObjectOfType<TurnsManager>();
        m_allCardsInPlay = new List<Card>();
        EventsManager.OnRequestDrawCards += DrawCardsFromDeck;
    }

    public void ResetCardsManager()
    {
        resetCompleted = false;
        deckGenerated = false;
        dealCompleted = false;
        deckShuffled = false;
        StartCoroutine(ResetCardsManagerRoutine());
    }

    IEnumerator ResetCardsManagerRoutine()
    {
        resetCompleted = false;

        /*
        if (m_allCardsInPlay.Count > 0)
        {
            for (int i = 0; i < m_allCardsInPlay.Count; i++)
            {
                Destroy(m_allCardsInPlay[i].gameObject);
            }
        }

        m_allCardsInPlay.Clear();

        */
        yield return null;

        resetCompleted = true;
    }

    public void GenerateDeck()
    {
        StartCoroutine(GenerateDeckRoutine());
    }

    IEnumerator GenerateDeckRoutine()
    {
        deckGenerated = false;
        yield return null;
        m_deck = new List<Card>();

        for (int s = 0; s < 4; s++)
        {
            CardSuit currentSuit = CardHelper.Instance.GetCardSuitFromIndex(s);

            for (int v = 0; v < 13; v++)
            {
                CardValue currentCardValue = CardHelper.Instance.GetCardValueFromInt(v);

                GameObject clonedCard = Instantiate(m_cardPrefab, m_deckPile.transform.position, Quaternion.identity);

                Card card = clonedCard.GetComponent<Card>();
                //card.Flip();
                card.SetFlipState(false);

                card.InitializeCard(v, currentSuit);

                clonedCard.name = currentCardValue.ToString() + " of " + currentSuit.ToString();

                m_deck.Add(card);

                m_allCardsInPlay.Add(card);

                yield return null;
            }
        }

        m_deckPile.AddCardsToPile(m_deck);

        deckGenerated = true;
    }

    public void ShuffleDeck()
    {
        StartCoroutine(ShuffleDeckRoutine());
    }

    IEnumerator ShuffleDeckRoutine()
    {
        deckShuffled = false;
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

            card.SetFlipState(false);
        }
      
        yield return null;

        deckShuffled = true;
    }

    public void DealCards()
    {
        StartCoroutine(DealCardsRoutine());
    }

    IEnumerator DealCardsRoutine()
    {
        dealCompleted = false;
        //Face downCards
        for (int i = 0; i < 7; i++)
        {
            CardPile selectedPile = m_gamePiles[i];

            for (int j = i; j > 0; j--)
            {
                Card topCard = m_deckPile.GetTopCard();

                m_deckPile.RemoveCardFromPile(topCard);

                selectedPile.AddCardToPile(topCard);

                yield return new WaitForSeconds(0.1f);
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

        for (int i = 0; i < m_gamePiles.Length; i++)
        {
            m_gamePiles[i].GetTopCard().Flip();

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;

        dealCompleted = true;
    }

    public void DrawCardsFromDeck()
    {
        StartCoroutine(DrawCardsFromDeckRoutine());
    }

    IEnumerator DrawCardsFromDeckRoutine()
    {
        Turn move = new Turn();
        move.originalPile = m_deckPile;
        move.endPile = m_drawPile;
        List<Card> drawnCards = new List<Card>();
        move.cardsMoved = drawnCards;
        //TODO
        //Check if remaining cards are enought to deal
        int amountOfCardsToDraw = 1;
        if (m_drawThreeCardMode == true)
        {
            amountOfCardsToDraw = 3;
        }

        //If 
        if (m_deckPile.m_cardsInPile.Count >= amountOfCardsToDraw)
        {
            Debug.Log("Enough cards on deck to draw");
            for (int i = 0; i < amountOfCardsToDraw; i++)
            {
                Card card = m_deckPile.GetTopCard();

                drawnCards.Add(card);

                m_deckPile.RemoveCardFromPile(card);

                m_drawPile.AddCardToPile(card);

                card.Flip();
                //card.FlipNoSignal();

                yield return new WaitForSeconds(0.1f);
            }
            m_turnsManager.AddMoveToStack(move);
        }
        else if (m_deckPile.m_cardsInPile.Count < amountOfCardsToDraw && m_deckPile.m_cardsInPile.Count > 0 && m_drawThreeCardMode)
        {
            //Not enought to deal three cards so we will deal all we get here
            Debug.Log(" cards in pile is less than 3 and we are playing three mode, so we will deal 1 or two cards max ");
            for (int i = m_deckPile.m_cardsInPile.Count - 1; i >= 0; i--)
            {
                Card card = m_deckPile.m_cardsInPile[i];

                drawnCards.Add(card);

                m_deckPile.RemoveCardFromPile(card);

                m_drawPile.AddCardToPile(card);

                //card.Flip();
                card.Flip();

                yield return new WaitForSeconds(0.1f);
            }
            m_turnsManager.AddMoveToStack(move);
        }
        else
        {
            StartCoroutine(RefillDeck());
        }
    }

    IEnumerator RefillDeck()
    {
        yield return null;

        List<Card> allCards = m_drawPile.GetAllCardsAboveSelected(m_drawPile.m_cardsInPile[0]);

        m_deckPile.RestackPile(allCards);

        while (m_deckPile.m_isAnimating)
        {
            yield return null;
        }

        m_drawPile.m_cardsInPile.Clear();

        m_deckPile.UpdatePositions();

        EventsManager.Fire_evt_OnDeckReshuffled();

        Turn move = new Turn();

        m_turnsManager.AddMoveToStack(move);

    }

    private void OnDisable()
    {
        EventsManager.OnRequestDrawCards -= DrawCardsFromDeck;
    }
}
