using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public int _Mass;
    public GameObject PlayerCentertarget;

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "GravityCube")
        {
            var obj = other.transform.Find("Particle").gameObject;
            if (!obj.activeSelf) { obj.SetActive(true); }
        }
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        var distace = Mathf.Pow(Vector3.Distance(transform.position, other.gameObject.transform.position), 2);
        if (rigidbody)
        {
            float gravitation = -Physics.gravity.y * (rigidbody.mass * _Mass / distace);
            other.gameObject.GetComponent<Rigidbody>().AddForce((this.transform.position - other.transform.position) * gravitation);
        }

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            CharacterCtrl c_Ctrl = other.gameObject.GetComponent<CharacterCtrl>();
            PlayerCentertarget = c_Ctrl.PlayerKernel;
            // PlayerCentertarget.transform.parent = this.transform;
            c_Ctrl.PlayerCenterTarget = this.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GravityCube") { other.transform.Find("Particle").gameObject.SetActive(false); }
        if (other.tag == "Player")
        {
            CharacterCtrl c_Ctrl = other.gameObject.GetComponent<CharacterCtrl>();
            PlayerCentertarget = other.GetComponent<CharacterCtrl>().PlayerKernel;
            //   PlayerCentertarget.transform.parent = other.transform;
            c_Ctrl.PlayerCenterTarget = other.transform;
        }
    }

}
