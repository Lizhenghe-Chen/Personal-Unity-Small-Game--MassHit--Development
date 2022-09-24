using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bitgem.VFX.StylisedWater
{
    public class FloatOnWater : MonoBehaviour
    {
        public WaterVolumeHelper WaterVolumeHelper;
        public float flotage;
        public float globalGravity;
        public float radius;
        public float initialdrag;
        Rigidbody rb;
        float test;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            radius = GetComponent<SphereCollider>().radius;
            //  flotage = rb.mass ;
            globalGravity = -Physics.gravity.y;
            initialdrag = rb.drag;
        }

        // Update is called once per frame
        void Update()
        {
            // var instance = WaterVolumeHelper ? WaterVolumeHelper : WaterVolumeHelper.Instance;
            //     if (!instance)
            //     {
            //         return;
            //     } 
            //   test= instance.GetHeight(transform.position)??transform.position.y ;
            //   Vector3 floatTarget = new Vector3(transform.position.x, test, transform.position.z);
            //     if(transform.position.y-test>=0.2f){
            //        rb.AddForce(Vector3.up*globalGravity*flotage*(test-transform.position.y+radius));
            //     //  transform.position = Vector3.Lerp(transform.position, floatTarget, flotage * Time.deltaTime);
            //     }
            //     else{ rb.AddForce(Vector3.up*globalGravity*flotage);}
        }
        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == GlobalRules.instance.waterLayerID)
            {
                //Debug.Log("In Water" + other.gameObject.layer + " " + other.gameObject.name);
                rb.drag = 3;
                //rb.angularDrag = 2;
                WaterVolumeHelper = other.gameObject.GetComponent<WaterVolumeHelper>();
                if (!WaterVolumeHelper)
                {
                    return;
                }
                test = WaterVolumeHelper.GetHeight(transform.position) ?? transform.position.y;
                //  Vector3 floatTarget = new Vector3(transform.position.x, test, transform.position.z);
                if (transform.position.y < test && (test - transform.position.y) <= radius)
                {
                    // Debug.Log(transform.position.y - test);
                    rb.AddForce(flotage * globalGravity * Mathf.PI * Mathf.Pow(test - transform.position.y, 3) * Vector3.up);
                    //  transform.position = Vector3.Lerp(transform.position, floatTarget, flotage * Time.deltaTime);
                }
                else if (transform.position.y < test)
                {
                    rb.AddForce(flotage * globalGravity * Mathf.PI * Mathf.Pow(radius, 3) * Vector3.up);
                    // transform.position = Vector3.Lerp(transform.position, floatTarget, Time.deltaTime * 10);
                }

            }
        }
        void OnTriggerExit(Collider other)
        {
            // Debug.Log("Out Water");
            if (other.gameObject.layer == GlobalRules.instance.waterLayerID)
            {
                while (rb.drag != initialdrag) { rb.drag = initialdrag; }
            }
        }
    }
}
