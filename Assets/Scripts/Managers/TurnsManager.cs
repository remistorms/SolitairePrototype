using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnsManager : MonoBehaviour
{
    [Header("Undo Movement")]
    public IntVariable m_turn;
    public IntVariable m_scoreIntVariable;
    private Stack<Turn> m_recordedMovements;
    public List<Turn> m_recordedPlayerMovements;

    public bool canRecord = false;
    public List<CardPile> m_allCardPiles;

    public List<TurnData> m_allTurnsData = new List<TurnData>();

    private void Awake()
    {
        m_turn.value = 0;
        m_recordedMovements = new Stack<Turn>();
        m_recordedPlayerMovements = new List<Turn>();
    }

    private void Start()
    {
        EventsManager.OnCardStackCheck      += OnCardStackCheck;
        EventsManager.OnCardFlipped         += OnCardFlipped;
        EventsManager.OnRequestDrawCards    += OnRequestedDrawCards;
    }

    private void OnRequestedDrawCards()
    {
        RecordTurn();
    }

    private void OnCardFlipped(Card obj)
    {
        RecordTurn();
    }

    public void UndoLastMove()
    {
        if (m_recordedMovements.Count > 0)
        {
            Turn move = m_recordedMovements.Pop();

            move.endPile.RemoveCardsFromPile(move.cardsMoved);

            move.originalPile.AddCardsToPile(move.cardsMoved);

            for (int i = 0; i < move.cardsMoved.Count; i++)
            {
                bool flipStateDuringTurn = false;
                move.cardFlipState.TryGetValue(move.cardsMoved[i], out flipStateDuringTurn);
                if (flipStateDuringTurn)
                {
                    move.cardsMoved[i].FlipDown();
                }
                else
                {
                    move.cardsMoved[i].FlipUp();
                }
            }

            move.endPile.UpdatePositions();

            move.originalPile.UpdatePositions();

            m_turn.value--;

            //Delete this
            Turn[] tempMovements = m_recordedMovements.ToArray();
            m_recordedPlayerMovements.Clear();
            for (int i = 0; i < tempMovements.Length; i++)
            {
                m_recordedPlayerMovements.Add(tempMovements[i]);
            }
            EventsManager.Fire_event_UndoMovement(move);
        }

    }

    private void OnCardStackCheck(Card card, CardPile pile, bool canStack)
    {
        RecordTurn();
    }

    public void AddMoveToStack(Turn move)
    {
        /*
        Debug.Log("Aded new move");

        move.recordedScore = ScoreManager.Instance.GetScore();

        //THis will save the flip state of each card
        Dictionary<Card, bool> flipState = new Dictionary<Card, bool>();
        for (int i = 0; i < move.cardsMoved.Count; i++)
        {
            flipState.Add(move.cardsMoved[i], move.cardsMoved[i].m_isFaceUp);
        }

        move.cardFlipState = flipState;

        m_recordedMovements.Push(move);

        //Delete this
        Turn[] tempMovements = m_recordedMovements.ToArray();
        m_recordedPlayerMovements.Clear();
        for (int i = 0; i < tempMovements.Length; i++)
        {
            m_recordedPlayerMovements.Add(tempMovements[i]);
        }
        m_movesIntVariable.value++;
        */
    }

    public void RecordTurn()
    {
        if (canRecord)
        {
            Debug.Log("Record turn here");

            TurnData newTurn = new TurnData(m_turn.value, m_allCardPiles);

            m_allTurnsData.Add(newTurn);

            m_turn.value++;
        }
    }

    public void GoToPreviousMove()
    {
        if (m_turn.value <= 0)
        {
            return;
        }
        else
        {
            m_turn.value--;

            TurnData previousTurnData = m_allTurnsData[m_turn.value];

            for (int i = 0; i < m_allCardPiles.Count; i++)
            {
                CardPile currentPile = m_allCardPiles[i];

                CardPileSnapshot snapshot = previousTurnData.GetCardPileSnapshot(currentPile);

                m_allCardPiles[i].SetSnapshot(snapshot);
            }

        }
    }

    private void OnDisable()
    {
        EventsManager.OnCardStackCheck -= OnCardStackCheck;
        EventsManager.OnCardFlipped -= OnCardFlipped;
        EventsManager.OnRequestDrawCards -= OnRequestedDrawCards;
    }
}

//TODO

[System.Serializable]
public struct Turn
{
    public List<Card> cardsMoved;
    public Dictionary<Card, bool> cardFlipState;
    public CardPile originalPile;
    public CardPile endPile;
    public int recordedScore;
}


[System.Serializable]
public class TurnData
{
    public int turn;
    public Dictionary<CardPile, CardPileSnapshot> m_cardPileAndSnapshotDictionary;

    public TurnData(int _turn, List<CardPile> cardPiles)
    {
        turn = _turn;
        m_cardPileAndSnapshotDictionary = new Dictionary<CardPile, CardPileSnapshot>();

        for (int i = 0; i < cardPiles.Count; i++)
        {
            m_cardPileAndSnapshotDictionary.Add(cardPiles[i], cardPiles[i].GetCurrentSnapshot());
        }
    }

    public CardPileSnapshot GetCardPileSnapshot(CardPile wantedPile)
    {
        CardPileSnapshot snapshot = null;

        m_cardPileAndSnapshotDictionary.TryGetValue(wantedPile, out snapshot);

        return snapshot;
    }
}

