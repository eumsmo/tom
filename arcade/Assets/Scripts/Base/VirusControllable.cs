using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusControllable : MonoBehaviour {
    static public List<VirusControllable> instances = new List<VirusControllable>();
    static public int numInfectados = 0;

    public GameObject identificadorSelecao;

    public bool sendoControlado = false;
    public bool recebeInput = true;
    public bool permiteRotacao = true;
    public bool habilitado = true;

    bool jaFoiInfectadoAlgumaVez = false;

    protected Enemy enemy;

    public static void Reset() {
        instances = new List<VirusControllable>();
        numInfectados = 0;
    }

    public virtual void Start() {
        VirusControllable.instances.Add(this);
        identificadorSelecao = Instantiate(identificadorSelecao, transform.position, transform.rotation);
        identificadorSelecao.transform.parent = transform;
        enemy = GetComponent<Enemy>();
    }

    void OnDestroy() {
        VirusControllable.instances.Remove(this);
    }

    public void UpdateStyle(bool emRange, bool maisProximo) {
        identificadorSelecao.SetActive(emRange);
        identificadorSelecao.GetComponent<Renderer>().material = maisProximo ? GameController.instance.selecionadoMaterial : GameController.instance.naoSelecionadoMaterial;
    }

    public virtual void HandleSetControlado(bool controlado) {
        if (controlado && !jaFoiInfectadoAlgumaVez) {
            jaFoiInfectadoAlgumaVez = true;
            VirusControllable.numInfectados++;
        }
        if (enemy) enemy.HandleSetControlado(controlado);
        sendoControlado = controlado;
    }

    public virtual void HandlePlayerAcao() { }
}
