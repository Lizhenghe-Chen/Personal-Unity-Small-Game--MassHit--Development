using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
namespace UIElements
{
    public class GlobalUIFunctions : MonoBehaviour
    {
        [Header("Below Are From GlobalUIFunctions")]
        public static Animator MaskAnimator;
        // public TMP_Dropdown videoDropdown;
        public string playerName;// https://docs.unity3d.com/Packages/com.unity.localization@1.2/manual/QuickStartGuideWithVariants.html
        public TMP_Text loadingText;
        public bool AsyncLoadPass = false;


        [System.Serializable]
        public struct MissionTextUpper_Lower
        {
            public int MissionTextIndexTarget;
            public int UpperLimit;
            public int LowerLimit;

        }
        private void OnEnable()
        {


            try { MaskAnimator = GameObject.Find("Mask").GetComponent<Animator>(); }
            catch (System.Exception)
            {
                Debug.LogWarning("MaskAnimator not found");
            }



            playerName = PlayerPrefs.GetString("PlayerName");
        }
        public void LoadVideoDropdown(TMP_Dropdown videoDropdown)
        {
            if (videoDropdown) { videoDropdown.value = QualitySettings.GetQualityLevel(); }
        }
        public void LoadLevelByName(string SceneName)
        {
            if (MaskAnimator) { PlayMaskAnimatorLeave(); }
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
            if (MaskAnimator) { PlayMaskAnimatorLeave(); }
            try { StartCoroutine(DelayLoadLevel(SceneIndex)); }
            catch (System.Exception e)
            {
                Debug.LogError("Load Scene failed: \n" + e);
                SceneManager.LoadScene("StartMenu");
            }
            //   StartCoroutine(DelayLoadLevel(SceneObj));
        }
        public void LoadSceneAsync(string NextSceneName)
        {
            StartCoroutine(AsyncLoadScene(NextSceneName));
            //SceneManager.LoadScene(NextSceneIndex);
        }

        AsyncOperation asyncOperation;
        IEnumerator AsyncLoadScene(string NextSceneName)//https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html
        {
            // yield return null;
            //Begin to load the Scene you specify
            asyncOperation = SceneManager.LoadSceneAsync(NextSceneName);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;

            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                //Output the current progress
                //loadingText.text = "Loading : " + (asyncOperation.progress / .9f * 100) + "%";
                //   Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");

                // Check if the load has finished

                //Activate the Scene
                // asyncOperation.allowSceneActivation = AsyncLoadPass;

                yield return null;
            }

        }
        public void FinnishAsyncLoad()
        {
            AsyncLoadPass = true;
            asyncOperation.allowSceneActivation = AsyncLoadPass;
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
        public void PlayMaskAnimatorLeave() { if (MaskAnimator) MaskAnimator.Play("Leave"); }
        public void QuitGame()
        {
            PlayMaskAnimatorLeave();
            Invoke(nameof(LateQuitGame), 1f);
        }
        public void LateQuitGame()
        {
            Application.Quit();
            Debug.Log("Quit");
        }
        public void ChangeQualityLevel(TMP_Dropdown videoDropdown)
        {
            QualitySettings.SetQualityLevel(videoDropdown.value, true);
        }
        public void ChangeLanguage(int choosedLangIndex)//https://docs.unity3d.com/Packages/com.unity.localization@1.3/manual/Scripting.html
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[choosedLangIndex];

        }
        public void UpdateText()
        {
            if (string.IsNullOrEmpty(playerName) && loadingText) { loadingText.GetComponentInChildren<LocalizeStringEvent>().SetEntry("NullPlayerName"); }
            else { loadingText.GetComponentInChildren<LocalizeStringEvent>().SetEntry("PlayerWelecome"); }
        }

    }
}

