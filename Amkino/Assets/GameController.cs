using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    Player player;

    public Transform[] enemySpawnPoints;
    public Transform[] itemSpawnPoints;

    public GameObject EnemyPrefab;
    public GameObject ItemPrefab;

    private float enemyTimeSinceSpawn;
    private float itemTimeSinceSpawn;

    public float enemySpawnRate;
    public float itemSpawnRate;

    void Start() {
        player = FindObjectOfType<Player>();    
    }

    // Update is called once per frame
    void Update () {
        enemyTimeSinceSpawn += Time.deltaTime;
        itemTimeSinceSpawn += Time.deltaTime;

        EnemySpawner();
        ItemSpawner();

        GoMainMenu();
        RestartGame();
	}

    void RestartGame() {
        if (Input.GetKeyUp(KeyCode.Space) && player.IsDead) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            SceneManager.LoadScene("Map");
        }
    }

    void GoMainMenu() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            SceneManager.LoadScene("Menu");
        }
    }

    void EnemySpawner() {
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

    void ItemSpawner() {
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
