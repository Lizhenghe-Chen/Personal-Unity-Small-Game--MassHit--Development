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
    public GameObject Player_Camera1, Player_Camera2;// cam1 is CinemachineFreeLook with Orbits, cam2 is CinemachineVirtualCamera with CinemachineTransposer
    public ParticleSystem landBendEffect;


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
        rb.maxAngularVelocity = (Input.GetKey(GlobalRules.instance.SpeedUp) ? speedUp_torque : initial_torque);
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
    private void OnCollisionEnter(Collision other)
    {
        if (landBendEffect) landBendEffect.Emit(1);
        if (other.gameObject.layer == GlobalRules.instance.groundLayerID) { ableToJump = true; }

    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == GlobalRules.instance.groundLayerID) { ableToJump = false; }
        if (other.gameObject.layer != GlobalRules.instance.groundLayerID) { isCliming = false; }
    }


    private void OnCollisionStay(Collision collision)
    {
        ClimbWall(collision);


    }
    float doubleClickTime = 1, lastClickTime; bool isDoubleClick, isCliming;
    private bool IsDoubleClick(KeyCode keyCode)
    {
        if (Input.GetKeyDown(keyCode))
        {
            isDoubleClick = ((Time.time - lastClickTime) <= doubleClickTime);
            lastClickTime = Time.time;
        }
        else { isDoubleClick = false; }

        return isDoubleClick;
    }
    private void ClimbWall(Collision collision)
    {
        if (collision.gameObject.layer == GlobalRules.instance.groundLayerID) { isCliming = false; return; }

        if (Input.GetKeyUp(GlobalRules.instance.Jump))
        {
            isCliming = false;
            rb.AddForce((transform.position - collision.GetContact(0).point).normalized * jumpForce);

        }
        else if (Input.GetKey(GlobalRules.instance.Jump) && CenterRotate.shootEnergy > 0)
        {
            isCliming = true;
            CenterRotate.shootEnergy -= Time.deltaTime * GlobalRules.instance.holdConsume;
            rb.AddForce((collision.GetContact(0).point - transform.position).normalized * -Physics.gravity.y);
            rb.AddForce(-Physics.gravity);// print("First point that collided: " + collision.contacts[0].point);
        }
    }
    void GiveForce()
    {
        var force = (Input.GetKey(GlobalRules.instance.SpeedUp) ? sliteForce * 2 : sliteForce);
        rb.AddForce(force * verticalInput * Camera.transform.forward);
        rb.AddForce(force * horizontalInput * Camera.transform.right);
    }
    void JumpCommand()
    {
        if (Input.GetKeyDown(GlobalRules.instance.Jump))
        {
            if (ableToJump)
            {
                Debug.Log("Jump");
                rb.AddForce(Vector3.up * jumpForce);
            }

        }
        else if (Input.GetKey(GlobalRules.instance.Jump) && !isCliming)
        {
            GiveForce();
            if (CenterRotate.shootEnergy > 0)
            {
                rb.useGravity = false;
                CenterRotate.shootEnergy -= Time.deltaTime * GlobalRules.instance.flyCosume;

                Debug.Log("Fly");
            }

        }
        else { rb.useGravity = true; }
    }
    void RushCommand()
    {
        if (Input.GetKeyDown(GlobalRules.instance.Rush))
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
        if (Input.GetKeyDown(GlobalRules.instance.DestoryHittedObj))
        {
            MenualCheckDestory();
        }
    }

    void Break()
    {
        if (Input.GetMouseButton(1)) { rb.angularVelocity = Vector3.zero; }
        if (Input.GetKey(GlobalRules.instance.Aim) || Input.GetKey(GlobalRules.instance.HoldObject))
        {
            ChangeMaterialsToTransparent();
        }
        else
        {
            ChangeMaterialsToOriginal();
        }
    }
    public void ChangeMaterialsToTransparent()
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
    public void ChangeMaterialsToOriginal()
    {
        isAming = false;
        foreach (var item in TransparentChangeList)
        {
            item.GetComponent<Renderer>().material = OriginalMaterialList[TransparentChangeList.IndexOf(item)];
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
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
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
    private void ChangeCamera()//cam1 is CinemachineFreeLook, cam2 is CinemachineTransposer
    {
        if (Input.GetKeyDown(GlobalRules.instance.SwitchCamera))
        {
            if (Player_Camera1.activeSelf == true)//cam1 to cam2
            {
                Player_Camera2.SetActive(true);
                Camera = Player_Camera2.transform.parent.Find("Main Camera").GetComponent<Transform>();
                GlobalRules.instance.FitCameraDirection(true);
                Player_Camera1.SetActive(false);

            }
            else//cam2 to cam1
            {
                Player_Camera1.SetActive(true);
                Camera = Player_Camera2.transform.parent.Find("Main Camera").GetComponent<Transform>();
                GlobalRules.instance.FitCameraDirection(false);
                Player_Camera2.SetActive(false);
            }
        }

    }
}
