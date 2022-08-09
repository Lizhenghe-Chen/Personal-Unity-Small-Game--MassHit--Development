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
        [SerializeField] CinemachineVirtualCamera vcam;
        [SerializeField] private Volume postProcessVolume;
        [SerializeField] private DepthOfField df;
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
            // SetTimeScale();
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
                Time.timeScale = 0.0001f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }

            GlobalRules.instance.isPause = menuPause;
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
}

