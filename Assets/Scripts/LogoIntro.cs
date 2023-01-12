using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Gamob
{
    public class LogoIntro : MonoBehaviour
    {
        [SerializeField] private Image logo;
        [SerializeField] private float fadeSpeed;
        [SerializeField] private Slider loadBar;

        private void Awake()
        {
            loadBar.value = 0f;
            Color c = logo.color;
            c.a = 0;
            logo.color = c;
            StartCoroutine("FadeInImage");
        }

        IEnumerator FadeInImage()
        {
            for (float i = 0; i <= 1f; i += fadeSpeed * Time.deltaTime)
            {
                Color c = logo.color;
                c.a = i;
                logo.color = c;
                yield return null;
            }
            //SceneManager.LoadScene(1);
            StartCoroutine(LoadSceneAsync());
        }

        IEnumerator LoadSceneAsync()
        {
            
            loadBar.gameObject.SetActive(true);
            Debug.Log("Inicio: " + loadBar.value);
            yield return null;
            //if (isLoading) yield break;
            //isLoading = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(1);
            //uiManager.SetLoadingScreen(true);


            while (!operation.isDone)
            {
                //float progress = Mathf.Clamp01(operation.progress);
                //uiManager.SetLoadingBarValue(operation.progress);

                loadBar.value = operation.progress;
                Debug.Log("Progress: " + loadBar.value);
                yield return null;
            }

            loadBar.value = 1f;            
            yield return null;
            
            //uiManager.SetLoadingBarValue(1);
            //StartCoroutine(FadeOutLoadingScreen());

            //if (scene != mainMenuSceneBuildIndex)
            //{
            //    InitializeScene();
            //    GameControl.Instance.LevelLoaded();
            //    StartCoroutine(FadeOutLoadingScreen(false));
            //}
            //else
            //{
            //    GameControl.Instance.StartMainMenu();
            //    StartCoroutine(FadeOutLoadingScreen(true));
            //}
            //isLoading = false;
        }
    }
}