using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spill : MonoBehaviour
{
    private Player[] _players;
    private Player _player1;
    private Player _player2;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _players = FindObjectsOfType<Player>();
        if (_players[0].tag == "Player1")
        {
            _player1 = _players[0];
            _player2 = _players[1];
        }
        else
        {
            _player1 = _players[1];
            _player2 = _players[0];
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // when it enter
        if (collision.name == "Player1" || collision.name == "Player2")
        {
            // give speed to player that lasts like 10 seconds
            if (collision.name == "Player1")
            {
                Debug.Log("PLayer1 slipped on wet spill");
                //_player1.movementSpeedHorizontal = 20f;
                //_player1.movementSpeedVertical = 17f;
                _player1.slip = true;
            }
            else if (collision.name == "Player2")
            {
                //_player2.movementSpeedHorizontal = 20f;
                //_player2.movementSpeedVertical = 17f;
                _player2.slip = true;
            }

            audioSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player1" || collision.name == "Player2")
        {
            // give speed to player that lasts like 10 seconds
            if (collision.name == "Player1")
            {
                Debug.Log("PLayer1 left wet spill");
                //_player1.movementSpeedHorizontal = 13f;
                //_player1.movementSpeedVertical = 10f;
                _player1.slip = false;
            }
            else if (collision.name == "Player2")
            {
                //_player2.movementSpeedHorizontal = 13f;
                //_player2.movementSpeedVertical = 10f;
                _player2.slip = false;
            }
        }
    }

    //void GivePlayerSpeed(Player player)
    //{

    //}
}
