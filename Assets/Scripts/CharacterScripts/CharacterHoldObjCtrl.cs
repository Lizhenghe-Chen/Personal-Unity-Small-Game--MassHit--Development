using UnityEngine;
using System;
using UIElements;
using UnityEngine.UI;
using UnityEngine.Animations;

public partial class CharacterCtrl : MonoBehaviour
{
    [Space]
    [Header("***Below is for Hold Object***")]
    [Header("Need Assign in Inspector: ")]
    [SerializeField] Image holdAim;
    [SerializeField] Transform shootTraget;
    [SerializeField] Transform HoldTarget;
    [SerializeField] Range holdRange;
    [SerializeField] PositionConstraint shootTargetpositionConstraint;
    [SerializeField] Image targetSceenIcon;
    [SerializeField] particleAttractorLinear kernelParticle;
    public LayerMask HoldRaycastIgnore;
    public LayerMask ShootRaycastIgnore;
    [Space]
    [Header("Need Assign in Inspector: ")]
    private float originalDrag;
    private Transform originalKernelParticleTarget;
    [SerializeField] float holdDistance = 0;
    public bool isHoldKeyPressing;
    [SerializeField] private Rigidbody holdingObject;

    [Serializable]
    public struct Range
    {
        public float min;
        public float max;
    }
    private void LateUpdate()
    {
        // screenBound = new Vector2(Screen.width, Screen.height);
        if (!targetSceenIcon.gameObject.activeSelf) { return; }
        if (!shootTargetpositionConstraint.GetSource(0).sourceTransform) { targetSceenIcon.gameObject.SetActive(false); return; }
        GlobalUIFunctions.ObjectToScreenPosition(Camera, shootTraget, targetSceenIcon, 50, 50);
        // var screenPosition = Camera.WorldToScreenPoint(shootTraget.position);
        // Debug.Log(screenPosition);
        // //if (screenPosition.x <= 0 || screenPosition.x >= screenBound.x || screenPosition.y <= 0 || screenPosition.y >= screenBound.y) return;
        // //targetSceenIcon.transform.position = screenPosition;
        // if (screenPosition.z < 0)
        // {
        //     screenPosition.y = 0;
        //     screenPosition.x = -screenPosition.x;
        // }

        // targetSceenIcon.transform.position = new Vector2(Mathf.Clamp(screenPosition.x, 50, screenBound.x - 50), Mathf.Clamp(screenPosition.y, 50, screenBound.y - 50));
        //                                                                                                                                                                // Debug.Log(screenPosition);
    }
    public void HoldObjectCommand()
    {

        if (!CharacterCtrl._CharacterCtrl.catchObjAbility) { return; }
        // if (!holdingObject) { holdAim.enabled = false; return; }
        if (Input.GetKeyUp(GlobalRules.instance.HoldObject) || PlayerBrain.shootEnergy <= 0)//set object free~
        {
            holdAim.enabled = false;
            isHoldKeyPressing = holdAim.enabled;
            holdAim.transform.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

            if (holdingObject)
            {
                holdingObject.drag = originalDrag;//change back it's drag
                kernelParticle.target = originalKernelParticleTarget;
                holdingObject = null;
            }
            return;
        }
        else if (Input.GetKeyDown(GlobalRules.instance.HoldObject))//middle mouse button
        {

            holdAim.enabled = true;
            isHoldKeyPressing = holdAim.enabled;

        }
    }
    public void SpectatorHoldObjectCommand()
    {

        if (Input.GetKeyDown(GlobalRules.instance.HoldObject))
        {
            holdAim.enabled = true;
            isHoldKeyPressing = holdAim.enabled;

        }

        if (Input.GetKeyUp(GlobalRules.instance.HoldObject))
        {
            holdAim.enabled = false;
            isHoldKeyPressing = holdAim.enabled;

            if (holdingObject)
            {
                holdingObject.drag = originalDrag;//change back it's drag
                holdingObject = null;
            }

        }
    }


