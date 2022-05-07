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
    private void Update()
    {
        // if (Vector3.Distance(PlayerKernel.transform.position, this.transform.position) <= myCollider.radius)
        // {
        //     // Debug.Log("PlayerKernel.transform.position == this.transform.position");
        //     transform.rotation = Quaternion.Lerp(this.transform.rotation, randomTransform.rotation, selfRotateSpeed * Time.deltaTime);
        //     // transform.RotateAround(this.transform.position, randomVector, selfRotateSpeed * Time.deltaTime);
        // }
        //if (is_Charging) { transform.rotation = Quaternion.Lerp(this.transform.rotation, randomTransform.rotation, selfRotateSpeed * Time.deltaTime); }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "PlayerKernel")
        {
            transform.rotation = Quaternion.Lerp(this.transform.rotation, randomTransform.rotation, selfRotateSpeed * Time.deltaTime);
            is_Charging = true;
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
            randomTransform.transform.Rotate(randomVector.x, randomVector.y, randomVector.z, Space.World);

            yield return new WaitForSeconds(3f);
        }
    }
}
