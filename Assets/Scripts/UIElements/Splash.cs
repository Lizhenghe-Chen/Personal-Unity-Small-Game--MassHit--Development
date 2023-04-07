using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace UIElements
{
    /* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class Splash : GlobalUIFunctions
    {
        public TMP_Text loadingText;
        public string playerName_;
        [SerializeField] private Volume postProcessVolume;
        public float focusDistance = 1f;

        private DepthOfField dof;
        private ChromaticAberration chromaticAberration;
        private void OnValidate()
        {
            postProcessVolume.profile.TryGet<DepthOfField>(out dof);
            postProcessVolume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
            dof.focusDistance.value = focusDistance;
            //Change the intensity of the chromatic aberration effect
            chromaticAberration.intensity.value = 1 - focusDistance;

        }
        private void Update()
        {
            if (dof.focusDistance.value != focusDistance)
            {
                dof.focusDistance.value = focusDistance;
                chromaticAberration.intensity.value = 1 - focusDistance;
            }

        }
        private void Awake()
        {
            playerName_ = PlayerPrefs.GetString("PlayerName");
            postProcessVolume.profile.TryGet<DepthOfField>(out dof);
        }
        public void UpdateText()
        {
            if (string.IsNullOrEmpty(playerName_) && loadingText) { loadingText.GetComponentInChildren<LocalizeStringEvent>().SetEntry("NullPlayerName"); }
            else { loadingText.GetComponentInChildren<LocalizeStringEvent>().SetEntry("PlayerWelecome"); }
        }
    }
}

