using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float rangeInfectar = 2.5f, rangePrecisa = 2f;
    public GameObject precisaoObject;
    public GameObject trail;
    public LayerMask layerMask;

    public float speed;
    public GameObject virusComponent;
    public GameObject controlledRobot;
    public GameObject luzIndicadora;
    public bool estaControlando = true;
    public float rotationSpeed;

    public bool podeDecair = true;
    public float velocidadeDecaimento = 1f;
    public float rateDecaimento = 0.1f;
    float cooldownTempo = 0f;

    public int balasAtiradas = 0;
    AudioSource shockAudio;

    public GameObject currentControlled {
        get {
            return estaControlando ?  (controlledRobot != null ? controlledRobot : virusComponent) : (virusComponent != null? virusComponent : controlledRobot);
        }
    }

    void Start() {
        if (instance == null) {
            instance = this;
        }

        shockAudio = GetComponent<AudioSource>();
        UIController.instance.AtualizarControlado(this);
        virusComponent.transform.Find("ArmaHolder").gameObject.GetComponent<GunManager>().OnShoot += HandleShoot;
    }

    void HandleShoot() {
        GameObject gunHolder = currentControlled.transform.Find("ArmaHolder").gameObject;
        balasAtiradas++;
        UIController.instance.AtualizarContagemBalas(gunHolder.GetComponent<GunManager>());
    }

    void HandleGunReload(float reloadPerc) {
        UIController.instance.AtualizarReloadProgresso(reloadPerc);
    }
    // Update is called once per frame
    void Update() {
        float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;

        // Controle de jogador atual
        if (Input.GetMouseButtonDown(1)) {
            if (!estaControlando) TentaInfectarRobo();
            else if (!TentaInfectarRobo()) DesinfectarRobo();
        }

        InstancesInRange();

        // Controle de movimento
        currentControlled.GetComponent<Rigidbody>().velocity = new Vector3(h, 0, v);
        luzIndicadora.transform.position = new Vector3(currentControlled.transform.position.x, luzIndicadora.transform.position.y, currentControlled.transform.position.z);

        // Controle de tiro
        GameObject gunHolder = currentControlled.transform.Find("ArmaHolder") ? currentControlled.transform.Find("ArmaHolder").gameObject : null;
        if (Input.GetMouseButtonDown(0)) {
            if (gunHolder != null) {
                gunHolder.GetComponent<GunManager>().onAutoShoot = true;
                // gunHolder.GetComponent<GunManager>().Atirar();
            }
            else currentControlled.GetComponent<VirusControllable>().HandlePlayerAcao();
        }
        else if (Input.GetMouseButtonUp(0)) {
            if (gunHolder != null) {
                gunHolder.GetComponent<GunManager>().onAutoShoot = false;
            }
        }
        
    }

    void FixedUpdate() {
        // Controle de rotação
        Vector3 mousePos = MousePosition();
        if (!estaControlando || currentControlled.GetComponent<VirusControllable>().permiteRotacao) {
            currentControlled.transform.LookAt(mousePos);
        }

        // Controle de precisão
        RoomManager roomManager = GameController.instance.currentRoom.GetComponent<RoomManager>();
        Vector3 centro = currentControlled.transform.position;
        Vector3 precisaoPos = mousePos;

        float distancePrecisao = Vector3.Distance(centro, mousePos);
        if (distancePrecisao > rangeInfectar -1) {
            Vector3 fromOriginToObject = precisaoPos - centro;
            fromOriginToObject *= (rangeInfectar-1) / distancePrecisao;
            precisaoPos = centro + fromOriginToObject;
        }

        precisaoObject.transform.position = roomManager.ClampOnRoom(precisaoPos);


        // Controle de tempo do decaimento de energia
        if (!estaControlando && podeDecair) {
            cooldownTempo += Time.deltaTime;
            if (cooldownTempo > velocidadeDecaimento) {
                cooldownTempo = 0f;
                currentControlled.GetComponent<AliveSomehow>().SofrerDano(rateDecaimento, false);
            }
        } else {
            cooldownTempo = 0f;
        }

        // Controle de visualização de invulnerabilidade
        /*
        if (currentControlled.GetComponent<AliveSomehow>().invulnerable) {
            float normalIntensity = 2f;
            float lowIntensity = 1f;
            float durationInSeconds = 0.2f;
            luzIndicadora.GetComponent<Light>().intensity = Mathf.Lerp(lowIntensity, normalIntensity, Mathf.PingPong(Time.time / durationInSeconds, 1));
        } else {
            luzIndicadora.GetComponent<Light>().intensity = 2;
        }
        */
    }

    public Vector3 MousePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask)) {
            Vector3 pos = hit.point;
            pos.y = currentControlled.transform.position.y;
            return pos;
        }
        return Vector3.zero;
        /*
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 12.5f));
        mousePos.y = currentControlled.transform.position.y;
        return mousePos;*/
    }

    public bool TentaInfectarRobo() {
        // Escolhe inimigo mais proximo para ser controlado
        VirusControllable proximo = InstancesInRange(controlledRobot != null);

        if (proximo != null) {
            InfectarRobo(proximo);
        }

        return proximo != null;
    }

    public void InfectarRobo(VirusControllable robo) {
        if (robo != null) {
            CreateTrailFrom(currentControlled, robo.gameObject);
            if (controlledRobot != null) {
                DesinfectarRobo(true);
            }
            controlledRobot = robo.gameObject;
            controlledRobot.GetComponent<VirusControllable>().HandleSetControlado(true);
            if (controlledRobot.transform.Find("ArmaHolder") != null){
                controlledRobot.transform.Find("ArmaHolder").gameObject.GetComponent<GunManager>().OnShoot += HandleShoot;
                controlledRobot.transform.Find("ArmaHolder").gameObject.GetComponent<GunManager>().OnReload += HandleGunReload;
            }
            virusComponent.SetActive(false);
            estaControlando = true;
            HandleUpdateControlado();
        }
    }

    public void DesinfectarRobo(bool ignoreVirus = false) {        
        if (controlledRobot != null) {
            if (!ignoreVirus) {
                virusComponent.transform.position = precisaoObject.transform.position;
                CreateTrailFrom(controlledRobot, virusComponent);
            }
            controlledRobot.GetComponent<VirusControllable>().HandleSetControlado(false);
            if (controlledRobot.transform.Find("ArmaHolder") != null)
                controlledRobot.transform.Find("ArmaHolder").gameObject.GetComponent<GunManager>().OnShoot -= HandleShoot;
                controlledRobot.transform.Find("ArmaHolder").gameObject.GetComponent<GunManager>().OnReload -= HandleGunReload;

        }

        controlledRobot = null;
        estaControlando = false;
        virusComponent.SetActive(true);
        virusComponent.GetComponent<AliveSomehow>().SetInvulnerable(1f);
        HandleUpdateControlado();
    }

    public void CreateTrailFrom(GameObject from, GameObject to) {
        Debug.Log(to);
        trail.transform.position = from.transform.position;
        trail.GetComponent<TrailScript>().SetFollow(to);
    }

    public void PlayShockAudio() {
        float randomPitch = Random.Range(0.8f, 1.2f);
        shockAudio.pitch = randomPitch;
        shockAudio.Play();
    }

    public void HandleUpdateControlado() {
        GameController.instance.currentRoom.GetComponent<RoomManager>().CheckRoomCondition();
        UIController.instance.AtualizarControlado(this);
        PlayShockAudio();
    }

    public VirusControllable InstancesInRange(bool preciseSelecionado = false) {
        Vector3 point = currentControlled.transform.position;
        Vector3 posMouse = MousePosition();

        VirusControllable instanciaProxima = null;

        List<VirusControllable> emRange = new List<VirusControllable>();
        foreach (VirusControllable instance in VirusControllable.instances) {
            // Se a instancia esta habilitada e no range do player
            if (instance.habilitado && instance.gameObject != controlledRobot && Vector3.Distance(point, instance.gameObject.transform.position) < rangeInfectar) {
                emRange.Add(instance);
            }
        }
        instanciaProxima = precisaoObject.GetComponent<InfectarPrecisao>().GetPreciso();
        if (preciseSelecionado && false) {
            instanciaProxima = null;
        }

        // Atualiza o estilo das intancias
        List<VirusControllable> naoEmRange = new List<VirusControllable>(VirusControllable.instances);
        foreach(VirusControllable instance in emRange) {
            instance.UpdateStyle(true, instance == instanciaProxima);
            naoEmRange.Remove(instance);
        }
        foreach(VirusControllable instance in naoEmRange) {
            instance.UpdateStyle(false, false);
        }
        
        return instanciaProxima;
    }

}
