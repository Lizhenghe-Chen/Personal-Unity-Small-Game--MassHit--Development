using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIElements
{
    public class AnimationMessionCtrl : GlobalUIFunctions
    {
        public MissionTextCrtl missionTextCrtl;
        public void NextMissionText() { missionTextCrtl.NextMissionText(); }
        public void PreviousMissionText() { missionTextCrtl.PreviousMissionText(); }
    }
}

