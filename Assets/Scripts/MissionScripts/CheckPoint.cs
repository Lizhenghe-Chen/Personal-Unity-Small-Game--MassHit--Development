using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CheckPoint : MonoBehaviour
{
    [Tooltip("False if wnat it to be the finnish point")]
    public bool isCheckPoint = true;
    public string playerTag = "Respawn";
    public Material checkPointMaterial, finnishPointMaterial;
    public Color checkPointColor, finnishPointColor;
   [SerializeField] TMP_Text display_Text;
    [SerializeField] Animator animator;
    [SerializeField] bool HitOnce = false;
    public bool isLoading = false;
    [SerializeField] private Image MaskImage;
    private void OnValidate()
    {
     
        if (isCheckPoint)
        {
            SetToChecnkPoint();
        }
        else
        {
            SetToFinnishPoint();
        }
    }
 
    private void Update()
    {
        if (isLoading)
        {
            Debug.Log(1 - MaskImage.color.a);
            AudioListener.volume = 1 - MaskImage.color.a;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {

            if (isCheckPoint)//if it is checkpoint
            {
                // SaveCurrentStatus();
                animator.Play("ShowChecked", 0, 0);
                Debug.Log("CheckPoint");
                CharacterCtrl._CharacterCtrl.CheckPoint = this.transform.position + new Vector3(Random.Range(1, 3), 0, Random.Range(1, 3));
                PlayerPrefs.SetString("SavedCheckPointScene", SceneManager.GetActiveScene().name);
                PlayerPrefs.SetFloat("SavedCheckPoint_X", CharacterCtrl._CharacterCtrl.CheckPoint.x);
                PlayerPrefs.SetFloat("SavedCheckPoint_Y", CharacterCtrl._CharacterCtrl.CheckPoint.y);
                PlayerPrefs.SetFloat("SavedCheckPoint_Z", CharacterCtrl._CharacterCtrl.CheckPoint.z);

            }
            else
            {
                if (HitOnce) { return; }
                PlayerPrefs.SetFloat("SavedCheckPoint_X", 0);
                PlayerPrefs.SetFloat("SavedCheckPoint_Y", 0);
                PlayerPrefs.SetFloat("SavedCheckPoint_Z", 0);
                Time.timeScale = 0;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                animator.Play("Mission Accomplished", 0, 0);
                PlayerPrefs.SetInt("UnlockedLevel", SceneManager.GetActiveScene().buildIndex - 1);
                HitOnce = true;
                // Debug.Log("FinishPoint now unlocked level" + ((Resources.Load("Data") as GameObject).GetComponent<SavedData>().UnlockedLevel = SceneManager.GetActiveScene().buildIndex - 1));
            }

        }

    }
    void SetToChecnkPoint()
    {
        GetComponent<MeshRenderer>().sharedMaterial = checkPointMaterial;
        GetComponent<MeshRenderer>().sharedMaterial.color = checkPointColor;
        GetComponent<Light>().color = checkPointColor;
        display_Text.text = "Check Point Checked";
    }
    void SetToFinnishPoint()
    {
        GetComponent<MeshRenderer>().sharedMaterial = finnishPointMaterial;
        GetComponent<MeshRenderer>().sharedMaterial.color = finnishPointColor;
        GetComponent<Light>().color = finnishPointColor;
        display_Text.text = "Finnished";
    }
    public void LoadNextLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SetIsLoading() { GlobalRules.instance.isLoadingNextLevel = isLoading = true; }

}
