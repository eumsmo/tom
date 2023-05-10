using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : VirusControllable {
    public bool apertado = false;

    public override void Start() {
        base.Start();
        permiteRotacao = false;
    }

    public override void HandlePlayerAcao() {
        apertado = !apertado;

        if (apertado) transform.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        else transform.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");

        HandleEstado(apertado);
    }

    public virtual void HandleEstado(bool apertado) {}

}
