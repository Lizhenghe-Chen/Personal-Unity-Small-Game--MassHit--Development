using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRotate : MonoBehaviour
{

    [Tooltip("true if buckyball to rotate in Start Menu")] public bool inMenuRotate = false;
    public Transform Player;
    public Transform PlayerKernel;
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
        if (!inMenuRotate)
        {
            PlayerKernel = Player.GetComponent<CharacterCtrl>().PlayerKernel;
            chargingRange = GetComponent<SphereCollider>().radius - 0.1f;
        }
        shootEnergy = 100;
        StartCoroutine(GenerateRandomVector());
    }
    private void Update()
    {
        shootEnergy = Mathf.Clamp(shootEnergy, 0, 100);
        if (!(Input.GetKey(GlobalRules.instance.Aim) || Input.GetKey(GlobalRules.instance.HoldObject))) { BuckyBallAtoms.GetComponent<Renderer>().material = shootEnergy < 90 ? un_ChargedMaterial : chargedMaterial; }
        //   Debug.Log(shootEnergy);   
    }
    private void FixedUpdate()
    {
        if (inMenuRotate) { transform.rotation = Quaternion.Lerp(transform.rotation, randomTransform.rotation, selfRotateSpeed * Time.deltaTime); return; }
        //  transform.position = Player.position;
        //  Debug.Log(Vector3.Distance(transform.position, PlayerKernel.transform.position));
        if (Vector3.Distance(transform.position, PlayerKernel.position) <= chargingRange) { is_Charging = true; } else { is_Charging = false; }
        if (is_Charging)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, randomTransform.rotation, selfRotateSpeed * Time.deltaTime);
            //Debug.Log(shootEnergy);

            if (shootEnergy < 100)
            {
                shootEnergy += GlobalRules.instance.energyChargeSpeed * Time.deltaTime;

            }
        }
        if (CharacterCtrl.isAming)
        {
            transform.RotateAround(transform.position, PlayerKernel.forward, 500 * Time.deltaTime);
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
