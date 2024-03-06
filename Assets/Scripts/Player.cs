using System.Collections;
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
    public float drinkProof;
    public float knockbackForce = 5f;
    public float knockbackDelay = 0.15f;
    private float aimingSlow = 0.85f;
    private float _normalSpeedHorizontal;
    private float _normalSpeedVertical;
    private float _aimingSlowHorizontal;
    private float _aimingSlowVertical;

    public float PunchCooldown = 1f; 

    public bool alive = true;
    public bool freeze = false;
    public bool slip = false;
    public bool holding = false;
    public bool stopBetweenRounds = false;
    public bool isPunched = false;
    private bool _isKnockbackRunning = false;
    public Vector2 knockbackDirection = new Vector2(0, 0);

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

    private StarsController _starsController;

    public FloatingDamage criticalDamage;

    private TrailRenderer _trailRenderer;

    private AudioSource _audioSource;
    public AudioClip soundOof;
    public AudioClip soundDrink1;
    public AudioClip soundDrink2;
    public AudioClip soundDrink3;
    public AudioClip soundVomit1;
    public AudioClip soundVomit2;
    public AudioClip soundVomit3;
    public AudioClip soundPunch;
    private bool _isPlayingSound = false;

    //my branch starts here
    public float dirX;
    public float dirY;
    public float rightDirX;

    public bool readyToThrow;
    public Bottle myBottle;
    public GameObject myDrinkObject;
    private GameObject _thrownObject;

    public GameObject nearestBottleObject;
    public Bottle nearestBottle;
    private string _Rhorizontal;
    private string _Rvertical;
    private string _Lhorizontal;
    private string _Lvertical;
    public string interact;
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
        _normalSpeedHorizontal = movementSpeedHorizontal;
        _normalSpeedVertical = movementSpeedVertical;
        _aimingSlowHorizontal = movementSpeedHorizontal * aimingSlow;
        _aimingSlowVertical = movementSpeedVertical * aimingSlow;

        health = Maxhealth;
        healthBar.SetMaxHealth(Maxhealth);

        drunkness = MinDrunk;
        drunkMeter.setMinDrunk(MinDrunk);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _damageFlash = GetComponent<DamageFlash>();
        _bubbleController = GetComponent<BubbleController>();
        _starsController = GetComponent<StarsController>();
        _trailRenderer = GetComponent<TrailRenderer>();
        criticalDamage = GetComponent<FloatingDamage>();

        _trailRenderer.enabled = false;

        if (gameObject.tag == "Player1")
        {
            _Rhorizontal = "RightHorizontal1";
            _Lhorizontal = "Horizontal1";
            _Rvertical = "RightVertical1";
            _Lvertical = "Vertical1";
            interact = "Interact1";
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
            interact = "Interact2";
            _drink = "Drink2";
            _throw = "rBumper2";
            _punch = "Punch2";
        }

        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!freeze && !slip & !isPunched)
        {
            dirX = Input.GetAxisRaw(_Lhorizontal);
            dirY = Input.GetAxisRaw(_Lvertical);
            rightDirX = Input.GetAxisRaw(_Rhorizontal);

            float drunkness_weight = drunkness * 0.004f;
            float input_weight = 1f - drunkness_weight;

            // Randomness to movement as character gets drunker
            float drunkenHorizontalOffset = Random.Range(-1f, 1f);
            float drunkenVerticalOffset = Random.Range(-1f, 1f);

            if (!((dirX == 0f) && (dirY == 0f)))
            {
                float dirXWithDrunkness = (dirX * input_weight) + (drunkenHorizontalOffset * drunkness_weight);
                float dirYWithDrunkness = (dirY * input_weight) + (drunkenVerticalOffset * drunkness_weight);
                rb.velocity = new Vector2(movementSpeedHorizontal * dirXWithDrunkness, movementSpeedVertical * dirYWithDrunkness);
            }
            else
                rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);
            // previous movement: rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);

        }
        if (health < _damageFlash.lowHealthThreshold)
            _damageFlash.StartLowHealthFlash();
        else
            _damageFlash.StopLowHealthFlash();

        if (stopBetweenRounds)
        {
            if (!_isPlayingSound)
            {
                int randy = Random.Range(0, 3);
                if (randy == 0)
                    _audioSource.PlayOneShot(soundVomit1, 1.0f);
                else if (randy == 1)
                    _audioSource.PlayOneShot(soundVomit2, 1.0f);
                else
                    _audioSource.PlayOneShot(soundVomit3, 1.0f);
            }
            StartCoroutine(freezeBetweenRounds());
        }

        if (isPunched && !_isKnockbackRunning)
            StartCoroutine(PunchStunned());

        if (freeze)
            StartCoroutine(freezePlayer());

        // do not put anything between the above if and below else or players will be able to pick up, drink, and throw while frozen

        else
        {
            if (!freeze && slip)
                HandleSpill(prevDirX, prevDirY, 10f);

            if (!((dirX == 0f) && (dirY == 0f)))
            {
                // bring down _horizontal variables
                _animator.SetFloat("XInput", dirX);
                _animator.SetFloat("YInput", dirY);
            }

            if (rb.velocity != Vector2.zero)
                _animator.SetBool("Walk", true);
            else
                _animator.SetBool("Walk", false);

            if (drunkness > 50f)
                _animator.SetBool("Drunk", true);
            else
                _animator.SetBool("Drunk", false);

            PlayerSoberUp();
            checkSloshed();
            drunkMeter.setDrunk(drunkness);

            if (!slip)
            {
                // same thing -> bring down dirX (make sure dirX and dirY are assigned beforehand)
                prevDirX = dirX;
                prevDirY = dirY;
            }

            // PICKUP LOGIC

            if (nearestBottleObject != null && Input.GetButtonDown(interact) && !holding && !stopBetweenRounds && !isPunched)
            {
                nearestBottle = nearestBottleObject.GetComponent<Bottle>();
                if (nearestBottle != null && !nearestBottle.pickedUp && !nearestBottle.empty)
                {
                    nearestBottle.PickUp(this);
                    holding = true;
                    myBottle = nearestBottle;
                    myDrinkObject = nearestBottleObject;
                    string myDrinkType = myDrinkObject.name;
                    if (myDrinkType.Contains("Vodka"))
                        _myTypeVodka = true;
                    else if (myDrinkType.Contains("Beer"))
                        _myTypeBeer = true;
                    else if (myDrinkType.Contains("Tequila"))
                        _myTypeTequila = true;
                }
            }

            //PUNCH LOGIC
            PunchCooldown -= Time.deltaTime;
            if (nearestEnemyObject != null && Input.GetButtonDown(_punch) && !holding && (PunchCooldown <= 0f) && !isPunched)  
            {
                nearestEnemy = nearestEnemyObject.GetComponent<PunchCollider>().thisPlayer;
                PunchCooldown = 3f;
                _animator.SetTrigger("Punch");
            }

            // DRINK LOGIC
            if (myDrinkObject != null & Input.GetButtonDown(_drink))
                if (!myBottle.empty)
                    Drink(myBottle);

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
                            myBottle.BottleDropped();

                        myBottle.Throw();
                        RemoveTrajectory(myBottle);
                        holding = false;
                        _thrownObject = myDrinkObject;
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
        _isPlayingSound = true;
        _animator.SetBool("Throwup", true);
        yield return new WaitForSeconds(4);
        _animator.SetBool("Throwup", false);
        stopBetweenRounds = false;
        _isPlayingSound = false;
    }

    IEnumerator PunchStunned()
    {
        _isKnockbackRunning = true;
        rb.isKinematic = true;
        rb.velocity = knockbackDirection * knockbackForce;
        
        yield return new WaitForSeconds(knockbackDelay);
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(1.5f);
        isPunched = false;
        _isKnockbackRunning = false;
        rb.isKinematic = false;
    }

    private void SlowDown()
    {
        movementSpeedHorizontal = _aimingSlowHorizontal;
        movementSpeedVertical = _aimingSlowVertical;
    }

    public void OnPunchConnect()
    {
        nearestEnemy.TriggerStars();
        _audioSource.PlayOneShot(soundPunch, 1.0f);

        // KNOCKBACK

        if (nearestEnemy.myBottle != null)
            nearestEnemy.ResetBottleVariables();

        nearestEnemy.isPunched = true;
        nearestEnemy.knockbackDirection = (nearestEnemy.transform.position - transform.position).normalized;
    }

    public void ResetBottleVariables()
    {
        myBottle._throwVector = new Vector3(0, 0, 0);
        myBottle.Throw();
        RemoveTrajectory(myBottle);
        myBottle.BottleDropped();
        holding = false;
        myDrinkObject = null;
        _myTypeBeer = false;
        _myTypeTequila = false;
        _myTypeVodka = false;
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
    }

    private void checkSloshed()
    {
        if (drunkness >= 99f)
        {
            freeze = true;
            drunkness = 0f;
            //drunkness = MinDrunk;
            int randy = Random.Range(0, 3);
            if (randy == 0)
                _audioSource.PlayOneShot(soundVomit1, 1.0f);
            else if (randy == 1)
                _audioSource.PlayOneShot(soundVomit2, 1.0f);
            else
                _audioSource.PlayOneShot(soundVomit3, 1.0f);
        }
    }

    private void PlayerSoberUp()
    {
        if (drunkness > 0f)
            drunkness -= soberRate * Time.deltaTime;
    }      
    
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
                Bottle colliderBottle = collider.gameObject.GetComponent<Bottle>();
                if (!colliderBottle.pickedUp && !colliderBottle.onTable)
                {
                    TakeDamage(collider.gameObject.GetComponent<Bottle>());
                    Destroy(collider.gameObject);
                    if (health <= 0)
                    {
                        if (gameObject.tag == "Player1")
                            GameManager.Instance.UpdateGameState(GameState.Player2WinsRound);
                        else if (gameObject.tag == "Player2")   // will need to change this to reflect second controller
                            GameManager.Instance.UpdateGameState(GameState.Player1WinsRound);

                        health = Maxhealth; // reset the health of the player
                        alive = false; // where death occurs, likely wanna play death animation as well
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bottle")
            if (collision.GetType() == typeof(CircleCollider2D))
                nearestBottleObject = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "bottle")
        {
            if (collision.GetType() == typeof(CircleCollider2D))
            {
                nearestBottle = null;
                nearestBottleObject = null;
            }
        }
    }

    void LowHealthFlasher()
    {
        if (health < _damageFlash.lowHealthThreshold)
            _damageFlash.StartLowHealthFlash();
    }

    void TakeDamage(Bottle bottle)
    {
        var damage = bottle.bottleDamage;
        health -= damage;
        _damageFlash.CallDamageFlash();
        healthBar.setHealth(health);
        _audioSource.PlayOneShot(soundOof, 1.0f);

        if (damage > bottle.baseBottleDamage)
            ShowText("CRIT!", Color.red);
    }

    // makes it so that player is slippery (their input takes longer to have an effect and harder to change direction) and faster
    void HandleSpill(float initialDirX, float initialDirY, float speedBoost)
    {
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

    public void resetPositions(float X,float Y)
    {
        rb.position = new Vector2(X, Y);
    }

    public void TriggerStars()
    {
        _starsController.TriggerStars();
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
        if (joystickDir != new Vector2(0, 0))
            SlowDown();
        else
        {
            movementSpeedVertical = _normalSpeedVertical;
            movementSpeedHorizontal = _normalSpeedHorizontal;
        }
        bottle._throwVector = joystickDir.normalized * bottle._throwPower;
    }

    public void Drink(Bottle bottle)
    {
        if (_myTypeBeer)
            myBeer.Drink(gameObject);
        else if (_myTypeTequila)
            myTequila.Drink(gameObject);
        else if (_myTypeVodka)
            myVodka.Drink(gameObject);

        bottle.empty = true;
        drunkMeter.setDrunk(drunkness + drinkProof);
        drunkness += drinkProof;
        TriggerBubbles();
        bottle.spriteRenderer.sprite = bottle.emptyBottle;

        int randy = Random.Range(0, 3);
        if (randy == 0)
            _audioSource.PlayOneShot(soundDrink1, 1.0f);
        else if (randy == 1)
            _audioSource.PlayOneShot(soundDrink2, 1.0f);
        else
            _audioSource.PlayOneShot(soundDrink3, 1.0f);
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        // Do nothing
    }

    public void ShowText(string str, Color color)
    {
        criticalDamage.TriggerDamageText(str, color);
    }
}
