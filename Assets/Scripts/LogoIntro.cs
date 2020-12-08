using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoIntro : MonoBehaviour
{
    [SerializeField] private Image logo;
    [SerializeField] private float fadeSpeed;

    private void Awake()
    {
        Color c = logo.color;
        c.a = 0;
        logo.color = c;
        StartCoroutine("FadeInImage");
    }

    IEnumerator FadeInImage()
    {
        for(float i = 0; i <= 1f; i += fadeSpeed * Time.deltaTime)
        {
            Color c = logo.color;
            c.a = i;
            logo.color = c;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
