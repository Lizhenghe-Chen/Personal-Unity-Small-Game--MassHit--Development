public partial class CharacterCtrl
{
    private void AnimatorCtrl()
    {
        switch (currentOutLookState)
        {
            case OutLookState.NORMAL:
                PlayerAnimator.SetBool("ToNormal", true);
                PlayerAnimator.SetBool("ToPlane", false);
                PlayerAnimator.SetBool("ToDiamond", false);
                break;
            case OutLookState.DIAMOND:
                PlayerAnimator.SetBool("ToDiamond", true);
                PlayerAnimator.SetBool("ToPlane", false);
                PlayerAnimator.SetBool("ToNormal", false);
                break;
            case OutLookState.AIRCRAFT:
                PlayerAnimator.SetBool("ToPlane", true);
                PlayerAnimator.SetBool("ToDiamond", false);
                PlayerAnimator.SetBool("ToNormal", false);
                break;
        }
    }
    public void ResetAnimateParamater()
    {
        PlayerAnimator.SetBool("ToDiamond", false); PlayerAnimator.SetBool("ToNormal", false);
    }
    public void PlayMaskLeaveClip()
    {
        MaskAnimator.Play("Leave");
    }
}