using System.Collections;
using UnityEngine;

public class Interceptor : MonoBehaviour
{
    public Transform TargetObj, PredictedObj;//assigin target 
    public GameObject bullet;// bullet prefab
    public float FiringRate = .3f, bulletSpeed = 10f, followSpeed = 1f, accurancy = 0.5f;

    public float flyingTime, distance;
    [SerializeField] Rigidbody TargetRig;
    [SerializeField] Vector3 lastVelocity, acceleration;


    void Start()
    {
        TargetRig = TargetObj.GetComponent<Rigidbody>();
        StartCoroutine(Shoot());// start shooting
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
            Instantiate(bullet, transform.position, transform.rotation).GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            yield return new WaitForSeconds(FiringRate);

        }
    }
}
