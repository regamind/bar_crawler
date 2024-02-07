using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public float movementSpeedVertical = 3f;
    public float movementSpeedHorizontal = 4f;
    public float Maxhealth = 100f;
    public float health;
    public bool alive = true;

    public HealthBar healthBar;

    private void Start()
    {
        health = Maxhealth;
        healthBar.SetMaxHealth(Maxhealth);
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
    void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.setHealth(health);
    }


}
