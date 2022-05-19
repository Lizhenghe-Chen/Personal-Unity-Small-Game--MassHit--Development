using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCtrl : MonoBehaviour
{
    public static CharacterCtrl _CharacterCtrl;
    public Transform Camera;
    public Transform PlayerKernelTarget;
    public bool towardWithCamera = true;
    public float initial_torque, speedUp_torque, jumpForce, rushForce, sliteForce = 5f;
    public Material TramsparentMaterial;
    public GameObject PlayerKernel;
    public float PlayerKernelSpeed = 3f;
    public List<GameObject> HitObjects = new();
    public Queue<GameObject> HitObjectsQueue = new();
    public static bool isAming;
    [SerializeField] bool ableToJump = false;
    [SerializeField] List<GameObject> TransparentChangeList = new();
    [SerializeField] List<Material> OriginalMaterialList = new();
    Rigidbody rb; // player
    float horizontalInput, verticalInput;
    void Awake()
    {
        _CharacterCtrl = this;
    }
    void Start()
    {
        StartCoroutine(CheckDestory(100));
        rb = GetComponent<Rigidbody>();
        //PlayerCenterTarget = this.transform;
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
    public void OnGravityCubeHitted(GameObject other, Material hittedObjectMaterial)
    {
        if (other.CompareTag("GravityCube"))
        {
            other.GetComponent<MeshRenderer>().material = hittedObjectMaterial;
            other.GetComponent<Light>().enabled = true;
            other.GetComponent<Rigidbody>().useGravity = false;
            if (HitObjects.Contains(other)) { return; }
            HitObjects.Add(other);

        }
    }
    void TurningTorque()
    {
        rb.maxAngularVelocity = (Input.GetKey(KeyCode.LeftShift) ? speedUp_torque : initial_torque);
        if (towardWithCamera)
        {

            rb.AddTorque(rb.maxAngularVelocity * verticalInput * Camera.transform.right);       //foward and back, rotate around Camera's red axis
            rb.AddTorque(rb.maxAngularVelocity * horizontalInput * -Camera.transform.forward);  //left and right,rotate around Camera's blue axis
        }
        else
        {
            rb.AddTorque(rb.maxAngularVelocity * verticalInput * Vector3.right);
            rb.AddTorque(rb.maxAngularVelocity * horizontalInput * -Vector3.forward);
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.layer == 0) { ableToJump = true; }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 0) { ableToJump = false; }
    }
    void GiveForce()
    {
        var force = (Input.GetKey(KeyCode.LeftShift) ? sliteForce * 2 : sliteForce);
        rb.AddForce(force * verticalInput * Camera.transform.forward);
        rb.AddForce(force * horizontalInput * Camera.transform.right);
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
        if (Input.GetMouseButton(1)) { rb.angularVelocity = Vector3.zero; }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isAming = true;
            if (CenterRotate.is_Charging)
            {
                foreach (var item in TransparentChangeList)
                {
                    item.GetComponent<MeshRenderer>().material = TramsparentMaterial;
                }
            }

        }
        else
        {
            isAming = false;
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
        PlayerKernel.transform.position = Vector3.Lerp(PlayerKernel.transform.position, PlayerKernelTarget.position, PlayerKernelSpeed * Time.deltaTime);
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
        // Destroy(Test.Dequeue());
    }
}
