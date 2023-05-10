using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    static public UIController instance;

    public Image barraProgressoHolder;
    public Image barraProgresso;
    
    public GameObject vidaControlado;
    public GameObject vidaPrefab;

    public GameObject bulletPrefab;
    public GameObject bulletHolder;

    public GameObject reloadHolder, reloadProgresso;

    public Player playerController = Player.instance;

    public bool mostrarVidaControlado = false;

    void Start() {
        instance = this;
    }

    public void AtualizarControlado(Player playerController) {
        vidaControlado.SetActive(playerController.estaControlando);
        if (playerController.estaControlando) {
            AliveSomehow vida = playerController.controlledRobot.GetComponent<AliveSomehow>();
            if (vida != null) {
                vidaControlado.SetActive(true);
                AtualizarVidaControlado(vida.life);
            } else {
                vidaControlado.SetActive(false);
            }

            Enemy roboInimigo = playerController.controlledRobot.GetComponent<Enemy>();
            if (roboInimigo != null) {
                bulletHolder.SetActive(true);
                reloadHolder.SetActive(true);
                AtualizarContagemBalas(roboInimigo.gunManager);
                AtualizaReloadOwner(roboInimigo.gunManager);
            } else {
                bulletHolder.SetActive(false);
                reloadHolder.SetActive(false);
            }
        } else {
            bulletHolder.SetActive(false);
            reloadHolder.SetActive(false);
        }
    }

    public void AtualizarVidaControlado(float life) {
        int intLife = (int)Mathf.Ceil(life);
        AtualizarVidaControlado(intLife);
    }

    public void AtualizarVidaControlado(int life) {
        if (vidaControlado.transform.childCount != life) {
            int diferenca = life - vidaControlado.transform.childCount;
            if (diferenca > 0) {
                for (int i = 0; i < diferenca; i++) {
                    GameObject vida = Instantiate(vidaPrefab, vidaControlado.transform);
                    vida.transform.SetParent(vidaControlado.transform);
                }
            } else {
                GameObject[] vidaArray = new GameObject[-diferenca];
                int i =0;
                foreach (Transform child in vidaControlado.transform) {
                    vidaArray[i] = child.gameObject;
                    i++;
                    if (i == -diferenca) break;
                }
                foreach (GameObject vida in vidaArray) {
                    Destroy(vida);
                }
            }
        }
    }

    public void AtualizarContagemBalas(GunManager gunManager) {
        int bullets = gunManager.bullets;
        if (bulletHolder.transform.childCount != bullets) {
            int diferenca = bullets - bulletHolder.transform.childCount;
            if (diferenca > 0) {
                for (int i = 0; i < diferenca; i++) {
                    GameObject bullet = Instantiate(bulletPrefab, bulletHolder.transform);
                    bullet.transform.SetParent(bulletHolder.transform);
                }
            } else {
                GameObject[] bulletsArray = new GameObject[-diferenca];
                int i =0;
                foreach (Transform child in bulletHolder.transform) {
                    bulletsArray[i] = child.gameObject;
                    i++;
                    if (i == -diferenca) break;
                }
                foreach (GameObject bullet in bulletsArray) {
                    Destroy(bullet);
                }
            }
        }
    }

    public void AtualizarEnergiaVirus(AliveSomehow virusLife) {
        float porcentagemEnergia = virusLife.life / virusLife.maxLife;
        barraProgresso.transform.localScale = new Vector3(porcentagemEnergia, 1, 1);
    }

    public void AtualizaReloadOwner(GunManager gunManager) {
        float bulletUIHeight = 15, spacingUI = 15;

        int mb = gunManager.maxBullets;
        float tamanho = (bulletUIHeight + spacingUI) * mb - spacingUI;

        reloadHolder.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tamanho);
        reloadProgresso.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tamanho);
        AtualizarReloadProgresso(0);
    }

    public void AtualizarReloadProgresso(float progresso) {
        reloadProgresso.transform.localScale = new Vector3(1, progresso, 1);
        if (progresso == 1) {
            reloadProgresso.transform.localScale = new Vector3(1, 0, 1);
            AtualizarContagemBalas(Player.instance.controlledRobot.GetComponent<Enemy>().gunManager);
        }
    }
}
