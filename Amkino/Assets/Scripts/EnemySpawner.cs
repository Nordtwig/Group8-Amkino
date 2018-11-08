using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public Transform[] spawnPoints;

    public GameObject EnemyPrefab;

    private float TimeSinceSpawn;
    public float TimeBetweenSpawns;

	// Use this for initialization
	void Update () {

        TimeSinceSpawn += Time.deltaTime;
        if (TimeSinceSpawn >= TimeBetweenSpawns)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);


            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (randomIndex == i)
                {
                    Instantiate(EnemyPrefab, spawnPoints[i].position, Quaternion.identity);
                }
            }

            TimeSinceSpawn = 0;
        }
       
	}
	
	
}
