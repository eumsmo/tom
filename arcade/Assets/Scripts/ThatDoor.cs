using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThatDoor : Lever {
    public override void HandleEstado(bool apertado) {
        Door.thatDoorOpen = apertado;
    }
}
