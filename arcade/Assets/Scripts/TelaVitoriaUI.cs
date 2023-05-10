using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelaVitoriaUI : MonoBehaviour {
    public Text estatistica;
    void Start() {
        Estatisticas estatisticas = GameController.instance.estatisticas;
        string texto = "<b>Tempo de execução:</b> " + estatisticas.tempoDeJogo.ToString("F2").Replace(",", ".") + "s\n";
        texto += "<b>Balas atiradas:</b> " + estatisticas.balasAtiradas + " balas\n";
        texto += "<b>Robôs infectados:</b> " + estatisticas.robosInfectados + " robôs\n";
        texto += "<b>Energia perdida:</b> " + estatisticas.danoTomado.ToString("F2").Replace(",", ".");

        estatistica.text = texto;
    }
}
