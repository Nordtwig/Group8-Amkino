using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public Player player;
    public float rotateSpeed = 3;

    private NavMeshAgent agent;
    public bool seenPlayer = false;
    public bool aimingAtPlayer = false;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update () {
        Ray ray = new Ray(transform.position, player.transform.position);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if (Physics.Raycast(ray.origin, ray.direction, out hit)) {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Wall") {
                Debug.Log("Player out of sight");
                aimingAtPlayer = false;
            }
            else if (hit.collider.tag == "Player") {
                seenPlayer = true;
                aimingAtPlayer = true;
            }
        }
        if (seenPlayer) {
            FollowPlayer(); 
        }
    }

    private void FollowPlayer() {
        Vector3 targetPoint = player.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        agent.SetDestination(player.transform.position);
    }
}
