using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Virus : AliveSomehow {

    public override void SofrerDano(float dano = 1f, bool invulnerableElegible = true) {
        base.SofrerDano(dano, invulnerableElegible);
        UIController.instance.AtualizarEnergiaVirus(this);
        if (life <= 0) {
            Destroy(gameObject);
            GameController.instance.LoseGame();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            SofrerDano();
        }
    }
}
