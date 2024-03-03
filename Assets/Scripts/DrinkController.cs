using System.Collections;
using UnityEngine;

public class DrinkController : MonoBehaviour
{
    public void ApplySpeedEffect(SpeedBuff speedBuff, GameObject target)
    {
        StartCoroutine(speedBuff.EffectCoroutine(target));
    }
}
