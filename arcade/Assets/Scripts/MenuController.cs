using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    public static bool firstTime = true;

    public AudioSource bootAudio, shutdownAudio, ambienceAudio;
    public float offset = 0.5f;

    public GameObject computer, screen, glitchScreen, glitchLight, glitchPlane;

    public GameObject titulo1, titulo2, menu, fadeOutImage;

    void Start() {
        if (firstTime) OnStart();
        else {
            PlayAmbience();
            glitchScreen.SetActive(true);
            computer.GetComponent<Animator>().enabled = true;
        }
    }

    public void OnStart() {
        firstTime = false;
        PlayBoot();
        StartCoroutine(BootAnimation());
    }

    public void PlayAmbience() {
        bootAudio.Stop();
        ambienceAudio.Play();
    }

    public void PlayShutdown() {
        bootAudio.Stop();
        ambienceAudio.Stop();
        shutdownAudio.Play();
        StartCoroutine(ShutdownAnimation(1f));
    }

    public void PlayBoot() {
        bootAudio.Play();
        ambienceAudio.PlayDelayed(bootAudio.clip.length - offset);
    }

    public void FecharJogo() {
        PlayShutdown();
        Invoke("InstantQuit", 2f);
    }

    public void InstantQuit() {
        Application.Quit();
    }

    public IEnumerator fadeInUI(CanvasGroup obj, float duration) {
        float time = 0;

        while (time < duration) {
            time += Time.deltaTime;
            float t = time / duration;
            obj.alpha = t;
            yield return null;
        }
    }

    public IEnumerator BootAnimation() {
        float time = 0;
        float duration = 3;
        SpriteRenderer screenSprite = screen.GetComponent<SpriteRenderer>();
        CanvasGroup titulo1Canvas = titulo1.GetComponent<CanvasGroup>();
        CanvasGroup titulo2Canvas = titulo2.GetComponent<CanvasGroup>();
        CanvasGroup menuCanvas = menu.GetComponent<CanvasGroup>();

        screenSprite.color = new Color(1, 1, 1, 0);
        titulo1Canvas.alpha = 0;
        titulo2Canvas.alpha = 0;

        yield return new WaitForSeconds(0.75f);

        while (time < duration) {
            time += Time.deltaTime;
            float t = time / duration;
            titulo1Canvas.alpha = t;
            screenSprite.color = new Color(1, 1, 1, t);
            yield return null;
        }
        screenSprite.color = new Color(1, 1, 1, 1);
        StartCoroutine(fadeInUI(titulo2Canvas, 1.5f));
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 2; i++) {
            glitchScreen.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            glitchScreen.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

        glitchScreen.SetActive(true);
        computer.GetComponent<Animator>().enabled = true;
    }

    public IEnumerator ShutdownAnimation(float duration) {
        float time = 0;
        screen.SetActive(false);
        fadeOutImage.SetActive(true);

        Light light = glitchLight.GetComponent<Light>();
        float startLightIntensity = light.intensity;
        Material glitchScreenMaterial = glitchPlane.GetComponent<Renderer>().material;
        Color startColor = glitchScreenMaterial.color;

        while (time < duration) {
            time += Time.deltaTime;
            float t = time / duration;
            float mult = 1-t;
            startColor.a = mult;
            glitchScreenMaterial.color = startColor;
            light.intensity = startLightIntensity * mult;
            yield return null;
        }

        StartCoroutine(fadeInUI(fadeOutImage.GetComponent<CanvasGroup>(), 0.5f));
    }
}
