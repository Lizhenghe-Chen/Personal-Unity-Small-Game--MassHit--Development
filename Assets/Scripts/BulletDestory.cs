using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestory : MonoBehaviour
{
    public float explodeRadius = 5f;
    public GameObject ignoreCollisionObject;
    public GameObject explosion;
    public float scaleOfTime = 0.5f;
    //   private SphereCollider damageRange;
    void Awake() { explosion.SetActive(false); }
    void Start()
    {
        //ignore collision by layer
        Physics.IgnoreLayerCollision(GlobalRules.instance.bulletLayerID, GlobalRules.instance.playerLayerID);
        Destroy(this.gameObject, 10f);
        //  damageRange = GetComponent<SphereCollider>();
        //Invoke("ChangeRadius", 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.gameObject.CompareTag("GravityCube")) { return; }
        this.GetComponent<MeshRenderer>().enabled = false;
        Time.timeScale = scaleOfTime;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        explosion.SetActive(true);
        //   if (damageRange.radius > explodeRadius) { damageRange.radius = explodeRadius; }
        // GetComponent<SphereCollider>().isTrigger = false;

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
