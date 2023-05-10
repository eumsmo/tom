using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<AliveSomehow>().SofrerDano();
        } else if (collision.gameObject.tag == "Bullet" || collision.gameObject.name == "Precisao") {
            return;
        }
        
        Destroy(gameObject);
    }
}
