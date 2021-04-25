using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : MonoBehaviour
{
    
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Vector2Int numberOfScenes;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int mainMenuSceneBuildIndex;
    [SerializeField] private string mainMenuSceneName;

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
        
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            Scene tempScene = SceneManager.GetSceneByBuildIndex(i);
            if (tempScene.name == mainMenuSceneName) mainMenuSceneBuildIndex = i;
        }
    }

    public void LoadRandomLevel()
    {
        int scn = Random.Range(numberOfScenes.x, numberOfScenes.y + 1);
        
        StartCoroutine(LoadSceneAsync(scn));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneAsync(mainMenuSceneBuildIndex));
    }
    IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        uiManager.SetLoadingScreen(true);
        

        while (!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress);
            uiManager.SetLoadingBarValue(operation.progress);
            yield return null;
        }
        uiManager.SetLoadingBarValue(1);
        StartCoroutine(FadeOutLoadingScreen());
        if (scene != 1) GameControl.Instance.LevelLoaded();
    }

    IEnumerator FadeOutLoadingScreen()
    {
        yield return new WaitForSeconds(fadeSpeed);
        uiManager.SetLoadingScreen(false);

    }

    public int ReturnSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
