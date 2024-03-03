using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpeedBuff")]
public class SpeedBuff : Drink_Effects
{

    public float speedBuff;


    public override void Effect(GameObject target)
    {
        DrinkController drinkController = FindObjectOfType<DrinkController>();
        if (drinkController != null)
        {
            drinkController.ApplySpeedEffect(this, target);
        }


    }

    public IEnumerator EffectCoroutine(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        player.movementSpeedHorizontal *= speedBuff;
        player.movementSpeedVertical *= speedBuff;
        if (speedBuff > 1f)
        {
            // speed-up effect has trail after it to indicate they're faster
            player.EnableTrail();
        }
        yield return new WaitForSeconds(4);
        player.movementSpeedHorizontal = player.movementSpeedHorizontal / speedBuff;
        player.movementSpeedVertical = player.movementSpeedVertical / speedBuff;
        player.DisableTrail();
    }

}