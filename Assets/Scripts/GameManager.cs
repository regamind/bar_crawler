using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject fill1A;
    [SerializeField] public GameObject fill1B;
    [SerializeField] public GameObject fill2A;
    [SerializeField] public GameObject fill2B;

    public HealthBar HealthBar1;
    public HealthBar HealthBar2;

    public DrunkMeter DrunkMeter1;
    public DrunkMeter DrunkMeter2;

    public Player player1;
    public Player player2;

    public TextMeshProUGUI roundWinner;

    public static GameManager Instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;

    public int P1Wins = 0;
    public int P2Wins = 0;

    private AudioSource audioSource;
    public AudioClip soundClick;

    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("StartScene") ||
            SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameOverScene"))
        {
           
            if (Input.GetButtonDown("Interact1") || (Input.GetButtonDown("Interact2")))
            {
                audioSource.PlayOneShot(soundClick, 1.0f);
                SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.StartGame);
        audioSource = GetComponent<AudioSource>();
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.StartGame:
                HandleStartGame();
                break;
            case GameState.Player1WinsRound:
                HandlePlayer1WinsRound();
                break;
            case GameState.Player2WinsRound:
                HandlePlayer2WinsRound();
                break;
            case GameState.Player1Victory:
                HandlePlayer1Victory();
                break;
            case GameState.Player2Victory:
                HandlePlayer2Victory();
                break;
            case GameState.ResetPositions:
                HandleResetPositions();
                break;
            case GameState.IdleState:
                HandleIdleState();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleIdleState()
    {
        // Do nothing
    }

    private void freezePlayers()
    {
        player1.StopBetweenRounds();
        player2.StopBetweenRounds();
    }

    private void HandleResetPositions()
    {
        player1.resetPositions(-6f, 0f);
        player2.resetPositions(6f, 0f);

        freezePlayers();

        HealthBar1.setHealth(100f);
        HealthBar2.setHealth(100f);

        player1.health = player1.Maxhealth;
        player2.health = player2.Maxhealth;

        DrunkMeter1.setDrunk(0f);
        DrunkMeter2.setDrunk(0f);

        player1.drunkness = player1.MinDrunk;
        player2.drunkness = player2.MinDrunk;

        GameObject[] bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (GameObject bottle in bottles)
            Destroy(bottle);

        TableSpawnPoint[] tables = FindObjectsOfType<TableSpawnPoint>();

        foreach (TableSpawnPoint table in tables)
            table.ResetTables();

        player1.holding = false;
        player2.holding = false;

        Instance.UpdateGameState(GameState.IdleState);
    }

    private void HandlePlayer1Victory()
    {
        StateNameTracker.victoriousPlayer = "Player 1 wins";
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    private void HandlePlayer2Victory()
    {
        StateNameTracker.victoriousPlayer = "Player 2 wins";
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        roundWinner.text = message;
        roundWinner.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        roundWinner.gameObject.SetActive(false);

        roundWinner.text = "Next Round Begins!";
        roundWinner.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        roundWinner.gameObject.SetActive(false);
    }

    private void HandlePlayer1WinsRound()

    {
        if (P1Wins == 1)
        {
            fill1B.gameObject.SetActive(true);
            Instance.UpdateGameState(GameState.Player1Victory);
        }
        else 
            fill1A.gameObject.SetActive(true);
        P1Wins += 1;

        StartCoroutine(ShowMessage("Player 1 wins round", 2));

        Instance.UpdateGameState(GameState.ResetPositions);
    }

    private void HandlePlayer2WinsRound()
    {
        if (P2Wins == 1)
        {
            fill2B.gameObject.SetActive(true);
            Instance.UpdateGameState(GameState.Player2Victory);
        }
        else
            fill2A.gameObject.SetActive(true);
        P2Wins += 1;

        StartCoroutine(ShowMessage("Player 2 wins round", 2));

        Instance.UpdateGameState(GameState.ResetPositions);
    }

    private void HandleStartGame()
    {
        // Do nothing
    }
}

public enum GameState
{
    IdleState,
    StartGame,
    Player1WinsRound,
    Player2WinsRound,
    Player1Victory,
    Player2Victory,
    ResetPositions
}
