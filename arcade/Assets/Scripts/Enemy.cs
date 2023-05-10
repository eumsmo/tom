using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public VirusControllable controllable;
    public GunManager gunManager;

    public bool olhaPlayer = true;
    public bool seguePlayer = false;
    public float speed = 1f;
    public float desativadoTempo = 0.5f, desativadoTimer = 0f;

    public GameObject dropItem;
    public float percentageDrop = 0.2f;

    bool onDespawn = false;

    void Start() {
        gunManager = transform.Find("ArmaHolder").gameObject.GetComponent<GunManager>();
        gunManager.onAutoShoot = true;

        controllable = GetComponent<VirusControllable>();
        GetComponent<AliveSomehow>().OnLifeChange += HandleLifeChange;
    }

    public void HandleLifeChange(float life) {
        if (controllable.sendoControlado) {
            UIController.instance.AtualizarVidaControlado(life);
        }
    }

    public void HandleSetControlado(bool controlado) {
        Player playerController = Player.instance;

        if (controllable.sendoControlado != controlado) {
            if (controlado) {
                gameObject.GetComponent<AliveSomehow>().hasCooldown = true;
                transform.parent = playerController.transform;
                gunManager.onAutoShoot = false;
            } else{
                gameObject.GetComponent<AliveSomehow>().hasCooldown = false;
                gameObject.GetComponent<AliveSomehow>().StopInvulnerable();
                transform.parent = GameController.instance.currentRoom.transform.Find("Inimigos");
                desativadoTimer = desativadoTempo;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (controllable.sendoControlado)
            return;

        if (seguePlayer) {
            olhaPlayer = true;
            transform.Translate(0, 0, speed * Time.fixedDeltaTime);
        }

        if (olhaPlayer) {
            GameObject target = Player.instance.currentControlled;
            Vector3 targetPos = target.transform.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);    
        }

        if (desativadoTimer > 0) {
            desativadoTimer -= Time.fixedDeltaTime;
            if (desativadoTimer < 0) {
                desativadoTimer = 0;
                gunManager.onAutoShoot = true;
            }
        }

        if (transform.position.y < -300) {
            Destroy(gameObject);
        }
    }

    public virtual void Atirar() {
        gunManager.Atirar();
    }

    public void SetOnDespawn(bool onDespawn) {
        this.onDespawn = onDespawn;
    }

    void OnDestroy() {
        if(!gameObject.scene.isLoaded) return;

        GameController.instance.currentRoom.GetComponent<RoomManager>().EnemyDestroyed();

        if (onDespawn) return;

        if (controllable.sendoControlado) {
            Player.instance.DesinfectarRobo();
        } else {
            if (dropItem && Random.value <= percentageDrop) {
                Instantiate(dropItem, transform.position, Quaternion.identity);
            }
        }

        AudioSource audio = GetComponent<AudioSource>();
        AudioSource.PlayClipAtPoint(audio.clip, transform.position);
    }
}
