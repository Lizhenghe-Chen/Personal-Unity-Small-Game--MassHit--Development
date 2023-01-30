using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using System;

namespace UIElements
{
    public class EscUI : GlobalUIFunctions
    {
        public TMP_Dropdown levelDropdown, videoDropdown;
        public GameObject SwitchButton;
        public Button resumeButton;
        [SerializeField] private GameObject PlayerBunble, SpectatorBunble;


        public GameObject PlayerBunbleHUD, SpectatorBunbleHUD;

        public Transform PlayerPos, SpectatorPos;
        //public GameObject escUI;
        public CharacterCtrl characterCtrl;
        public Transform MainCamera;

        public CinemachineVirtualCamera PlayervCam, SpectatorvCam;
        public CinemachineFreeLook PlayerfreeLook;
        public Canvas escCanvas;
        public GameObject MissionCanvas;
        CinemachineBrain cameraBrain;
        public GameObject PlayerTips;
        [Header("SpectatorUI System:")]
        [SerializeField] Canvas SpectatorBundleCanavs, PlayerBundleCanvas;
        [SerializeField] SpectatorUI SpectatorUIScript;
        // Start is called before the first frame update

        void OnEnable()
        {
            escCanvas = this.GetComponent<Canvas>();
            PlayerBundleCanvas = PlayerBunbleHUD.GetComponent<Canvas>();
            SpectatorBundleCanavs = SpectatorBunbleHUD.GetComponent<Canvas>();
            SpectatorUIScript = SpectatorBunbleHUD.GetComponent<SpectatorUI>();
            // videoDropdown.value = QualitySettings.GetQualityLevel();

            //ChangeQualityLevel();
            cameraBrain = MainCamera.GetComponent<CinemachineBrain>();
            try { MissionCanvas = GameObject.Find("MissionCanvas"); } catch (Exception) { }
            LoadVideoDropdown(videoDropdown);
            // if (videoDropdown) { videoDropdown.value = QualitySettings.GetQualityLevel(); }
            levelDropdown.value = SceneManager.GetActiveScene().buildIndex - 1;
        }
        private void Start()
        {
            InGameMenu();
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                InGameMenu();
            }

        }
        public void InGameMenu()
        {

            GlobalRules.PauseTime();

            if (escCanvas.enabled && !SpectatorBunble.activeSelf)
            //if menu is active and photoBundle is inactive,
            //it is time to disable the menu and let time normal
            {
                GlobalRules.instance.normalTime = true;
            }
            else
            {
                GlobalRules.instance.normalTime = false;
            }

            CancelInvoke();

            if (escCanvas.enabled)//if menu is active, switch it inactive
            {
                escCanvas.enabled = false;
                UnFrozenCMBrain();
                PlayerTips.SetActive(false);
                SpectatorBundleCanavs.enabled = false;
                SpectatorUIScript.EnableCameraCtrol();
                GlobalUIFunctions.HideCrusor();
            }
            else// if menu is inactive, switch it active,open the menu
            {
                escCanvas.enabled = true;
                FrozeCMBrain();
                PlayerTips.SetActive(true);
                SpectatorBundleCanavs.enabled = true;
                SpectatorUIScript.DisableCameraCtrol();
                GlobalUIFunctions.ShowCrusor();

            }
        }
        public void SwitchBunble()
        {
            GlobalRules.PauseTime();
            GlobalRules.instance.normalTime = false;
            Debug.Log("SwitchBunble");
            
            CancelInvoke();

            if (characterCtrl.enabled)
            {

                characterCtrl.enabled = false;
                PlayerBunbleHUD.SetActive(false);
                if (MissionCanvas) { MissionCanvas.SetActive(false); }
                SpectatorBunble.SetActive(true);
                resumeButton.interactable = false;
                SwitchButton.GetComponentInChildren<LocalizeStringEvent>().SetEntry("Player Mode");
            }
            else
            {

                characterCtrl.enabled = true;
                PlayerBunbleHUD.SetActive(true);
                if (MissionCanvas) { MissionCanvas.SetActive(true); }
                resumeButton.interactable = true;
                SpectatorBunble.SetActive(false);
                SwitchButton.GetComponentInChildren<LocalizeStringEvent>().SetEntry("Photo Mode");
            }

        }

        void SwitchBubleWithCamera()
        { //let two bundle's camera have same direction

            if (PlayervCam.gameObject.activeSelf)
            {
                SpectatorvCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = PlayervCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value;
                // SpectatorvCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = PlayervCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_XAxis.Value;
            }
            else if (PlayerfreeLook.gameObject.activeSelf)
            {
                SpectatorvCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = PlayerfreeLook.m_XAxis.Value;
                SpectatorvCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = PlayerfreeLook.m_YAxis.Value * 90;
            }
            SpectatorPos.position = MainCamera.position;
        }
        void FrozeCMBrain()
        {
            cameraBrain.m_IgnoreTimeScale = false;
            cameraBrain.enabled = false;
            Debug.Log("FrozeCMBrain");
        }
        void UnFrozenCMBrain()
        {
            cameraBrain.m_IgnoreTimeScale = true;
            cameraBrain.enabled = true;
            Debug.Log("UnFrozenCMBrain");
        }

    }
}


