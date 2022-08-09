using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackGroundMusicCtrl : MonoBehaviour
{
    public List<AudioClip> MusicList = new();
    public bool autoPlay = true, useTargetSound = false;
    public AudioClip TargetSound;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (autoPlay) { LoadMusic(); }

    }
    public void Play()
    {
        audioSource.Play();
    }
    //private void Update()
    //{
    //    Debug.Log(audioSource.time + "|" + audioSource.clip.length);
    //}
    public void LoadMusic()
    {

        audioSource.clip = useTargetSound ? TargetSound : MusicList[SceneManager.GetActiveScene().buildIndex - 1];
        // audioSource.clip = MusicList[TargetSound];
        //audioSource.time = 53f;
        Invoke("Play", 1f);
    }
}
