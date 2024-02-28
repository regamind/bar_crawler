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
    public LineRenderer _lr;
    
    public Vector3 _throwVector;
    public float bottleDamage;
    private Vector3 _spawnPoint;
    public float _throwPower;

    public Sprite emptyBottle;
    public SpriteRenderer spriteRenderer;
    public DamageBuff damageBuff;

    public bool empty;
    public bool pickedUp;
    public Player holdingPlayer;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        empty = false;
        onTable = true;
        pickedUp = false;
        _throwPower = 1000f;
        bottleDamage = 30f;
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();

        
    }

    // Update is called once per frame
    void Update()
    {

        if (onTable)
        {
            _rb.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }

        if (pickedUp)
        {
            transform.position = holdingPlayer.transform.position + holdingPlayer.transform.right * 1.1f;
        }

        
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name.Equals("Table"))
        {
            onTable = false;
        }
    }

    public void PickUp(Player player)
    {
        transform.parent = player.transform;
        transform.position = player.transform.right;

        //if (player.tag == "Player1")
        //    pickedUp1 = true;
        //else if (player.tag == "Player2")
        //    pickedUp2 = true;
        pickedUp = true;
        onTable = false;
        holdingPlayer = player;
        
        
    }

    public void Throw()
    {
        pickedUp = false;
        empty = false;
        _rb.AddForce(_throwVector);
    }

    //private void OnTriggerEnter2D(Collider2D collider)
    //{
    //    if (!pickedUp && !onTable)
    //    {
    //        if(collider.tag == "Player1" || collider.tag == "Player2") 
    //        {
    //            Debug.Log("trigger collision");
    //            Destroy(this.gameObject);
    //        }
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!pickedUp && !onTable)
        {
            if (collision.collider.name == "Player1" || collision.collider.name == "Player2")
            {
                Debug.Log("bottle collided in Bottle");
                Destroy(this.gameObject);
            }
        }
    }
}
