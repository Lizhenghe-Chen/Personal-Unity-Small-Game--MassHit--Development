using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestory : MonoBehaviour
{
    void Start() { Destroy(this.gameObject, 10f); }
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Trigger: " + other.gameObject.tag);
        if (other.gameObject.tag == "BlackHole") { return; }
        GetComponent<SphereCollider>().isTrigger = false;
        GetComponent<SphereCollider>().radius = 2;
        Destroy(this.gameObject, 2f);
    }
    private void OnCollisionEnter(Collision other)
    {
        CharacterCtrl._CharacterCtrl.OnGravityCubeHitted(other.gameObject, CharacterCtrl._CharacterCtrl.hittedObjectMaterial);
    }
}
