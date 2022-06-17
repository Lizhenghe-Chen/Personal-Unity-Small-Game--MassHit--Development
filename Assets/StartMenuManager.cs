using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{

    public List<GameObject> MenuList = new();
    public GameObject StartMenu;
    void Start()
    {
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
        SceneManager.LoadScene(leveID);
    }
}
