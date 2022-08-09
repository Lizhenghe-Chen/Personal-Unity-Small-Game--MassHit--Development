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
    public PlayerBrain BuckyBall;
    //================================================================
    [Header("\n")]
    public Transform PlayerKernelTarget;
    public float PlayerKernelSpeed = 3f;
    public float cameraOffset;
    public Transform Camera;
    public bool isLookAt;

    public float bulletSpeed;
    public GameObject bullet;


    [SerializeField] GameObject GreenAim, RedAim;

    void OnEnable()
    {
        Physics.IgnoreLayerCollision(GlobalRules.instance.playerLayerID, GlobalRules.instance.playerLayerID);
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
    private void FixedUpdate()
    {
        MovePlayerKernel();
    }
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerBrain.shootEnergy -= 25;
            //  Instantiate(bullet, muzzle.position, muzzle.rotation).GetComponent<Rigidbody>().AddForce(muzzle.forward * bulletSpeed);
            var temp = Instantiate(bullet, transform.position, transform.rotation);
            Physics.IgnoreCollision(temp.GetComponent<Collider>(), Player.GetComponent<Collider>(), true);
            temp.GetComponent<Rigidbody>().velocity = -transform.forward * bulletSpeed;
        }
    }
    void Aim()
    {
        if (!CharacterCtrl._CharacterCtrl.shootAbility) { return; }
        // this.transform.forward = Camera.transform.forward;
        if (CharacterCtrl._CharacterCtrl.playerActionState == CharacterCtrl.ActionState.AIMING)
        {

            if (isLookAt) { transform.LookAt(Camera); } else { transform.forward = new(-Camera.forward.x, 0, -Camera.forward.z); }
            AimPoint.SetActive(true);

            if (PlayerBrain.is_Charging && PlayerBrain.shootEnergy > 25)
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
    void MovePlayerKernel()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, PlayerKernelTarget.position, PlayerKernelSpeed * Time.deltaTime);
    }
}
