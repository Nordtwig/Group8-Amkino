using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public AudioClip BulletSound;

    public float speed = 8;
    public float lifeDuration = 2;

    private float lifeTimer;

    private bool shotByPlayer;
    public bool ShotByPlayer { get { return shotByPlayer; } set { shotByPlayer = value; } }

    // Use this for initialization
    void Start () {
        lifeTimer = lifeDuration;
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
   
        AudioSource.PlayClipAtPoint(BulletSound, transform.position, 1f);
        Destroy(gameObject);
        
    }
}
