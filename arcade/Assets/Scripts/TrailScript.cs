using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour {
    public float speed = 10;
    public GameObject toFollow;
    
    GameObject trail;
    void Start() {
        trail = transform.Find("Trail").gameObject;
    }

    public void SetFollow(GameObject toFollow) {
        this.toFollow = toFollow;
        gameObject.SetActive(true);
    }


    void FixedUpdate() {
        GameObject toFollow = Player.instance.currentControlled;
        if (toFollow) {
            transform.position =  Vector3.Lerp(transform.position, toFollow.transform.position, speed * Time.fixedDeltaTime);
        }

        float distance = Vector3.Distance(transform.position, toFollow.transform.position);
        if (distance < 0.1f) gameObject.SetActive(false);
    }
}
