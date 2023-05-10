using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour {

    // Update is called once per frame
    void Update() {
        if (Input.anyKey) {
            SceneManager.LoadScene("Fase1");
        }
    }
}
