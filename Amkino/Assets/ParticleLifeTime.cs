using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLifeTime : MonoBehaviour {

    private float lifeTime = 0.32f;
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0) {
            Destroy(gameObject);
        }
	}
}
