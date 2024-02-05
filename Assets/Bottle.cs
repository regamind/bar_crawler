using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{

    public bool onTable;
    private Rigidbody2D _rb;
    private LineRenderer _lr;
    private Player _player;
    private Transform _playerTransform;
    private float _distanceToPlayer;
    private bool _pickedUp = false;
    private Vector3 _throwVector;
    private float _throwPower = 5;
    // Start is called before the first frame update
    void Start()
    {
        onTable = true;
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerTransform = _player.transform;
        _distanceToPlayer = Vector3.Distance(_playerTransform.position, transform.position);
        if (Input.GetButtonDown("rBumper"))
        {
            Debug.Log("rBumper pressed");
        }


        if (onTable)
        {
            _rb.bodyType = RigidbodyType2D.Static;


        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (onTable && Input.GetButtonDown("Interact") && _distanceToPlayer < 1)
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
    }

    private void SetTrajectory()
    {
        _lr.positionCount = 2;
        _lr.SetPosition(0, Vector3.zero);
        _lr.SetPosition(1, _throwVector.normalized/2);
        _lr.enabled = true;
    }

    private void RemoveTrajectory()
    {
        _lr.enabled = false;
    }

    private void CalculateThrowVec()
    {
        Vector2 joystickDir = new Vector2(Input.GetAxis("RightHorizontal"), Input.GetAxis("RightVertical"));
        _throwVector = joystickDir.normalized/_throwPower;


    }
    

    private void Throw()
    {
        _rb.AddForce(_throwVector);
    }

    
}
