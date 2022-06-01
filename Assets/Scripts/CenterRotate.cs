using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRotate : MonoBehaviour
{
    public Transform Player;
    public GameObject PlayerKernel;
    public static bool is_Charging;
    public float selfRotateSpeed = 1f;
    public Vector3 randomVector;
    public Transform randomTransform;
    public float chargeSpeed = 0.01f;
    public static float shootEnergy = 0;

    [Header("charge actions:")]
    public GameObject BuckyBallAtoms;
    public Material chargedMaterial, un_ChargedMaterial;
    // [SerializeField] SphereCollider myCollider;
    private int min = -90, max = 90;
    float chargingRange;
    private void Start()
    {
        // myCollider = GetComponent<SphereCollider>();
        PlayerKernel = Player.GetComponent<CharacterCtrl>().PlayerKernel;
        chargingRange = GetComponent<SphereCollider>().radius - 0.1f;
        StartCoroutine(GenerateRandomVector());
    }
    private void Update()
    {
        //  transform.position = Player.position;
        //  Debug.Log(Vector3.Distance(transform.position, PlayerKernel.transform.position));
        if (Vector3.Distance(transform.position, PlayerKernel.transform.position) <= chargingRange) { is_Charging = true; } else { is_Charging = false; }
        if (is_Charging)
        {
            //Debug.Log(shootEnergy);
            Mathf.Clamp(shootEnergy, 0, 101);
            if (shootEnergy < 100)
            {
                shootEnergy += GlobalRules.instance.energyChargeSpeed * Time.deltaTime;

            }
            if (!(Input.GetKey(GlobalRules.instance.Aim) || Input.GetKey(GlobalRules.instance.HoldObject))) { BuckyBallAtoms.GetComponent<Renderer>().material = shootEnergy < 100 ? un_ChargedMaterial : chargedMaterial; }


            if (CharacterCtrl.isAming && shootEnergy >= 100)
            {
                transform.RotateAround(transform.position, PlayerKernel.transform.forward, 500 * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, randomTransform.rotation, selfRotateSpeed * Time.deltaTime);
            }
        }
    }
    // private void OnTriggerStay(Collider other)
    // {

    //     if (other.gameObject == PlayerKernel)
    //     {
    //         Debug.Log("Stay" + other.gameObject);
    //         is_Charging = true;
    //         if (Input.GetMouseButton(1))
    //         {

    //             //   randomTransform.transform.forward = PlayerKernel.transform.forward;
    //             transform.RotateAround(transform.position, PlayerKernel.transform.forward, 100 * Time.deltaTime);
    //         }
    //         else
    //         {
    //             transform.rotation = Quaternion.Lerp(transform.rotation, randomTransform.rotation, selfRotateSpeed * Time.deltaTime);
    //         }


    //     }
    // }
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.name == "PlayerKernel") { is_Charging = false; }
    // }
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
