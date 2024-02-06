using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{

    [SerializeField] GameObject newBottle;

    public bool onTable;
    private Rigidbody2D _rb;
    private LineRenderer _lr;
    private Player _player;
    private Transform _playerTransform;
    private float _distanceToPlayer;
    private bool _pickedUp = false;
    private Vector3 _throwVector;
    private float _throwPower = 350;
    private Vector3 _startPos;
    // Start is called before the first frame update
    void Start()
    {
        onTable = true;
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();
        _player = FindObjectOfType<Player>();
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _playerTransform = _player.transform;
        _distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
        if (Input.GetButtonDown("rBumper"))
        {
            Debug.Log("right bumper");
        }


        if (onTable)
        {
            _rb.bodyType = RigidbodyType2D.Static;


        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (onTable && Input.GetButtonDown("Interact") && _distanceToPlayer < 2)
        {
            PickUp();

        }

        if (_pickedUp)
        {
            transform.position = _player.transform.position + _player.transform.right*1.1f;
        }

        if (Input.GetButton("rBumper") && _pickedUp)
        {
            CalculateThrowVec();
            SetTrajectory();

        }

        if (Input.GetButtonUp("rBumper"))
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

    private void PickUp()
    {
        transform.parent = _player.transform;
        transform.position = _player.transform.right;
        _pickedUp = true;
        onTable = false;
        Instantiate(newBottle, _startPos, Quaternion.identity);
    }

    private void SetTrajectory()
    {
        _lr.positionCount = 2;
        _lr.SetPosition(0, transform.position);
        _lr.SetPosition(1, transform.position + _throwVector.normalized * _throwPower/100);
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
        _pickedUp = false;
        //transform.parent = null;
        _rb.AddForce(_throwVector);
    }


}
