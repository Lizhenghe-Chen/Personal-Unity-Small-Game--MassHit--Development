using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{

    public List<GameObject> MenuList = new();
    public GameObject StartMenu;
    public GameObject StartCharacterBundle;
    public Animator MaskAnimator;
    void Start()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        // OpenMenu(StartMenu);
        Invoke("LateOpenMenu", 2f);
    }

    public void OpenMenu(GameObject targetMenu)
    {

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
        Invoke("LateQuitGame", 2f);
    }
    public void LoadLevel(string leveID)
    {
        if (MaskAnimator != null) { MaskAnimator.Play("Leave"); }
        StartCoroutine(DelayLoadLevel(leveID));


    }
    public IEnumerator DelayLoadLevel(string leveID)
    {
        yield return new WaitForSecondsRealtime(1f);
        //   Destroy(StartCharacterBundle);
        SceneManager.LoadScene(leveID);
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
}
