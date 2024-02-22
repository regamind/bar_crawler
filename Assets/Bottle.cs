using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine.UI;
using System;

public class Bottle : MonoBehaviour
{

    public bool onTable;
    private Rigidbody2D _rb;
    private LineRenderer _lr;
    private Player[] _players;
    private Player _player1;
    private Transform _player1Transform;
    private float _distanceToPlayer1;
    private Player _player2;
    private Transform _player2Transform;
    private float _distanceToPlayer2;
    public  bool pickedUp1 = false;
    public  bool pickedUp2 = false;
    private Vector3 _throwVector;
    public  float bottleDamage;
    private Vector3 _spawnPoint;
    private float _throwPower;

    [SerializeField] Sprite emptyBottle;
    public SpriteRenderer spriteRenderer;


  //  public float fillTime;

    public bool empty1;
    public bool empty2;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        empty1 = false;
        empty2 = false;
        _throwPower = 1000f;
        onTable = true;
        bottleDamage = 10f;
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();

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
    }

    // Update is called once per frame
    void Update()
    {
        _player1Transform = _player1.transform;
        _distanceToPlayer1 = Vector3.Distance(_player1Transform.position, transform.position);
        _player2Transform = _player2.transform;
        _distanceToPlayer2 = Vector3.Distance(_player2Transform.position, transform.position);

        if (onTable)
        {
            _rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (onTable && Input.GetButtonDown("Interact1") && _distanceToPlayer1 < 2 && !_player1.freeze)
            PickUp(_player1);
        else if (onTable && Input.GetButtonDown("Interact2") && _distanceToPlayer2 < 2 && !_player2.freeze)
            PickUp(_player2);

        if (pickedUp1)
        {
            Debug.Log("Bottle picked up by player1");
            Debug.Log($"Float of InputX: {_player1.GetComponent<Animator>().GetFloat("XInput")}");
            if (_player1.GetComponent<Animator>().GetFloat("XInput") >= 0f)
                transform.position = _player1.transform.position + _player1.transform.right * 1.1f;
            else
                transform.position = _player1.transform.position + _player1.transform.right * -1.1f;
        }
        else if (pickedUp2)
        {
            if (_player2.GetComponent<Animator>().GetFloat("XInput") >= 0f)
                transform.position = _player2.transform.position + _player2.transform.right * 1.1f;
            else
                transform.position = _player2.transform.position + _player2.transform.right * -1.1f;
        }

        if (pickedUp1 && Input.GetButtonDown("Drink1") && !empty1)
        {
            Debug.Log("Player1 drank the bottle");
            Drink(_player1);
        }

        if (pickedUp2 && Input.GetButtonDown("Drink2") && !empty2)
        {
            Debug.Log("Player2 drank the bottle");
            Drink(_player2);
        }

        
        if (pickedUp1 && empty1 && !_player1.freeze)
        {
            CalculateThrowVec("1");
            SetTrajectory("1");
        }
        else if (pickedUp2 && empty2 && !_player2.freeze)
        {
            CalculateThrowVec("2");
            SetTrajectory("2");
        }

        if (Input.GetButtonDown("rBumper1") && pickedUp1 && empty1 && !_player1.freeze)
        {
            RemoveTrajectory();
            Throw();
        }
        else if (Input.GetButtonDown("rBumper2") && pickedUp2 && empty2 && !_player2.freeze)
        {
            RemoveTrajectory();
            Throw();
        }

      //  DepleteDrunk();
        
    }
/*
    void DepleteDrunk()
    {

        _player1.drunkness = Mathf.Lerp(_player1.MinDrunk, _player1.Maxhealth, fillTime) ;
        _player1.drunkMeter.setDrunk(_player1.drunkness);

        _player2.drunkness = Mathf.Lerp(_player2.MinDrunk, _player2.Maxhealth, fillTime);
        _player2.drunkMeter.setDrunk(_player2.drunkness);


        fillTime -= 0.375f * Time.deltaTime;

    }
*/

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name.Equals("Table"))
        {
            onTable = false;
        }
    }

    private void PickUp(Player player)
    {
        transform.parent = player.transform;
        transform.position = player.transform.right;

        if (player.tag == "Player1")
            pickedUp1 = true;
        else if (player.tag == "Player2")
            pickedUp2 = true;

        onTable = false;
    }

    private void SetTrajectory(string player_num)
    {
        if (player_num == "1")
        {
            _lr.positionCount = 2;
            _lr.SetPosition(0, _player1.transform.position + _player1.transform.up * 1.1f);
            //_lr.SetPosition(1, _throwVector.normalized);
            _lr.SetPosition(1, _player1.transform.position + _player1.transform.up * 1.1f + _throwVector.normalized);
            _lr.enabled = true;
        }
        else
        {
            _lr.positionCount = 2;
            _lr.SetPosition(0, _player2.transform.position + _player2.transform.up * 1.1f);
            //_lr.SetPosition(1, _throwVector.normalized);
            _lr.SetPosition(1, _player2.transform.position + _player2.transform.up * 1.1f + _throwVector.normalized);
            _lr.enabled = true;
        }
    }

    private void RemoveTrajectory()
    {
        _lr.enabled = false;
    }

    private void CalculateThrowVec(string player_num)
    {
        Vector2 joystickDir = new Vector2(Input.GetAxis("RightHorizontal" + player_num), -1*Input.GetAxis("RightVertical" + player_num));
        //Vector2 testDir = new Vector2(1, 1);
        _throwVector = joystickDir.normalized*_throwPower;
    }   

    private void Throw()
    {
        pickedUp1 = false;
        pickedUp2 = false;
        //transform.parent = null;
        _rb.AddForce(_throwVector);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!pickedUp1 && !pickedUp2)
        {
            if(collider.tag == "Player1" || collider.tag == "Player2") 
            {
                Debug.Log("trigger collision");
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!pickedUp1 && !pickedUp2)
        {
            if (collision.collider.name == "Player1" || collision.collider.name == "Player2")
            {
                Debug.Log("bottle collided in Bottle");
                Destroy(this.gameObject);
            }
        }
    }

    private void Drink(Player player)
    {
        if (player == _player1)
        {
            empty1 = true;
            _player1.drunkMeter.setDrunk(_player1.drunkness + 20f);
            _player1.drunkness += 20f;
        }
        else
        {
            empty2 = true;
            _player2.drunkMeter.setDrunk(_player2.drunkness + 20f);
            _player2.drunkness += 20f;
        }
        spriteRenderer.sprite = emptyBottle;
    }
}
