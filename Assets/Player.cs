using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public float movementSpeedVertical = 3f;
    public float movementSpeedHorizontal = 4f;


    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Player1")
        {
            var dirX = Input.GetAxisRaw("Horizontal");
            var dirY = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);
        }
        else if (gameObject.tag == "Player2")
        {
            var dirX = 0;
            if (Input.GetKey("l"))
                dirX += 1;
            if (Input.GetKey("j"))
                dirX -= 1;

            var dirY = 0;
            if (Input.GetKey("i"))
                dirY += 1;
            if (Input.GetKey("k"))
                dirY -= 1;

            rb.velocity = new Vector2(movementSpeedHorizontal * dirX, movementSpeedVertical * dirY);
        }
    }
}
