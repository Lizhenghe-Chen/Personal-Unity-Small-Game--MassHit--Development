using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestory : MonoBehaviour
{
    public float explodeRadius = 5f;// should be the sphere collider trigger's radius
    //public GameObject ignoreCollisionObject;
    //public GameObject explosion;
    public float scaleOfTime = 0.01f;
    public bool ignorePlayerColletion, isPlayerBullet;
    //   private SphereCollider damageRange;
    //void Awake() { explosion.SetActive(false); }
    void Awake()
    {
        if (ignorePlayerColletion)
        {//ignore collision by layer
            Physics.IgnoreLayerCollision(GlobalRules.instance.bulletLayerID, GlobalRules.instance.playerLayerID);
        }
        Destroy(this.gameObject, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("GravityCube") || !ignorePlayerColletion) { return; }
        other.gameObject.GetComponent<Rigidbody>().AddForce((other.transform.position - this.transform.position) * 1000);
        //  this.GetComponent<MeshRenderer>().enabled = false;
        Time.timeScale = scaleOfTime;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;



    }

    private void OnCollisionEnter(Collision other)
    {
        if (isPlayerBullet) { } else Destroy(this.gameObject, 0.5f);
        if (other.gameObject.name == "Target") { Debug.Log("hit"); }
    }



}
