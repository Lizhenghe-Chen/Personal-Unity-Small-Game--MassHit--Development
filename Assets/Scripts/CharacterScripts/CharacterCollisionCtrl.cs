using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public partial class CharacterCtrl
{

    private void OnCollisionEnter(Collision other)
    {
        DamageEvent(other);//for bullet and other damageable objects
        if (landBendEffect) landBendEffect.Emit(1);
        // if (currentOutLookState == OutLookState.AIRCRAFT) SwitchAircraftMode();
        SetPlayerSkinStateByCollision(other);
        if (other.collider.name == "FinnishPoint" || other.collider.name == "CheckPoint")
        {
            other.gameObject.GetComponent<CheckPoint>().enabled = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        ClimbWall(collision);

        GetSpeed_Friction_Direction(collision);

        if ((transform.position - collision.GetContact(0).point).normalized.y >= 0.2) { ableToJump = true; }
    }
    private void OnCollisionExit(Collision other)
    {
        isCliming = false;
        ableToJump = false;//no matter what, if player is not on an object, he can't jump
        //if layer is ground
        //if (GlobalRules.IsGameObjInLayerMask(other.gameObject, GlobalRules.instance.GoundLayer))
        //{
        //    ableToJump = false;
        //}
    }
    //public void OnGravityCubeHitted(GameObject other, Material hittedObjectMaterial)
    //{
    //    if (other.CompareTag("GravityCube"))
    //    {
    //        other.GetComponent<MeshRenderer>().material = hittedObjectMaterial;
    //        other.GetComponent<Light>().enabled = true;
    //        other.GetComponent<Rigidbody>().useGravity = false;
    //        if (HitObjects.Contains(other)) { return; }
    //        HitObjects.Add(other);

    //    }
    //}

}