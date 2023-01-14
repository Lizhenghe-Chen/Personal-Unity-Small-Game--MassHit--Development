
using System.Collections;
using System.Collections.Generic;
using UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
public partial class CharacterCtrl : MonoBehaviour
{
    public static CharacterCtrl _CharacterCtrl;
    public enum OutLookState
    {
        NORMAL,
        DIAMOND,
        AIRCRAFT

    }
    public enum ActionState
    {
        IDLE,
        NORMALSPEED,
        HIGHSPEED,
        BREAKING,
        AIMING
    }
    [Range(0, 100)] public float PlayerHealth = 100f;
    public OutLookState currentOutLookState = OutLookState.NORMAL;
    [SerializeField] private OutLookState priviousOutlookState;
    public ActionState playerActionState = ActionState.IDLE;
    public LayerMask groundLayer;
    public Transform Camera;
    public Transform PlayerKernel;
    public Vector3 CheckPoint;
    public bool towardWithCamera = true, moveAbility = true, climbAbility = true, shootAbility = true,
    catchObjAbility = true, jumpAbility = true, rushAbility = true, flyAbility = true, AircraftAility = true, AudioTail = true;
    public float initial_torque, speedUp_torque, jumpForce, rushForce, sliteForce = 5f;
    public Material TransparentMaterial;
    public Animator MaskAnimator, PlayerAnimator;
    public List<GameObject> playerSkinList = new();
    public ParticleSystem SwitchParticleSystem;
    public GunScript gunScript;
    public GameObject Player_Camera1, Player_Camera2;// cam1 is CinemachineFreeLook with Orbits, cam2 is CinemachineVirtualCamera with CinemachineTransposer
    public ParticleSystem landBendEffect;
    public ParticleSystem frictionParticleSystem;

    public Vector3 PlayerRotationDirection, PlayerFrictionDirection;
    //public List<GameObject> HitObjects = new();
    public Queue<GameObject> HitObjectsQueue = new();
    [Header("For Status Debuging")]
    public bool isDoubleClick, isCliming, ableToJump = false;
    [SerializeField] MeshRenderer playerMeshRenderer;
    // [SerializeField] List<GameObject> TransparentChangeList = new();
    //[SerializeField] List<Material> OriginalMaterialList = new();
    public Rigidbody rb; // player
    float horizontalInput, verticalInput;
    private void Awake()
    {
        _CharacterCtrl = this;
        //foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Respawn"))
        //{
        //    if (temp != this.transform.parent.parent.gameObject) { Destroy(temp); }

        //}

        //if (GameObject.FindGameObjectsWithTag("Respawn"))
        //{

        //    Destroy(this.transform.parent.parent.gameObject);
        //}
        //  Debug.Log("Awake");

        //DontDestroyOnLoad(this.transform.parent.parent);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (MaskAnimator != null) { MaskAnimator.Play("Enter"); }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //GlobalRules.instance.cam1 = Player_Camera1.GetComponent<Cinemachine.CinemachineFreeLook>();
        //GlobalRules.instance.cam2 = Player_Camera2.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        PlayerKernel.parent = this.transform.parent.parent;
        gunScript = PlayerKernel.GetComponent<GunScript>();
        //gunScript.PlayerKernelTarget = this.gameObject.transform;

        StartCoroutine(AutoDestory());

        if (SceneManager.GetActiveScene().name == GlobalUIFunctions.levelList.StartMenu.levelName) return;
        CheckPoint = new Vector3(PlayerPrefs.GetFloat("SavedCheckPoint_X"), PlayerPrefs.GetFloat("SavedCheckPoint_Y"), PlayerPrefs.GetFloat("SavedCheckPoint_Z"));
        PlayerPrefs.SetString("SavedCheckPointScene", SceneManager.GetActiveScene().name);//save player's current scene
        if (CheckPoint == Vector3.zero) { CheckPoint = this.transform.position; }
        else { this.transform.position = CheckPoint; }
    }


    void Update()
    {
        AnimatorCtrl();
        if (AircraftAility) AircraftModeDetect();
        if (!moveAbility) { return; }
       // PlayerHealth = Mathf.Clamp(PlayerHealth, -0.1f, 100);
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        JumpCommand(); RushCommand();
        DestroyCommand();
        ChangeCamera();
        Break_Aim();

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

    private void OnCollisionEnter(Collision other)
    {
        DamageEvent(other);
        if (landBendEffect) landBendEffect.Emit(1);
        // if (currentOutLookState == OutLookState.AIRCRAFT) SwitchAircraftMode();
        SetPlayerSkinStateByCollision(other);
        if (other.collider.name == "FinnishPoint" || other.collider.name == "CheckPoint")
        {
            other.gameObject.GetComponent<CheckPoint>().enabled = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        ClimbWall(collision);

        GetSpeed_Friction_Direction(collision);

        if ((transform.position - collision.GetContact(0).point).normalized.y >= 0.2) { ableToJump = true; }
    }
    private void OnCollisionExit(Collision other)
    {
        isCliming = false;
        ableToJump = false;//no matter what, if player is not on an object, he can't jump
        //if layer is ground
        //if (GlobalRules.IsGameObjInLayerMask(other.gameObject, GlobalRules.instance.GoundLayer))
        //{
        //    ableToJump = false;
        //}
    }

    void DestroyCommand()
    {
        if (Input.GetKey(KeyCode.C))
        {
            MenualCheckDestory();
        }
    }

    void MenualCheckDestory()
    {
        if (HitObjectsQueue.Count > 0) { Destroy(HitObjectsQueue.Dequeue()); }
        //foreach (var item in HitObjects)
        //{
        //    Destroy(item);

        //}
        //HitObjects.Clear();
        // Destroy(Test.Dequeue());
    }
 
    // public IEnumerator DelayLoadLevel(int leveID)
    // {
    //     PlayMaskLeaveClip();
    //     yield return new WaitForSecondsRealtime(2f);
    //     SceneManager.LoadScene(leveID);
    // }
    // public IEnumerator DelayBackToStartMenu()
    // {
    //     PlayMaskLeaveClip();
    //     yield return new WaitForSecondsRealtime(2f);
    //     SceneManager.LoadScene(GlobalRules.instance.StartSceneName);
    //     Destroy(this.transform.parent.parent.gameObject);
    // }
    public IEnumerator AutoDestory()
    {
        while (true)
        {
            MenualCheckDestory();
            //Debug.Log("AutoDestory");
            yield return new WaitForSeconds(3f);
        }
    }
     

}
