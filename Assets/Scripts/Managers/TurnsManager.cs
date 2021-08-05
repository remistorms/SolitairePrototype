using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnsManager : MonoBehaviour
{
    [Header("Undo Movement")]
    public IntVariable m_movesIntVariable;
    public IntVariable m_scoreIntVariable;
    private Stack<Turn> m_recordedMovements;
    public List<Turn> m_recordedPlayerMovements;
  

    private void Awake()
    {
        m_movesIntVariable.value = 0;
        m_recordedMovements = new Stack<Turn>();
        m_recordedPlayerMovements = new List<Turn>();
    }

    private void Start()
    {
        EventsManager.OnCardStackCheck += OnCardStackCheck;
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

            m_movesIntVariable.value--;

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
        Turn move = new Turn();
        move.cardsMoved = card.m_cardAndAllAbove;

        Dictionary<Card, bool> flipState = new Dictionary<Card, bool>();
        for (int i = 0; i < move.cardsMoved.Count; i++)
        {
            flipState.Add(move.cardsMoved[i], move.cardsMoved[i].m_isFaceUp);
        }

        move.cardFlipState = flipState;

        move.originalPile = card.m_previousPile;
        move.endPile = pile;

        if (canStack)
        {
            AddMoveToStack(move);
        }

    }

    private void OnDisable()
    {
        EventsManager.OnCardStackCheck -= OnCardStackCheck;
    }

    public void AddMoveToStack(Turn move)
    {
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

