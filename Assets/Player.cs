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

    public float MinDrunk = 0f;
    public float drunkness;
    public float soberRate = .5f;

    public bool alive = true;
    public bool freeze = false;

    private SpriteRenderer _spriteRenderer;

    [SerializeField] public GameObject fillA;
    [SerializeField] public GameObject fillB;

    public HealthBar healthBar;

    public DrunkMeter drunkMeter;

    private Animator _animator;

    private void Start()
    {
        movementSpeedHorizontal = 13f;
        movementSpeedVertical = 10f;

        health = Maxhealth;
        healthBar.SetMaxHealth(Maxhealth);

        drunkness = MinDrunk;
        drunkMeter.setMinDrunk(MinDrunk);

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();

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
        var rightDirX = 0f;

        if (freeze == true)
        {
            StartCoroutine(freezePlayer());
        }

        if (freeze == false)
        {
            if (gameObject.tag == "Player1")
            {
                dirX = Input.GetAxisRaw("Horizontal1");
                dirY = Input.GetAxisRaw("Vertical1");
                rightDirX = Input.GetAxisRaw("RightHorizontal1");
            }
            else if (gameObject.tag == "Player2")
            {
                dirX = Input.GetAxisRaw("Horizontal2");
                dirY = Input.GetAxisRaw("Vertical2");
                rightDirX = Input.GetAxisRaw("RightHorizontal2");
            }


            rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);
            //    if (rightDirX > 0 || (rightDirX == 0 && dirX > 0))
            //        _spriteRenderer.flipX = false;
            //    else if (rightDirX < 0 || (rightDirX == 0 && dirX < 0))
            //        _spriteRenderer.flipX = true;
            //}
        }
        if (!((dirX == 0f) && (dirY == 0f)))
        {
            _animator.SetFloat("XInput", dirX);
            _animator.SetFloat("YInput", dirY);
        }

        if (rb.velocity != Vector2.zero)
        {
            _animator.SetBool("Walk", true);
        }
        else
        {
            _animator.SetBool("Walk", false);
        }

        PlayerSoberUp();
        checkSloshed();
        drunkMeter.setDrunk(drunkness);
    }

    IEnumerator freezePlayer()
    {       
        rb.velocity = new Vector2(0,0);
        yield return new WaitForSeconds(2);
        freeze = false;
    }

    private void checkSloshed()
    {
        if (drunkness >= 99f)
        {
            freeze = true;
            drunkness = MinDrunk;
        }
    }

    private void PlayerSoberUp()
    {
        if (drunkness > 0f)
        {
            drunkness -= soberRate * Time.deltaTime;
        }

        /*
        if (health <= 0)
        {
            drunkness = MinDrunk;
            drunkMeter.setDrunk(drunkness);
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "bottle")
        {
            //if ((gameObject.tag == "Player1" && !collider.gameObject.GetComponent<Bottle>().pickedUp1) ||
            //    (gameObject.tag == "Player2" && !collider.gameObject.GetComponent<Bottle>().pickedUp2))
            if (!collider.gameObject.GetComponent<Bottle>().pickedUp1 && !collider.gameObject.GetComponent<Bottle>().pickedUp2)
            {
                Debug.Log("bottle trigger Player");
                TakeDamage(collider.gameObject.GetComponent<Bottle>().bottleDamage);
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

                   // drunkness = MinDrunk; // reset drunkness
                   // Debug.Log("mindrunk in player");
                   // Debug.Log(drunkness);
                    health = Maxhealth; // reset the health of the player
                    alive = false; // where death occurs, likely wanna play death animation as well
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Bottle")
        {           
            if (!collision.collider.gameObject.GetComponent<Bottle>().pickedUp1 &&
                    !collision.collider.gameObject.GetComponent<Bottle>().pickedUp2)
            {
                Debug.Log("bottle collided in Player");
                TakeDamage(collision.collider.gameObject.GetComponent<Bottle>().bottleDamage); 
            }
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.setHealth(health);
    }
}
