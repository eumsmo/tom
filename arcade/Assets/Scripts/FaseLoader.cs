using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaseLoader : MonoBehaviour {
    public bool isLoaded = false;

    void Start() {
        LoadScene();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void LoadScene() {
        if (!isLoaded) {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    public void UnloadScene() {
        if (!isLoaded) {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }
}
