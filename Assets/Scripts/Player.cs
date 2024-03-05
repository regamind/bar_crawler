// using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public float PunchCooldown = 1f; 

    public bool alive = true;
    public bool freeze = false;
    public bool slip = false;
    public bool holding = false;
    public bool stopBetweenRounds = false;
    public bool isPunched = false;


    public GameObject nearestEnemyObject;
    public Player nearestEnemy;


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

    public FloatingDamage criticalDamage;

    private TrailRenderer _trailRenderer;

    private AudioSource audioSource;
    public AudioClip soundOof;
    public AudioClip soundDrink1;
    public AudioClip soundDrink2;
    public AudioClip soundDrink3;
    public AudioClip soundVomit1;
    public AudioClip soundVomit2;
    public AudioClip soundVomit3;

    //my branch starts here
    public float dirX;
    public float dirY;
    public float rightDirX;

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
    private string _punch;

    [SerializeField] Vodka myVodka;
    [SerializeField] Tequila myTequila;
    [SerializeField] Beer myBeer;


    private bool _myTypeVodka;
    private bool _myTypeTequila;
    private bool _myTypeBeer;


    void Awake()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void Start()
    {
        _myTypeBeer = false;
        _myTypeTequila = false;
        _myTypeVodka = false;



        readyToThrow = false;
        nearestBottle = null;
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

        _trailRenderer = GetComponent<TrailRenderer>();

        criticalDamage = GetComponent<FloatingDamage>();

        _trailRenderer.enabled = false;

        if (gameObject.tag == "Player1")
        {
            _Rhorizontal = "RightHorizontal1";
            _Lhorizontal = "Horizontal1";
            _Rvertical = "RightVertical1";
            _Lvertical = "Vertical1";
            _interact = "Interact1";
            _drink = "Drink1";
            _throw = "rBumper1";
            _punch = "Punch1";

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
            _punch = "Punch2";
        }


        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {

        if (!freeze && !slip)
        {
            dirX = Input.GetAxisRaw(_Lhorizontal);
            dirY = Input.GetAxisRaw(_Lvertical);
            rightDirX = Input.GetAxisRaw(_Rhorizontal);


            
            float drunkness_weight = drunkness * 0.004f;
            float input_weight = 1f - drunkness_weight;

            // Randomness to movement as character gets drunker
            float drunkenHorizontalOffset = UnityEngine.Random.Range(-1f, 1f);
            float drunkenVerticalOffset = UnityEngine.Random.Range(-1f, 1f);


            if (!((dirX == 0f) && (dirY == 0f)))
            {
                float dirXWithDrunkness = (dirX * input_weight) + (drunkenHorizontalOffset * drunkness_weight);
                float dirYWithDrunkness = (dirY * input_weight) + (drunkenVerticalOffset * drunkness_weight);
                rb.velocity = new Vector2(movementSpeedHorizontal * dirXWithDrunkness, movementSpeedVertical * dirYWithDrunkness);
            }
            else
            {
                rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);
            }

            // previous movement: rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);
        }
        if (health < _damageFlash.lowHealthThreshold)
        {
            _damageFlash.StartLowHealthFlash();
        }
        else
        {
            _damageFlash.StopLowHealthFlash();
        }

        if (stopBetweenRounds == true)
        {
           // Debug.Log($"When Player can't pick up, does this repeat? what player {gameObject.tag}");
            StartCoroutine(freezeBetweenRounds());
        }

        if (isPunched == true)
        {
            StartCoroutine(PunchStunned());
        }



        if (freeze == true)
        {
           // Debug.Log($"assuming it's not this, but does this repeat? {gameObject.tag}");
            StartCoroutine(freezePlayer());
        }

        // do not put anything between the above if and below else or players will be able to pick up, drink, and throw while frozen


        else
        {
            if (freeze == false && slip == true)
            {
                HandleSpill(prevDirX, prevDirY, 10f);
            }


            if (!((dirX == 0f) && (dirY == 0f)))
            {
                // bring down _horizontal variables
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
                // same thing -> bring down dirX (make sure dirX and dirY are assigned beforehand)
                prevDirX = dirX;
                prevDirY = dirY;
            }

            // PICKUP LOGIC
            if (nearestBottleObject != null && Input.GetButtonDown(_interact) && !holding)
            {
                nearestBottle = nearestBottleObject.GetComponent<Bottle>();
                if (nearestBottle != null && !nearestBottle.pickedUp && !nearestBottle.empty)
                {
                    nearestBottle.PickUp(this);
                    holding = true;
                    myBottle = nearestBottle;
                    myDrinkObject = nearestBottleObject;
                    string myDrinkType = myDrinkObject.name;
                    if (myDrinkType == "Vodka(Clone)")
                    {
                        _myTypeVodka = true;


                    }
                    else if (myDrinkType == "BottlePrefab(Clone)")
                    {
                        _myTypeBeer = true;
                    }
                    else if (myDrinkType == "Tequila(Clone)")
                    {
                        _myTypeTequila = true;
                    }
                    /// MORE DRINK TYPES HERE
                }
            }



            /*
            if (Input.GetButtonDown(_punch) && (PunchCooldown <= 0f))
            {
                Debug.Log("Punch input");
                PunchCooldown = 5f;

            }
            */
            

            //PUNCH LOGIC

            PunchCooldown -= Time.deltaTime;

            if (nearestEnemyObject != null && Input.GetButtonDown(_punch) && !holding && (PunchCooldown <= 0f))  
            {

              //  Debug.Log(nearestEnemyObject);
                nearestEnemy = nearestEnemyObject.GetComponent<PunchCollider>().thisPlayer;
              //  Debug.Log(nearestEnemy);
               
                Debug.Log("punch happened here");

                nearestEnemy.isPunched = true;

                PunchCooldown = 3f;  // reset punch cooldown


            }

            // DRINK LOGIC
            if (myDrinkObject != null & Input.GetButtonDown(_drink))
            {
                if (!myBottle.empty)
                {
                    Drink(myBottle);
                }
                
            }

            //THROW LOGIC
            if (myBottle != null)
            {
                if (myBottle.pickedUp && myBottle.empty)
                {
                    CalculateThrowVec(myBottle);
                    SetTrajectory(myBottle);
                    if (Input.GetButtonDown(_throw))
                    {
                        if (myBottle._throwVector == new Vector3(0, 0, 0))
                        {
                            myBottle.BottleDropped();
                        }
                        myBottle.Throw();
                        RemoveTrajectory(myBottle);
                        holding = false;
                        myDrinkObject = null;
                        _myTypeBeer = false;
                        _myTypeTequila = false;
                        _myTypeVodka = false;
                    }
                }
            }
        }
        
        

    }

    IEnumerator freezeBetweenRounds()
    {
        rb.velocity = new Vector2(0, 0);
        _animator.SetBool("Throwup", true);
        yield return new WaitForSeconds(4);
        _animator.SetBool("Throwup", false);
        stopBetweenRounds = false;
        
    }

    IEnumerator PunchStunned()
    {
       // string thistag = gameObject.tag;
       // Debug.Log(thistag);
       // Debug.Log("punchStun called");
        rb.velocity = new Vector2(0, 0);
        //_animator.SetBool("Throwup", true); PUNCH ANIMATION HERE
        yield return new WaitForSeconds(1.5f);
        //  _animator.SetBool("Throwup", false);
        isPunched = false;

    }

    public void EnableTrail()
    {
        _trailRenderer.enabled = true;
    }

    // Function to disable the trail renderer
    public void DisableTrail()
    {
        _trailRenderer.enabled = false;
    }


    IEnumerator freezePlayer()
    {       
        rb.velocity = new Vector2(0,0);
        _animator.SetBool("Throwup", true);
        yield return new WaitForSeconds(2);
        freeze = false;
        _animator.SetBool("Throwup", false);
    }

    public void StopBetweenRounds()
    {
        stopBetweenRounds = true;

       // rb.velocity = new Vector2(0, 0);
    }

    private void checkSloshed()
    {
        if (drunkness >= 99f)
        {
            freeze = true;
            drunkness = 20f;
            //drunkness = MinDrunk;
            int randy = Random.Range(0, 3);
            if (randy == 0)
                audioSource.PlayOneShot(soundVomit1, 1.0f);
            else if (randy == 1)
                audioSource.PlayOneShot(soundVomit2, 1.0f);
            else
                audioSource.PlayOneShot(soundVomit3, 1.0f);
        }
    }

    private void PlayerSoberUp()
    {
        if (drunkness > 0f)
        {
            drunkness -= soberRate * Time.deltaTime;
        }
    }

        /*
        if (health <= 0)
        {
            drunkness = MinDrunk;
            drunkMeter.setDrunk(drunkness);
        }
        */
        
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "bottle")
        {
            if (collider.GetType() == typeof(CircleCollider2D))
            {
                nearestBottleObject = collider.gameObject;
            }
            else
            {
                if (!collider.gameObject.GetComponent<Bottle>().pickedUp)
                {
             
                    TakeDamage(collider.gameObject.GetComponent<Bottle>());
                    Destroy(collider.gameObject);
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Bottle")
        {
            //if ((gameObject.tag == "Player1" && !collision.collider.gameObject.GetComponent<Bottle>().pickedUp1) ||
            //    (gameObject.tag == "Player2" && !collision.collider.gameObject.GetComponent<Bottle>().pickedUp2))
            if (!collision.collider.gameObject.GetComponent<Bottle>().pickedUp)
                    
            {
                TakeDamage(collision.collider.gameObject.GetComponent<Bottle>());
                if (health <= 0)
                {
                    alive = false; // where death occurs, likely wanna play death animation as well
                }
            }
        }
    }

    void LowHealthFlasher()
    {
        if (health < _damageFlash.lowHealthThreshold)
        {
            _damageFlash.StartLowHealthFlash();
        }
    }

    void TakeDamage(Bottle bottle)
    {
        var damage = bottle.bottleDamage;
        health -= damage;
        _damageFlash.CallDamageFlash();
        healthBar.setHealth(health);
        audioSource.PlayOneShot(soundOof, 1.0f);

        if (damage > bottle.baseBottleDamage)
        {
            // other option is (damage * -1).toString()
            ShowText("CRIT!", Color.red);
        }
    }

    // makes it so that player is slippery (their input takes longer to have an effect and harder to change direction) and faster
    void HandleSpill(float initialDirX, float initialDirY, float speedBoost)
    {
        var dirX = 0f;
        var dirY = 0f;

        dirX = Input.GetAxisRaw(_Lhorizontal);
        dirY = Input.GetAxisRaw(_Lvertical);

        // keep dirX and dirY
        prevDirX = ((initialDirX * 0.99f) + (dirX * 0.01f));
        prevDirY = ((initialDirY * 0.99f) + (dirY * 0.01f));

        rb.velocity = new Vector2((movementSpeedHorizontal + speedBoost) * prevDirX, (movementSpeedVertical + speedBoost) * prevDirY);

    }

    public void TriggerBubbles()
    {
        _bubbleController.TriggerBubbles();
    }
    

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

    public void Drink(Bottle bottle)
    {
        if (_myTypeBeer)
        {
            myBeer.Drink(gameObject);
        }
        else if (_myTypeTequila)
        {
            myTequila.Drink(gameObject);
        }
        else if (_myTypeVodka)
        {
            myVodka.Drink(gameObject);
        }
        bottle.empty = true;
        drunkMeter.setDrunk(drunkness + 20f);
        drunkness += 20f;
        TriggerBubbles();
        bottle.spriteRenderer.sprite = bottle.emptyBottle;

        int randy = Random.Range(0, 3);
        if (randy == 0)
        {
            audioSource.PlayOneShot(soundDrink1, 1.0f);
        }
        else if (randy == 1)
        {

            audioSource.PlayOneShot(soundDrink2, 1.0f);
        }
        else
        {
            audioSource.PlayOneShot(soundDrink3, 1.0f);
        }
    }


    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        //fillA.SetActive(state == GameState.StartGame);
    }

    public void ShowText(string str, Color color)
    {
        criticalDamage.TriggerDamageText(str, color);
    }

}
