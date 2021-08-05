using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public int m_maxUndoMovements = 3;
    public IntVariable m_movesIntVariable;
    public IntVariable m_scoreIntVariable;

    private Stack<PlayerMovement> m_recordedMovements;
    public List<PlayerMovement> m_recordedPlayerMovements;
  

    private void Awake()
    {
        m_movesIntVariable.value = 0;
        m_recordedMovements = new Stack<PlayerMovement>();
        m_recordedPlayerMovements = new List<PlayerMovement>();
    }

    private void Start()
    {
        EventsManager.OnCardDragStarted += OnCardDragStarted;
        EventsManager.OnCardStackCheck += OnCardStackCheck;
    }

    public void StartGame()
    {
    }

    public void PauseGame()
    {
    }

    public void RestartGame()
    {
    }

    public void UndoLastMove()
    {
        if (m_recordedMovements.Count > 0)
        {
            PlayerMovement move = m_recordedMovements.Pop();

            move.endPile.RemoveCardsFromPile(move.cardsMoved);

            move.originalPile.AddCardsToPile(move.cardsMoved);

            move.endPile.UpdatePositions();

            move.originalPile.UpdatePositions();

            m_movesIntVariable.value--;

            //Delete this
            PlayerMovement[] tempMovements = m_recordedMovements.ToArray();
            m_recordedPlayerMovements.Clear();
            for (int i = 0; i < tempMovements.Length; i++)
            {
                m_recordedPlayerMovements.Add(tempMovements[i]);
            }
            EventsManager.Fire_event_UndoMovement(move);
        }

    }

    //Creates a move when draggin
    private void OnCardDragStarted(Card card, PointerEventData pointerData)
    {

    }

    private void OnCardStackCheck(Card card, CardPile pile, bool canStack)
    {
        if (canStack)
        {
            Debug.Log("Record player movement here");

            PlayerMovement move = new PlayerMovement();
            move.cardsMoved = card.m_cardAndAllAbove;
            move.originalPile = card.m_previousPile;
            move.endPile = pile;

            m_recordedMovements.Push(move);
            //Debug only
            m_recordedPlayerMovements.Add(m_recordedMovements.Peek());

            m_movesIntVariable.value++;

        }

    }

    private void OnDisable()
    {
        EventsManager.OnCardDragStarted -= OnCardDragStarted;
        EventsManager.OnCardStackCheck -= OnCardStackCheck;
    }
}

//TODO
[System.Serializable]
public struct PlayerMovement
{
    public List<Card> cardsMoved;
    public CardPile originalPile;
    public CardPile endPile;
    public int scoreBeforeMove;
    public int scoreAfterMove;
}
