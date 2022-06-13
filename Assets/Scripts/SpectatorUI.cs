using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class SpectatorUI : MonoBehaviour
{
    public static bool isPause = false;
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private DepthOfField df;
    public Slider focusDistanceSlider, FieldOfViewSlider, focalLengthSlider, aptureSlider;
    public Transform MainCamera;
    void Start()
    {
        postProcessVolume.sharedProfile.TryGet<DepthOfField>(out df);

        df.aperture.value = aptureSlider.value;
        df.focalLength.value = focalLengthSlider.value;
        df.focusDistance.value = focusDistanceSlider.value;
        FieldOfViewSlider.value = vcam.m_Lens.FieldOfView;

    }
    private void OnEnable()
    {
        // FieldOfViewSlider.value = MainCamera.GetComponent<Camera>().fieldOfView = 35f;
    }

    public void SetFocusDistance()
    {
        df.focusDistance.value = focusDistanceSlider.value;
    }
    public void SetFieldOfView()
    {
        vcam.m_Lens.FieldOfView = FieldOfViewSlider.value;
        MainCamera.GetComponent<Camera>().fieldOfView = FieldOfViewSlider.value;

    }
    public void SetFocalLength()
    {
        df.focalLength.value = focalLengthSlider.value;
    }
    public void SetApture()
    {
        df.aperture.value = aptureSlider.value;
    }
}
