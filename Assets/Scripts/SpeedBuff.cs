using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SpeedBuff")]
public class SpeedBuff : Drink_Effects
{
    
    public float speedBuff;


    public override void Effect(GameObject target)
    {
        Bottle _bottle = target.GetComponent<Bottle>();
        _bottle.bottleDamage *= speedBuff;


    }

    
}
