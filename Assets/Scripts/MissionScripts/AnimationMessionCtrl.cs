using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIElements
{
    public class AnimationMessionCtrl : GlobalUIFunctions
    {
        public Animator MessionCtrlAnimator;
        public int MessionCtrlAnimatorIndex;
        private void Start()
        {
            MessionCtrlAnimator = this.GetComponent<Animator>();
            MessionCtrlAnimator.SetInteger("MissionIndex", MessionCtrlAnimatorIndex);
        }

        public MissionTextCrtl missionTextCrtl;
        public void NextMissionText() { missionTextCrtl.NextMissionText(); }
        public void PreviousMissionText() { missionTextCrtl.PreviousMissionText(); }
        public void SetTotargetMissionText(int target) { missionTextCrtl.SetTotargetMissionText(target); }
        public void EnableAnimationCtrl() { MessionCtrlAnimator.enabled = true; }        public void DisableAnimationCtrl() { MessionCtrlAnimator.enabled = false; }
    }
}

