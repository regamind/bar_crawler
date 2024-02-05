using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public float movementSpeedVertical = 3f;
    public float movementSpeedHorizontal = 4f;
    public float health = 100f;
    public bool alive = true;



    // Update is called once per frame
    void Update()
    {
        var dirX = Input.GetAxisRaw("Horizontal");
        var dirY = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);

        // handle death here: didn't add it here since wasn't sure if we wanna implement death once we do rounds
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.collider.name == "Bottle"))
        {
            health -= 10f; //reducing health by 10 each time on bottle hit
            if (health <= 0)
            {
                alive = false; // where death occurs, likely wanna play death animation as well
            }
        }

    }


}
