using System;
using System.Collections;
using System.Collections.Generic;
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


    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("StartScene") ||
            SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameOverScene"))
        {
           
            if (Input.GetButtonDown("Interact1") || (Input.GetButtonDown("Interact2")))
            {
                audioSource.Play();

                SceneManager.LoadSceneAsync(3, LoadSceneMode.Single);
               // SceneManager.SetActiveScene(SceneManager.GetSceneByName("SampleScene"));
             //   Debug.Log("nextScene called");
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
        //roundWinner.gameObject.SetActive(false);


        //SceneManager.LoadScene(1);


        //fill1A.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
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
      //  Debug.Log("am in IDLE");
    }

    private void freezePlayers()
    {
        Debug.Log("freeze in GM");
        player1.StopBetweenRounds();
        player2.StopBetweenRounds();
        //yield return new WaitForSeconds(4);

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

      //  Debug.Log("handle drunk reset");


        GameObject[] bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (GameObject bottle in bottles)
        {
         //   Debug.Log("destroyed bottle");
            Destroy(bottle);
        }

        TableSpawnPoint[] tables = FindObjectsOfType<TableSpawnPoint>();

        foreach (TableSpawnPoint table in tables)
        {
            table.ResetTables();
       //     Debug.Log("reset table");
            
        }

        player1.holding = false;
        player2.holding = false;


        Instance.UpdateGameState(GameState.IdleState);


        //reset drunkeness meter when we have it
        //clear all bottles
        // bug here in reseting bottles after a round, bottles don't always spawn again.


    }

private void HandlePlayer2Victory()
    {
      //  Debug.Log("state: player 2 victory, next scene called");
        StateNameTracker.victoriousPlayer = "Player 2 wins";
        SceneManager.LoadScene(2, LoadSceneMode.Single);
       // SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameOverScene"));
        
        
    }

    private void HandlePlayer1Victory()
    {
     //   Debug.Log("state: player 2 victory, next scene called");
        StateNameTracker.victoriousPlayer = "Player 1 wins";
        SceneManager.LoadScene(2, LoadSceneMode.Single);
       // SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameOverScene"));
        
        
    }


    IEnumerator ShowMessage(string message, float delay)
    {
        roundWinner.text = message;
        roundWinner.gameObject.SetActive(true);
        //debug.Log("showMessage called");
        yield return new WaitForSeconds(delay);
        roundWinner.gameObject.SetActive(false);

        roundWinner.text = "Next Round Begins!";
        roundWinner.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        roundWinner.gameObject.SetActive(false);


    }


    

    private void HandlePlayer1WinsRound()

    {
        // Debug.Log("handle 1 wins round");

        if (P1Wins == 2)
        {
       // Instance.UpdateGameState(GameState.Player1Victory);
        }

        if (P1Wins == 1)
        {
            fill1B.gameObject.SetActive(true);
            Instance.UpdateGameState(GameState.Player1Victory);
        }
        else 
        {
            fill1A.gameObject.SetActive(true);
        }
        P1Wins += 1;


        StartCoroutine(ShowMessage("Player 1 wins round", 2));

       


        Instance.UpdateGameState(GameState.ResetPositions);
    }

    private void HandlePlayer2WinsRound()

    {
        //  Debug.Log("handle 2 wins round");

        if (P2Wins == 2)
        {
        //    Instance.UpdateGameState(GameState.Player2Victory);
        }

        if (P2Wins == 1)
        {
            fill2B.gameObject.SetActive(true);
            Instance.UpdateGameState(GameState.Player2Victory);
        }
        else
        {
            fill2A.gameObject.SetActive(true);
        }

        P2Wins += 1;


        StartCoroutine(ShowMessage("Player 2 wins round", 2));

        Instance.UpdateGameState(GameState.ResetPositions);
    }

    private void HandleStartGame()
    {
      //  Debug.Log("state startgame");
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

