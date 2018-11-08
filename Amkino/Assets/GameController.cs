using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public Transform[] enemySpawnPoints;
    public Transform[] itemSpawnPoints;

    public GameObject EnemyPrefab;
    public GameObject ItemPrefab;

    private float enemyTimeSinceSpawn;
    private float itemTimeSinceSpawn;

    public float enemySpawnRate;
    public float itemSpawnRate;
	
	// Update is called once per frame
	void Update () {
        enemyTimeSinceSpawn += Time.deltaTime;
        itemTimeSinceSpawn += Time.deltaTime;

        EnemySpawner();
        ItemSpawner();
	}

    private void EnemySpawner() {
        if (enemyTimeSinceSpawn >= enemySpawnRate) {
            int randomIndex = UnityEngine.Random.Range(0, enemySpawnPoints.Length);


            for (int i = 0; i < enemySpawnPoints.Length; i++) {
                if (randomIndex == i) {
                    Instantiate(EnemyPrefab, enemySpawnPoints[i].position, Quaternion.identity);
                }
            }

            enemyTimeSinceSpawn = 0;
        }
    }

    private void ItemSpawner() {
        if (itemTimeSinceSpawn >= itemSpawnRate) {
            int randomIndex = UnityEngine.Random.Range(0, itemSpawnPoints.Length);


            for (int i = 0; i < itemSpawnPoints.Length; i++) {
                if (randomIndex == i) {
                    Instantiate(ItemPrefab, itemSpawnPoints[i].position, Quaternion.identity);
                }
            }

            itemTimeSinceSpawn = 0;
        }
    }
}
