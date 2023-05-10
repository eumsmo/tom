using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsController : MonoBehaviour {
    public AudioMixer audioMixer;
    public Slider musicSlide;
    public Slider fxSlider;
    
    public void ChangeMusicVolume() {
        audioMixer.SetFloat("MusicVol", musicSlide.value);
    }

    public void ChangeFXVolume() {
        audioMixer.SetFloat("FXVol", fxSlider.value);
    }
}
