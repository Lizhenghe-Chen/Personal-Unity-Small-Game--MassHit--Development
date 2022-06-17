using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusicCtrl : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSource.time = 53f;
        Invoke("Play", 1f);


    }

    void Play()
    {
        audioSource.Play();
    }
}
