using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData {
    public bool enemiesKilled;

    public RoomData(bool enemiesKilled) {
        this.enemiesKilled = enemiesKilled;
    }
}

public class RoomManager : MonoBehaviour {
    public static Dictionary<string, RoomData> roomsData = new Dictionary<string, RoomData>();
    public static void ResetRoomsData() {
        roomsData.Clear();
    }

    public bool isBossRoom = false;
    public GameObject door;
    public string[] adjacentScenes;
    GameObject floor;

    void Start() {
        floor = transform.Find("Mapa").Find("Floor").gameObject;
        Transform inimigosHolder = transform.Find("Inimigos");
        RoomData data;
        if (RoomManager.roomsData.ContainsKey(gameObject.name)) {
            data = RoomManager.roomsData[gameObject.name];
        } else {
            data = new RoomData(inimigosHolder.childCount == 0);
            roomsData.Add(gameObject.name, data);
        }

        if (data.enemiesKilled) {
            foreach(Transform child in inimigosHolder) {
                child.gameObject.GetComponent<Enemy>().SetOnDespawn(true);
                Destroy(child.gameObject);
            }
        }

        CheckRoomCondition();
    }

    public RoomData UpdateRoomData() {
        Transform inimigosHolder = transform.Find("Inimigos");
        RoomData data;
        if (RoomManager.roomsData.ContainsKey(gameObject.name)) {
            data = RoomManager.roomsData[gameObject.name];
            data.enemiesKilled = inimigosHolder.childCount == 0;
        } else {
            data = new RoomData(inimigosHolder.childCount == 0);
            roomsData.Add(gameObject.scene.name, data);
        }

        return data;
    }

    void OnDestroy() {
        UpdateRoomData();
    }

    public Vector3 ClampOnRoom(Vector3 pos, float padding = 0f) {
        Vector3 roomSize = floor.GetComponent<Renderer>().bounds.size;
        float xPos = floor.transform.position.x;
        float zPos = floor.transform.position.z;

        Vector3 newPos = pos;
        newPos.x = Mathf.Clamp(newPos.x, xPos - roomSize.x / 2 - padding, xPos + roomSize.x / 2 + padding);
        newPos.z = Mathf.Clamp(newPos.z, + zPos - roomSize.z / 2 - padding, zPos + roomSize.z / 2 + padding);
        return newPos;
    }

    public void CheckRoomCondition(bool beforeEnemyDestroyed = false) {
        GameObject inimigosHolder = transform.Find("Inimigos").gameObject;
        if (inimigosHolder.transform.childCount <= (beforeEnemyDestroyed ? 1 : 0) && door != null) {
            OpenDoor(door);
            RoomManager.roomsData[gameObject.name].enemiesKilled = true;
        }
    }

    public void EnemyDestroyed() {
        // Método chamado um pouco antes de um inimigo ser destruído.
        CheckRoomCondition(true);
    }

    public void OpenDoor(GameObject door) {
        door.GetComponent<Door>().Open();
    }

    public void UnloadAdjacentsBut(string roomToKeep) {
        foreach (string scene in adjacentScenes) {
            if (scene != roomToKeep) {
                GameController.instance.UnloadScene(scene);
            }
        }
    }

    public void LoadAdjacentsBut(string roomToNotLoad) {
        foreach (string scene in adjacentScenes) {
            if (scene != roomToNotLoad) {
                GameController.instance.LoadScene(scene);
            }
        }
    }
}
