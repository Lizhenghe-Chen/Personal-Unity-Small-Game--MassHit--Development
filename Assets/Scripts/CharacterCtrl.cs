using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCtrl : MonoBehaviour
{
    public static CharacterCtrl _CharacterCtrl;
    public Transform Camera;
    public bool towardWithCamera = true;
    public float initial_torque, speedUp_torque, jumpForce, rushForce, sliteForce = 5f;
    private const float torque_multiplier = 3;
    float horizontalInput, verticalInput;
    public bool ableToJump = false;
    public GameObject PlayerKernel;
    [HideInInspector] public Transform PlayerCenterTarget;
    // public Transform playerAim;
    public float PlayerKernelSpeed = 3f;
    public Material hittedObjectMaterial;
    public List<GameObject> HitObjects = new List<GameObject>();
    public Material TramsparentMaterial;
    public List<GameObject> TransparentChangeList = new List<GameObject>();
    public List<Material> OriginalMaterialList = new List<Material>();
    Rigidbody rb; // player
    void Awake()
    {
        _CharacterCtrl = this;
    }
    void Start()
    {
        StartCoroutine(CheckDestory(100));
        rb = GetComponent<Rigidbody>();
        PlayerCenterTarget = this.transform;
        PlayerKernel.transform.parent = this.transform.parent.parent;
        foreach (var item in TransparentChangeList)
        {
            OriginalMaterialList.Add(item.GetComponent<Renderer>().material);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ONBelowDeathAltitude();
        TurningTorque();
        GiveForce();
        MovePlayerKernel();
        Break();
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        JumpCommand(); RushCommand();
        DestroyCommand();
    }
    private void OnCollisionEnter(Collision other)
    {
        OnGravityCubeHitted(other.gameObject, hittedObjectMaterial);
    }
    public void OnGravityCubeHitted(GameObject other, Material hittedObjectMaterial)
    {
        if (other.tag == "GravityCube")
        {
            other.GetComponent<MeshRenderer>().material = hittedObjectMaterial;
            other.GetComponent<Light>().enabled = true;
            other.GetComponent<Rigidbody>().useGravity = false;
            if (HitObjects.Contains(other.gameObject)) { return; }
            HitObjects.Add(other.gameObject);
        }
    }
    void TurningTorque()
    {
        // angularVelocity = rb.angularVelocity.magnitude;


        rb.maxAngularVelocity = (Input.GetKey(KeyCode.LeftShift) ? speedUp_torque : initial_torque);
        if (towardWithCamera)
        {
            rb.AddTorque(Camera.transform.right * rb.maxAngularVelocity * verticalInput);
            rb.AddTorque(-Camera.transform.forward * rb.maxAngularVelocity * horizontalInput);
        }
        else
        {
            rb.AddTorque(Vector3.zero * rb.maxAngularVelocity * verticalInput);
            rb.AddTorque(Vector3.zero * rb.maxAngularVelocity * horizontalInput);
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.name == "Terrain") { ableToJump = true; }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "Terrain") { ableToJump = false; }
    }
    void GiveForce()
    {
        var force = (Input.GetKey(KeyCode.LeftShift) ? sliteForce * 2 : sliteForce);
        rb.AddForce(Camera.transform.forward * force * verticalInput);
        rb.AddForce(Camera.transform.right * force * horizontalInput);
    }
    void JumpCommand()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!ableToJump) { return; }

            rb.AddForce(Vector3.up * jumpForce);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rb.useGravity = false;
        }
        else { rb.useGravity = true; }
    }
    void RushCommand()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (towardWithCamera)
            { rb.AddForce(Camera.transform.forward * rushForce); }
            else
            {
                rb.AddForce(Vector3.zero * rushForce);
            }
        }
    }
    void DestroyCommand()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            MenualCheckDestory();
        }
    }

    void Break()
    {
        if (Input.GetKey(KeyCode.Tab) || Input.GetMouseButton(1))
        {
            rb.angularVelocity = Vector3.zero;
            foreach (var item in TransparentChangeList)
            {
                item.GetComponent<MeshRenderer>().material = TramsparentMaterial;
            }
        }
        else { 
            foreach (var item in TransparentChangeList)
            {
                item.GetComponent<MeshRenderer>().material = OriginalMaterialList[TransparentChangeList.IndexOf(item)];
            }
        }
    }
    void ONBelowDeathAltitude()
    {
        if (transform.position.y < -10)
        {
            LoadScene(0);
        }
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    void MovePlayerKernel()
    {
        PlayerKernel.transform.position = Vector3.Lerp(PlayerKernel.transform.position, PlayerCenterTarget.position, PlayerKernelSpeed * Time.deltaTime);
    }
    IEnumerator CheckDestory(int distance)
    {
        while (true)
        {
            for (int i = 0; i < HitObjects.Count; i++)
            {
                if (HitObjects[i] == null) { HitObjects.Remove(HitObjects[i]); continue; }
                if (Vector3.Distance(HitObjects[i].transform.position, this.transform.position) > distance)
                {
                    Destroy(HitObjects[i]);
                    HitObjects.Remove(HitObjects[i]);
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }
    void MenualCheckDestory()
    {
        // for (int i = 0; i < HitObjects.Count; i++)
        // {
        //     // if (HitObjects[i] == null) { HitObjects.Remove(HitObjects[i]); continue; }

        //     Destroy(HitObjects[i]);
        //     HitObjects.Remove(HitObjects[i]);

        // }
        foreach (var item in HitObjects)
        {
            Destroy(item);
        }
        HitObjects.Clear();
    }
}
