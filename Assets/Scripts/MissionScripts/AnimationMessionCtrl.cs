using Cinemachine;
using UnityEngine;
namespace UIElements
{
    /* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class AnimationMessionCtrl : GlobalUIFunctions
    {
        public Animator MessionCtrlAnimator;
        public int MessionCtrlAnimatorIndex;
        [SerializeField] CinemachineFreeLook virtualCamera1;
        [SerializeField] Transform MainCamera, Player;
        [SerializeField] float disdance;

        private void Start()
        {
            //  Disable_VirtualCamera1_Input();
            MessionCtrlAnimator = this.GetComponent<Animator>();
            MessionCtrlAnimator.SetInteger("MissionIndex", MessionCtrlAnimatorIndex);
        }
        // private void OnValidate()
        // {
        //     disdance = Vector3.Distance(Player.position, MainCamera.transform.position);
        // }
        // private void Update()
        // {
        //     disdance = Vector3.Distance(Player.position, MainCamera.transform.position);
        //     if (disdance <= 3) { Enable_VirtualCamera1_Input(); this.enabled = false; }

        // }
        public MissionTextCrtl missionTextCrtl;
        public void NextMissionText() { missionTextCrtl.NextMissionText(); }
        public void PreviousMissionText() { missionTextCrtl.PreviousMissionText(); }
        public void SetTotargetMissionText(int target) { missionTextCrtl.SetTotargetMissionText(target); }
        //https://gamedev.stackexchange.com/questions/196915/what-does-write-defaults-in-the-animation-state-inspector-actually-do
        public void EnableAnimationCtrl() { MessionCtrlAnimator.enabled = true; }
        public void DisableAnimationCtrl() { MessionCtrlAnimator.enabled = false; }
        public void Disable_VirtualCamera1_Input() { virtualCamera1.m_XAxis.m_InputAxisName = ""; virtualCamera1.m_YAxis.m_InputAxisName = ""; }
        public void Enable_VirtualCamera1_Input()
        {
            virtualCamera1.m_XAxis.m_InputAxisName = "Mouse X"; virtualCamera1.m_YAxis.m_InputAxisName = "Mouse Y";
            CharacterCtrl._CharacterCtrl.PlayerParticle();
        }
    }
}

