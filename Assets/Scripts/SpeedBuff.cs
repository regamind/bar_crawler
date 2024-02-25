using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpeedBuff")]
public class SpeedBuff : Drink_Effects
{
    
    public float speedBuff;


    public override void Effect(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        player.movementSpeedHorizontal *= speedBuff;
        player.movementSpeedVertical *= speedBuff;


    }

    
}
