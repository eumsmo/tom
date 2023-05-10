using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject bullet;

    // Cooldown de recarga dos tiros
    public int maxBullets = 0, bullets = 0;
    public float rechargeCooldown = .5f, rechargeCooldownTimer = 0f;

    // Cooldown de cada tiro individualmente
    public float shootCooldown = .1f, shootCooldownTimer = 0f;
    public bool canShoot = true;

    // Arma atirar automaticamente
    public bool onAutoShoot = false;

    public event Action OnShoot;
    public event Action<float> OnReload;
    
    public bool Atirar(GameObject alternativeBullet = null) {
        // Se espera ter balas e não possui nenhuma bala, não atira
        if ((maxBullets > 0 && bullets <= 0) || !canShoot) return false;

        GameObject currentBullet = alternativeBullet == null ? bullet : alternativeBullet;

        // Atira por todos as armas
        foreach(Transform child in transform) {
            if (child.gameObject.activeSelf) {
                Instantiate(currentBullet, child.position, child.rotation);
            }
        }

        // Controle de balas caso houver
        if (maxBullets > 0) {
            bullets--;
            if (bullets <= 0) {
                bullets = 0;

                // Se possuir cooldown de recarga
                if (rechargeCooldown > 0) {
                    rechargeCooldownTimer = rechargeCooldown;
                }
            }
        }

        // Se mantem o controle da velocidade de tiro
        if (shootCooldown > 0) {
            canShoot = false;
            shootCooldownTimer = shootCooldown;
        }

        OnShoot?.Invoke();

        return true;
    }

    public void FixedUpdate() {
        if (rechargeCooldown > 0 && rechargeCooldownTimer > 0) {
            rechargeCooldownTimer -= Time.fixedDeltaTime;
            
            if (rechargeCooldownTimer <= 0) {
                bullets = maxBullets;
                rechargeCooldownTimer = 0;
                OnReload?.Invoke(1f);
            } else {
                OnReload?.Invoke(1f - (rechargeCooldownTimer / rechargeCooldown));
            }
        }

        if (shootCooldown > 0 && shootCooldownTimer > 0) {
            shootCooldownTimer -= Time.fixedDeltaTime;
            if (shootCooldownTimer <= 0) {
                canShoot = true;
                shootCooldownTimer = 0;
            }
        }

        if (onAutoShoot && canShoot && rechargeCooldownTimer <= 0) {
            Atirar();
        }
    }
}
