using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDetector : MonoBehaviour
{

    public GameObject cubePrefab;
    private float beatTimer = 0f; // Timer for beat detection
    [SerializeField] float[] spectrum;
    [SerializeField] Transform visualizerParent;
    [SerializeField] Animator beatAnimator;
    [SerializeField] Transform[] cubeLights;
    [Serializable]
    public struct VisualizeSettings
    {
        public string musicName;
        public AudioClip musicClip;
        public int startSampleIndex; public int endSampleIndex;
        public float beatThreshold; // Threshold for beat detection
        public float beatYeild; // Delay between beat detections
        public float leadTime; // Time before the beat

    }
    [SerializeField] int visualizeSettingsIndex = 0;
    public VisualizeSettings[] visualizeSettings;
    private AudioSource audioSource;
    [SerializeField] Dictionary<int, Transform> cubeDict = new Dictionary<int, Transform>();
    private void OnValidate()
    {
        // audioSource = GetComponent<AudioSource>();

        // audioSource.clip = visualizeSettings[visualizeSettingsIndex].musicClip;
        // audioSource.Play();


    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = visualizeSettings[visualizeSettingsIndex].musicClip;
        audioSource.Play();
        InstantiateCubes();

    }

    private void InstantiateCubes()
    {
        // Instantiate spectrum size of cubes in a line
        for (int i = 0; i < spectrum.Length; i++)
        {
            GameObject cube = Instantiate(cubePrefab, visualizerParent.position, Quaternion.identity, visualizerParent);
            cube.transform.localPosition = new Vector3(i * 0.05f, 0, 0);
            cube.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        }
    }

    [System.Obsolete]
    private void Update()
    {
        // Get the current audio spectrum from the audio source
        spectrum = AudioListener.GetSpectrumData(2048, 0, FFTWindow.BlackmanHarris);


        VisualizeCube();
        GetBeats();
    }
    void VisualizeCube()
    {
        for (int i = 0; i < spectrum.Length; i++)
        {
            spectrum[i] *= 10;
            // Get the current cube
            GameObject cube = visualizerParent.GetChild(i).gameObject;

            // Calculate the new cube scale
            Vector3 newScale = cube.transform.localScale;
            newScale.y = spectrum[i];

            // Apply the new scale to the cube, use lerp for smoothing
            cube.transform.localScale = Vector3.Lerp(cube.transform.localScale, newScale, Time.deltaTime * smoothing);
        }
    }

    [SerializeField] float average = 0f;
     [SerializeField] float smoothing = 30f;
    float lastBeatTime = 0f;
    void GetBeats()
    {
        // Get the first 52 samples from the spectrum, then calculate the average amplitude
        // Calculate the average amplitude of the spectrum

        for (int i = visualizeSettings[visualizeSettingsIndex].startSampleIndex; i < visualizeSettings[visualizeSettingsIndex].endSampleIndex; i++)
        {
            average += spectrum[i];
        }
        average /= (visualizeSettings[visualizeSettingsIndex].endSampleIndex - visualizeSettings[visualizeSettingsIndex].startSampleIndex);

        // Check if the current amplitude exceeds the beat threshold
        if (average > visualizeSettings[visualizeSettingsIndex].beatThreshold)
        {
            // Check if the time since the last beat is greater than the beat yield
            if (Time.time - lastBeatTime > visualizeSettings[visualizeSettingsIndex].beatYeild)
            {
                // Set the last beat time to the current time, add lead time
                lastBeatTime = Time.time + visualizeSettings[visualizeSettingsIndex].leadTime;

                DoBeat();
            }
        }
    }
    void DoBeat()
    {
        // Trigger the beat event
        Debug.Log("Beat");
        beatAnimator.Play("BeatScale", 0, 0f);
        foreach (var item in cubeLights)
        {
            item.GetComponent<Animator>().Play("FlashLight", 0, 0f);
        }
    }

}
