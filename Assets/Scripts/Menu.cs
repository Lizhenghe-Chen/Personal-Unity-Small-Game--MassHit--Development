using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public TMP_Dropdown videoDropdown, levelDropdown;
    public GameObject SwitchButton;
    public Button resumeButton;
    [SerializeField] private GameObject PlayerBunble, SpectatorBunble;

    public GameObject PlayerBunbleHUD, SpectatorBunbleHUD;
    public Canvas SpectatorBunbleCnavs;
    public Transform PlayerPos, SpectatorPos;
    //public GameObject escUI;
    public CharacterCtrl characterCtrl;
    public Transform MainCamera;

    public CinemachineVirtualCamera PlayervCam, SpectatorvCam;
    public CinemachineFreeLook PlayerfreeLook;
    public Canvas escCanvas;
    CinemachineBrain cameraBrain;


    // Start is called before the first frame update
    void Awake()
    {
        escCanvas = this.GetComponent<Canvas>();
        videoDropdown.value = QualitySettings.GetQualityLevel();

        ChangeQualityLevel();
        cameraBrain = MainCamera.GetComponent<CinemachineBrain>();
        InGameMenu();
        levelDropdown.value = SceneManager.GetActiveScene().buildIndex - 1;
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
        CancelInvoke();
        if (escCanvas.enabled)//if menu is active, switch it inactive
        {
            escCanvas.enabled = false;

            Cursor.visible = false;

            Cursor.lockState = CursorLockMode.Locked;
            //  SpectatorBunbleHUD.SetActive(false);
            SpectatorBunbleCnavs.enabled = false;
            //PlayerBunbleHUD.SetActive(false);
            //if (!characterCtrl.enabled) { SwitchBunble(); CancelInvoke(); }//back to player mode
            cameraBrain.enabled = true;
        }
        else// if menu is inactive, switch it active,open the menu
        {
            escCanvas.enabled = true;
            cameraBrain.enabled = false;
            SpectatorBunbleCnavs.enabled = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //SpectatorBunbleHUD.SetActive(true);
            //PlayerBunbleHUD.SetActive(true);
            Time.timeScale = 0.0001f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            //   Invoke("UnableCinemachineBrain", 1f * Time.timeScale);
        }

    }
    public void QuitGame()
    {
        Application.Quit();
    }


    public void SwitchBunble()
    {
        CancelInvoke();
        cameraBrain.enabled = true;
        if (characterCtrl.enabled)
        {
            characterCtrl.enabled = false;
            PlayervCam.gameObject.SetActive(false);
            PlayerfreeLook.gameObject.SetActive(false);
            PlayerBunbleHUD.SetActive(false);
            SpectatorBunble.SetActive(true);
            resumeButton.interactable = false;
            //PlayerBunble.SetActive(false);
            SwitchBubleWithCamera();
            //  var a = SwitchButton.GetComponentInChildren(typeof(Text)) as Text;
            SwitchButton.GetComponentInChildren<TextMeshProUGUI>().text = "Player Mode";
        }
        else
        {
            characterCtrl.enabled = true;
            // PlayervCam.gameObject.SetActive(true);
            PlayerfreeLook.gameObject.SetActive(true);
            PlayerBunbleHUD.SetActive(true);
            resumeButton.interactable = true;
            // PlayerBunble.SetActive(true);
            SpectatorBunble.SetActive(false);
            //PlayerPos.position = SpectatorPos.position;
            SwitchButton.GetComponentInChildren<TextMeshProUGUI>().text = "Photo Mode";
        }
        Invoke(nameof(UnableCinemachineBrain), 1f * Time.timeScale);
    }


    public void ChangeQualityLevel()
    {
        QualitySettings.SetQualityLevel(videoDropdown.value, true);

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

