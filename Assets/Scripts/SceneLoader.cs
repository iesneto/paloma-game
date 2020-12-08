using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour
{
    
    [SerializeField] private GameObject LoadingScreenBG;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Vector2Int numberOfScenes;

    private void Awake()
    {
        LoadingScreenBG.SetActive(false);
        //loadingBar.gameObject.SetActive(false);
    }

    public void LoadRandomLevel()
    {
        int scn = Random.Range(numberOfScenes.x, numberOfScenes.y + 1);
        
        StartCoroutine(LoadSceneAsync(scn));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneAsync(1));
    }
    IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        LoadingScreenBG.SetActive(true);
        //loadingBar.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress);
            loadingBar.value = operation.progress;
            
            yield return null;
        }

        loadingBar.value = 1;
        StartCoroutine(FadeOutLoadingScreen());
        if (scene != 1) GameControl.Instance.ShowInGameUI();

        //LoadingScreenBG.SetActive(false);
        //loadingBar.gameObject.SetActive(false);
    }

    IEnumerator FadeOutLoadingScreen()
    {
        yield return new WaitForSeconds(fadeSpeed);
        LoadingScreenBG.SetActive(false);
        
        //loadingBar.gameObject.SetActive(false);
    }
}
