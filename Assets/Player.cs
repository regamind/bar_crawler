using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public float movementSpeedVertical;
    public float movementSpeedHorizontal;
    public float Maxhealth = 100f;
    public float health;
    public bool alive = true;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] public GameObject fillA;
    [SerializeField] public GameObject fillB;

    public HealthBar healthBar;

    private void Start()
    {
        movementSpeedHorizontal = 13f;
        movementSpeedVertical = 10f;
        health = Maxhealth;
        healthBar.SetMaxHealth(Maxhealth);

        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (gameObject.tag == "Player1")
            _spriteRenderer.flipX = false;
        else if (gameObject.tag == "Player2")
            _spriteRenderer.flipX = true;
    }

     void Awake()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        //fillA.SetActive(state == GameState.StartGame);
    }



    // Update is called once per frame
    void Update()
    {
        var dirX = 0f;
        var dirY = 0f;

        if (gameObject.tag == "Player1")
        {
            dirX = Input.GetAxisRaw("Horizontal1");
            dirY = Input.GetAxisRaw("Vertical1");
        }
        else if (gameObject.tag == "Player2")   // will need to change this to reflect second controller
        {
            dirX = Input.GetAxisRaw("Horizontal2");
            dirY = Input.GetAxisRaw("Vertical2");
        }

        rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);
        if (dirX > 0)
            _spriteRenderer.flipX = false;
        else if (dirX < 0)
            _spriteRenderer.flipX = true;

        // handle death here: didn't add it here since wasn't sure if we wanna implement death once we do rounds
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.collider.name == "Bottle"))
        {
            Debug.Log("bottle collided in Player");
            TakeDamage(10f);
            //health -= 10f; //reducing health by 10 each time on bottle hit
            if (health <= 0)
            {
                alive = false; // where death occurs, likely wanna play death animation as well
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bottle")
        {
            Debug.Log("bottle trigger Player");
            TakeDamage(10f);
            //health -= 10f; //reducing health by 10 each time on bottle hit
            if (health <= 0)
            {

                Debug.Log("health <= 0");

                if (gameObject.tag == "Player1")
                {
                    GameManager.Instance.UpdateGameState(GameState.Player2WinsRound);
                }
                else if (gameObject.tag == "Player2")   // will need to change this to reflect second controller
                {
                    GameManager.Instance.UpdateGameState(GameState.Player1WinsRound);
                }



                health = Maxhealth; // reset the 
                alive = false; // where death occurs, likely wanna play death animation as well
            }

        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.setHealth(health);
    }


}
