using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestory : MonoBehaviour
{
    public float explodeRadius = 5f;
    void Start() { Destroy(this.gameObject, 10f); }
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Trigger: " + other.gameObject.tag);
        if (other.gameObject.tag == "BlackHole") { return; }
       // GetComponent<SphereCollider>().isTrigger = false;
        
        Destroy(this.gameObject, 2f);
    }
    private void OnCollisionEnter(Collision other)
    {GetComponent<SphereCollider>().radius = explodeRadius;
        CharacterCtrl._CharacterCtrl.OnGravityCubeHitted(other.gameObject, CharacterCtrl._CharacterCtrl.hittedObjectMaterial);
    }
}
