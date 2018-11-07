using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public Player player;
    public float rotateSpeed = 3;

    private NavMeshAgent agent;
    public bool seenPlayer = false;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update () {
        Ray ray = new Ray();
        RaycastHit hit;

        ray.origin = transform.position;
        Debug.DrawRay(ray.origin, player.transform.position);
        if (Physics.Raycast(ray.origin, player.transform.position, out hit)) {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Wall") {
                Debug.Log("Player out of sight");
            }
            else if (hit.collider.tag == "Player") {
                seenPlayer = true;
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
