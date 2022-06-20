using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{

    public List<GameObject> MenuList = new();
    public GameObject StartMenu;
    public GameObject StartCharacterBundle;
    void Awake()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        OpenMenu(StartMenu);
    }

    // Update is called once per frame
    void Update()
    {

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
        Application.Quit();
    }
    public void LoadLevel(int leveID)
    {

        StartCoroutine(DelayLoadLevel(leveID));


    }
    public IEnumerator DelayLoadLevel(int leveID)
    {
        yield return new WaitForSecondsRealtime(1f);
        //   Destroy(StartCharacterBundle);
        SceneManager.LoadScene(leveID);
        GlobalRules.instance = null;

    }
}
