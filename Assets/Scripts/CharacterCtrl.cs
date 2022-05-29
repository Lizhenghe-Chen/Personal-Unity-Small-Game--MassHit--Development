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
    public GameObject Player_Camera1, Player_Camera2;

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
        //StartCoroutine(CheckDestory(100));
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
        ChangeCamera();
    }
    //public void OnGravityCubeHitted(GameObject other, Material hittedObjectMaterial)
    //{
    //    if (other.CompareTag("GravityCube"))
    //    {
    //        other.GetComponent<MeshRenderer>().material = hittedObjectMaterial;
    //        other.GetComponent<Light>().enabled = true;
    //        other.GetComponent<Rigidbody>().useGravity = false;
    //        if (HitObjects.Contains(other)) { return; }
    //        HitObjects.Add(other);

    //    }
    //}
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
                    item.GetComponent<Renderer>().material = TramsparentMaterial;
                }
            }

        }
        else
        {
            isAming = false;
            foreach (var item in TransparentChangeList)
            {
                item.GetComponent<Renderer>().material = OriginalMaterialList[TransparentChangeList.IndexOf(item)];
            }
        }
    }
    void ONBelowDeathAltitude()
    {
        if (transform.position.y < GlobalRules.instance.DeathAltitude)
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

    void MenualCheckDestory()
    {
        Destroy(HitObjectsQueue.Dequeue());

        //foreach (var item in HitObjects)
        //{
        //    Destroy(item);

        //}
        //HitObjects.Clear();
        // Destroy(Test.Dequeue());
    }
    private void ChangeCamera()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (Player_Camera1.activeSelf == true) { Player_Camera1.SetActive(false); Player_Camera2.SetActive(true); Camera = Player_Camera2.transform.Find("Main Camera").GetComponent<Transform>(); }
            else { Player_Camera1.SetActive(true); Player_Camera2.SetActive(false); Camera = Player_Camera1.transform.Find("Main Camera").GetComponent<Transform>(); }
        }

    }
}
