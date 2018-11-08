using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    Camera playerCamera;
    GameObject Hearts;

    public float moveSpeed = 3;
    public float rotateSpeed = 5;
    public float fireRate = 0.25f;

    public GameObject raycastTarget;
    public GameObject bulletSpawn;
    public Bullet bulletPrefab;

    public AudioClip WaterSplash;
    public AudioClip ManScream;

    public int playerHitCount;

    private Vector3 offset;
    private float fireTimer;

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        playerCamera = FindObjectOfType<Camera>();
        Hearts = GameObject.Find("Hearts");

        offset = playerCamera.transform.position - transform.position;
	}

    void Update() {
        fireTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && fireTimer >= fireRate) {
            Fire();
            fireTimer = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        Rotate();
        Move();

        // Camera follows player.
        playerCamera.transform.position = rb.transform.position + offset;
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
        bullet.ShotByPlayer = true;
        bullet.transform.position = bulletSpawn.transform.position;
        bullet.transform.forward = transform.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            playerHitCount++;
            AudioSource.PlayClipAtPoint(ManScream, transform.position, 1f);

            if (playerHitCount == 1)
            {
                
                GameObject Hearts = GameObject.Find("Hearts/Heart3");
                Hearts.GetComponent<Image>().enabled = false; // GetComponent image needs using UnityEngine.UI; 
            }

            if (playerHitCount == 2)
            {
                GameObject Hearts = GameObject.Find("Hearts/Heart2");
                Hearts.GetComponent<Image>().enabled = false;
            }

            if (playerHitCount == 3)
            {
                GameObject Hearts = GameObject.Find("Hearts/Heart1");
                Hearts.GetComponent<Image>().enabled = false;

                Destroy(gameObject);
            }

            
        }

        if (collision.gameObject.tag == "Water")
        {
            Destroy(gameObject);
            AudioSource.PlayClipAtPoint(WaterSplash, transform.position, 1f);
        }
            
    }
}
