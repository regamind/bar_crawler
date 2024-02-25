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
    public bool slip = false;

    public float prevDirX = 0f;
    public float prevDirY = 0f;

    private SpriteRenderer _spriteRenderer;

    [SerializeField] public GameObject fillA;
    [SerializeField] public GameObject fillB;

    public HealthBar healthBar;

    public DrunkMeter drunkMeter;

    private Animator _animator;

    private DamageFlash _damageFlash;

    private BubbleController _bubbleController;

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

        _damageFlash = GetComponent<DamageFlash>();

        _bubbleController = GetComponent<BubbleController>();

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

        //Debug.Log($"Previous dirX {prevDirX}");
        //Debug.Log($"Previous dirY {prevDirY}");

        if (freeze == true)
        {
            StartCoroutine(freezePlayer());
        }

        if (freeze == false && slip == false)
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
        
        }

        if (freeze == false && slip == true)
        {
            HandleSpill(prevDirX, prevDirY, 10f);
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

        if (drunkness > 50f)
        {
            _animator.SetBool("Drunk", true);
        }
        else
        {
            _animator.SetBool("Drunk", false);
        }

        PlayerSoberUp();
        checkSloshed();
        drunkMeter.setDrunk(drunkness);

        if (slip == false)
        {
            prevDirX = dirX;
            prevDirY = dirY;
        }

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
        _damageFlash.CallDamageFlash();
        healthBar.setHealth(health);
    }

    // makes it so that 
    void HandleSpill(float initialDirX, float initialDirY, float speedBoost)
    {
        Debug.Log("Handling Spill");
        // make faster, and freeze directional vector
        var dirX = 0f;
        var dirY = 0f;
        

        if (gameObject.tag == "Player1")
        {
            dirX = Input.GetAxisRaw("Horizontal1");
            dirY = Input.GetAxisRaw("Vertical1");
        }
        else if (gameObject.tag == "Player2")
        {
            dirX = Input.GetAxisRaw("Horizontal2");
            dirY = Input.GetAxisRaw("Vertical2");
        }

        prevDirX = ((initialDirX * 0.99f) + (dirX * 0.01f));
        prevDirY = ((initialDirY * 0.99f) + (dirY * 0.01f));

        rb.velocity = new Vector2((movementSpeedHorizontal + speedBoost) * prevDirX, (movementSpeedVertical + speedBoost) * prevDirY);

    }

    public void TriggerBubbles()
    {
        _bubbleController.TriggerBubbles();
    }
}
