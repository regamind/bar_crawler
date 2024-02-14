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

    public HealthBar healthBar;

    private void Start()
    {
        movementSpeedHorizontal = 13f;
        movementSpeedVertical = 10f;
        health = Maxhealth;
        healthBar.SetMaxHealth(Maxhealth);

        _spriteRenderer = GetComponent<SpriteRenderer>();

        //if (gameObject.tag == "Player1")
        //    _spriteRenderer.flipX = false;
        //else if (gameObject.tag == "Player2")
        //    _spriteRenderer.flipX = true;
    }



    // Update is called once per frame
    void Update()
    {
        var dirX = 0f;
        var dirY = 0f;
        var rightDirX = 0f;

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
        if (rightDirX > 0 || (rightDirX == 0 && dirX > 0))
            _spriteRenderer.flipX = false;
        else if (rightDirX < 0 || (rightDirX == 0 && dirX < 0))
            _spriteRenderer.flipX = true;

        // handle death here: didn't add it here since wasn't sure if we wanna implement death once we do rounds
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "bottle")
        {
            //if ((gameObject.tag == "Player1" && !collider.gameObject.GetComponent<Bottle>().pickedUp1) ||
            //    (gameObject.tag == "Player2" && !collider.gameObject.GetComponent<Bottle>().pickedUp2))
            if (!collider.gameObject.GetComponent<Bottle>().pickedUp1 && !collider.gameObject.GetComponent<Bottle>().pickedUp1)
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
            if (!collision.collider.gameObject.GetComponent<Bottle>().pickedUp1 &&
                    !collision.collider.gameObject.GetComponent<Bottle>().pickedUp2)
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
        healthBar.setHealth(health);
    }


}
