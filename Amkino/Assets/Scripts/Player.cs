using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class Player : MonoBehaviour {

    Camera playerCamera;
    Rigidbody rb;

    public float moveSpeed = 3;
    public float rotateSpeed = 5;
    public float fireRate = 0.25f;
    public float noHitDuration = 0.5f;

    public GameObject raycastTarget;
    public GameObject bulletSpawn;
    public Light muzzleFlash;
    public Bullet bulletPrefab;
    public GameObject ejectionPort;
    public GameObject casingPrefab;
    public ParticleSystem waterSplashEffect;

    public GameObject AmmoLeft;

    public AudioClip WaterSplash;
    public AudioClip ManScream;
    public AudioClip ReloadAudio;
    public AudioClip HeartPickupAudio;

    public int playerHitCount;

    private bool isDead = false;
    public bool IsDead { get { return isDead; } protected set { isDead = value; } }

    private bool recentlyHealed = false;
    private float recentlyHealedTimer;

    private float timeDuringReload;
    public float reloadTime;
    private bool reloading = false;
    private float ammoCount;
    private float ammoLeft;
    public float maxAmmo;

    private Vector3 offset;
    private float fireTimer;
    private bool recentlyHit = false;
    private float hitTimer;

    

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        playerCamera = FindObjectOfType<Camera>();

        muzzleFlash.enabled = false;

        ammoLeft = maxAmmo;

        offset = playerCamera.transform.position - transform.position;
	}

    void Update() {
        fireTimer += Time.deltaTime;
        

        AmmoLeft.GetComponent<Text>().text = ammoLeft.ToString();

        if (fireTimer > (fireRate / 2)) {
            muzzleFlash.enabled = false;
        }

        if (recentlyHealed) {
            recentlyHealedTimer += Time.deltaTime;
        }

        if (recentlyHit) {
            hitTimer += Time.deltaTime;
        }

        if (hitTimer >= noHitDuration) {
            recentlyHit = false;
            hitTimer = 0;
        }
        if (Input.GetMouseButton(0) && fireTimer >= fireRate && reloading == false ) {
            Fire();
            ammoCount++;
            ammoLeft--;
            fireTimer = 0;
        }

        if (recentlyHealedTimer > 2f)
        {
            recentlyHealed = false;
            recentlyHealedTimer = 0;
        }

        if (ammoCount == maxAmmo || Input.GetKeyDown("r") && ammoCount != 0)
        {
            
            reloading = true;
            AudioSource.PlayClipAtPoint(ReloadAudio, transform.position, 1f);
            
            ammoCount = 0;
        }
        
        if (reloading)
        {
            timeDuringReload += Time.deltaTime;
            
            if (timeDuringReload > reloadTime)
            {
                reloading = false;
                ammoLeft = maxAmmo;
                timeDuringReload = 0;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        Rotate();
        Move();

        // Camera follows player.
        if (!isDead) {
            playerCamera.transform.position = rb.transform.position + offset;
        }
    }

    private void Move() {
        float x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        rb.velocity = new Vector3(x, rb.velocity.y, rb.velocity.z);

        float z = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, z);
    }

    void Rotate() {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        float hitDist = 0.0f;

        if (playerPlane.Raycast(ray, out hitDist)) {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    void Fire() {
        Bullet bullet = Instantiate(bulletPrefab);
        GameObject casing = Instantiate(casingPrefab, ejectionPort.transform.position, Quaternion.identity);
        Physics.IgnoreCollision(casing.GetComponent<Collider>(), GetComponent<Collider>());
        casing.transform.position = ejectionPort.transform.position;
        casing.GetComponent<Rigidbody>().AddForce(transform.right * 1.25f, ForceMode.Impulse);
        bullet.ShotByPlayer = true;
        muzzleFlash.enabled = true;
        bullet.transform.position = bulletSpawn.transform.position;
        bullet.transform.forward = transform.forward;
        Destroy(casing, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" && !recentlyHit)
        {
            recentlyHit = true;
            playerHitCount++;
            AudioSource.PlayClipAtPoint(ManScream, transform.position, 1f);

            if (playerHitCount == 1)
            {
                
                GameObject Hearts = GameObject.Find("HUD/Heart3");
                Hearts.GetComponent<Image>().enabled = false; // GetComponent image needs using UnityEngine.UI; 
            }

            if (playerHitCount == 2)
            {
                GameObject Hearts = GameObject.Find("HUD/Heart2");
                Hearts.GetComponent<Image>().enabled = false;
            }

            if (playerHitCount == 3)
            {
                GameObject Hearts = GameObject.Find("HUD/Heart1");
                Hearts.GetComponent<Image>().enabled = false;

                Die();
            }
        }
    }

    void Die() {
        isDead = true;
        muzzleFlash.enabled = false;
        float randomForce = UnityEngine.Random.Range(UnityEngine.Random.Range(-1, -0.1f), UnityEngine.Random.Range(0.1f, 1));
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(new Vector3(randomForce, 0, randomForce), ForceMode.Impulse);
        this.enabled = false;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "HealthPickup")
        {
            if (playerHitCount != 0)
            {
                playerHitCount -= 1;
                recentlyHealed = true;
                AudioSource.PlayClipAtPoint(HeartPickupAudio, transform.position, 0.2f);
                Destroy(collision.gameObject);
            }

            if (playerHitCount == 0)
            {
                GameObject Hearts = GameObject.Find("HUD/Heart3");
                Hearts.GetComponent<Image>().enabled = true;
            }

            if (playerHitCount == 1)
            {
                GameObject Hearts = GameObject.Find("HUD/Heart2");
                Hearts.GetComponent<Image>().enabled = true;
            }
        }

        if (collision.gameObject.tag == "Water") {
            AudioSource.PlayClipAtPoint(WaterSplash, transform.position, 1f);
            waterSplashEffect = Instantiate(waterSplashEffect, transform.position, Quaternion.identity);
            Destroy(waterSplashEffect, 0.5f);
            Die();
        }
    }
}
