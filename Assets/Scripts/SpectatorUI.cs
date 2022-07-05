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
    Camera MainCameraForFieldOfView;
    void Start()
    {
        postProcessVolume.sharedProfile.TryGet<DepthOfField>(out df);
        MainCameraForFieldOfView = MainCamera.GetComponent<Camera>();
        SetApture();
        SetFocalLength();
        SetFocusDistance();
        SetFieldOfView();

    }
    private void OnEnable()
    {
        // FieldOfViewSlider.value = MainCamera.GetComponent<Camera>().fieldOfView = 35f;
    }
    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0) { vcam.m_Lens.FieldOfView = FieldOfViewSlider.value = MainCameraForFieldOfView.fieldOfView += Input.mouseScrollDelta.y; }
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(MainCameraForFieldOfView.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                focusDistanceSlider.value = df.focusDistance.value = hit.distance;
            }

        }
    }
    public void SetFocusDistance()
    {
        df.focusDistance.value = focusDistanceSlider.value;
    }
    public void SetFieldOfView()
    {
        vcam.m_Lens.FieldOfView = MainCameraForFieldOfView.fieldOfView = FieldOfViewSlider.value;
        // MainCameraForFieldOfView.fieldOfView = FieldOfViewSlider.value;

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
