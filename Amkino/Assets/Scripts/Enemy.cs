using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    Player player;

    public Bullet bulletPrefab;
    public GameObject bulletSpawn;
    public GameObject raycastOrigin;
    public Light muzzleFlash;
    public ParticleSystem waterSplashEffect;
    public AudioClip WaterSplash;
    public GameObject ejectionPort;
    public GameObject casingPrefab;

    public float rotateSpeed;
    public float detectionRange;
    public float fireRate;
    public float health = 3;

    private NavMeshAgent agent;
    private bool seenPlayer = true;
    private bool aimingAtPlayer = false;
    private float fireTimer;
    private bool recentlyHit = false;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();

        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        fireTimer += Time.deltaTime;
        if (!player.IsDead) {
            TrackPlayer();
        }

        if (fireTimer > (fireRate / 2)) {
            muzzleFlash.enabled = false;
        }

        if (fireTimer >= fireRate && aimingAtPlayer && !player.IsDead) {
            Fire();
            fireTimer = 0;
        }

        if (seenPlayer && !player.IsDead) {
            FollowPlayer();
        }
    }

    private void TrackPlayer() {
        RaycastHit hit;
        Ray ray = new Ray();
        ray.origin = raycastOrigin.transform.position;
        ray.direction = (player.raycastTarget.transform.position - raycastOrigin.transform.position).normalized;

        Physics.Raycast(ray.origin, ray.direction, out hit, detectionRange);
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (hit.collider != null) {
            if (hit.collider.tag == "Player") {
                seenPlayer = true;
                aimingAtPlayer = true;

            }
            else {
                aimingAtPlayer = false;
            }
        }
    }

    private void Fire() {
        Bullet bullet = Instantiate(bulletPrefab);
        GameObject casing = Instantiate(casingPrefab, ejectionPort.transform.position, Quaternion.identity);
        Physics.IgnoreCollision(casing.GetComponent<Collider>(), GetComponent<Collider>());
        casing.transform.position = ejectionPort.transform.position;
        casing.GetComponent<Rigidbody>().AddForce(transform.right * 1.25f, ForceMode.Impulse);
        bullet.ShotByPlayer = false;
        muzzleFlash.enabled = true;
        bullet.transform.position = bulletSpawn.transform.position;
        bullet.transform.forward = transform.forward;
        Destroy(casing, 5f);
    }

    private void FollowPlayer() {
        Vector3 targetPoint = player.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        agent.SetDestination(player.transform.position);
    }

    private void Die() {
        float randomForce = UnityEngine.Random.Range(-1, 1);
        muzzleFlash.enabled = false;
        GameController.OnEnemyDie();
        Rigidbody rb = GetComponent<Rigidbody>();
        agent.enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(new Vector3(transform.position.x + randomForce, transform.position.y, transform.position.z + randomForce), ForceMode.Impulse);
        this.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" && !recentlyHit)
        {
            recentlyHit = true;
            StartCoroutine("HitCooldown");
            health--;
            if (health <= 0) {
                Die();
                Destroy(gameObject, 2f);
            }
        }
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Water") {
            AudioSource.PlayClipAtPoint(WaterSplash, transform.position, 1f);
            waterSplashEffect = Instantiate(waterSplashEffect, transform.position, Quaternion.identity);
            waterSplashEffect.transform.localScale = new Vector3(2, 2, 2);
            Destroy(waterSplashEffect, 0.5f);
            Die();
        }
    }

    IEnumerator HitCooldown() {
        yield return new WaitForSeconds(0.1f);
        recentlyHit = false;
    }
}