    public void HoldObject()
    {
        if (!isHoldKeyPressing) { return; }
        // Debug.Log(holdAim.rectTransform.position);
        LockTarget();
        // Does the ray intersect any objects excluding the player layer
        if (holdingObject == null && Physics.Raycast(Camera.ViewportPointToRay(new Vector3(
            holdAim.rectTransform.position.x / Screen.width,
            holdAim.rectTransform.position.y / Screen.height, 0)),
            out RaycastHit hit, Mathf.Infinity, ~HoldRaycastIgnore))
        {
            //draw ray from screen

            // Debug.DrawRay(Camera.transform.position, Camera.transform.forward * 1000, Color.green, 2);
            // Debug.Log(Vector3.Distance(Player.position, hit.transform.position));
            if (hit.rigidbody && Vector3.Distance(transform.position, hit.transform.position) <= holdRange.max)
            {
                holdAim.enabled = false;
                HoldTarget.position = hit.point;

                holdingObject = hit.collider.gameObject.GetComponent<Rigidbody>();

                originalDrag = holdingObject.drag;
                originalKernelParticleTarget = kernelParticle.target;

                holdingObject.drag = 5f;
                kernelParticle.target = holdingObject.transform;
                //holdingObject.isKinematic = false;
                // Debug.DrawRay(Camera.transform.position, Camera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                Debug.Log(hit.transform.tag);
            }
        }
        //else
        //{
        //    //  Debug.DrawRay(Camera.transform.position, Camera.transform.forward * 1000, Color.red, 2);
        //    //return;
        //}
        if (holdingObject)
        {
            HoldTarget.LookAt(Camera.transform);
            holdDistance = Vector3.Distance(transform.position, HoldTarget.position);
            if (Input.GetKey(GlobalRules.instance.ExtendHoldObjectDist) && (holdDistance <= holdRange.max)) { HoldTarget.position += 5f * Time.deltaTime * Camera.transform.forward.normalized; }
            else if (Input.GetKey(GlobalRules.instance.CloseHoldObjectDist) && (holdDistance >= holdRange.min)) { HoldTarget.position -= 5f * Time.deltaTime * Camera.transform.forward.normalized; }

            if (Input.GetKey(GlobalRules.instance.MoveUp) && (holdDistance <= holdRange.max)) { HoldTarget.position += 5f * Time.deltaTime * Vector3.up; }
            else if (Input.GetKey(GlobalRules.instance.MoveDown) && (HoldTarget.position.y - transform.position.y) > 0) { HoldTarget.position -= 5f * Time.deltaTime * Vector3.up; }

            PlayerBrain.shootEnergy -= Time.deltaTime * GlobalRules.instance.holdConsume;

            //HoldTarget.position = (Camera.forward.normalized * fowardback_holdoffset + Vector3.up * updown_holdOffset);
            holdingObject.AddForce(GlobalRules.instance.holdForce * (HoldTarget.position - holdingObject.position));
            //holdingObject.MovePosition((HoldTarget.position - holdingObject.position) * 50f * Time.deltaTime + holdingObject.position);

        }
    }
    void LockTarget()
    {
        if (holdingObject)
        {
            SetConstrantTarget(holdingObject.transform);
            //   Debug.LogWarning("holdingObject"); 
            return;
        }
        if (Physics.Raycast(Camera.ViewportPointToRay(new Vector3(holdAim.rectTransform.position.x / Screen.width, holdAim.rectTransform.position.y / Screen.height, 0)), out RaycastHit shoot_hit, Mathf.Infinity, ~HoldRaycastIgnore))
        {

            if (!shoot_hit.rigidbody)
            {
                AlignCameraView();
                return;
            }
            SetConstrantTarget(shoot_hit.transform);
            //shootTraget.GetComponent<PositionConstraint>().enabled = true;
            shootTraget.gameObject.SetActive(true);
            targetSceenIcon.gameObject.SetActive(true);

        }
        else
        {
            AlignCameraView();
        }
    }
    void AlignCameraView()
    {
        shootTraget.position = Camera.transform.position + 1000 * Camera.transform.forward;
        targetSceenIcon.gameObject.SetActive(false);
        shootTraget.gameObject.SetActive(false);
    }
    void SetConstrantTarget(Transform target)
    {
        var constraintSource = new ConstraintSource
        {
            // shootTraget.position = shoot_hit.point;
            sourceTransform = target,
            weight = 1
        };
        shootTargetpositionConstraint.SetSource(0, constraintSource);
    }
    public void SpectatorHoldObject()
    {
        if (!isHoldKeyPressing) { return; }
        if (Input.GetKey(GlobalRules.instance.ExtendHoldObjectDist) && (holdDistance <= holdRange.max)) { holdDistance += Time.deltaTime * 5f; }
        else if (Input.GetKey(GlobalRules.instance.CloseHoldObjectDist) && (holdDistance >= holdRange.min)) { holdDistance -= Time.deltaTime * 5f; }
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.transform.position, Camera.transform.TransformDirection(Vector3.forward), out RaycastHit hit, holdRange.max, ~HoldRaycastIgnore))
        {
            if (hit.rigidbody && holdingObject == null)
            {
                holdAim.enabled = false;
                HoldTarget.position = hit.point;
                HoldTarget.LookAt(Camera.transform);
                holdDistance = Vector3.Distance(Camera.transform.position, hit.point);
                holdingObject = hit.collider.gameObject.GetComponent<Rigidbody>();

                originalDrag = holdingObject.drag;
                holdingObject.drag = 5f;

            }
        }
        else
        {
            Debug.DrawRay(Camera.transform.position, Camera.transform.TransformDirection(Vector3.forward) * holdRange.max, Color.red);
        }
        if (holdingObject)
        {
            PlayerBrain.shootEnergy -= Time.deltaTime * GlobalRules.instance.holdConsume;
            //holdingObject.transform.position = Vector3.Lerp(holdingObject.transform.position, HoldTarget.position, Time.deltaTime * 1000f);
            HoldTarget.position = ((Camera.transform.forward).normalized * holdDistance + Camera.transform.position);
            holdingObject.AddForce(50f * holdingObject.mass * (HoldTarget.position - holdingObject.position));
            //holdingObject.MovePosition((HoldTarget.position - holdingObject.position) * 50f * Time.deltaTime + holdingObject.position);

        }
    }

























}