using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject InGameUI;
    public Image KernelState;
    public Color chargedColor, unchargedColor;
    private float value = 0f;
    void Start()
    {
        InGameMenu();//set menu off when start
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InGameMenu();
        }


    }
    private void FixedUpdate()
    {
        value = CenterRotate.shootEnergy / 100f;
        KernelState.color = (value >= 1) ? chargedColor : unchargedColor;

        KernelState.fillAmount = value;
    }
    public void InGameMenu()
    {
        if (InGameUI.activeSelf)//if menu is active, switch it inactive
        {
            InGameUI.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            InGameUI.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
