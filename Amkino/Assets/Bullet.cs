using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float fireRate = 1.0f;
    private float lastShot = 0.0f;

    [Header("Tuning")]
    public float speed = 5;
	
	void Start () {
		
	}
	
	
	void Update () {

        if (Input.GetMouseButton(0) && Time.time > fireRate + lastShot)
        {
            Fire();
            lastShot = Time.time;
        }
    }

    void Fire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * speed;

        // Destroy the bullet after 2 seconds
        
        Destroy(bullet, 2.0f);
        
    }

    
}
