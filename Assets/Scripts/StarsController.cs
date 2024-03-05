using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsController : MonoBehaviour
{
    public GameObject starsPrefab;
    private Animator animator;
    public float yOffsetMultiplier = 0.45f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerStars()
    {

        // Calculate the offset based on the player's height
        float yOffset = transform.localScale.y * yOffsetMultiplier;

        // Calculate the position with the dynamic offset in the y-direction
        Vector3 spawnPosition = transform.position + new Vector3(0f, yOffset, 0f);


        GameObject stars = Instantiate(starsPrefab, spawnPosition, Quaternion.identity);
        stars.transform.parent = transform; // since this script is attached to player, it should set the star's position to a child of player

        stars.GetComponent<Animator>().SetTrigger("Stars"); // Trigger the animation
        Destroy(stars, stars.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); // Destroy the bubble after animation
    }

}
