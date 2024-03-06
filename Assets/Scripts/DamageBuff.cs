using UnityEngine;

[CreateAssetMenu(menuName = "DamageBuff")] 
public class DamageBuff : Drink_Effects
{
    public float damageBuff;
    
    public override void Effect(GameObject target)
    {
        Player player = target.GetComponent<Player>();
        Bottle _bottle = player.myBottle;
        _bottle.bottleDamage *= damageBuff;
    }
}
