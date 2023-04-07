
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UIElements
{
    public class UIManager : GlobalUIFunctions
    {
        public static UIManager _UIManager;
        public bool isPlayer = true;
        public Transform playerKernel;
        public Transform Player;
        public GameObject InGameUI;
        [Header("Show Power")]
        public Image KernelState;
        public Color chargedColor, unchargedColor;
        [Header("Show Health")]
        public Image HealthState;
        public Color GoodHealthColor, NormalHealthColor, BadHealthColor;
        [SerializeField] private float kernelValue, healthValue;

        private void OnValidate()
        {
          //  UpdateKernelStateBar();
        }
        void Start()
        {
            _UIManager = this;

        }
        // Update is called once per frame
        void Update()
        {
            if (isPlayer)
            {
                UpdateKernelStateBar(); UpdatePlayerHealthBar();
                //HoldObject();
            }
            // HoldObjectCommand();

            //Debug.Log(vcam.m_Lens.FieldOfView);
        }

        // private void FixedUpdate()
        // {
        //     //if (!postProcessVolume)
        //     //{
        //     //    postProcessVolume = transform.parent.Find("Global Volume").GetComponent<Volume>();
        //     //    postProcessVolume.sharedProfile.TryGet<DepthOfField>(out df);
        //     //    df.focusDistance.value = focusDistanceSlider.value;
        //     //    df.aperture.value = aptureSlider.value;
        //     //    df.focalLength.value = focalLengthSlider.value;
        //     //    Debug.Log("postProcessVolume not found");
        //     //}

        //     //else
        //     //{
        //     //    SpectatorHoldObjectCommand();
        //     //    SpectatorHoldObject();
        //     //}

        // }


        public void UpdateKernelStateBar()
        {
            KernelState.color = (UpdateImageFill(KernelState, PlayerBrain.instance.shootEnergy, 100) >= 0.3) ? chargedColor : unchargedColor;
        }
        public void UpdatePlayerHealthBar()
        {
            switch (GlobalUIFunctions.UpdateImageFill(HealthState, CharacterCtrl._CharacterCtrl.PlayerHealth, 100))
            {
                case >= .8f:
                    HealthState.color = GoodHealthColor;
                    break;
                case >= .25f and < .8f:
                    HealthState.color = NormalHealthColor;
                    break;
                case < .25f:
                    HealthState.color = BadHealthColor;
                    break;
            }

        }
    }
}
