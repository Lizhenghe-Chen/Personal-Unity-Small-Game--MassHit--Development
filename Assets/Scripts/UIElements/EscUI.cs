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
        [SerializeField] Canvas SpectatorBundleCanavs, PlayerBundleCanvas;

        // Start is called before the first frame update

        void OnEnable()
        {
            escCanvas = this.GetComponent<Canvas>();
            PlayerBundleCanvas = PlayerBunbleHUD.GetComponent<Canvas>();
            SpectatorBundleCanavs = SpectatorBunbleHUD.GetComponent<Canvas>();
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
            Time.timeScale = 0.0001f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            CancelInvoke();
            if (escCanvas.enabled)//if menu is active, switch it inactive
            {
                escCanvas.enabled = false;

                Cursor.visible = false;


                Cursor.lockState = CursorLockMode.Locked;
                //  SpectatorBunbleHUD.SetActive(false);
                PlayerTips.SetActive(false); SpectatorBundleCanavs.enabled = false;
                //PlayerBunbleHUD.SetActive(false);
                if (characterCtrl.enabled) { GlobalRules.instance.isPause = false; }
                cameraBrain.enabled = true;
            }
            else// if menu is inactive, switch it active,open the menu
            {
                escCanvas.enabled = true;
                cameraBrain.enabled = false;
                GlobalRules.instance.isPause = true;
                PlayerTips.SetActive(true);
                SpectatorBundleCanavs.enabled = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                //SpectatorBunbleHUD.SetActive(true);
                //PlayerBunbleHUD.SetActive(true);

                //   Invoke("UnableCinemachineBrain", 1f * Time.timeScale);
            }

        }

        public void SwitchBunble()
        {
            CancelInvoke();
            cameraBrain.enabled = true;
            if (characterCtrl.enabled)
            {
                GlobalRules.instance.isPause = true;
                characterCtrl.enabled = false;
                PlayervCam.gameObject.SetActive(false);
                PlayerfreeLook.gameObject.SetActive(false);
                PlayerBunbleHUD.SetActive(false);
                if (MissionCanvas) { MissionCanvas.SetActive(false); }
                SpectatorBunble.SetActive(true);
                resumeButton.interactable = false;
                //PlayerBunble.SetActive(false);
                SwitchBubleWithCamera();
                //  var a = SwitchButton.GetComponentInChildren(typeof(Text)) as Text;
                //SwitchButton.GetComponentInChildren<TextMeshProUGUI>().text = "Player Mode";
                SwitchButton.GetComponentInChildren<LocalizeStringEvent>().SetEntry("Player Mode");
            }
            else
            {
                characterCtrl.enabled = true;
                // PlayervCam.gameObject.SetActive(true);
                PlayerfreeLook.gameObject.SetActive(true);
                PlayerBunbleHUD.SetActive(true);
                if (MissionCanvas) { MissionCanvas.SetActive(true); }
                resumeButton.interactable = true;
                // PlayerBunble.SetActive(true);
                SpectatorBunble.SetActive(false);
                //PlayerPos.position = SpectatorPos.position;
                //SwitchButton.GetComponentInChildren<TextMeshProUGUI>().text = "Photo Mode";
                SwitchButton.GetComponentInChildren<LocalizeStringEvent>().SetEntry("Photo Mode");
            }
            Invoke(nameof(UnableCinemachineBrain), 1f * Time.timeScale);
        }

        //public void ChangeLevel()
        //{
        //    if (SceneManager.GetActiveScene().buildIndex == levelDropdown.value + 1)
        //    {
        //        levelDropdown.value = SceneManager.GetActiveScene().buildIndex - 1;
        //        Debug.LogWarning("Same Level");

        //        return;
        //    }

        //    PlayerPos.parent.position += new Vector3(0, 2, 0);
        //    Debug.Log(levelDropdown.value + 1);
        //    SceneManager.LoadScene(levelDropdown.value + 1);
        //    StartCoroutine(DelayLoadLevel(levelDropdown.value + 1));

        //    Time.timeScale = 0.01f;
        //    Time.fixedDeltaTime = Time.timeScale * 0.02f;

        //}

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
        void UnableCinemachineBrain()
        {
            cameraBrain.enabled = false;
            Debug.Log("UnableCinemachineBrain");
        }

    }
}


