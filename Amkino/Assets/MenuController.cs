using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject selection;

    int currentSelect;
    int newSelect;

    [SerializeField]
    GameObject[] buttons;

	// Use this for initialization
	void Start () {
        currentSelect = 0;
        MoveSelection();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp("s")) {
            newSelect = currentSelect + 1;
            if (newSelect > (buttons.Length - 1)) {
                newSelect = 0;
            }
            currentSelect = newSelect;
            MoveSelection();
        }
        else if (Input.GetKeyUp("w")) {
            newSelect = currentSelect - 1;
            if (newSelect < 0) {
                newSelect = 4;
            }
            currentSelect = newSelect;
            MoveSelection();
        }
	}

    public void MoveSelection() {
        Vector3 newPos = new Vector3(selection.transform.position.x, selection.transform.position.y, buttons[currentSelect].transform.position.z);
        selection.transform.position = newPos;
    }
}
