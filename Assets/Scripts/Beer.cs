using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour
{
    [SerializeField] DamageBuff damageBuff;
    [SerializeField] SpeedBuff speedBuff;

    public virtual void Drink(GameObject playerObject)
    {
        damageBuff.Effect(playerObject);
        speedBuff.Effect(playerObject);
    }

}
