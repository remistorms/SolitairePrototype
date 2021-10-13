using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Main Objects")]
    public bool hasWon = false;
    public bool isGamePaused = false;

    public static GameManager instance;

    [SerializeField] private InGameUI m_uiManager;
    [SerializeField] private CardsManager m_cardsManager;
    [SerializeField] private TurnsManager m_turnsManager;
    [SerializeField] private FloatVariable m_timer;
    [SerializeField] private CardPile[] endPiles;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        m_turnsManager.isRecordingMoves = false;
        m_timer.value = 0.0f;
    }

    private void Start()
    {
        m_cardsManager = FindObjectOfType<CardsManager>();

        //StartGame();
    }

    public void ResetGame()
    {

    }

    public void StartGame(bool threeCardMode = false, bool shuffleCards = true)
    {
        StopAllCoroutines();
        StartCoroutine(StartGameRoutine(threeCardMode, shuffleCards));
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

    IEnumerator StartGameRoutine(bool threeCardMode = false, bool shuffleCards = true)
    {
        //Reset
        m_cardsManager.ResetCardsManager();

        while (!m_cardsManager.resetCompleted)
        {
            yield return null;
        }

        StartCoroutine(StartGameLoop(threeCardMode, shuffleCards));
    }

    IEnumerator StartGameLoop(bool threeCardMode = false, bool shuffleCards = true)
    {
        m_uiManager.SwitchScreens(1);

        m_cardsManager.m_drawThreeCardMode = threeCardMode;
        m_timer.value = 0;
        //Initialize Stuff here
        m_cardsManager.GenerateDeck();

        while (!m_cardsManager.deckGenerated)
        {
            yield return null;
        }

        if (shuffleCards)
        {
            m_cardsManager.ShuffleDeck();
        }
        yield return null;
        m_cardsManager.DealCards();

        while (!m_cardsManager.dealCompleted)
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

        m_uiManager.SwitchScreens(3);

        Debug.Log("YOU WIN");
    }

}

public struct GameSettings
{
    
}
