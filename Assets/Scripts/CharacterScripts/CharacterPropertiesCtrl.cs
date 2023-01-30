using UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class CharacterCtrl
{
    private void PlayerInitialize()
    {
        if (SceneManager.GetActiveScene().name == GlobalUIFunctions.levelList.StartMenu.levelName) return;
        CheckPoint = new Vector3(PlayerPrefs.GetFloat("SavedCheckPoint_X"), PlayerPrefs.GetFloat("SavedCheckPoint_Y"), PlayerPrefs.GetFloat("SavedCheckPoint_Z"));
        PlayerPrefs.SetString("SavedCheckPointScene", SceneManager.GetActiveScene().name);//save player's current scene
        if (CheckPoint == Vector3.zero) { CheckPoint = this.transform.position; }
        else { this.transform.position = CheckPoint; }
    }
    public void OnBelowDeathAltitude()
    {
        if (transform.position.y < GlobalRules.instance.DeathAltitude)
        {
            CharacterCtrl._CharacterCtrl.MaskAnimator.Play("Enter", 0, 0);
            CharacterCtrl._CharacterCtrl.currentOutLookState = CharacterCtrl.OutLookState.NORMAL;
            Debug.LogWarning("Below Death Altitude");
            CharacterCtrl._CharacterCtrl.transform.position = CharacterCtrl._CharacterCtrl.CheckPoint;
            CharacterCtrl._CharacterCtrl.rb.velocity = Vector3.zero;
            ProceedPlayerHealth(false, 30);
            MaskAnimator.Play("Injured", 0, 0);
            // LoadScene(SceneManager.GetActiveScene().buildIndex);
            // this.transform.position = new Vector3(0, 5, 0);
        }
    }
    private void DamageEvent(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //            Debug.Log("Hit By Bullet" + other.relativeVelocity.magnitude);
            MaskAnimator.Play("Injured", 0, 0);
            ProceedPlayerHealth(false, other.relativeVelocity.magnitude * other.rigidbody.mass);

            //   if (PlayerHealth <= 0) OnHealthRunOut();
        }

    }
    void OnHealthRunOut()
    {
        MaskAnimator.Play("Enter", 0, 0);
        currentOutLookState = OutLookState.NORMAL;
        Debug.LogWarning("Player is dead");
        transform.position = CheckPoint;
        rb.velocity = Vector3.zero;
        SceneManager.LoadScene(GlobalUIFunctions.levelList.StartMenu.levelName);
        //  PlayerHealth = 100;
        // LoadScene(SceneManager.GetActiveScene().buildIndex);
        // this.transform.position = new Vector3(0, 5, 0);
    }
    /// <summary>
    /// This function is used to increase or decrease player's health
    /// </summary>
    /// <param name="isIncreasing">true: increase health; false: decrease health</param>
    /// <param name="value">the value of health</param>
    private void ProceedPlayerHealth(bool isIncreasing, float value)
    {
        if (isIncreasing)
        {
            PlayerHealth += value;
            if (PlayerHealth > 100) PlayerHealth = 100;
        }
        else
        {
            PlayerHealth -= value;
            if (PlayerHealth < 0) { PlayerHealth = 0; OnHealthRunOut(); }
        }
    }
    private void GetSpeed_Friction_Direction(Collision collision)
    {
        // PlayerSpeedDirection.forward = rb.velocity.normalized;
        //draw a line to show the direction of rigidbody
        PlayerRotationDirection = Vector3.Cross(rb.angularVelocity, Vector3.up).normalized;
        PlayerFrictionDirection = collision.relativeVelocity.normalized;
        // if PlayerRotationDirection and PlayerFrictionDirection are not in the same line, then the player is sliding
        // Debug.Log(Vector3.Dot(PlayerRotationDirection, PlayerFrictionDirection));
        var emission = frictionParticleSystem.emission;
        if (Vector3.Dot(PlayerRotationDirection, PlayerFrictionDirection) > -0.9f || (int)(transform.localScale.x * rb.angularVelocity.magnitude) != (int)(collision.relativeVelocity.magnitude))
        {
            emission.rateOverDistance = 6;
            frictionParticleSystem.transform.position = collision.GetContact(0).point;
        }

        else emission.rateOverDistance = 0;

        Debug.DrawLine(transform.position, transform.position + PlayerFrictionDirection * 10, Color.green);
        Debug.DrawLine(transform.position, transform.position + PlayerRotationDirection * 10, Color.red);
        //        Debug.Log((int)(transform.localScale.x * rb.angularVelocity.magnitude) + "," + (int)(collision.relativeVelocity.magnitude));
    }

    private void SetPlayerSkinStateByCollision(Collision other)
    {
        if (other.collider.name == "Diamond" && currentOutLookState != OutLookState.DIAMOND)
        {
            SavePriviousOutlookState();
            currentOutLookState = OutLookState.DIAMOND;
        }
        if (other.collider.name == "Normal" && currentOutLookState != OutLookState.NORMAL)
        {
            SavePriviousOutlookState();
            currentOutLookState = OutLookState.NORMAL;
        }
    }
    float doubleClickTimetThreshold = 0.5f, lastClickTime;
    private bool IsDoubleClick(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            isDoubleClick = ((Time.time - lastClickTime) <= doubleClickTimetThreshold);
            lastClickTime = Time.time;
        }
        else { isDoubleClick = false; }
        return isDoubleClick;
    }
    public void AircraftModeDetect()
    {
        if (PlayerBrain.shootEnergy <= 25)
        {
            currentOutLookState = OutLookState.NORMAL; return;
        }
        if ((IsDoubleClick(GlobalRules.instance.Jump) && !ableToJump))
        {
            SwitchAircraftMode();
            PlayerBrain.shootEnergy += 1;
        }
    }
    public void SwitchAircraftMode()
    {
        Debug.Log("Double Click, FlyModeSwitching");
        if (currentOutLookState != OutLookState.AIRCRAFT)
        {
            SavePriviousOutlookState();
            currentOutLookState = OutLookState.AIRCRAFT;
        }
        else { currentOutLookState = priviousOutlookState; }
        //PlayerAnimator.SetBool("ToPlane", !PlayerAnimator.GetBool("ToPlane"));
    }
    private void SavePriviousOutlookState()
    {
        //if (priviousOutlookState == currentOutLookState) { return; }
        priviousOutlookState = currentOutLookState;
    }
    public void ChangeMaterialsToTransparent()
    {
        playerActionState = ActionState.AIMING;

        // Material[] newMaterials = new Material[] { TransparentMaterial };

        if (PlayerBrain.is_Charging)
        {
            playerMeshRenderer = GetComponent<MeshRenderer>();

            playerMeshRenderer.materials = new Material[] { TransparentMaterial };

            //for (int i = 0; i < playerMeshRenderer.materials.Length; i++)
            //{
            //    OriginalMaterialList.Add(playerMeshRenderer.materials[i]);
            //    playerMeshRenderer.materials[i] = new Material[] { TransparentMaterial };
            //}
        }
    }

    public void PlayerParticle()//play the particle cover
    {
        SwitchParticleSystem.Stop();
        SwitchParticleSystem.Play();
        //OriginalMaterialList.Clear();
        //foreach (var item in playerMeshRenderer.materials)
        //{
        //    OriginalMaterialList.Add(item);
        //}
    }
    public void SetPlayerSkin(int targetSkinIndex)
    {
        Debug.Log("SetPlayerSkin");
        for (int i = 0; i < playerSkinList.Count; i++)
        {
            if (i == targetSkinIndex)
            {
                playerSkinList[i].SetActive(true);
            }
            else { playerSkinList[i].SetActive(false); }

        }
    }
}