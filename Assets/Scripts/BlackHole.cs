using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public int _Mass;
    public Transform PlayerCentertarget;

    void OnTriggerStay(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (!rigidbody) { return; }
        //var distace = Mathf.Pow(Vector3.Distance(transform.position, other.gameObject.transform.position), 2);
        float gravitation = -Physics.gravity.y * (rigidbody.mass * _Mass / Mathf.Pow(Vector3.Distance(transform.position, other.gameObject.transform.position), 2));
        other.gameObject.GetComponent<Rigidbody>().AddForce((this.transform.position - other.transform.position) * gravitation);

    }

    //private float originialSpeed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GravityCube")) { other.transform.Find("Particle").gameObject.SetActive(true); }
        if (other.CompareTag("Player"))
        {
            CharacterCtrl c_Ctrl = other.gameObject.GetComponent<CharacterCtrl>();
            PlayerCentertarget = c_Ctrl.PlayerKernelTarget;
            // PlayerCentertarget.transform.parent = this.transform;
            c_Ctrl.PlayerKernelTarget = this.transform;
            //originialSpeed=c_Ctrl.PlayerKernelSpeed;
            //c_Ctrl.PlayerKernelSpeed*=_Mass*0.1f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GravityCube")) { other.transform.Find("Particle").gameObject.SetActive(false); }
        if (other.CompareTag("Player"))
        {
            CharacterCtrl c_Ctrl = other.gameObject.GetComponent<CharacterCtrl>();
           // PlayerCentertarget = other.GetComponent<CharacterCtrl>().PlayerKernel;
            c_Ctrl.PlayerKernelTarget = PlayerCentertarget;
            //   PlayerCentertarget.transform.parent = other.transform;
            //c_Ctrl.PlayerKernelTarget = other.transform;
            //c_Ctrl.PlayerKernelSpeed = originialSpeed;
        }
    }

}
