using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRotate : MonoBehaviour
{
    public GameObject PlayerKernel;
    public static bool is_Charging;
    public float selfRotateSpeed = 1f;
    public Vector3 randomVector;
    public Transform randomTransform;
    // [SerializeField] SphereCollider myCollider;
    int min = -90, max = 90;
    private void Start()
    {
        // myCollider = GetComponent<SphereCollider>();
        PlayerKernel = this.transform.parent.GetComponent<CharacterCtrl>().PlayerKernel;
        StartCoroutine(GenerateRandomVector());
    }
    // private void Update()
    // {

    // }
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "PlayerKernel")
        {
            is_Charging = true;
            if (Input.GetMouseButton(1))
            {

                //   randomTransform.transform.forward = PlayerKernel.transform.forward;
                transform.RotateAround(transform.position, PlayerKernel.transform.forward, 100 * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, randomTransform.rotation, selfRotateSpeed * Time.deltaTime);
            }


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "PlayerKernel") { is_Charging = false; }
    }
    IEnumerator GenerateRandomVector()
    {
        while (true)
        {


            randomVector = new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
            if (randomVector.magnitude == 0) { yield return new WaitForSeconds(0); }
            randomTransform.transform.forward = randomVector;

            yield return new WaitForSeconds(3f);
        }
    }
}
