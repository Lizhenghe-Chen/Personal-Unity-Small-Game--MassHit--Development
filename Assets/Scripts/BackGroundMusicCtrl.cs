using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackGroundMusicCtrl : MonoBehaviour
{
    public List<AudioClip> MusicList = new();
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadMusic();
    }
    void Play()
    {
        audioSource.Play();
    }
    public void LoadMusic()
    {

        audioSource.clip = MusicList[SceneManager.GetActiveScene().buildIndex - 1];
        //audioSource.time = 53f;
        Invoke("Play", 1f);
    }
}
