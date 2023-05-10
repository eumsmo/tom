using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickChangeToScene : MonoBehaviour
{
    public string nextSceneName;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Creditos");
        } else if (Input.anyKey) {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
