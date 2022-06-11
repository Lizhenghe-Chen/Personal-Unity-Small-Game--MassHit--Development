using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public TMP_Dropdown videoDropdown, levelDropdown;
    [SerializeField] private GameObject PlayerBunble, SpectatorBunble;
    public GameObject PlayerBunbleHUD, SpectatorBunbleHUD;
    public Transform PlayerPos, SpectatorPos;
    public GameObject escUI;
    public Transform MainCamera;
    CinemachineBrain cameraBrain;


    // Start is called before the first frame update
    void Awake()
    {
        videoDropdown.value = QualitySettings.GetQualityLevel();
        levelDropdown.value = SceneManager.GetActiveScene().buildIndex;
        ChangeQualityLevel();
        cameraBrain = MainCamera.GetComponent<CinemachineBrain>();
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
        if (escUI.activeSelf)//if menu is active, switch it inactive
        {
            escUI.SetActive(false);

            Cursor.visible = false;
            cameraBrain.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            SpectatorBunbleHUD.SetActive(false);
            PlayerBunbleHUD.SetActive(false);
            //Time.timeScale = 1f;
            // Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        else// if menu is inactive, switch it active,open the menu
        {
            escUI.SetActive(true);
            cameraBrain.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SpectatorBunbleHUD.SetActive(true);
            PlayerBunbleHUD.SetActive(true);
            Time.timeScale = 0.01f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

    }
    public void QuitGame()
    {
        Application.Quit();
    }


    public void SwitchBunble()
    {
        if (PlayerBunble.activeSelf)
        {
            PlayerBunble.SetActive(false); SpectatorBunble.SetActive(true);
            SpectatorPos.position = PlayerPos.position;
        }
        else
        {
            PlayerBunble.SetActive(true); SpectatorBunble.SetActive(false);
            PlayerPos.position = SpectatorPos.position;
        }

    }


    public void ChangeQualityLevel()
    {
        QualitySettings.SetQualityLevel(videoDropdown.value, true);

    }
    public void ChangeLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == levelDropdown.value)
        {
            return;
        }

        Debug.Log(levelDropdown.value);
        SceneManager.LoadScene(levelDropdown.value);
    }
}

