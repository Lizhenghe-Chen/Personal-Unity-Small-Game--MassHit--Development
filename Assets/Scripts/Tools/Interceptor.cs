using System.Collections;
using UnityEngine;

public class Interceptor : MonoBehaviour
{

    public Animator cannonAnimation;
    [SerializeField] private Transform PredictedObj, FirePoint;//assigin target 
    [SerializeField] private GameObject bullet;// bullet prefab
    [SerializeField] private ParticleSystem muzzleFlash;//muzzle flash
    [SerializeField] private float attackRange = 50, FiringRate = .3f, bulletSpeed = 10f, followSpeed = 1f, accurancy = 0.5f;
    [SerializeField] private string ratgetTag = "Respawn";

    [Header("Below For Debug Use:")]
    [SerializeField] private float flyingTime;
    [SerializeField] private float distance;
    [SerializeField] private Rigidbody TargetRig;
    [SerializeField] private Vector3 lastVelocity, acceleration;
    [SerializeField] private Transform TargetObj;
    private void OnValidate() { this.GetComponent<SphereCollider>().radius = attackRange; }
    // void Start()
    // {
    //     TargetRig = TargetObj.GetComponent<Rigidbody>();
    //     //StartCoroutine(Shoot());// start shooting
    // }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ratgetTag))// buckeyball is the tag of the player
        {
            TargetObj = other.gameObject.transform;
            TargetRig = TargetObj.GetComponent<Rigidbody>();
            StartCoroutine(Shoot());// start shooting
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(ratgetTag))
        {
            TargetObj = null;
            TargetRig = null;
            StopAllCoroutines();
        }
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        if (!TargetObj || Time.timeScale < 0.02f) { return; }
        acceleration = (TargetRig.velocity - lastVelocity) / Time.deltaTime;//acceleration
        lastVelocity = TargetRig.velocity;//get the current velocity for next frame acceleration caculation
        distance = Vector3.Distance(transform.position, TargetObj.position);//get the current distance(insufficiently strict)
        flyingTime = distance / bulletSpeed; //dt = Distance / vb

        PredictedObj.position = Vector3.Lerp(PredictedObj.position,//use Lerp to control cannon's follow speed
               (
                TargetObj.position // current position
                + TargetRig.velocity * flyingTime // next position with uniform linear motion: dL1 = L + dt * v
                + 0.5f * Mathf.Pow(flyingTime, 2f) * acceleration// then add distance with uniformly variable motiond during dt: dL2= dL1 + 1/2 * a * dt^2
                + 0.5f * Mathf.Pow(flyingTime, 2f) * -Physics.gravity// next add distance with gravity's uniformly variable motion during dt: dL3= dL2 + 1/2 * a * dt^2
                + new Vector3(Random.Range(-accurancy, accurancy), Random.Range(-accurancy, accurancy), Random.Range(-accurancy, accurancy))//finally add alittle random fractors to incrase hit rate chance 
                ),
                Time.deltaTime * followSpeed);
    }
    IEnumerator Shoot()
    {
        while (TargetObj)
        {
            //  for (int i = 0; i <= num_bullets_pertime; i++) { Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed * Random.Range(0.1f, 2f); }
            // Invoke("ShootWithMuzzleFlash", 2f);
            cannonAnimation.Play("Shoot", 0, 0);
            Debug.Log("Shoot");
            //   Instantiate(bullet, FirePoint.position, Quaternion.identity).GetComponent<Rigidbody>().velocity = FirePoint.forward * bulletSpeed;
            yield return new WaitForSeconds(FiringRate);

        }
    }
    void ShootBullet()
    {
        Instantiate(bullet, FirePoint.position, Quaternion.identity).GetComponent<Rigidbody>().velocity = FirePoint.forward * bulletSpeed;
    }
    void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

}
