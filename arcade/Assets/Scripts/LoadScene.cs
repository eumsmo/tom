using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string cena;
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Bullet") {
            Destroy(other.gameObject);
            return;
        }
        
        TrocaCena();
    }

    public void TrocaCena() {
        SceneManager.LoadScene(cena);
    }
}
