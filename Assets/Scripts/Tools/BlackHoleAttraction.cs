using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class BlackHoleAttraction : MonoBehaviour
{
    public int _Mass;
    public Transform PlayerCentertarget;

    void OnTriggerStay(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (!rigidbody || rigidbody.CompareTag("BlackHole") || rigidbody.gameObject.layer == 1) { return; }
        //var distace = Mathf.Pow(Vector3.Distance(transform.position, other.gameObject.transform.position), 2);
        float gravitation = -Physics.gravity.y * (rigidbody.mass * _Mass / Mathf.Pow(Vector3.Distance(transform.position, other.gameObject.transform.position), 2));
        other.gameObject.GetComponent<Rigidbody>().AddForce((this.transform.position - other.transform.position) * gravitation);

    }

    //private float originialSpeed;
    private void OnTriggerEnter(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (!rigidbody || rigidbody.CompareTag("BlackHole") || rigidbody.gameObject.layer == 1) { return; }
        if (other.CompareTag("GravityCube")) { other.transform.Find("Particle").gameObject.SetActive(true); }
        if (other.CompareTag("Player"))
        {
            PlayerCentertarget = CharacterCtrl._CharacterCtrl.gunScript.PlayerKernelTarget;
            // PlayerCentertarget.transform.parent = this.transform;
            CharacterCtrl._CharacterCtrl.gunScript.PlayerKernelTarget = this.transform;
            //originialSpeed=c_Ctrl.PlayerKernelSpeed;
            //c_Ctrl.PlayerKernelSpeed*=_Mass*0.1f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (!rigidbody || rigidbody.CompareTag("BlackHole") || rigidbody.gameObject.layer == 1) { return; }
        if (other.CompareTag("GravityCube")) { other.transform.Find("Particle").gameObject.SetActive(false); }
        if (other.CompareTag("Player"))
        {

            CharacterCtrl._CharacterCtrl.gunScript.PlayerKernelTarget = PlayerCentertarget;
            //   PlayerCentertarget.transform.parent = other.transform;
            //c_Ctrl.PlayerKernelTarget = other.transform;
            //c_Ctrl.PlayerKernelSpeed = originialSpeed;
        }
    }

}
