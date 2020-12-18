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

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
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
        uiManager.SetLoadingScreen(true);
        

        while (!operation.isDone)
        {
            //float progress = Mathf.Clamp01(operation.progress);
            uiManager.SetLoadingBarValue(operation.progress);
            yield return null;
        }
        uiManager.SetLoadingBarValue(1);
        StartCoroutine(FadeOutLoadingScreen());
        if (scene != 1) GameControl.Instance.ShowInGameUI();
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
