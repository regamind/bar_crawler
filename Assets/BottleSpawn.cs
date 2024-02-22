using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleSpawn : MonoBehaviour
{
    public float spawnInterval = 6f; // adjust as needed
    public GameObject bottlePrefab;

    private List<TableSpawnPoint> tables = new List<TableSpawnPoint>();

    private void Start()
    {
        // find all tables in the scene
        TableSpawnPoint[] tableSpawnPoints = FindObjectsOfType<TableSpawnPoint>();
        tables.AddRange(tableSpawnPoints);


        // start the spawning routine
        StartCoroutine(SpawnBottlesRoutine());
    }



    IEnumerator SpawnBottlesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            // loop through each table and check if it has a bottle

            // randomizes list and chooses random amount to spawn to
            Debug.Log($"Table spawn point number: {tables.Count}");
            ShuffleTables();
            int numberOfTablesToSpawnOn = Random.Range(1, tables.Count + 1);
            Debug.Log($"Table random spawn point number: {numberOfTablesToSpawnOn}");
            // loop through a subset of the shuffled list
            int numberSpawned = 0;
            for (int i = 0; i < numberOfTablesToSpawnOn; i++)
            {
                TableSpawnPoint table = tables[i];
                if (!table.BottleOnTable)
                {
                    numberSpawned += 1;
                    Instantiate(bottlePrefab, table.GetSpawnPoint(), Quaternion.identity);
                }
            }
            Debug.Log($"Number spawned: {numberSpawned}");
         }
     }

    // credit to fisher yates algorithm for shuffling: https://stackoverflow.com/questions/273313/randomize-a-listt
    private void ShuffleTables()
    {
        int n = tables.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            TableSpawnPoint temp = tables[k];
            tables[k] = tables[n];
            tables[n] = temp;
        }
    }


}




