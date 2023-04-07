using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UIElements;
using UnityEngine.UI;

/* Copyright (c) [2023] [Lizhneghe.Chen https://github.com/Lizhenghe-Chen]
* Please do not use these code directly without permission.
*/
public class GlobalRules : MonoBehaviour
{
    public static GlobalRules instance;
    public bool normalTime = false;
    public int waterLayerID, playerLayerID, bulletLayerID, groundLayerID, IgnoreHoldObjectID;
    public string playerTagName;
    public LayerMask GoundLayer;
    public int DeathAltitude;
    public EscUI escMenu;
    [SerializeField] Image MaskImage;
    public bool isLoadingNextLevel;
    public LayerIgnoreCollision AudioCubeIgnoreCollision;
    public List<Transform> checkParentLists = new();
    public WaitForSeconds waitTime = new(5);
    public KeyCode Break, Jump, SpeedUp, MoveUp, MoveDown, Rush, PreShoot, Shoot,
        HoldObject, Climb, ExtendHoldObjectDist, CloseHoldObjectDist, SwitchCamera, DestoryHittedObj;
    public float energyChargeSpeed, holdConsume, holdForce, flyConsume, rushConsume;
    public string StartSceneName;
    [Tooltip("CharacterCtrl.cs will allocate below camera")]
    public CinemachineFreeLook cam1;
    public CinemachineVirtualCamera cam2;
    public float recoverTimeSpeed = 1f;
    [SerializeField] Transform Player;

    [System.Serializable]
    public struct LayerIgnoreCollision
    {
        public LayerMask layerIndex_self;
        public LayerMask layerIndex_other;
    }

    // Start is called before the first frame update
    void Awake()
    {
        //Time.timeScale = 1;
        //Time.fixedDeltaTime = 0.02f;
        if (instance == null)
        {
            instance = this;
        }
        Physics.IgnoreLayerCollision(GlobalRules.instance.playerLayerID, GlobalRules.instance.playerLayerID);
        try
        {
            MaskImage = GlobalUIFunctions.MaskAnimator.GetComponentInChildren<Image>();

        }
        catch (System.Exception)
        {
            //Debug.LogWarning(e);
            if (MaskImage == null) { MaskImage = GameObject.Find("Mask").GetComponentInChildren<Image>(); }
        }


    }
    void OnEnable()
    {

        //Debug.Log("OnEnable called");
        //if (SceneManager.GetActiveScene().buildIndex == 0) return;
        SceneManager.sceneLoaded += OnSceneLoaded;
        ReallocateCheckDestoryList();
    }
    void Start()
    {



        //Player = GameObject.Find("Player").transform;
        // cam1 = Player.GetComponent<CharacterCtrl>().Player_Camera1.GetComponent<CinemachineFreeLook>();
        // cam2 = Player.GetComponent<CharacterCtrl>().Player_Camera2.GetComponent<CinemachineVirtualCamera>();
        // Debug.Log(Player.GetComponent<CharacterCtrl>().Player_Camera1);


        //try
        //{
        //    StartCoroutine(CheckDestoryByDistanceFromPlayer(CharacterCtrl._CharacterCtrl.HitObjectsQueue));
        //}
        //catch (System.Exception e)//it may because at start menu
        //{
        //    Debug.LogWarning(e);
        //}
    }


    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //  Debug.Log(scene.buildIndex + "Secen Loaded");
        if (scene.name == StartSceneName) { normalTime = false; return; }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f; normalTime = true;
        }
        //if (SceneManager.GetActiveScene().name == "Acknowledgements" || SceneManager.GetActiveScene().name == "Splash")
        //{
        //    // Debug.Log("Acknowledgements Scene");

        //}
        string tempname = PlayerPrefs.GetString("PlayerName");
        if (string.IsNullOrEmpty(tempname)) { PlayerPrefs.SetString("PlayerName", "Stranger"); }
        //MaskAnimator.Play("Enter");
        ReallocateCheckDestoryList();
        // Debug.Log("OnSceneLoaded: " + scene.name);
        // Debug.Log(mode);
    }
    // Update is called once per frame
    void Update()
    {
        if (instance == null)
        {
            instance = this;
        }
        TimeCtrl();
        AudioListenerCtrl();
        GlobalUIFunctions.screenBound = new Vector2(Screen.width, Screen.height);
    }
    private void TimeCtrl()
    {
        // Debug.Log(Time.fixedDeltaTime + "," + normalTime);

        if (normalTime)
        {
            if (Time.timeScale > 1) { Time.timeScale = 1; return; }
            //  Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.timeScale += recoverTimeSpeed * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

    }
    private void AudioListenerCtrl()
    {
        if (!MaskImage || MaskImage.color.a > 1 || isLoadingNextLevel) { return; }

        AudioListener.volume = 1 - MaskImage.color.a;
        // Debug.Log(AudioListener.volume + ", " + MaskImage.color.a);
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
    private void IgnoreCollisionSetting()
    {
    
            Physics.IgnoreLayerCollision(AudioCubeIgnoreCollision.layerIndex_self, AudioCubeIgnoreCollision.layerIndex_other);
      
    }
    IEnumerator CheckDestoryByDeathAltitude(List<Transform> checkParentLists)
    {
        if (checkParentLists.Count == 0) { yield return null; }
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

    private void ReallocateCheckDestoryList()
    {
        checkParentLists.Clear();
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("GravityCubeList"))
        {
            checkParentLists.Add(temp.transform);
        }
        if (checkParentLists.Count > 0)
        {
            try
            {
                StartCoroutine(CheckDestoryByDeathAltitude(checkParentLists));
                // StartCoroutine(CheckDestoryByDistanceFromPlayer(CharacterCtrl._CharacterCtrl.HitObjectsQueue));
            }
            catch (System.Exception e) { Debug.Log(e.Message); }

        }
    }
     public static void PauseTime()
        {
            if (GlobalRules.instance.normalTime)
            {
                Time.timeScale = 0.01f;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
            }
        }
    public static bool IsGameObjInLayerMask(GameObject gameObject, LayerMask layerMask)
    {
        return layerMask.value == (layerMask.value | (1 << gameObject.layer));
    }
        // private void FitCameraDirection(bool isCam1ToCam2)
    // {
    //     var (A, B) = GetCamerasDetails();
    //     // Debug.Log(A.m_XAxis.Value + ", " + B.m_XAxis.Value);
    //     if (isCam1ToCam2)
    //     {
    //         B.m_XAxis.Value = A.m_XAxis.Value;
    //     }
    //     else
    //     {
    //         A.m_XAxis.Value = B.m_XAxis.Value;
    //     }
    //     // Debug.Log("->" + A.m_XAxis.Value + ", " + B.m_XAxis.Value);
    // }
    // private (CinemachineFreeLook, CinemachineOrbitalTransposer) GetCamerasDetails()
    // {
    //     //cam1 = Player.GetComponent<CharacterCtrl>().Player_Camera1.GetComponent<CinemachineFreeLook>();
    //     //cam2 = Player.GetComponent<CharacterCtrl>().Player_Camera2.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();

    //     return (cam1, cam2.GetCinemachineComponent<CinemachineOrbitalTransposer>());
    // }
}
