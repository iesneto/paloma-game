using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenObject;
    [SerializeField] private Slider loadingBar;
    [System.Serializable]
    private struct InGameUI
    {

        // ##Ivo Put Here Any In-GameUI
        [SerializeField]private Image cowFillBar;

    }
    [SerializeField]private InGameUI inGameUI;

    private void Awake()
    {
        SetLoadingScreen(false);
    }

    public void SetLoadingScreen(bool s)
    {
        loadingScreenObject.SetActive(s);
    }

    public void SetLoadingBarValue(float v)
    {
        loadingBar.value = v;
    }
}
