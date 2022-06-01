using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UIManager : MonoBehaviour
{
    public Transform Player;
    public GameObject InGameUI;
    public Image KernelState;
    public Color chargedColor, unchargedColor;

    public particleAttractorLinear kernelParticle;
    public Image holdAim;
    public Transform aimRay, HoldTarget, MainCamera;
    public Range holdRange;

    [Serializable]
    public struct Range
    {
        public float min;
        public float max;
    }

    [SerializeField] Rigidbody holdingObject;
    [SerializeField] private float value = 0f;
    [SerializeField] private bool isHoldKeyPressing = false;
    readonly int IgnorLayer = 1 << 6;
    void Start()
    {
        holdAim.enabled = false;
        InGameMenu();//set menu off when start
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            InGameMenu();
        }
        HoldObjectCommand();

    }

    private void FixedUpdate()
    {
        UpdateKernelStateBar();
        HoldObject();
    }
    public void InGameMenu()
    {
        if (InGameUI.activeSelf)//if menu is active, switch it inactive
        {
            InGameUI.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            InGameUI.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private float originalDrag;
    private Transform originalKernelParticleTarget;

    public void HoldObjectCommand()
    {
        if (CenterRotate.shootEnergy <= 1) { return; }
        if (Input.GetKeyDown(GlobalRules.instance.HoldObject))
        {
            holdAim.enabled = true;
            isHoldKeyPressing = holdAim.enabled;

        }
        else if (Input.GetKeyUp(GlobalRules.instance.HoldObject) || CenterRotate.shootEnergy <= 2)
        {
            holdAim.enabled = false;
            isHoldKeyPressing = holdAim.enabled;

            if (holdingObject)
            {
                // holdingObject.isKinematic = false;
                holdingObject.drag = originalDrag;//change back it's drag
                kernelParticle.target = originalKernelParticleTarget;
                holdingObject = null;
            }

        }
    }
    [SerializeField] float holdDistance = 10;
    public void HoldObject()
    {
        if (!isHoldKeyPressing) { return; }
        if (Input.GetKey(GlobalRules.instance.ExtemdHoldObjectDist) && (holdDistance <= holdRange.max)) { holdDistance += Time.deltaTime * 5f; }
        else if (Input.GetKey(GlobalRules.instance.CloseHoldObjectDist) && (holdDistance >= holdRange.min)) { holdDistance -= Time.deltaTime * 5f; }
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(MainCamera.position, MainCamera.TransformDirection(Vector3.forward), out RaycastHit hit, holdRange.max, ~IgnorLayer))
        {
            if (hit.rigidbody && holdingObject == null)
            {
                HoldTarget.position = hit.point;
                HoldTarget.LookAt(MainCamera);
                holdDistance = Vector3.Distance(Player.position, hit.point);
                holdingObject = hit.collider.gameObject.GetComponent<Rigidbody>();

                originalDrag = holdingObject.drag;
                originalKernelParticleTarget = kernelParticle.target;

                holdingObject.drag = 5f;
                kernelParticle.target = holdingObject.transform;
                //holdingObject.isKinematic = true;
                // Debug.DrawRay(MainCamera.transform.position, MainCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                Debug.Log(hit.transform.tag);
            }
        }
        else
        {
            Debug.DrawRay(MainCamera.transform.position, MainCamera.transform.TransformDirection(Vector3.forward) * holdRange.max, Color.red);
        }
        if (holdingObject)
        {
            CenterRotate.shootEnergy -= Time.deltaTime * GlobalRules.instance.holdConsume;
            //holdingObject.transform.position = Vector3.Lerp(holdingObject.transform.position, HoldTarget.position, Time.deltaTime * 1000f);
            HoldTarget.position = Vector3.Lerp(HoldTarget.position, ((-HoldTarget.forward).normalized * holdDistance + Player.position), Time.deltaTime * 1000f);
            holdingObject.AddForce(50f * holdingObject.mass * Vector3.Distance(holdingObject.position, HoldTarget.position) * (HoldTarget.position - holdingObject.position));
            //holdingObject.MovePosition((HoldTarget.position - holdingObject.position) * 50f * Time.deltaTime + holdingObject.position);

        }

        //else
        //{
        //    Debug.DrawRay(aimRay.transform.position, aimRay.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //    Debug.Log("Did not Hit");
        //}


    }
    void UpdateKernelStateBar()
    {
        value = CenterRotate.shootEnergy / 100f;
        KernelState.color = (value >= 1) ? chargedColor : unchargedColor;
        KernelState.fillAmount = value;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
