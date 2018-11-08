using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back : MonoBehaviour {

    public GameObject selection;
    public GameObject backButton;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) {
            SceneManager.LoadScene(0);
        }
    }

    private void MouseNavigation() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider != null) {
                GameObject target = hit.collider.gameObject;
                if (target.name == "Back") {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}
