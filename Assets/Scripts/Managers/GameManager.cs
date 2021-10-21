using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameState m_currentGameState = GameState.None;

    [Header("Main Objects")]
    public bool hasWon = false;
    public bool isGamePaused = false;
    public FloatVariable m_timer;

    [SerializeField] private InGameUI m_uiManager;
    [SerializeField] private CardsManager m_cardsManager;
    [SerializeField] private TurnsManager m_turnsManager;
    [SerializeField] private CardPile[] endPiles;
    [SerializeField] private ScoreManager m_scoreManager;

    private bool _threeCardMode;
    private bool _shuffleCards;

    public override void Awake()
    {
        base.Awake();

        TryChangeState(GameState.Initializing);
    }

    //Initialization of all pertinent objects to run the game
    private void InitializeGame()
    {
        //Stop recording turns
        m_turnsManager.isRecordingMoves = false;
        //reset win condition
        hasWon = false;
        //Reset Timer
        m_timer.value = 0.0f;
        //Reset Score
        m_scoreManager.InitScore();
    }

    private void Start()
    {
        m_cardsManager = FindObjectOfType<CardsManager>();
        m_scoreManager = FindObjectOfType<ScoreManager>();

        //StartGame();
    }

    public void GoToMainMenu()
    {
        StopAllCoroutines();
        //Play a different music
        SoundManager.Instance.ChangeMusic();
        //Reset Timer
        m_timer.value = 0.0f;
        //Reset Score
        m_scoreManager.InitScore();
        //Cards Manager Reset
        m_cardsManager.ResetCardsManager();
        //Go to main menu screen
        m_uiManager.SwitchScreens(0);
    }

    public void ResetGame()
    {
        //Start game
        StartGame(_threeCardMode, _shuffleCards);
    }

    public void StartGame(bool threeCardMode = false, bool shuffleCards = true)
    {
        _threeCardMode = threeCardMode;
        _shuffleCards = shuffleCards;
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

        GameWon();
    }

    public void GameWon()
    {
        StartCoroutine(GameWonRoutine());
    }

    IEnumerator GameWonRoutine()
    {
        m_turnsManager.isRecordingMoves = false;

        ScoreManager.Instance.CalculateFinalScore();

        yield return null;

        m_uiManager.SwitchScreens(3);
    }

    public bool TryChangeState(GameState gameState)
    {
        if (m_currentGameState == gameState)
        {
            Debug.Log("Already in that state...");
            return false;
        }
        else
        {
            Debug.Log("GameManager: Entering new state: " + gameState);
            switch (gameState)
            {
                case GameState.None:
                    break;

                case GameState.Initializing:
                    break;

                case GameState.GameRunning:
                    break;

                case GameState.GamePaused:
                    break;

                case GameState.GameWon:
                    break;

                case GameState.GameLost:
                    break;

                default:
                    break;
            }

            m_currentGameState = gameState;
            return true;
        }
    }

}

public struct GameSettings
{
    
}

public enum GameState
{
    None,
    Initializing,
    GameRunning,
    GamePaused,
    GameWon,
    GameLost
}
