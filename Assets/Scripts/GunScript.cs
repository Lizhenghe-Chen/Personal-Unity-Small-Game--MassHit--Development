using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using ui
using UnityEngine.UI;
public class GunScript : MonoBehaviour
{
    [Header("Need Assign in Inspector")]
    public Transform Player;
    public Transform muzzle;
    public GameObject AimPoint;
    public CenterRotate BuckyBall;
    //================================================================
    [Header("\n")]
    public float cameraOffset;
    public Transform Camera;

    public float bulletSpeed;
    public GameObject bullet;


    [SerializeField] GameObject GreenAim, RedAim;
    void Start()
    {
        Physics.IgnoreLayerCollision(3, 3);
        Camera = Player.GetComponent<CharacterCtrl>().Camera;
        GreenAim = AimPoint.transform.Find("GreenAim").gameObject;
        RedAim = AimPoint.transform.Find("RedAim").gameObject;
        GreenAim.SetActive(false);
        RedAim.SetActive(false);
        // AimPoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null) { return; }
        Aim();
        // var position = new Vector3(Player.position.x + cameraOffset, Player.position.y, Player.position.z);

        // this.transform.position = Vector3.Lerp(this.transform.position, position, 1f * Time.deltaTime);
    }
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //  Instantiate(bullet, muzzle.position, muzzle.rotation).GetComponent<Rigidbody>().AddForce(muzzle.forward * bulletSpeed);
            var temp = Instantiate(bullet, muzzle.position, muzzle.rotation);
            Physics.IgnoreCollision(temp.GetComponent<Collider>(), Player.GetComponent<Collider>(),true);
            temp.GetComponent<Rigidbody>().velocity = muzzle.forward * bulletSpeed;
        }
    }
    void Aim()
    {
        // this.transform.forward = Camera.transform.forward;
        if (CharacterCtrl.isAming)
        {
            AimPoint.SetActive(true);

            if (CenterRotate.is_Charging)
            {
                //BuckyBall.selfRotateSpeed = 10f;
                GreenAim.SetActive(true);
                RedAim.SetActive(false);
                Shoot();
            }
            else
            {
                // BuckyBall.selfRotateSpeed = 1f;
                GreenAim.SetActive(false);
                RedAim.SetActive(true);
            }
        }
        else
        {
            // BuckyBall.selfRotateSpeed = 1f;
            AimPoint.SetActive(false);
        }

    }
}
