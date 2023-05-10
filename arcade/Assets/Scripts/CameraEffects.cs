using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffects : MonoBehaviour {
    static public CameraEffects instance;
    public PostProcessVolume volume;

    public float distortedChrom = 0.5f;
    public float speed, time = 0f;

    public bool changingValues = false;

    void Start() {
        volume = GetComponent<PostProcessVolume>();
        instance = this;
    }

    float Spike(float x) {
        return Mathf.Sin(x * Mathf.PI);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (changingValues) {
            volume.profile.TryGetSettings(out ChromaticAberration chromatic);
            
            chromatic.intensity.value = Mathf.Lerp(0f, distortedChrom, Spike(time));

            if (time >= 1f) {
                changingValues = false;
            }
            time +=  Time.fixedDeltaTime * speed;
        }
    }

    public void MakeDistortion(float speed = 5f) {
        this.speed = speed;
        changingValues = true;
        time = 0f;
    }
}
