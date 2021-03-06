﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public AudioClip BulletSound;
    public AudioClip RicochetSound;

    [SerializeField]
    ParticleSystem[] onHitEffects;

    public float speed = 8;
    public float lifeDuration = 2;

    private float lifeTimer;

    private bool shotByPlayer;
    public bool ShotByPlayer { get { return shotByPlayer; } set { shotByPlayer = value; } }

    // Use this for initialization
    void Start () {
        lifeTimer = lifeDuration;
        AudioSource.PlayClipAtPoint(BulletSound, transform.position, 0.5f);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * speed * Time.deltaTime;
        lifeTimer -= Time.deltaTime;

        if (lifeTimer < 0) {
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Wall") {
            AudioSource.PlayClipAtPoint(RicochetSound, transform.position, 1f);
            Instantiate(onHitEffects[0], transform.position, Quaternion.Euler(-transform.rotation.eulerAngles));
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player") {
            Instantiate(onHitEffects[1], transform.position, Quaternion.Euler(-transform.rotation.eulerAngles));
            Destroy(gameObject);
        }
    }
}
