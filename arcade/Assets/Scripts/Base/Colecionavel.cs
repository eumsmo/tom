using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colecionavel : MonoBehaviour
{
    public float life = 1f; 
    public float maxLife = 0f;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            AliveSomehow player = other.gameObject.GetComponent<AliveSomehow>();
            player.maxLife += maxLife;
            player.Heal(life);
        } else if(other.gameObject.CompareTag("Enemy") && other.gameObject.GetComponent<VirusControllable>().sendoControlado) {
            AliveSomehow enemy = other.gameObject.GetComponent<AliveSomehow>();
            enemy.Heal(life);
        } else {
            return;
        }

        AudioSource audio = GetComponent<AudioSource>();
        AudioSource.PlayClipAtPoint(audio.clip, transform.position);
        Destroy(gameObject);
    }
}
