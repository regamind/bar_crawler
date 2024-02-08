using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool _pickedUp1 = false;
    private bool _pickedUp2 = false;
    private Vector3 _throwVector;
    private float _bottleDamage = 10f;
    private float _throwPower = 200f;

    // Start is called before the first frame update
    void Start()
    {
        onTable = true;
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

        Debug.Log(_player1.tag);
    }

    // Update is called once per frame
    void Update()
    {
        _player1Transform = _player1.transform;
        _distanceToPlayer1 = Vector3.Distance(_player1Transform.position, transform.position);
        _player2Transform = _player2.transform;
        _distanceToPlayer2 = Vector3.Distance(_player2Transform.position, transform.position);

        if (Input.GetAxis("RightHorizontal") != 0)
        {
            Debug.Log("right stick");
        }

        if (Input.GetButtonDown("Interact"))
        {
            Debug.Log("Interact pressed");
        }


        if (onTable)
        {
            _rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (onTable && Input.GetButtonDown("Interact") && _distanceToPlayer1 < 2)
            PickUp(_player1);
        else if (onTable && Input.GetKey("m") && _distanceToPlayer2 < 2)
            PickUp(_player2);

        if (_pickedUp1)
            transform.position = _player1.transform.position + _player1.transform.right * 1.1f;
        else if (_pickedUp2)
            transform.position = _player2.transform.position + _player2.transform.right * 1.1f;


        // right now only player 1 can throw
        if (Input.GetButton("rBumper") && _pickedUp1)
        {
            CalculateThrowVec();
            SetTrajectory();

        }

        if (Input.GetButtonUp("rBumper") && _pickedUp1)
        {
            RemoveTrajectory();
            Throw();
        }
        
        
    }

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
            _pickedUp1 = true;
        else if (player.tag == "Player2")
            _pickedUp2 = true;

        onTable = false;
    }

    private void SetTrajectory()
    {
        _lr.positionCount = 2;
        _lr.SetPosition(0, transform.position);
        //_lr.SetPosition(1, _throwVector.normalized);
        _lr.SetPosition(1, transform.position + _throwVector/100);
        _lr.enabled = true;
    }

    private void RemoveTrajectory()
    {
        _lr.enabled = false;
    }

    private void CalculateThrowVec()
    {
        Vector2 joystickDir = new Vector2(Input.GetAxis("RightHorizontal"), -1*Input.GetAxis("RightVertical"));
        //Vector2 testDir = new Vector2(1, 1);
        _throwVector = joystickDir.normalized*_throwPower;
    }
    

    private void Throw()
    {
        _pickedUp1 = false;
        _pickedUp2 = false;
        //transform.parent = null;
        _rb.AddForce(_throwVector);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player1" || collision.tag == "Player2") 
        {
            Debug.Log("trigger collision");
            Destroy(this.gameObject);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player1" || collision.collider.name == "Player2")
        {
            Debug.Log("bottle collided in Bottle" );
            Destroy(this.gameObject);
        }
    }
}
