﻿using System;
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

    public float rotateSpeed;
    public float detectionRange;
    public float fireRate;
    public float health = 3;

    private NavMeshAgent agent;
    private bool seenPlayer = false;
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
        bullet.ShotByPlayer = false;
        muzzleFlash.enabled = true;
        bullet.transform.position = bulletSpawn.transform.position;
        bullet.transform.forward = transform.forward;
    }

    private void FollowPlayer() {
        Vector3 targetPoint = player.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        agent.SetDestination(player.transform.position);
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" && !recentlyHit)
        {
            recentlyHit = true;
            StartCoroutine("HitCooldown");
            health--;
            if (health <= 0) {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator HitCooldown() {
        yield return new WaitForSeconds(0.1f);
        recentlyHit = false;
    }
}
