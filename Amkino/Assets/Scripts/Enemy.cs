using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public Player player;
    public float rotateSpeed = 3;
    public float detectionRange = 6;

    private NavMeshAgent agent;
    public bool seenPlayer = false;
    public bool aimingAtPlayer = false;
    public GameObject raycastOrigin;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update () {
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
