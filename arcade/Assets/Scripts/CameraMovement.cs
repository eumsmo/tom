using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public float speed;

    public Vector3 roomSize, posRelative = Vector3.zero;
    public float xCameraOffset, zCameraOffset;
    public float clampVal, xDifClamp = 0, zDifClamp = 0;

    public float offsetPercent = 0.2f;
    public float offsetMovSpeed = 10f;
    
    void Update() {
        GameObject objectToFollow = Player.instance.currentControlled;
        Vector3 personagemPos = objectToFollow.transform.position;
        GameObject currentRoom = GameController.instance.currentRoom;

        /*
        // Calcula a posição do mouse no jogo
        float cameraYRelative = Camera.main.transform.position.y - personagemPos.y;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraYRelative));
        mousePos.y = objectToFollow.transform.position.y;

        // Calcula o ponto na porcentagem offsetPercent do segmento entre o personagem e o mouse
        Vector3 foco = personagemPos + offsetPercent * (mousePos - personagemPos);
        foco.y = transform.position.y;


        transform.position = foco;
        // transform.position = Vector3.Lerp(transform.position, foco, Time.deltaTime * offsetMovSpeed);
        */



        Vector3 foco = personagemPos;
        GameObject floor = currentRoom.transform.Find("Mapa").Find("Floor").gameObject;
        roomSize = floor.GetComponent<Renderer>().bounds.size;

        float xPos = floor.transform.position.x;
        float zPos = floor.transform.position.z;

        float xOffset = xCameraOffset - clampVal;
        float zOffset = zCameraOffset - clampVal;

        float x = Mathf.Clamp(foco.x, xPos - roomSize.x / 2 + xOffset, xPos + roomSize.x / 2 - xOffset + xDifClamp) + posRelative.x;
        float z = Mathf.Clamp(foco.z, + zPos - roomSize.z / 2 + zOffset, zPos + roomSize.z / 2 - zOffset + zDifClamp) + posRelative.z;

        foco = new Vector3(x, transform.position.y, z);
        transform.position = Vector3.Lerp(transform.position, foco, speed * Time.deltaTime);

        /*
        if (currentRoom != null) {
            Vector3 target = new Vector3(currentRoom.transform.position.x - 2.5f, transform.position.y, currentRoom.transform.position.z + 0.25f);
            transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        }
        */
    }
}
