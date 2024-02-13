using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSpawnPoint : MonoBehaviour
{
    private bool _bottleOnTable;
    public float positionThreshold = 1f;


    public bool BottleOnTable
    {
        get { return _bottleOnTable;}
    }

    void Update()
    {
        CheckBottleOnTable();
    }


    private void CheckBottleOnTable()
    {
        GameObject[] bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (GameObject bottle in bottles)
        {
            float distance = Vector3.Distance(bottle.transform.position, transform.position);

            if (distance < positionThreshold)
            {
                _bottleOnTable = true;
                return;
            }
            _bottleOnTable = false; // reset if no bottle is found on the table
        }
    }

    public Vector3 GetSpawnPoint()
    {
        return transform.position;
    }
}
