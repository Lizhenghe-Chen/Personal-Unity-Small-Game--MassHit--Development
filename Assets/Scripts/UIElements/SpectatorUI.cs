using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
namespace UIElements
{
    public class SpectatorUI : MonoBehaviour
    {
        public bool menuPause;
        public Slider focusDistanceSlider, FieldOfViewSlider, focalLengthSlider, aptureSlider;
        public Transform MainCamera;
        Camera MainCameraForFieldOfView;
        private CinemachineBrain cameraBrain;
        [SerializeField] CinemachineVirtualCamera vcam;
        [SerializeField] CinemachinePOV POV;
        [SerializeField] GameObject playerVCam;
        [SerializeField] private Volume postProcessVolume;
        private DepthOfField df;

        void Start()
        {//modify the sensor size of the virtual camera to match the sensor size of the camera

            POV = vcam.GetComponentInChildren<CinemachinePOV>();
            postProcessVolume.sharedProfile.TryGet<DepthOfField>(out df);
            MainCameraForFieldOfView = MainCamera.GetComponent<Camera>();
            cameraBrain = MainCamera.GetComponent<CinemachineBrain>();
            SetApture();
            SetFocalLength();
            SetFocusDistance();
            SetFieldOfView();

        }

        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0) { vcam.m_Lens.FieldOfView = FieldOfViewSlider.value += Input.mouseScrollDelta.y; }
            if (Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(MainCameraForFieldOfView.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    focusDistanceSlider.value = df.focusDistance.value = hit.distance;
                }

            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Pressed P");
                menuPause = !menuPause;
                SetTimeScale();
            }
        }
        public void SetTimeScale()
        {
            if (menuPause)
            {
                Time.timeScale = 0;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }

            GlobalRules.instance.isPause = menuPause;
        }
        public void EnableCameraCtrol()
        {
            POV.m_VerticalAxis.m_MaxSpeed = POV.m_HorizontalAxis.m_MaxSpeed = 1;
        }
        public void DisableCameraCtrol()
        {
            POV.m_VerticalAxis.m_MaxSpeed = POV.m_HorizontalAxis.m_MaxSpeed = 0;
        }
        public void SetFocusDistance()
        {
            df.focusDistance.value = focusDistanceSlider.value;
        }
        public void SetFieldOfView()
        {
            vcam.m_Lens.FieldOfView = FieldOfViewSlider.value;
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
}

