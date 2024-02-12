using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BottleSpawn : MonoBehaviour
{
    public float spawnInterval = 10f; // Adjust as needed
    public GameObject bottlePrefab;

    private List<TableSpawnPoint> tables = new List<TableSpawnPoint>();

    private void Start()
    {
        // Find all tables in the scene
        TableSpawnPoint[] tableSpawnPoints = FindObjectsOfType<TableSpawnPoint>();
        tables.AddRange(tableSpawnPoints);

        // Start the spawning routine
        StartCoroutine(SpawnBottlesRoutine());
    }

    IEnumerator SpawnBottlesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Loop through each table and check if it has a bottle
            foreach (TableSpawnPoint table in tables)
            {
                if (!table.BottleOnTable)
                {
                    // Spawn a bottle at the table's spawn point
                    Instantiate(bottlePrefab, table.GetSpawnPoint(), Quaternion.identity);
                }
            }
        }
    }
}

