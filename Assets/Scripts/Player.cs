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
    float dirX;
    float dirY;
    float rightDirX;



    public HealthBar healthBar;

    public bool readyToThrow;
    public Bottle myBottle;
    public GameObject myDrink;

    public GameObject nearestBottle;
    private string _Rhorizontal;
    private string _Rvertical;
    private string _Lhorizontal;
    private string _Lvertical;
    private string _interact;
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
            _throw = "rBumper2";

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
        if (nearestBottle != null && Input.GetButton(_interact))
        {
            myBottle = nearestBottle.GetComponent<Bottle>();
            if (myBottle != null && !myBottle.pickedUp)
            {
                myBottle.PickUp(this);
                myDrink = nearestBottle;
                string myDrinkType = myDrink.name;
                if (myDrinkType == "Vodka")
                {
                    var myType = myDrink.GetComponent<Vodka>();

                }
                /// MORE DRINK TYPES HERE
            }
        }

        // DRINK LOGIC
        if (myDrink != null & Input.GetButton(_drink))
        {
            Drink(myBottle);
        }

        if (myBottle.pickedUp && myBottle.empty)
        {
            CalculateThrowVec(myBottle);
            SetTrajectory(myBottle);
            if (Input.GetButtonDown(_throw))
            {
                myBottle.Throw();
                RemoveTrajectory(myBottle);
            }
        }

        // handle death here: didn't add it here since wasn't sure if we wanna implement death once we do rounds
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "bottle")
        {
            //if ((gameObject.tag == "Player1" && !collider.gameObject.GetComponent<Bottle>().pickedUp1) ||
            //    (gameObject.tag == "Player2" && !collider.gameObject.GetComponent<Bottle>().pickedUp2))
            if (!collider.gameObject.GetComponent<Bottle>().pickedUp)
            {
                Debug.Log("bottle trigger Player");
                TakeDamage(collider.gameObject.GetComponent<Bottle>().bottleDamage);
                if (health <= 0)
                {
                    alive = false; // where death occurs, likely wanna play death animation as well
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
                Debug.Log("bottle collided in Player");
                TakeDamage(collision.collider.gameObject.GetComponent<Bottle>().bottleDamage);
                if (health <= 0)
                {
                    alive = false; // where death occurs, likely wanna play death animation as well
                }
            }
        }
    }

    public void Drink(Bottle bottle)
    {
        bottle.empty = true;
        myType.Drink(this);
        bottle.spriteRenderer.sprite = bottle.emptyBottle;
    }


    
    

    void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.setHealth(health);
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


}
