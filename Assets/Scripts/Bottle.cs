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
    public Sprite brokenBottle;
    

    public bool empty;
    public bool pickedUp;
    public Player holdingPlayer;
    public bool toRight;


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
        toRight = true;
        

        
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
            if (holdingPlayer.rightDirX < 0)
            {
                transform.position = holdingPlayer.transform.position + holdingPlayer.transform.right * -1.1f;
                toRight = false;
            }
            else if (holdingPlayer.rightDirX > 0)
            {
                transform.position = holdingPlayer.transform.position + holdingPlayer.transform.right * 1.1f;
                toRight = true;
            }
            else if (toRight)
            {
                transform.position = holdingPlayer.transform.position + holdingPlayer.transform.right * 1.1f;
            }
            else
            {
                transform.position = holdingPlayer.transform.position + holdingPlayer.transform.right * -1.1f;
            }
            
        }

        //if (!pickedUp && empty && _rb.totalForce == new Vector2(0, 0)){
        //    BottleDropped();
            
        //}


        

        
        
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
        empty = true;
        _rb.AddForce(_throwVector);
    }



    public void BottleDropped()
    {
        spriteRenderer.sprite = brokenBottle;
    }
    // picked up false
    // empty false


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
