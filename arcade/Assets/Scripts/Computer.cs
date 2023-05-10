using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : VirusControllable {
    public Vector3 scale = Vector3.zero;
    public GameObject tela;

    public override void Start() {
        base.Start();
        identificadorSelecao.transform.localScale = scale;
    }

    public override void HandleSetControlado(bool controlado) {
        base.HandleSetControlado(controlado);
        if (controlado) {
            tela.SetActive(true);
            GameController.instance.Invoke("WinGame", 1f);
        }
    }

}
