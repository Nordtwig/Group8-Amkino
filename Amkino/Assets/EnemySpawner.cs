using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public Transform[] spawnPoints;

    public GameObject EnemyPrefab;

	// Use this for initialization
	void Start () {

        int randomIndex = Random.Range(0, spawnPoints.Length);


        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (randomIndex == i)
            {
                Instantiate(EnemyPrefab, spawnPoints[i].position, Quaternion.identity);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
