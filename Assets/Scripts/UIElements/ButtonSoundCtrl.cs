using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class ButtonSoundCtrl : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound;
    public AudioClip Hover, Clickable, UnClickable;
    // Start is called before the first frame update
    void Start()
    {
        LoadButtonClickSound();
    }
    public void PlayButtonSound()
    {
        if (this.GetComponent<Button>().interactable)
        {
            buttonSound.clip = Clickable;
        }
        else
        {
            buttonSound.clip = UnClickable;
        }
        buttonSound.Play();
    }
    public void PlayButtonHoverSound()
    {
        if (this.GetComponent<Button>().interactable)
        {
            buttonSound.clip = Hover;
        }
        else { buttonSound.clip = UnClickable; }

        buttonSound.Play();
    }
    void LoadButtonClickSound()
    {
        try { buttonSound = GameObject.Find("buttonClickSound").GetComponent<AudioSource>(); }
        catch (System.Exception e)
        {
            Debug.LogWarning("failed to load button sound" + e);
            return;
        }

        buttonSound.transform.parent = null;
        DontDestroyOnLoad(buttonSound);
    }
}
