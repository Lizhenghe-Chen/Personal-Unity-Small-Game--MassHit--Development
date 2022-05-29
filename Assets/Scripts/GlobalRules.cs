using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRules : MonoBehaviour
{
    public static GlobalRules instance;
    public int waterLayerID, playerLayerID, bulletLayerID;
    public int DeathAltitude;
    public float recoverTimeSpeed = .2f;
    public List<Transform> checkParentLists = new();
    public WaitForSeconds waitTime = new(5);
    Transform Player;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        StartCoroutine(CheckDestoryByDistanceFromPlayer(100, CharacterCtrl._CharacterCtrl.HitObjectsQueue));
        StartCoroutine(CheckDestoryByDeathAltitude(checkParentLists));
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.fixedDeltaTime);
        Time.timeScale += recoverTimeSpeed * Time.unscaledDeltaTime;

        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);
    }
    IEnumerator CheckDestoryByDistanceFromPlayer(int maxDistance, Queue<GameObject> checkLists)
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
}
