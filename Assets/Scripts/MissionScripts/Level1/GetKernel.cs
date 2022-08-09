using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
namespace UIElements
{
    public class GetKernel : GlobalUIFunctions
    {
        public LocalizeStringEvent MissionTextEvent;
        public MissionTextCrtl missionTextCrtl;
        public MissionTextUpper_Lower CollectedKernelMission;
        //public int CollectedKernelMissionTextIndex, CollectedKernelMissionText_UpperLimit, CollectedKernelMissionText_LowerLimit;
        private void Awake()
        {
            PlayerBrain.shootEnergy = 0;
        }
        // Start is called before the first frame update
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "PlayerKernel")
            {
                collision.gameObject.GetComponent<GunScript>().enabled = true;
                this.GetComponent<CharacterCtrl>().SwitchParticleSystem.Play();
                this.GetComponent<CharacterCtrl>().flyAbility = true;
                missionTextCrtl.Mission_Text_Progress = CollectedKernelMission.MissionTextIndexTarget - 1;
                missionTextCrtl.Mission_Text_Progress_UpperLimit = CollectedKernelMission.UpperLimit;
                missionTextCrtl.Mission_Text_Progress_LowerLimit = CollectedKernelMission.LowerLimit;

                missionTextCrtl.NextMissionTextNow();

                Debug.Log("Kernel Collected");
            }

        }
    }
}

