using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCollider : MonoBehaviour
{

    [SerializeField] private CircleCollider2D punchcircle;
    public GameObject nearestEnemyObject;
    public Player nearestEnemy;
    public Player thisPlayer;


    


    private void OnTriggerEnter2D(Collider2D collider)
    {



        if (collider.tag == "Player1" || collider.tag == "Player2")
        {
            if (collider.GetType() == typeof(CircleCollider2D))
            {
                thisPlayer.nearestEnemyObject = collider.gameObject;
                Debug.Log("Enter Range");

            }
        }
    }


    void OnTriggerExit2D(UnityEngine.Collider2D collider)
    {
        if (collider.tag == "Player1" || collider.tag == "Player2")
        {
            if (collider.GetType() == typeof(CircleCollider2D))
            {
                thisPlayer.nearestEnemyObject = null;
                Debug.Log("Exit Range");

            }
        }
    }




}



