using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    Player player;

    public Transform[] enemySpawnPoints;
    public Transform[] itemSpawnPoints;

    public GameObject EnemyPrefab;
    public GameObject ItemPrefab;
    public Image CrosshairPrefab; // Crosshair-objektet är inte en sprite, den tar bara sin source från en.
    public Canvas GameOverScreen;
    public Canvas HealthHUD;
    public AudioClip oceanWaves;

    private string TotalScore;

    private float enemyTimeSinceSpawn;
    private float itemTimeSinceSpawn;
    private float killScore;

    public float enemySpawnRate;
    public float itemSpawnRate;

    public delegate void OnEnemyDieHandler();
    public static OnEnemyDieHandler OnEnemyDie;

    void Start() {
        AudioSource.PlayClipAtPoint(oceanWaves, transform.position, 0.5f);
        player = FindObjectOfType<Player>();
        GameOverScreen.enabled = false;
        Cursor.visible = false;
        OnEnemyDie += AddScore;
    }

    void AddScore() {
        killScore += 75;
    }

    void Update() {
        enemyTimeSinceSpawn += Time.deltaTime;
        itemTimeSinceSpawn += Time.deltaTime;

        if (!player.IsDead) {
            EnemySpawner();
            ItemSpawner();
        }
        else {
            StartCoroutine("StartGameOver");
        }

        CrosshairPrefab.transform.position = Input.mousePosition;

        GameScore();
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

    IEnumerator StartGameOver() {
        yield return new WaitForSeconds(1);
        Text GameOverScore = GameObject.Find("GameOver/GameOverScore").GetComponent<Text>();
        GameOverScore.text = "Your Score: " + TotalScore;
        
        
        GameOverScreen.enabled = true;
        HealthHUD.enabled = false;

    }

    void GameScore() { 

        if (!player.IsDead)
        {
            GameObject GameScore = GameObject.Find("HUD/GameScore");
            GameScore.GetComponent<Text>().text = Mathf.Round(Time.timeSinceLevelLoad + killScore).ToString();
            TotalScore = GameScore.GetComponent<Text>().text;
        }
    }
}
