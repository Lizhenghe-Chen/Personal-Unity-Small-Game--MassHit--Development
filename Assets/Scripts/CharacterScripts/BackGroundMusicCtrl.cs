using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackGroundMusicCtrl : MonoBehaviour
{
    public AudioSource unAvalibleSound;
    public List<AudioClip> MusicList = new();
    public bool autoPlay = true, randomPlay = false, useTargetSound = false;
    public AudioClip TargetSound;
    
    AudioSource audioSource; 
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (autoPlay) { LoadMusic(); }
        else if (randomPlay) { RandomPlay(); }

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
    public void PlayUnAvalibleSound()
    {
        unAvalibleSound.Play();
    }
    public void RandomPlay() {
        audioSource.clip = MusicList[Random.Range(0, MusicList.Count)];
        Invoke("Play", 1f);
    }
}
