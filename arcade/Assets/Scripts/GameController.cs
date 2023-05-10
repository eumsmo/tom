using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    static public GameController instance;

    public GameObject currentRoom;
    public Material selecionadoMaterial, naoSelecionadoMaterial;

    public Estatisticas estatisticas;
    bool contandoTempo = true;

    public GameObject pausePanel, musica;

    void Start() {
        instance = this;
        
        // Carrega cenas adjacentes ao primeiro quarto
        currentRoom.GetComponent<RoomManager>().LoadAdjacentsBut("");

        estatisticas = new Estatisticas();
        Despausar();
        Door.thatDoorOpen = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (pausePanel.activeSelf) {
                Despausar();
            } else {
                Pausar();
            }
        }
    }

    void FixedUpdate() {
        if (contandoTempo) AddTime(Time.fixedDeltaTime);
    }

    // Scene Loaders

    void HandleGameEnd() {
        contandoTempo = false;
        GerarEstatisticas();
        RoomManager.ResetRoomsData();
        VirusControllable.Reset();
        Time.timeScale = 1;
    }

    public void WinGame() {
        HandleGameEnd();
        SceneManager.LoadScene("CenaVitoria");
    }

    public void LoseGame() {
        HandleGameEnd();
        SceneManager.LoadScene("CenaDerrota");
    }

    public void ReloadGame() {
        HandleGameEnd();
        SceneManager.LoadScene("Fase1");
    }

    public void LeaveToMenu() {
        HandleGameEnd();
        SceneManager.LoadScene("Menu");
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName) {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    // Estatisticas

    public void AddTime(float time) {
        estatisticas.tempoDeJogo += time;
    }

    public void GerarEstatisticas() {
        estatisticas.danoTomado = Player.instance.virusComponent.GetComponent<AliveSomehow>().danoTomado;
        estatisticas.balasAtiradas = Player.instance.balasAtiradas;
        estatisticas.robosInfectados += VirusControllable.numInfectados;
    }

    // Pausa

    public void Pausar() {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        musica.GetComponent<AudioSource>().Pause();
    }

    public void Despausar() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        musica.GetComponent<AudioSource>().Play();
    }

}
