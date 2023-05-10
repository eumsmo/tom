using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {
    public GameObject room;

    public void OnTriggerExit(Collider other) {
        GameObject oldRoom = GameController.instance.currentRoom;

        if(other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player") {
            return;
        }
        
        // Descarrega cenas adjacentes ao quarto anterior e carrega cenas adjacentes ao quarto atual
        if (oldRoom != room) {
            oldRoom.GetComponent<RoomManager>().UnloadAdjacentsBut(room.name);
            room.GetComponent<RoomManager>().LoadAdjacentsBut(oldRoom.name);
            oldRoom.SetActive(false);
        }

        // Define quarto atual e ativa o mapa e inimigos
        GameController.instance.currentRoom = room;
        room.SetActive(true);
    }
}
