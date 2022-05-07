using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Transform Player;
    public float cameraOffset;
    public Transform Camera;

    public float bulletSpeed;
    public GameObject bullet;
    public Transform muzzle;

    void Start()
    {
        Camera = Player.GetComponent<CharacterCtrl>().Camera;
    }

    // Update is called once per frame
    void Update()
    {
        if (CenterRotate.is_Charging) { Shoot(); }

        if (Player == null) { return; }
        var position = new Vector3(Player.position.x + cameraOffset, Player.position.y, Player.position.z);
        this.transform.forward = Camera.transform.forward;
        this.transform.position = Vector3.Lerp(this.transform.position, position, 1f * Time.deltaTime);
    }
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, muzzle.position, muzzle.rotation).GetComponent<Rigidbody>().AddForce(muzzle.forward * bulletSpeed);
        }
    }
}
