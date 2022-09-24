using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

using TMPro;
using UnityEngine.UI;
namespace UIElements
{
    public class StartMenuManager : GlobalUIFunctions
    {
        [Header("StartMenuManager")]
        public TMP_Dropdown videoDropdown;
        public Button contine_Start;
        public TMP_InputField nameInputField;
        public GameObject SavedDataPrefab;
        public List<GameObject> MenuList = new();
        public Button startButton;
        public Transform LevelList;
        public Button[] LevelButtons;
        public GameObject StartMenu;
        public GameObject StartCharacterBundle;
        //public Animator MaskAnimator;
        void Start()
        {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            nameInputField.text = PlayerPrefs.GetString("PlayerName");
            LoadVideoDropdown(videoDropdown);
            // videoDropdown.value = QualitySettings.GetQualityLevel();
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

        public void LateOpenMenu()
        {
            GlobalUIFunctions.ShowCrusor();
            OpenMenu(StartMenu);
        }

        public void Continue(string StartScene)
        {
            // LoadLevelByName(PlayerPrefs.GetString("SavedCheckPointScene"));
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("SavedCheckPointScene"))) { LoadLevelByName(StartScene); }
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
                else
                {
                    LevelButtons[i].interactable = false;
                    LevelButtons[i].GetComponentInChildren<LocalizeStringEvent>().enabled = false;


                    LevelButtons[i].GetComponentInChildren<TMP_Text>().text = "?";
                }
            }
        }
        void ChangeStart_ContinueButton()//let start button text different accroding to the current unlocked level https://forum.unity.com/threads/how-to-update-localizestring-at-runtime.969270/
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("SavedCheckPointScene")))
            {
                //   test.SetTable("MenuTable");
                contine_Start.GetComponentInChildren<LocalizeStringEvent>().SetEntry("StartGame");
                // startButton.GetComponentInChildren<TMP_Text>().text = StartString.GetLocalizedStringAsync().ToString();
            }
            else
            {
                contine_Start.GetComponentInChildren<LocalizeStringEvent>().SetEntry("Continue");
                //startButton.GetComponentInChildren<TMP_Text>().text = ContinueString.GetLocalizedString();
            }
        }

        public void SetPlayerName(TMP_InputField input)
        {
            PlayerPrefs.SetString("PlayerName", input.text);
        }
        public void CleanAllData()
        {
            PlayerPrefs.DeleteAll();
        }

        //public static void LoadLocale(string languageIdentifier) { LocalizationSettings.SelectedLocale.Identifier = languageIdentifier; }
    }
}

