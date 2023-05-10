using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiscaUmaVez : MonoBehaviour
{
    public GameObject pisca;
    public float intervaloPiscas = 0.25f;
    public int quantPiscas = 3;

    int piscasAtual = 0;
    float timerAtual = 0;

    void FixedUpdate() {
        if (piscasAtual > quantPiscas) return;

        timerAtual += Time.fixedDeltaTime;
        if ((piscasAtual+1) * intervaloPiscas <= timerAtual) {
            Debug.Log("Pisca");
            piscasAtual++;
            pisca.SetActive(!pisca.activeSelf);
        }
    }
}
