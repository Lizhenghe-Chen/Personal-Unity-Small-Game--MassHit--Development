using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryGun : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TrajectoryProjection TrajectoryProjection;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 100f;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int maxPhysicsStepsPerFrame = 100;

    private GameObject tempBullet;
    private void OnValidate()
    {
        lineRenderer.positionCount = maxPhysicsStepsPerFrame;
    }
    void Start()
    {
        tempBullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
        tempBullet.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(tempBullet.gameObject, TrajectoryProjection.simulateScene);
        lineRenderer.positionCount = maxPhysicsStepsPerFrame;
        //Hide and center the mouse cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
            Fire(bullet, false);
            Destroy(bullet, 8f);
        }
        if (Input.GetMouseButton(1)) { SimulateTrajectory(tempBullet, transform.position); }
        RotateGunByMouse();
    }
    private void FixedUpdate()
    {

    }
    void Fire(GameObject bullet, bool isGhost)
    {
        bullet.GetComponent<Rigidbody>().velocity = this.transform.forward * bulletSpeed;
    }
    [SerializeField] float simulationTime = 0.02f;
    public void SimulateTrajectory(GameObject bullet, Vector3 position)
    {   //rest tempBullet all physics properties
        tempBullet.transform.position = this.transform.position;
        tempBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        tempBullet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        tempBullet.transform.rotation = Quaternion.identity;

        SceneManager.MoveGameObjectToScene(tempBullet.gameObject, TrajectoryProjection.simulateScene);
        Fire(tempBullet, true);
        for (int i = 0; i < maxPhysicsStepsPerFrame; i++)
        {
            TrajectoryProjection.physicsSimulateScene.Simulate(simulationTime);
            lineRenderer.SetPosition(i, tempBullet.transform.position);
        }
    }
    public void SimulateTrajectory_Heavy(GameObject bullet, Vector3 position)
    {
        tempBullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(tempBullet.gameObject, TrajectoryProjection.simulateScene);
        Fire(tempBullet, true);
        for (int i = 0; i < maxPhysicsStepsPerFrame; i++)
        {
            TrajectoryProjection.physicsSimulateScene.Simulate(Time.fixedDeltaTime);
            lineRenderer.SetPosition(i, tempBullet.transform.position);
        }
        Destroy(tempBullet.gameObject);
    }
    void RotateGunByMouse()
    {
        // Get the mouse input velues and convert them to angles
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        // Rotate the gun based on the mouse input
        transform.Rotate(new Vector3(-mouseY, mouseX, 0));

    }
}
