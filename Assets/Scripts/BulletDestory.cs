using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestory : MonoBehaviour
{
    public float explodeRadius = 5f;
    public GameObject ignoreCollisionObject;
    public GameObject explosion;
    private SphereCollider damageRange;
    void Awake() { explosion.SetActive(false); }
    void Start()
    {

        Destroy(this.gameObject, 10f);
        damageRange = GetComponent<SphereCollider>();
        //Invoke("ChangeRadius", 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Trigger: " + other.gameObject.tag);
        if (!other.gameObject.CompareTag("GravityCube")) { return; }

        explosion.SetActive(true);
        //   if (damageRange.radius > explodeRadius) { damageRange.radius = explodeRadius; }
        // GetComponent<SphereCollider>().isTrigger = false;

        Destroy(this.gameObject, 2f);
    }
    private void OnCollisionEnter(Collision other)
    {


        // CharacterCtrl._CharacterCtrl.OnGravityCubeHitted(other.gameObject, CharacterCtrl._CharacterCtrl.hittedObjectMaterial);
    }
    void ChangeRadius()
    {
        damageRange.radius = explodeRadius;
    }


}
