using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlockCtrl : MonoBehaviour
{
    public GameObject BoolSign;
    public Material TrueMaterial, FalseMaterial;
    [Header("check and fill only one below:")]
    public bool isRaiseDoor;
    //public RaiseRange raiseRange;
    public Rigidbody controlTarget;
    public Transform raiseTargetStart, raiseTargetEnd;
    public float RaiseSpeed;

    //[System.Serializable]
    //public struct RaiseRange
    //{
    //    public float min;
    //    public float max;
    //}
    [Header("")]
    public bool SimpleSwitch;

    public bool handleSwitch;
    public HingeJoint hingeSwitch;

    [SerializeField] Rigidbody switchRig;
    private void Start()
    {
        switchRig = GetComponent<Rigidbody>();
    }

    [SerializeField] int maxOnetime = 0;
    private void Update()
    {

        // if (switchRig.isKinematic) { return; }
        // if (isRaiseDoor) { if (controlTarget.position != raiseTargetStart.position) { controlTarget.position = Vector3.MoveTowards(controlTarget.transform.position, raiseTargetStart.position, RaiseSpeed * Time.deltaTime); } }
        if (handleSwitch)
        {
            //Debug.Log(hingeSwitch.currentForce + ", " + hingeSwitch.currentTorque);
            // Debug.Log(hingeSwitch.limits.max);
            //if (hingeSwitch.currentForce == Vector3.zero) { return; }

            if (hingeSwitch.angle >= hingeSwitch.limits.max - 1 && (maxOnetime == 0 || maxOnetime == -1))
            {
                BoolSign.GetComponent<Renderer>().material = TrueMaterial;
                BoolSign.GetComponent<Light>().color = TrueMaterial.GetColor("_EmissionColor");

                JointSpring spring = hingeSwitch.spring;
                spring.targetPosition = hingeSwitch.limits.max;
                hingeSwitch.spring = spring;
                maxOnetime = 1;
                Debug.Log("Reach Max");

            }
            else
            if (hingeSwitch.angle <= hingeSwitch.limits.min + 1 && (maxOnetime == 0 || maxOnetime == 1))
            {
                BoolSign.GetComponent<Renderer>().material = FalseMaterial;
                BoolSign.GetComponent<Light>().color = FalseMaterial.GetColor("_EmissionColor");

                JointSpring spring = hingeSwitch.spring;
                spring.targetPosition = hingeSwitch.limits.min;
                hingeSwitch.spring = spring;
                maxOnetime = -1;
                Debug.Log("Reach Min");
            }


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Base")
        {
            BoolSign.GetComponent<Renderer>().material = FalseMaterial;
            BoolSign.GetComponent<Light>().color = FalseMaterial.GetColor("_EmissionColor");
            controlTarget.isKinematic = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Base")
        {
            // if (isRaiseDoor && SimpleSwitch) { Debug.LogWarning("Multiple effet applied!"); return; }
            if (isRaiseDoor)
            {
                BoolSign.GetComponent<Renderer>().material = TrueMaterial;
                BoolSign.GetComponent<Light>().color = TrueMaterial.GetColor("_EmissionColor");
                //  Debug.Log("RaiseDoor");
                controlTarget.isKinematic = true;
                controlTarget.position = Vector3.MoveTowards(controlTarget.position, raiseTargetEnd.position, RaiseSpeed * Time.deltaTime);
            }
        }
    }
    //private void OnCollisionStay(Collision collision)
    //{

    //    if (collision.collider.name == "Base")
    //    {
    //        if (isRaiseDoor && SimpleSwitch) { Debug.LogWarning("Multiple effet applied!"); return; }
    //        if (isRaiseDoor)
    //        {
    //            Debug.Log("RaiseDoor");
    //            controlTarget.AddForce(Vector3.up * RaiseSpeed);
    //        }
    //    }

    //}
}
