using UnityEngine;

public partial class CharacterCtrl
{  // Update is called once per frame
    void FixedUpdate()
    {
        if (moveAbility) { TurningTorque(); }
        GiveForce();//swimming
    }
    /// <summary>
    ///This is the function that control the character's movement by adding torque to the rigidbody
    /// </summary>
    private void TurningTorque()
    {
        rb.maxAngularVelocity = _input.sprint ? speedUp_torque : initial_torque;
        if (towardWithCamera)
        {
            rb.AddTorque(rb.maxAngularVelocity * _input.move.y * Camera.transform.right);       //foward and back, rotate around Camera's red axis
            rb.AddTorque(rb.maxAngularVelocity * _input.move.x * -Camera.transform.forward);  //left and right,rotate around Camera's blue axis
        }
        else
        {
            rb.AddTorque(rb.maxAngularVelocity * _input.move.y * Vector3.right);
            rb.AddTorque(rb.maxAngularVelocity * _input.move.x * -Vector3.forward);
        }
        if (!ableToJump)//disable the particle system when the character is in the air
        {
            var emission = frictionParticleSystem.emission;
            emission.rateOverDistance = 0;
        }
        // rb.maxAngularVelocity = (Input.GetKey(GlobalRules.instance.SpeedUp) ? speedUp_torque : initial_torque);
        // if (towardWithCamera)
        // {

        //     rb.AddTorque(rb.maxAngularVelocity * verticalInput * Camera.transform.right);       //foward and back, rotate around Camera's red axis
        //     rb.AddTorque(rb.maxAngularVelocity * horizontalInput * -Camera.transform.forward);  //left and right,rotate around Camera's blue axis
        // }
        // else
        // {
        //     rb.AddTorque(rb.maxAngularVelocity * verticalInput * Vector3.right);
        //     rb.AddTorque(rb.maxAngularVelocity * horizontalInput * -Vector3.forward);
        // }
    }
    /// <summary>
    /// This is the function that control the character's movement by adding force to the rigidbody, it is used when the character is swimming or in the air
    /// </summary>
    private void GiveForce()
    {
        if (PlayerBrain.shootEnergy <= 0 || ableToJump) { return; }
        //  Debug.Log("GiveForce");
        //var force = (Input.GetKey(GlobalRules.instance.SpeedUp) ? sliteForce * 2 : sliteForce);
        // rb.AddForce(force * verticalInput * Camera.transform.forward);
        // rb.AddForce(force * horizontalInput * Camera.transform.right);

        var force = _input.sprint ? sliteForce * 2 : sliteForce;
        if (Input.GetKey(GlobalRules.instance.MoveUp))
        {
            rb.AddForce(Vector3.up);
        }
        if (Input.GetKey(GlobalRules.instance.MoveDown))
        {
            rb.AddForce(-Vector3.up);
        }

        //PlayerBrain.shootEnergy -= Time.deltaTime * GlobalRules.instance.holdConsume;
    }
    /// <summary>
    /// This is the function that control the character's movement by adding force to the rigidbody towards the contact point of the wall
    /// </summary>
    private void ClimbWall(Collision collision)
    {
        //if (collision.gameObject.layer == GlobalRules.instance.groundLayerID) { isCliming = false; return; }
        if (!climbAbility) { return; }
        if (Input.GetKey(GlobalRules.instance.Climb) && PlayerBrain.shootEnergy > 0)
        {
            if (Input.GetKeyDown(GlobalRules.instance.Jump))
            {
                rb.AddForce((transform.position - collision.GetContact(0).point).normalized * jumpForce);
                isCliming = false;
                return;
            }
            isCliming = true;
            PlayerBrain.shootEnergy -= Time.deltaTime * GlobalRules.instance.holdConsume;
            rb.AddForce(2 * -Physics.gravity.y * (collision.GetContact(0).point - transform.position).normalized);
            rb.AddForce(-Physics.gravity);// print("First point that collided: " + collision.contacts[0].point);
        }
        else { isCliming = false; }

    }
    public void JumpCommand()
    {
        if (!jumpAbility) { return; }
        // if (Input.GetKeyDown(GlobalRules.instance.Jump))
        // {
        //     if (ableToJump)
        //     {
        //         //  Debug.Log("Jump");
        //         rb.AddForce(Vector3.up * jumpForce);
        //     }
        // }
        if (_input.jump)
        {
            if (ableToJump)
            {
                //  Debug.Log("Jump");
                rb.AddForce(Vector3.up * jumpForce);
                _input.jump = false;
            }
        }

        // if ((Input.GetKey(GlobalRules.instance.Jump)))
          if (_input.jumpPressing)
        {
            if (PlayerBrain.shootEnergy < 1f)
            {
                rb.useGravity = true;
            }
            else { rb.useGravity = false; }

            PlayerBrain.shootEnergy -= Time.deltaTime * GlobalRules.instance.flyConsume;
            // Debug.Log("Fly");
        }
        else { rb.useGravity = true; }

    }
    public void RushCommand()
    {
        if (!rushAbility) { return; }
        // if (Input.GetKeyDown(GlobalRules.instance.Rush) && PlayerBrain.shootEnergy > 0)
        // {
        //     PlayerBrain.shootEnergy -= GlobalRules.instance.rushConsume;
        //     if (towardWithCamera)
        //     { rb.AddForce(Camera.transform.forward * rushForce); }
        //     else
        //     {
        //         rb.AddForce(Vector3.zero * rushForce);
        //     }
        // }
        if (PlayerBrain.shootEnergy > 0)
        {
            PlayerBrain.shootEnergy -= GlobalRules.instance.rushConsume;
            if (towardWithCamera)
            { rb.AddForce(Camera.transform.forward * rushForce); }
            else
            {
                rb.AddForce(Vector3.zero * rushForce);
            }
        }
    }
    void Break_Aim()
    {
        if (Input.GetKey(GlobalRules.instance.Break)) { rb.angularVelocity = Vector3.zero; }
        if (!shootAbility) { return; }
        if (Input.GetKey(GlobalRules.instance.HoldObject))
        {
            // Debug.Log("GetKeyDown");
            playerActionState = ActionState.AIMING;
            if (PlayerBrain.is_Charging) { PlayerAnimator.SetBool("isAiming", true); } else { PlayerAnimator.SetBool("isAiming", false); }

            //ChangeMaterialsToTransparent();
        }
        if (Input.GetKeyUp(GlobalRules.instance.PreShoot) || Input.GetKeyUp(GlobalRules.instance.HoldObject))
        {
            playerActionState = ActionState.IDLE;
            PlayerAnimator.SetBool("isAiming", false);
            //ChangeMaterialsToOriginal();
        }
    }


}