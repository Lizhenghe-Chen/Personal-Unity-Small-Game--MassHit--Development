using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestory : MonoBehaviour
{
    public float explodeRadius = 5f;// should be the sphere collider trigger's radius
    public GameObject ignoreCollisionObject;
    //public GameObject explosion;
    public float scaleOfTime = 0.01f;
    //   private SphereCollider damageRange;
    //void Awake() { explosion.SetActive(false); }
    void Start()
    {
        //ignore collision by layer
        Physics.IgnoreLayerCollision(GlobalRules.instance.bulletLayerID, GlobalRules.instance.playerLayerID);
        Destroy(this.gameObject, 10f);

    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("GravityCube")) { return; }
        other.gameObject.GetComponent<Rigidbody>().AddForce((other.transform.position - this.transform.position) * 1000);
        //  this.GetComponent<MeshRenderer>().enabled = false;
        Time.timeScale = scaleOfTime;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;


        Destroy(this.gameObject, 0.5f);
    }
    //private void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("Collision: " + other.gameObject.tag);

    //    // CharacterCtrl._CharacterCtrl.OnGravityCubeHitted(other.gameObject, CharacterCtrl._CharacterCtrl.hittedObjectMaterial);
    //}
    //void ChangeRadius()
    //{
    //    damageRange.radius = explodeRadius;
    //}


}
