using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveSomehow : MonoBehaviour {
    public float maxLife;
    public float life = 3f;

    public bool hasCooldown = false;
    public float tempoCooldown = 3f;
    float cooldown = 0f;
    public bool invulnerable = false;

    public float danoTomado = 0f;

    public event Action<float> OnLifeChange;

    void Update() {
        if (invulnerable && hasCooldown) {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0) {
                StopInvulnerable();
            }
        }
    }

    public void StopInvulnerable() {
        invulnerable = false;
        cooldown = 0f;
    }

    public virtual void SofrerDano(float dano = 1f, bool invulnerableElegible = true) {
        if (invulnerable)
            return;

        danoTomado += dano;
        life -= dano;
        OnLifeChange?.Invoke(life);
        if (life <= 0) {
            Destroy(gameObject);
        } else if (hasCooldown && invulnerableElegible) {
            SetInvulnerable(tempoCooldown);
        }
    }

    public void SetInvulnerable(float tempo) {
        if (invulnerable && cooldown > tempo)
            return;
            
        invulnerable = true;
        cooldown = tempo;
    }

    public virtual void Heal(float quantidade) {
        life += quantidade;
        if (maxLife > 0 && life > maxLife) {
            life = maxLife;
        }
        OnLifeChange?.Invoke(life);
    }
}
