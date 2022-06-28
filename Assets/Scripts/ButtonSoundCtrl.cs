using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundCtrl : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound;
    public AudioClip Clickable, UnClickable;
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
