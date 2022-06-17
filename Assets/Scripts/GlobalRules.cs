using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
public class GlobalRules : MonoBehaviour
{
    public static GlobalRules instance;
    public int waterLayerID, playerLayerID, bulletLayerID, groundLayerID;
    public int DeathAltitude;
    public Menu escMenu;

    public List<Transform> checkParentLists = new();
    public WaitForSeconds waitTime = new(5);
    public KeyCode Break, Jump, SpeedUp, MoveUp, MoveDown, Rush, Aim, Shoot,
        HoldObject, ExtendHoldObjectDist, CloseHoldObjectDist, SwitchCamera, DestoryHittedObj;
    public float energyChargeSpeed, holdConsume, flyCosume;
    [Tooltip("CharacterCtrl.cs will allocate below camera")]
    public CinemachineFreeLook cam1;
    public CinemachineVirtualCamera cam2;
    readonly float recoverTimeSpeed = 2f;
    [SerializeField] Transform Player;
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        if (instance == null)
        {
            instance = this;
        }
        ReallocateCheckDestoryList();

    }
    void Start()
    {
        //Player = GameObject.Find("Player").transform;
        // cam1 = Player.GetComponent<CharacterCtrl>().Player_Camera1.GetComponent<CinemachineFreeLook>();
        // cam2 = Player.GetComponent<CharacterCtrl>().Player_Camera2.GetComponent<CinemachineVirtualCamera>();
        // Debug.Log(Player.GetComponent<CharacterCtrl>().Player_Camera1);
        try
        {
            StartCoroutine(CheckDestoryByDistanceFromPlayer(CharacterCtrl._CharacterCtrl.HitObjectsQueue));
        }
        catch (System.Exception e)//it may because at start menu
        {
            Debug.LogWarning(e);
        }
    }
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReallocateCheckDestoryList();


        Debug.Log("OnSceneLoaded: " + scene.name);
        // Debug.Log(mode);
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.fixedDeltaTime);
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        if (!escMenu.escCanvas.enabled && !SpectatorUI.isPause)
        {
            Time.timeScale += recoverTimeSpeed * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pressed P");
            SpectatorUI.isPause = !SpectatorUI.isPause;

        }



    }
    IEnumerator CheckDestoryByDistanceFromPlayer(Queue<GameObject> checkLists)
    {
        while (true)
        {
            //is not empty

            if (checkLists.Count > 0) { Destroy(checkLists.Dequeue()); }

            //foreach (GameObject child in checkLists)
            //{

            //    if (Vector3.Distance(child.transform.position, Player.position) > maxDistance)
            //    {
            //        Destroy(checkLists.Dequeue());

            //    }
            //}

            //for (int i = 0; i < ObjectList.Count; i++)
            //{
            //    if (ObjectList[i] == null) { ObjectList.Remove(ObjectList[i]); continue; }
            //    if (Vector3.Distance(ObjectList[i].transform.position, Player.position) > maxDistance)
            //    {
            //        Destroy(ObjectList[i]);
            //        ObjectList.Remove(ObjectList[i]);
            //    }
            //}
            // Debug.Log("CheckDestoryByDistanceFromPlayer");
            yield return waitTime;
        }
    }
    IEnumerator CheckDestoryByDeathAltitude(List<Transform> checkParentLists)
    {
        while (true)
        {
            foreach (Transform parent in checkParentLists)
            {
                foreach (Transform child in parent)
                {
                    if (child.transform.position.y <= DeathAltitude) { Destroy(child.gameObject); }
                }
            }


            yield return waitTime;
        }
    }
    public void FitCameraDirection(bool isCam1ToCam2)
    {
        var (A, B) = GetCamerasDetails();
        Debug.Log(A.m_XAxis.Value + ", " + B.m_XAxis.Value);
        if (isCam1ToCam2)
        {
            B.m_XAxis.Value = A.m_XAxis.Value;
        }
        else
        {
            A.m_XAxis.Value = B.m_XAxis.Value;
        }
        Debug.Log("->" + A.m_XAxis.Value + ", " + B.m_XAxis.Value);
    }
    public (CinemachineFreeLook, CinemachineOrbitalTransposer) GetCamerasDetails()
    {
        //cam1 = Player.GetComponent<CharacterCtrl>().Player_Camera1.GetComponent<CinemachineFreeLook>();
        //cam2 = Player.GetComponent<CharacterCtrl>().Player_Camera2.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();

        return (cam1, cam2.GetCinemachineComponent<CinemachineOrbitalTransposer>());
    }
    void ReallocateCheckDestoryList()
    {
        checkParentLists.Clear();
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("GravityCubeList"))
        {
            checkParentLists.Add(temp.transform);
        }
        if (checkParentLists.Count > 0)
        {
            try { StartCoroutine(CheckDestoryByDeathAltitude(checkParentLists)); }
            catch (System.Exception e) { Debug.Log(e.Message); }

        }
    }
}
