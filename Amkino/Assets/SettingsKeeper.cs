using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsKeeper : MonoBehaviour {

    public static SettingsKeeper instanceRef;


    bool fullscreen;
    bool music;

    void Awake() {
        if (instanceRef == null) {
            instanceRef = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instanceRef != this) {
            Destroy(gameObject);
        }

        fullscreen = BoolConverter(PlayerPrefs.GetInt("fullscreen"));
        music = BoolConverter(PlayerPrefs.GetInt("music"));

        SceneManager.sceneLoaded += OnLoadCallback;

        OnLoadCallback(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    bool BoolConverter(int value) {
        if (value == 0) {
            return false;
        }
        else {
            return true;
        }
    }

    void OnLoadCallback(Scene scene, LoadSceneMode load) {
        Screen.fullScreen = fullscreen;
        GameObject MusicPlayer = GameObject.Find("MusicPlayer");
        MusicPlayer.GetComponent<AudioSource>().mute = music;
    }
}
