using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CharacterInputManager : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool jumpPressing;
    public bool sprint;
    public bool cursorLocked;

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());

    }
    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }
    public void OnRush(InputValue value)
    {
        RushInput(value.isPressed);
    }

#endif


    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
        jumpPressing = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        Debug.Log("Sprint");
        sprint = newSprintState;
    }
    public void RushInput(bool newRushState)
    {
        // Debug.Log("Rush");
        CharacterCtrl._CharacterCtrl.RushCommand();
    }

    
}
