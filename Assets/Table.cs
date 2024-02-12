using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSpawnPoint : MonoBehaviour
{
    // public Transform spawnPoint;
    private bool _bottleOnTable;

    public bool BottleOnTable
    {
        get { return _bottleOnTable;}
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to a bottle
        if (other.CompareTag("Bottle"))
        {
            _bottleOnTable = true;
            // Bottle is on the table, maybe want to adjust ranges later?
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to a bottle
        if (other.CompareTag("Bottle"))
        {
            _bottleOnTable = false;
            // Bottle has left the table
        }
    }

    public Vector3 GetSpawnPoint()
    {
        // Return the position where the bottle should spawn on this table
        return transform.position;
    }
}
