using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public float speed = 2;
    public float desiredAngle;
    bool definedAngle = false;
    public bool isThatDoor = false;

    public static bool thatDoorOpen = false;

    GameObject dobradica;
    void Start() {
        dobradica = GameObject.Find("Dobradica");
        
        // Métodos de mudar angulo podem ser definidos antes do Start ser chamado
        if (!definedAngle) desiredAngle = dobradica.transform.localEulerAngles.y;
    }

    public void Open() {
        desiredAngle = 0;
        definedAngle = true;
    }

    public void Close() {
        desiredAngle = 90;
        definedAngle = true;
    }
    
    void FixedUpdate() {
        dobradica.transform.localEulerAngles = new Vector3(0, Mathf.LerpAngle(dobradica.transform.localEulerAngles.y, desiredAngle, Time.fixedDeltaTime * speed), 0);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.O)) {
            Open();
        }
    }
}
