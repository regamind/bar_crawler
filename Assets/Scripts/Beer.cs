using UnityEngine;

public class Beer : MonoBehaviour
{
    [SerializeField] DamageBuff damageBuff;
    [SerializeField] SpeedBuff speedBuff;
    public float proof = 15f;

    public virtual void Drink(GameObject playerObject)
    {
        Player myPlayer = playerObject.GetComponent<Player>();
        myPlayer.drinkProof = proof;
        damageBuff.Effect(playerObject);
        speedBuff.Effect(playerObject);
    }
}
