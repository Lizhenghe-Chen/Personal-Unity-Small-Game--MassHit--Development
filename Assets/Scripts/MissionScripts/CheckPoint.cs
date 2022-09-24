using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    [Tooltip("False if wnat it to be the finnish point")]
    public bool isCheckPoint = true;
    [SerializeField] Animator animator;
    int HitCount;
    public bool isLoading = false;
    [SerializeField] Image MaskImage;
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
        if (collision.gameObject.CompareTag("Player"))
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
                if (HitCount > 0) { return; }
                PlayerPrefs.SetFloat("SavedCheckPoint_X", 0);
                PlayerPrefs.SetFloat("SavedCheckPoint_Y", 0);
                PlayerPrefs.SetFloat("SavedCheckPoint_Z", 0);
                Time.timeScale = 0;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                animator.Play("Mission Accomplished", 0, 0);


                PlayerPrefs.SetInt("UnlockedLevel", SceneManager.GetActiveScene().buildIndex - 1);
                // Debug.Log("FinishPoint now unlocked level" + ((Resources.Load("Data") as GameObject).GetComponent<SavedData>().UnlockedLevel = SceneManager.GetActiveScene().buildIndex - 1));
            }
            HitCount++;
        }


    }
    public void LoadNextLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SetIsLoading() { GlobalRules.instance.isLoadingNextLevel = isLoading = true; }

}
