using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] Slider loadingSlider;
    [SerializeField] Text loadingText;

    int LoadingLevel;

    void Start()
    {
        LoadingLevel = FindObjectOfType<LevelManager>().LoadingLevel;
        Invoke("CallCoroutine", 0.5f);
        
    }


    void CallCoroutine()
    {
        StartCoroutine("LoadLevel");
      
    }

    IEnumerator LoadLevel()
    {
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(LoadingLevel);
 

        while(!operation.isDone)
        {

            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = progress;
            loadingText.text = Mathf.Round(progress*100f) + " %";
           
            yield return null;
        }
       
    }
}
