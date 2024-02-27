using System;
using System.Collections;
using System.Collections.Generic;
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

    


    public static GameManager Instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;

    public int P1Wins = 0;
    public int P2Wins = 0;


    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("StartScene") ||
            SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameOverScene"))
        {
           
            if (Input.GetButtonDown("Interact1"))
            {


                SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
               // SceneManager.SetActiveScene(SceneManager.GetSceneByName("SampleScene"));
                Debug.Log("nextScene called");
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
       

        //SceneManager.LoadScene(1);


        //fill1A.gameObject.SetActive(false);
    }


    public void UpdateGameState(GameState newState)
    {
        state = newState;
       // Debug.Log("switch state to);
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
       // Debug.Log("am in IDLE");
    }


    private void HandleResetPositions()
    {
        HealthBar1.setHealth(100f);
        HealthBar2.setHealth(100f);

        DrunkMeter1.setDrunk(0f);
        DrunkMeter2.setDrunk(0f);

        player1.drunkness = player1.MinDrunk;
        player2.drunkness = player2.MinDrunk;

        Debug.Log("handle drunk reset");


        GameObject[] bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (GameObject bottle in bottles)
        {
            Destroy(bottle);
        }

        Instance.UpdateGameState(GameState.IdleState);


        //reset drunkeness meter when we have it
        //clear all bottles
        // bug here in reseting bottles after a round, bottles don't always spawn again.


    }

private void HandlePlayer2Victory()
    {
        StateNameTracker.victoriousPlayer = "Player 2 wins!";
        SceneManager.LoadScene(2, LoadSceneMode.Single);
       // SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameOverScene"));
        Debug.Log("nextScene called");
        
    }

    private void HandlePlayer1Victory()
    {
        StateNameTracker.victoriousPlayer = "Player 1 wins!";
        SceneManager.LoadScene(2, LoadSceneMode.Single);
       // SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameOverScene"));
        Debug.Log("nextScene called");
        
    }

    private void HandlePlayer1WinsRound()

    {
        Debug.Log("handle 1 wins round");

        if (P1Wins == 2)
        {
        Instance.UpdateGameState(GameState.Player1Victory);
        }

        if (P1Wins == 1)
        {
            fill1B.gameObject.SetActive(true);
        }
        else 
        {
            fill1A.gameObject.SetActive(true);
        }
        P1Wins += 1;
        Instance.UpdateGameState(GameState.ResetPositions);
    }

    private void HandlePlayer2WinsRound()
    {
        Debug.Log("handle 2 wins round");

        if (P1Wins == 2)
        {
            Instance.UpdateGameState(GameState.Player2Victory);
        }

        if (P2Wins == 1)
        {
            fill2B.gameObject.SetActive(true);
        }
        else
        {
            fill2A.gameObject.SetActive(true);
        }

        P2Wins += 1;
        Instance.UpdateGameState(GameState.ResetPositions);
    }

    private void HandleStartGame()
    {
       // SceneManager.LoadScene(1, LoadSceneMode.Single);
       // Debug.Log("god please help me");
       // SceneManager.SetActiveScene(SceneManager.GetSceneByName("StartScene"));
       // Instance.UpdateGameState(GameState.IdleState);
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

