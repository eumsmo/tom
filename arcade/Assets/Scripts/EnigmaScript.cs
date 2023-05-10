using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EnigmaScript : MonoBehaviour {
    public int posInOrder = 0;

    public static Dictionary<string, int[]> enigmaRelations = new Dictionary<string, int[]>
    {
        {"Enigma", new int[] {6,3,10,4}},
        {"VideoAreaCacto", new int[] {2,4,2,4}},
        {"Fase2", new int[] {2,2,2,2}},
        {"Backroom", new int[] {4,1,1,3}}
    };

    static int[] clicksOrder = new int[] {6,3,10,4};
    public static int[] currentOrder = new int[] {0,0,0,0};

    void Start() {
        Reset();
    }

    public void PanelClicked() {
        if (posInOrder == 4){
            Check();
            return;
        }

        currentOrder[posInOrder]++;
        Debug.Log(currentOrder);
    }

    bool CheckOrder(int[] order) {
        for (int i = 0; i < 4; i++) {
            if(EnigmaScript.currentOrder[i] != order[i]) return false;
        }

        return true;
    }

    void PrintOrder(int[] order) {
        Debug.LogFormat("Order: {0},{1},{2},{3}", order[0], order[1], order[2], order[3]);
    }

    public void Check() {
        string name = "";
        bool isComb = false;

        PrintOrder(currentOrder);

        foreach (var (sceneName, order) in enigmaRelations) {
            Debug.LogFormat("Testing with...");
            PrintOrder(order);
            if(CheckOrder(order)) {
                isComb = true;
                name = sceneName;
            }
        }

        if (isComb) {
            Recompensa(name);
        } else {
            Reset();
        }
    }

    public void Reset() {
        EnigmaScript.currentOrder[0] = 0;
        EnigmaScript.currentOrder[1] = 0;
        EnigmaScript.currentOrder[2] = 0;
        EnigmaScript.currentOrder[3] = 0;
        Debug.Log("reset");
    }

    public void Recompensa(string sceneName) { 
        Reset();
        SceneManager.LoadScene(sceneName);
    }
}
