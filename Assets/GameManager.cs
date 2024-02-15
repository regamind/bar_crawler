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

    public static GameManager Instance;

    public GameState state;

    public static event Action<GameState> OnGameStateChanged;

    public int P1Wins = 0;
    public int P2Wins = 0;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.StartGame);

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

            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleResetPositions()
    {
        HealthBar1.setHealth(100f);
        HealthBar2.setHealth(100f);

        GameObject[] bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (GameObject bottle in bottles)
        {
            Destroy(bottle);
        }

            //reset drunkeness meter when we have it
            //clear all bottles


        }

private void HandlePlayer2Victory()
    {
        throw new NotImplementedException();
    }

    private void HandlePlayer1Victory()
    {
        throw new NotImplementedException();
    }

    private void HandlePlayer1WinsRound()

    {
        Debug.Log("handle 1 wins round");

        if (P1Wins == 2)
        {

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
      //  throw new NotImplementedException();
    }


}

public enum GameState
{
    StartGame,
    Player1WinsRound,
    Player2WinsRound,
    Player1Victory,
    Player2Victory,
    ResetPositions
}

