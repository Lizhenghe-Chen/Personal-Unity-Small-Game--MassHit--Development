using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundCtrl : MonoBehaviour
{
    public AudioSource buttonClickSound;
    // Start is called before the first frame update
    void Start()
    {
        LoadButtonClickSound();
    }
    public void PlayButtonSound() { buttonClickSound.Play(); }
    void LoadButtonClickSound()
    {
        try { buttonClickSound = GameObject.Find("buttonClickSound").GetComponent<AudioSource>(); }
        catch (System.Exception e)
        {
            Debug.LogWarning("failed to load button sound" + e);
            return;
        }

        buttonClickSound.transform.parent = null;
        DontDestroyOnLoad(buttonClickSound);
    }
}
