using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour {

    public GameObject screenCheckMark;
    public GameObject musicCheckMark;
    public GameObject selection;
    GameObject MusicPlayer;

    int currentSelect;
    int newSelect;


    [SerializeField]
    GameObject[] buttons;

    // Use this for initialization
    void Start() {
        MusicPlayer = GameObject.Find("MusicPlayer");
        Screen.fullScreen = BoolConverter(PlayerPrefs.GetInt("fullscreen"));
        MusicPlayer.GetComponent<AudioSource>().mute = BoolConverter(PlayerPrefs.GetInt("music"));
        screenCheckMark.SetActive(Screen.fullScreen);
        musicCheckMark.SetActive(MusicPlayer.GetComponent<AudioSource>().mute);
        currentSelect = 0;
        MoveSelection();
    }

    // Update is called once per frame
    void Update() {
        KeyNavigation();
        MouseNavigation();

        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) {
            switch (currentSelect) {
                case 0:
                    screenCheckMark.SetActive(!screenCheckMark.activeInHierarchy);
                    Screen.fullScreen = !Screen.fullScreen;
                    break;
                case 1:
                    musicCheckMark.SetActive(!musicCheckMark.activeInHierarchy);
                    MusicPlayer.GetComponent<AudioSource>().mute = !MusicPlayer.GetComponent<AudioSource>().mute;
                    break;
                case 2:
                    PlayerPrefs.SetInt("fullscreen", BoolConverter(screenCheckMark.activeInHierarchy));
                    PlayerPrefs.SetInt("music", BoolConverter(musicCheckMark.activeInHierarchy));
                    SceneManager.LoadScene(0);
                    break;
                default:
                    break;
            }
        }
    }

    private void MouseNavigation() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider != null) {
                GameObject target = hit.collider.gameObject;
                switch (target.name) {
                    case "Fullscreen":
                        currentSelect = 0;
                        MoveSelection();
                        break;
                    case "Music":
                        currentSelect = 1;
                        MoveSelection();
                        break;
                    case "Back":
                        currentSelect = 2;
                        MoveSelection();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void KeyNavigation() {
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
                newSelect = 2;
            }
            currentSelect = newSelect;
            MoveSelection();
        }
    }

    private void MoveSelection() {
        Vector3 newPos = new Vector3(buttons[currentSelect].transform.position.x, selection.transform.position.y, buttons[currentSelect].transform.position.z);
        selection.transform.position = newPos;
    }

    private bool BoolConverter(int value) {
        if (value == 0) {
            return false;
        }
        else {
            return true;
        }
    }

    private int BoolConverter(bool value) {
        if (value == false) {
            return 0;
        }
        else {
            return 1;
        }
    }
}
