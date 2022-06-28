using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class SplashCtrl : MonoBehaviour
{
    public TMP_Text loadingText;
    public bool finnieshedAnimation = false;
    // Start is called before the first frame update

    private void Start()
    {
        loadingText.text = "Hi ^_^";
    }
    public void LoadScene(int NextSceneIndex)
    {

        StartCoroutine(AsyncLoadScene(NextSceneIndex));
        //SceneManager.LoadScene(NextSceneIndex);
    }


    IEnumerator AsyncLoadScene(int NextSceneIndex)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(NextSceneIndex);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = finnieshedAnimation;

        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            loadingText.text = "Loading : " + (asyncOperation.progress / .9f * 100) + "%";
            //   Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {

                //Activate the Scene
                asyncOperation.allowSceneActivation = finnieshedAnimation;


                yield return null;
            }
        }
    }
    public void FinnishedAnimation()
    {
        finnieshedAnimation = true;
    }

}
