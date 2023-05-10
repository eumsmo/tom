using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLever : Lever {
    public GameObject door;

    public override void HandleEstado(bool apertado) {
        Door doorScript = door.GetComponent<Door>();
        if (apertado) doorScript.Open();
        else doorScript.Close();
    }

}
