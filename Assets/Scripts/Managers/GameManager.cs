using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Main Objects")]
    public bool hasWon = false;
    public bool isGamePaused = false;
    [SerializeField] private UIManager m_uiManager;
    [SerializeField] private CardsManager m_cardsManager;
    [SerializeField] private TurnsManager m_turnsManager;
    [SerializeField] private FloatVariable m_timer;
    [SerializeField] private CardPile[] endPiles;

    private void Awake()
    {
        m_turnsManager.isRecordingMoves = false;
        m_timer.value = 0.0f;
    }

    private void Start()
    {

    }

    public void StartGame()
    {
        StartCoroutine(StartGameLoop());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }
    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
    }

    bool CheckWinCondition()
    {
        if (endPiles[0].m_cardsInPile.Count <= 0)
        {
            return false;
        }

        else if (endPiles[0].GetTopCard() != null && endPiles[0].GetTopCard().m_cardValue == CardValue.King &&
            endPiles[1].GetTopCard() != null && endPiles[1].GetTopCard().m_cardValue == CardValue.King &&
            endPiles[2].GetTopCard() != null && endPiles[2].GetTopCard().m_cardValue == CardValue.King &&
            endPiles[3].GetTopCard() != null && endPiles[3].GetTopCard().m_cardValue == CardValue.King)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator StartGameLoop()
    {
        m_timer.value = 0;
        //Initialize Stuff here
        m_cardsManager.GenerateDeck();
        yield return null;
        //m_cardsManager.ShuffleDeck();
        yield return null;
        m_cardsManager.DealCards();

        while (!m_cardsManager.hasFinishedDealing)
        {
            yield return null;
        }

        m_turnsManager.isRecordingMoves = true;

        while (!hasWon)
        {
            if (isGamePaused)
            {
                yield return null;
            }
            else
            {
                m_timer.value += Time.deltaTime;
                yield return null;
            }

            hasWon = CheckWinCondition();
        }

        m_turnsManager.isRecordingMoves = false;

        Debug.Log("YOU WIN");
    }

    public void ResetGame()
    {
    }
}
