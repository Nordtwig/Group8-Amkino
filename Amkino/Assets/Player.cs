using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Camera playerCamera;
    public float moveSpeed;
    public float rotateSpeed;

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
}
