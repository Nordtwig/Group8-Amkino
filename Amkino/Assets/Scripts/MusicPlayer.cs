using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    public static MusicPlayer instanceRef;

	// Use this for initialization
	void Awake () {
        if (instanceRef == null) {
            instanceRef = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instanceRef != this) {
            Destroy(gameObject);
        }
	}
}
