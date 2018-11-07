using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    public Camera playerCamera;
    public float moveSpeed = 3;
    public float rotateSpeed = 5;

    public GameObject Hearts;

    public AudioClip WaterSplash;
    public AudioClip ManScream;

    public int playerHitCount;

    private Vector3 offset;

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        offset = playerCamera.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        float hitDist = 0.0f;

        if (playerPlane.Raycast (ray, out hitDist)) {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        

        playerCamera.transform.position = rb.transform.position + offset;

        float x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        rb.velocity = new Vector3(x, rb.velocity.y, rb.velocity.z);

        float z = Input.GetAxisRaw("Vertical") * moveSpeed;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, z);
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
