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
    public bool alive = true;
    private SpriteRenderer _spriteRenderer;
    float dirX;
    float dirY;
    float rightDirX;



    public HealthBar healthBar;

    public bool readyToThrow;
    public Bottle myBottle;
    public GameObject myDrinkObject;

    public GameObject nearestBottleObject;
    public Bottle nearestBottle;
    private string _Rhorizontal;
    private string _Rvertical;
    private string _Lhorizontal;
    private string _Lvertical;
    public string _interact;
    private string _drink;
    private string _throw;

    private void Start()
    {
        readyToThrow = false;
        movementSpeedHorizontal = 13f;
        movementSpeedVertical = 10f;
        health = Maxhealth;
        healthBar.SetMaxHealth(Maxhealth);
        nearestBottle = null;

        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (gameObject.tag == "Player1")
        {
            _Rhorizontal = "RightHorizontal1";
            _Lhorizontal = "Horizontal1";
            _Rvertical = "RightVertical1";
            _Lvertical = "Vertical1";
            _interact = "Interact1";
            _drink = "Drink1";
            _throw = "rBumper1";

        }
        else if (gameObject.tag == "Player2")
        {
            _Rhorizontal = "RightHorizontal2";
            _Lhorizontal = "Horizontal2";
            _Rvertical = "RightVertical2";
            _Lvertical = "Vertical2";
            _interact = "Interact2";
            _drink = "Drink2";
            _throw = "rBumper2";
        }


        
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
        _animator.SetBool("Throwup", true);
        yield return new WaitForSeconds(2);
        freeze = false;
        _animator.SetBool("Throwup", false);
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
        Debug.Log(Input.GetButtonDown(_interact));
        // PLAYER MOVEMENT
        

        dirX = Input.GetAxisRaw(_Lhorizontal);
        dirY = Input.GetAxisRaw(_Lvertical);
        rightDirX = Input.GetAxisRaw(_Rhorizontal);
        
        

        rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);
        if (rightDirX > 0 || (rightDirX == 0 && dirX > 0))
            _spriteRenderer.flipX = false;
        else if (rightDirX < 0 || (rightDirX == 0 && dirX < 0))
            _spriteRenderer.flipX = true;

        // PICKUP LOGIC
        if (nearestBottleObject != null && Input.GetButton(_interact))
        {
            nearestBottle = nearestBottleObject.GetComponent<Bottle>();
            if (nearestBottle != null && !nearestBottle.pickedUp)
            {
                nearestBottle.PickUp(this);
                myBottle = nearestBottle;
                myDrinkObject = nearestBottleObject;
                string myDrinkType = myDrinkObject.name;
                if (myDrinkType == "Vodka")
                {
                    Vodka myType = myDrinkObject.GetComponent<Vodka>();

                }
                /// MORE DRINK TYPES HERE
            }
        }

        // DRINK LOGIC
        if (myDrinkObject != null & Input.GetButton(_drink))
        {
            Drink(myBottle);
        }

        if (myBottle != null)
        {
            if (myBottle.pickedUp && myBottle.empty)
            {
                CalculateThrowVec(myBottle);
                SetTrajectory(myBottle);
                if (Input.GetButtonDown(_throw))
                {
                    myBottle.Throw();
                    RemoveTrajectory(myBottle);
                    myDrinkObject = null;
                    myBottle = null;
                }
            }
        }
        

        // handle death here: didn't add it here since wasn't sure if we wanna implement death once we do rounds
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "bottle")
        {
            if (collider.GetType() == typeof(CircleCollider2D))
            {
                // then deal with pickup
                Debug.Log("collided with circle: pickup logic");
                nearestBottleObject = collider.gameObject;
            }
            else
            {
                if (!collider.gameObject.GetComponent<Bottle>().pickedUp)
                {
             
                TakeDamage(collider.gameObject.GetComponent<Bottle>().bottleDamage);
                if (health <= 0)
                {
                

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
    }

    //         if (!collider.gameObject.GetComponent<Bottle>().pickedUp1 && !collider.gameObject.GetComponent<Bottle>().pickedUp2)
    //         {
    //          //   Debug.Log("bottle trigger Player");
    //             TakeDamage(collider.gameObject.GetComponent<Bottle>().bottleDamage);
    //             if (health <= 0)
    //             {
    //              //   Debug.Log("health <= 0");

    //                 if (gameObject.tag == "Player1")
    //                 {
    //                     GameManager.Instance.UpdateGameState(GameState.Player2WinsRound);
    //                 }
    //                 else if (gameObject.tag == "Player2")   // will need to change this to reflect second controller
    //                 {
    //                     GameManager.Instance.UpdateGameState(GameState.Player1WinsRound);
    //                 }

    //                 health = Maxhealth; // reset the health of the player
    //                 alive = false; // where death occurs, likely wanna play death animation as well
    //             }
    //         }
    //         Debug.Log(collider.GetType());
    //         if (collider.GetType() == typeof(CircleCollider2D))
    //         {
    //             // then deal with pickup
    //             Debug.Log("collided with circle: pickup logic");
    //             nearestBottleObject = collider.gameObject;
    //         }
    //         else
    //         {
    //             //if ((gameObject.tag == "Player1" && !collider.gameObject.GetComponent<Bottle>().pickedUp1) ||
    //             //    (gameObject.tag == "Player2" && !collider.gameObject.GetComponent<Bottle>().pickedUp2))
    //             //deal with damage
    //             if (!collider.gameObject.GetComponent<Bottle>().pickedUp)
    //             {
    //                 Debug.Log("bottle trigger Player");
    //                 TakeDamage(collider.gameObject.GetComponent<Bottle>().bottleDamage);
    //                 if (health <= 0)
    //                 {
    //                     alive = false; // where death occurs, likely wanna play death animation as well
    //                 }
    //             }
    //         }
            
            
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Bottle")
        {
            //if ((gameObject.tag == "Player1" && !collision.collider.gameObject.GetComponent<Bottle>().pickedUp1) ||
            //    (gameObject.tag == "Player2" && !collision.collider.gameObject.GetComponent<Bottle>().pickedUp2))
            if (!collision.collider.gameObject.GetComponent<Bottle>().pickedUp)
                    
            {
                Debug.Log("bottle collided in Player");
                TakeDamage(collision.collider.gameObject.GetComponent<Bottle>().bottleDamage);
                if (health <= 0)
                {
                    alive = false; // where death occurs, likely wanna play death animation as well
                }
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
    // public void Drink(Bottle bottle)
    // {
    //     bottle.empty = true;
    //     bottle.spriteRenderer.sprite = bottle.emptyBottle;
    // }

    public void SetTrajectory(Bottle bottle)
    {
        
        
        bottle._lr.positionCount = 2;
        bottle._lr.SetPosition(0, transform.position + transform.up * 1.1f);
        //_lr.SetPosition(1, _throwVector.normalized);
        bottle._lr.SetPosition(1, transform.position + transform.up * 1.1f + bottle._throwVector.normalized);
        bottle._lr.enabled = true;
        
        
    }

    private void RemoveTrajectory(Bottle bottle)
    {
        bottle._lr.enabled = false;
    }

    private void CalculateThrowVec(Bottle bottle)
    {
        Vector2 joystickDir = new Vector2(Input.GetAxis(_Rhorizontal), -1 * Input.GetAxis(_Rvertical));
        //Vector2 testDir = new Vector2(1, 1);
        bottle._throwVector = joystickDir.normalized * bottle._throwPower;
    }


}
