using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class StartMenuManager : MonoBehaviour
{
    public LocalizeStringEvent test;
    public GameObject SavedDataPrefab;
    public List<GameObject> MenuList = new();
    public Button startButton;
    public Transform LevelList;
    public Button[] LevelButtons;
    public TMP_Dropdown videoDropdown;

    public GameObject StartMenu;
    public GameObject StartCharacterBundle;
    public Animator MaskAnimator;
    void Start()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        videoDropdown.value = QualitySettings.GetQualityLevel();
        ChangeStart_ContinueButton();
        // OpenMenu(StartMenu);
        Invoke("LateOpenMenu", 1f);
    }

    public void OpenMenu(GameObject targetMenu)
    {
        LoadLevelData();
        foreach (GameObject menu in MenuList)
        {
            if (menu == targetMenu)
            {
                menu.SetActive(true);
            }
            else
            {
                menu.SetActive(false);
            }
        }

    }
    public void ReturnToMainMenu()
    {
        OpenMenu(StartMenu);
    }
    public void QuitGame()
    {
        if (MaskAnimator != null) { MaskAnimator.Play("Leave"); }
        Invoke("LateQuitGame", 1f);
    }
    public void LoadLevelByName(string SceneName)
    {
        if (MaskAnimator != null) { MaskAnimator.Play("Leave"); }
        try { StartCoroutine(DelayLoadLevel(SceneName)); }
        catch (System.Exception e)
        {
            Debug.LogError("Load Scene failed: \n" + e);
            SceneManager.LoadScene("StartMenu");
        }
        //   StartCoroutine(DelayLoadLevel(SceneObj));
    }
    public void LoadLevelByIndex(int SceneIndex)
    {
        if (MaskAnimator != null) { MaskAnimator.Play("Leave"); }
        try { StartCoroutine(DelayLoadLevel(SceneIndex)); }
        catch (System.Exception e)
        {
            Debug.LogError("Load Scene failed: \n" + e);
            SceneManager.LoadScene("StartMenu");
        }
        //   StartCoroutine(DelayLoadLevel(SceneObj));
    }
    public IEnumerator DelayLoadLevel(string SceneName)
    {
        yield return new WaitForSecondsRealtime(1f);
        //   Destroy(StartCharacterBundle);
        SceneManager.LoadScene(SceneName);
        GlobalRules.instance = null;

    }
    public IEnumerator DelayLoadLevel(int SceneIndex)
    {
        yield return new WaitForSecondsRealtime(1f);
        //   Destroy(StartCharacterBundle);
        SceneManager.LoadScene(SceneIndex);
        GlobalRules.instance = null;

    }
    public void LateOpenMenu()
    {
        OpenMenu(StartMenu);
    }
    public void LateQuitGame()
    {
        Application.Quit();
    }
    public void Continue()
    {
        // LoadLevelByName(PlayerPrefs.GetString("SavedCheckPointScene"));
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("SavedCheckPointScene"))) { LoadLevelByName("TestScene"); }
        else { LoadLevelByName(PlayerPrefs.GetString("SavedCheckPointScene")); }

    }
    void LoadLevelData()
    {
        //load unlocked Level index
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel");
        LevelButtons = LevelList.GetComponentsInChildren<Button>();
        for (int i = 0; i < LevelButtons.Length; i++)
        {
            if (i <= unlockedLevel) { LevelButtons[i].interactable = true; }
            else { LevelButtons[i].interactable = false; }
        }
    }
    void ChangeStart_ContinueButton()//let start button text different accroding to the current unlocked level https://forum.unity.com/threads/how-to-update-localizestring-at-runtime.969270/
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("SavedCheckPointScene")))
        {
            //   test.SetTable("MenuTable");
            test.SetEntry("StartGame");
            // startButton.GetComponentInChildren<TMP_Text>().text = StartString.GetLocalizedStringAsync().ToString();
        }
        else
        {
            test.SetEntry("Continue");
            //startButton.GetComponentInChildren<TMP_Text>().text = ContinueString.GetLocalizedString();
        }
    }

    public void ChangeQualityLevel()
    {
        QualitySettings.SetQualityLevel(videoDropdown.value, true);

    }
    public void CleanAllData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void ChangeLanguage(int choosedLangIndex)//https://docs.unity3d.com/Packages/com.unity.localization@1.3/manual/Scripting.html
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[choosedLangIndex];

    }
    //public static void LoadLocale(string languageIdentifier) { LocalizationSettings.SelectedLocale.Identifier = languageIdentifier; }
}
