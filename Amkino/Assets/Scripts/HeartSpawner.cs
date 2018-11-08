using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSpawner : MonoBehaviour {

    public Transform[] spawnPoints;

    public GameObject HeartPrefab;

    private float TimeSinceSpawn;
    public float TimeBetweenSpawns;

    // Use this for initialization
    void Update()
    {

        TimeSinceSpawn += Time.deltaTime;
        if (TimeSinceSpawn >= TimeBetweenSpawns)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);


            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (randomIndex == i)
                {
                    Instantiate(HeartPrefab, spawnPoints[i].position, Quaternion.identity);
                }
            }

            TimeSinceSpawn = 0;
        }

    }
}
