using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "DamageBuff")] 
public class DamageBuff : Drink_Effects
{
    public float damageBuff;
    

    public override void Effect(GameObject target)
    {
        Bottle _bottle = target.myBottle;
        _bottle.bottleDamage *= damageBuff;


    }

}

