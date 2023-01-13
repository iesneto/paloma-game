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
        [SerializeField] private GameObject loadingIcon;
        //[SerializeField] private Slider loadBar;

        private void Awake()
        {
            // loadBar.value = 0f;
            loadingIcon.SetActive(false);
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
            loadingIcon.SetActive(true);

            SceneManager.LoadSceneAsync(1);
            
        }
        
    }
}