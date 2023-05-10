using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectarPrecisao : MonoBehaviour
{
    List<VirusControllable> proximos = new List<VirusControllable>();

    public VirusControllable GetPreciso () {
        if (proximos.Count == 0) return null;
        else if (proximos.Count == 1) return proximos[0];

        float minDist = float.MaxValue;
        int minIndex = 0;
        for (int i = 0; i < proximos.Count; i++) {
            if (proximos[i] == null) continue; // Remover os que estão como null
            float dist = Vector3.Distance(transform.position, proximos[i].gameObject.transform.position);
            if (dist < minDist) {
                minDist = dist;
                minIndex = i;
            }
        }
        return proximos[minIndex];
    }

    void OnTriggerEnter(Collider other) {
        VirusControllable controllable = other.gameObject.GetComponent<VirusControllable>();
        
        if (controllable != null && !proximos.Contains(controllable)) {
            proximos.Add(controllable);
        }
    }

    void OnTriggerExit(Collider other) {
        VirusControllable controllable = other.gameObject.GetComponent<VirusControllable>();
        if (controllable != null && proximos.Contains(controllable)) {
            proximos.Remove(controllable);
        }
    }
}
