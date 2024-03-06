using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine.UI;
// using System;

public class Bottle : MonoBehaviour
{

    public bool onTable;
    private Rigidbody2D _rb;
    public LineRenderer _lr;
    
    public Vector3 _throwVector;
    public float baseBottleDamage; // for handling situations where we need to know if a bottle has a damage buff
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

    private AudioSource audioSource;
    public AudioClip soundShatter1;
    public AudioClip soundShatter2;
    public AudioClip soundShatter3;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        empty = false;
        onTable = true;
        pickedUp = false;
        _throwPower = 1000f;
        baseBottleDamage = 15f;
        bottleDamage = baseBottleDamage;
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();
        toRight = true;

        audioSource = GetComponent<AudioSource>();
        
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
                transform.position = holdingPlayer.transform.position + holdingPlayer.transform.right * -1.05f;
                toRight = false;
            }
            else if (holdingPlayer.rightDirX > 0)
            {
                transform.position = holdingPlayer.transform.position + holdingPlayer.transform.right * 1.05f;
                toRight = true;
            }
            else if (toRight)
            {
                transform.position = holdingPlayer.transform.position + holdingPlayer.transform.right * 1.05f;
            }
            else
            {
                transform.position = holdingPlayer.transform.position + holdingPlayer.transform.right * -1.05f;
            }
            
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
        empty = true;
        _rb.AddForce(_throwVector);
    }



    public void BottleDropped()
    {
        spriteRenderer.sprite = brokenBottle;
        bottleDamage = 10f;

        int randy = Random.Range(0, 3);
        Debug.Log(randy);
        if (randy == 0)
            audioSource.PlayOneShot(soundShatter1, 1.0f);
        else if (randy == 1)
            audioSource.PlayOneShot(soundShatter2, 1.0f);
        else
            audioSource.PlayOneShot(soundShatter3, 1.0f);

        GetComponent<CircleCollider2D>().enabled = false;
    }
    // picked up false
    // empty false


    



}
