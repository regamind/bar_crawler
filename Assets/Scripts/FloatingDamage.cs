using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    public GameObject floatingDamagePrefab;
    private Animator animator;
    public float yOffsetMultiplier = 0.45f;
    public float xOffsetMultiplier = -0.45f;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerDamageText(string textToDisplay, Color color)
    {

        // Calculate the offset based on the player's height
        float yOffset = transform.localScale.y * yOffsetMultiplier;
        float xOffset = transform.localScale.x * xOffsetMultiplier;

        // Calculate the position with the dynamic offset in the y-direction
        Vector3 spawnPosition = transform.position + new Vector3(xOffset, yOffset, 0f);


        GameObject floatingText = Instantiate(floatingDamagePrefab, spawnPosition, Quaternion.identity);
        floatingText.transform.parent = transform; // since this script is attached to player, it should set the text's position to a child of player
        //damage *= -1;
        // floatingText.GetComponent<TextMesh>().text = damage.ToString();
        floatingText.GetComponent<TextMesh>().text = textToDisplay;
        floatingText.GetComponent<TextMesh>().color = color;

        floatingText.GetComponent<Animator>().SetTrigger("CriticalHit"); // Trigger the animation
        Destroy(floatingText, floatingText.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); // Destroy the bubble after animation
    }
}
