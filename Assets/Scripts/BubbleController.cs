using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public GameObject bubblePrefab;
    private Animator animator;
    public float yOffsetMultiplier = 0.45f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerBubbles()
    {
        
        // Calculate the offset based on the player's height
        float yOffset = transform.localScale.y * yOffsetMultiplier;

        // Calculate the position with the dynamic offset in the y-direction
        Vector3 spawnPosition = transform.position + new Vector3(0f, yOffset, 0f);


        GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
        bubble.transform.parent = transform; // since this script is attached to player, it should set the bubble's position to a child of player

        bubble.GetComponent<Animator>().SetTrigger("Bubble"); // Trigger the animation
        Destroy(bubble, bubble.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); // Destroy the bubble after animation
    }



    // this controller takes in a bubble prefab, and when triggerbubbles is called, it should instantiate a bubble animation at that position
    // when the animation does play, then focus on getting the animation to stay
}
