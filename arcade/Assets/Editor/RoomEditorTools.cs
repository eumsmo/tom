using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class RoomEditorTools {

    public static bool scriptWorking = false;

    public static string[] salasPrefix = {"Sala", "Fim", "F2_"}; 

    [InitializeOnLoadMethod]
    static void Initialize() {
        UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnEditorSceneManagerSceneOpened;
        UnityEditor.SceneManagement.EditorSceneManager.sceneSaving += OnEditorSceneManagerSceneSaving;
        UnityEditor.SceneManagement.EditorSceneManager.sceneSaved += OnEditorSceneManagerSceneSaved;
    }

    static GameObject GetRoomOnScene(UnityEngine.SceneManagement.Scene scene) {
        GameObject room = null;
        GameObject[] objectList = scene.GetRootGameObjects();

        foreach (GameObject gameobject in objectList) {
            if (gameobject.name == scene.name)
                room = gameobject;
        }

        return room;
    }

    static bool IsSceneARoomScene(UnityEngine.SceneManagement.Scene scene) {
        foreach (string prefix in salasPrefix) {
            if (scene.name.StartsWith(prefix))
                return true;
        }

        return false;
    }

    static void OnEditorSceneManagerSceneOpened(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode) {
        Debug.LogFormat("SceneOpened: {0}", scene.name);
        if (IsSceneARoomScene(scene)) OnRoomSceneOpened(scene);
    }

    static void OnRoomSceneOpened(UnityEngine.SceneManagement.Scene scene) {
        if (!scriptWorking) return;
        GameObject room = GetRoomOnScene(scene);
        if (!room) return;
        room.SetActive(true);
    }

    static void OnEditorSceneManagerSceneSaving(UnityEngine.SceneManagement.Scene scene, string path) {
        Debug.LogFormat("SceneSaving: {0}", scene.name);
        if (IsSceneARoomScene(scene)) OnRoomSceneSaving(scene);
    }

    static void OnRoomSceneSaving(UnityEngine.SceneManagement.Scene scene) {
        if (!scriptWorking) return;
        GameObject room = GetRoomOnScene(scene);
        if (!room) return;
        room.SetActive(false);
    }

    static void OnEditorSceneManagerSceneSaved(UnityEngine.SceneManagement.Scene scene) {
        Debug.LogFormat("SceneSaved: {0}", scene.name);
        if (IsSceneARoomScene(scene)) OnRoomSceneOpened(scene);
    }

}
