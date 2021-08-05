using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnsManager : MonoBehaviour
{
    [Header("Undo Movement")]
    public bool isRecordingMoves = false;
    public IntVariable m_movesIntVariable;
    //public IntVariable m_scoreIntVariable;
    //private Stack<Turn> m_recordedMovements;
    public List<Turn> m_recordedMovements;
  

    private void Awake()
    {
        m_movesIntVariable.value = 0;
        //m_recordedMovements = new Stack<Turn>();
        m_recordedMovements = new List<Turn>();
    }

    private void Start()
    {
        EventsManager.OnCardStackCheck += OnCardStackCheck;
        EventsManager.OnCardFlipped += OnCardFlipped;
    }

    private void OnCardFlipped(Card card)
    {
        Turn move = new Turn();
        move.recordedScore = ScoreManager.Instance.GetPreviousScore();

        List<Card> flippedCards = new List<Card>();
        flippedCards.Add(card);

        move.cardsMoved = flippedCards;

        //move.originalPile = card.m_cardPile;
        //move.endPile = card.m_cardPile;

        Debug.Log(move.cardsMoved.Count);

        Dictionary<Card, bool> flipState = new Dictionary<Card, bool>();
        for (int i = 0; i < move.cardsMoved.Count; i++)
        {
            if (move.cardsMoved[i].m_isFaceUp)
            {
                flipState.Add(move.cardsMoved[i], true);
            }
            else
            {
                flipState.Add(move.cardsMoved[i], false);
            }
        }

        move.cardFlipState = flipState;

        AddMoveToStack(move);

    }

    

    private void OnCardStackCheck(Card card, CardPile pile, bool canStack)
    {
        Turn move = new Turn();
        move.cardsMoved = card.m_cardAndAllAbove;
        move.recordedScore = ScoreManager.Instance.GetPreviousScore();

        Dictionary<Card, bool> flipState = new Dictionary<Card, bool>();
        for (int i = 0; i < move.cardsMoved.Count; i++)
        {
            if (move.cardsMoved[i].m_isFaceUp)
            {
                flipState.Add(move.cardsMoved[i], true);
            }
            else
            {
                flipState.Add(move.cardsMoved[i], false);
            }
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
        if (!isRecordingMoves)
            return;

        move.recordedScore = ScoreManager.Instance.GetPreviousScore();

        Debug.Log("Added move to stack: " + move.cardsMoved[0].name);

        //THis will save the flip state of each card
        Dictionary<Card, bool> flipStatesDictionary = new Dictionary<Card, bool>();
        for (int i = 0; i < move.cardsMoved.Count; i++)
        {
            if (move.cardsMoved[i].m_isFaceUp)
            {
                Debug.Log("Aded new move : Face Up");
                flipStatesDictionary.Add(move.cardsMoved[i], true);
            }
            else
            {
                Debug.Log("Aded new move : Face Down");
                flipStatesDictionary.Add(move.cardsMoved[i], false);
            }
        }

        move.cardFlipState = flipStatesDictionary;

        m_recordedMovements.Add(move);

        //Delete this
        Turn[] tempMovements = m_recordedMovements.ToArray();
        m_recordedMovements.Clear();
        for (int i = 0; i < tempMovements.Length; i++)
        {
            m_recordedMovements.Add(tempMovements[i]);
        }
        m_movesIntVariable.value++;
    }

    public void UndoLastMove()
    {
        if (m_recordedMovements.Count > 0)
        {
            Turn move = m_recordedMovements[m_recordedMovements.Count - 1];

            if (move.endPile != null)
                move.endPile.RemoveCardsFromPile(move.cardsMoved);

            if (move.originalPile != null)
                move.originalPile.AddCardsToPile(move.cardsMoved);

            for (int i = 0; i < move.cardsMoved.Count; i++)
            {
                bool flipStateDuringTurn = false;
                move.cardFlipState.TryGetValue(move.cardsMoved[i], out flipStateDuringTurn);
                if (flipStateDuringTurn)
                {
                    //move.cardsMoved[i].FlipUp();
                    move.cardsMoved[i].SetFlipState(true);
                }
                else
                {
                    //move.cardsMoved[i].FlipDown();
                    move.cardsMoved[i].SetFlipState(false);
                }
            }

            if (move.endPile != null)
                move.endPile.UpdatePositions();

            if (move.originalPile != null)
                move.originalPile.UpdatePositions();

            ScoreManager.Instance.SetPreviousScore(0);
            ScoreManager.Instance.SetScore(move.recordedScore);

            m_movesIntVariable.value--;

            Debug.Log("Remove lAst here");
            m_recordedMovements.Remove(m_recordedMovements[m_recordedMovements.Count - 1]);

            EventsManager.Fire_event_UndoMovement(move);

        }

    }
}

//TODO
[System.Serializable]
public struct Turn
{
    string turnName;
    public List<Card> cardsMoved;
    public Dictionary<Card, bool> cardFlipState;
    public CardPile originalPile;
    public CardPile endPile;
    public int recordedScore;
}

