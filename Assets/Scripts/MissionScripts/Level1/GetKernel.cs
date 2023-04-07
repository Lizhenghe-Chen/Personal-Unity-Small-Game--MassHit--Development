using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
namespace UIElements
{
    public class GetKernel : GlobalUIFunctions
    {
        [Header("GetKernelMission, Neeed Assign:")]
        public LocalizeStringEvent MissionTextEvent;
        public MissionTextCrtl missionTextCrtl;
        public MissionTextUpper_Lower CollectedKernelMission;
        public GunScript GunScript;
        public CharacterCtrl CharacterCtrl;
        //public int CollectedKernelMissionTextIndex, CollectedKernelMissionText_UpperLimit, CollectedKernelMissionText_LowerLimit;
        private void Awake()
        {
           PlayerBrain.instance.shootEnergy = 0;

        }
        // Start is called before the first frame update
        private void OnTriggerEnter(Collider collision)
        {
            //Debug.Log(collision.name);
            if (collision.gameObject.name == "Player")
            {
                this.gameObject.layer = GlobalRules.instance.playerLayerID;

                GunScript.enabled = true;
                CharacterCtrl._CharacterCtrl.SwitchParticleSystem.Play();
                CharacterCtrl._CharacterCtrl.flyAbility = true;
                missionTextCrtl.Mission_Text_Progress = CollectedKernelMission.MissionTextIndexTarget - 1;
                missionTextCrtl.Mission_Text_Progress_UpperLimit = CollectedKernelMission.UpperLimit;
                missionTextCrtl.Mission_Text_Progress_LowerLimit = CollectedKernelMission.LowerLimit;

                missionTextCrtl.NextMissionTextNow();

                Debug.Log("Kernel Collected");
            }

        }
    }
}

