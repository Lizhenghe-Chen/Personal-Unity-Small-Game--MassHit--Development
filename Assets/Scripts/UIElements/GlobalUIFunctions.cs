using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
namespace UIElements
{
    /* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class GlobalUIFunctions : MonoBehaviour
    {
        [Header("Below Are From GlobalUIFunctions")]
        public static Animator MaskAnimator;
        public static LevelList levelList;
        // public TMP_Dropdown videoDropdown;
        public static string playerName;// https://docs.unity3d.com/Packages/com.unity.localization@1.2/manual/QuickStartGuideWithVariants.html

        [HideInInspector] public bool AsyncLoadPass = false;
        public static Vector2 screenBound;

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
            levelList = Resources.Load<LevelList>("LevelList");
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
                LoadStartMenu();
            }
            //   StartCoroutine(DelayLoadLevel(SceneObj));
        }
        // public void LoadLevelByIndex(int SceneIndex)
        // {
        //     LoadScene(levelList.List[SceneIndex].levelName);
        // }

        /// <summary>
        /// Load the level by the index in the level list from the Recource folder, for example,
        /// Index 0 is the Splash Screen, Index 1 is the Start Menu, Index 2 is the first level in the level list, and so on.
        /// </summary>
        /// <param name="Index"> the index in the level list from the Recource folder</param>
        public void LoadLevelBy_LevelList_Index(int Index)
        {
            LoadScene(FindLevelNamebyIndex(Index));
        }
        public void ClearSavedLocation()
        {
            PlayerPrefs.DeleteKey("SavedCheckPointScene");
            PlayerPrefs.DeleteKey("SavedCheckPoint_X");
            PlayerPrefs.DeleteKey("SavedCheckPoint_Y");
            PlayerPrefs.DeleteKey("SavedCheckPoint_Z");
        }
        public string FindLevelNamebyIndex(int Index)
        {
            foreach (var item in levelList.List)
            {
                if (item.levelIndex == Index)
                {
                    return item.levelName;
                }
            }
            Debug.LogError("Level Name not found");
            return levelList.StartMenu.levelName;
        }
        public int FindLevelIndexbyName(string Name)
        {
            foreach (var item in levelList.List)
            {
                if (item.levelName == Name)
                {
                    return item.levelIndex;
                }
            }
            Debug.LogError("Level Index not found");
            return levelList.StartMenu.levelIndex;
        }
        public void LoadScene(string sceneName)
        {
            if (MaskAnimator) { PlayMaskAnimatorLeave(); }
            try { StartCoroutine(DelayLoadLevel(sceneName)); }
            catch (System.Exception e)
            {
                Debug.LogError("Load Scene failed: \n" + e);
                LoadStartMenu();
            }
        }
        public void LoadStartMenu() { LoadScene(levelList.List[0].levelName); }

        /// <summary>
        /// the index in the level list from the Recource folder
        /// </summary>
        public void LoadSceneAsync(int index)
        {
            StartCoroutine(AsyncLoadScene(levelList.List[index].levelName));
            //SceneManager.LoadScene(NextSceneIndex);
        }

        AsyncOperation asyncOperation;
        IEnumerator AsyncLoadScene(string NextSceneName)//https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html
        {
            yield return null;
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
            //  AsyncLoadPass = true;
            asyncOperation.allowSceneActivation = true;
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
        public static void HideCrusor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        public static void ShowCrusor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        ///<Summary>
        /// This funtion can be used fo fill the iamge bar according to the value
        ///</Summary>
        ///<param name="image">the target image </param>
        ///<param name="currentValue">the updated value of the image</param>
        ///<param name="maxValue">the max value of the image, (percentage)</param>
        public static float UpdateImageFill(Image image, float currentValue, float maxValue)
        {
            currentValue /= maxValue;
            image.fillAmount = currentValue;
            return currentValue;
        }
        ///<Summary>
        /// This funtion can be used fo locate the screen position of the target object according to the target object's physical position
        ///</Summary>
        /// (<paramref name="mainCamera"/>,paramref name="target"/>,<paramref name="screenIcon"/>,<paramref name="screenIconWidth"/>,<paramref name="screenIconHeight"/>).
        ///<param name="mainCamera">the current active Camera </param>
        ///<param name="target">the target object </param>
        ///<param name ="screenIcon">the target object's screen Icon</param>
        ///<param name ="screenIconWidth">the screen Icon width</param>
        ///<param name ="screenIconHeight">the screen Icon height</param>
        public static void ObjectToScreenPosition(Camera mainCamera, Transform target, Image screenIcon, int screenIconWidth, int screenIconHeight)
        {
            var screenPosition = mainCamera.WorldToScreenPoint(target.position);
            Debug.Log(screenBound);
            if (screenPosition.z < 0)
            {
                screenPosition *= -1;
            }
            screenIcon.transform.position = new Vector2(
                Mathf.Clamp(screenPosition.x, screenBound.x / 50, screenBound.x - screenBound.x / 50),
                Mathf.Clamp(screenPosition.y, screenBound.y / 50, screenBound.y - screenBound.y / 50));//https://docs.unity3d.com/ScriptReference/Mathf.Clamp.html
        }
        public static bool IntToBool(int number)
        {
            return number == 0 ? false : true;
        }
    }
}

