using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{

    private AudioSource audioSource;
    //========== variables for the FFT analysis ==========
    [Header("Aduio Visualizer Settings")]
    public int num_Samples = 128;
    public int channels = 1;
    public int ignoreFirst_Samples = 8;
    public int ignoreFirst_SamplesMultiply = 10;
    public float[] averageSamples = new float[8];

    float[] emm = new float[2048];
    public int rangeA, rangeB;
    public float TEST;


    //============================================================================
    [Header("\n")]
    public float[] samples;


    public float BaseValue;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        samples = new float[num_Samples];
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();

    }
    private void FixedUpdate()
    {
        TEST = EMM();
    }

    void GetSpectrumAudioSource()    //get spectrum audio source
    {
        //get spectrum data
        audioSource.GetSpectrumData(samples, channels, FFTWindow.Blackman);
    }
    // void GetAverageSpectrum()
    // {
    //     int count = 0;
    //     for (int i = 0; i < 8; i++)
    //     {
    //         float average = 0f;
    //         int sampleCount = (int)Mathf.Pow(2, i) * 2;
    //         if (i == 7)
    //         {
    //             sampleCount += 2;
    //         }
    //         for (int j = 0; j < sampleCount; j++)
    //         {
    //             average += samples[j];
    //             count++;
    //         }
    //         average /= count;
    //         averageSamples[i] = average * 10;
    //     }
    // }
    float EMM()
    {
        audioSource.GetSpectrumData(emm, channels, FFTWindow.Blackman);
        float average = 0;
        int count = 0;
        for (int j = rangeA; j < rangeB; j++)
        {
            average += emm[j];
            count++;
        }
        return average /= count;
    }

    
}