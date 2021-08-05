using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [Header("Undo Movement")]
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

    private void OnCardStackCheck(Card card, CardPile pile, bool canStack)
    {
        PlayerMovement move = new PlayerMovement();
        move.cardsMoved = card.m_cardAndAllAbove;
        move.originalPile = card.m_previousPile;
        move.endPile = pile;
        move.recordedScore = ScoreManager.Instance.GetScore();

        if (canStack)
        {
            AddMoveToStack(move);
        }

    }

    private void OnDisable()
    {
        EventsManager.OnCardStackCheck -= OnCardStackCheck;
    }

    public void AddMoveToStack(PlayerMovement move)
    {
        Debug.Log("Aded new move");

        m_recordedMovements.Push(move);

        //Delete this
        PlayerMovement[] tempMovements = m_recordedMovements.ToArray();
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
public struct PlayerMovement
{
    public List<Card> cardsMoved;
    public CardPile originalPile;
    public CardPile endPile;
    public int recordedScore;
}
