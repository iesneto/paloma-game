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

        // Put Any In-GameUI Here
        public GameObject gameObject;
        public Image cowFillBar;
        public Text coinsValueShadow;
        public Text coinsValue;


    }
    [SerializeField]private InGameUI inGameUI;

    [System.Serializable]
    private struct MenuUI
    {
        public GameObject gameObject;
        public Text coinsValueShadow;
        public Text coinsValue;
    }
    [SerializeField] private MenuUI menuUI;

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

    #region IN_GAME_UI_METHODS

    public void ShowInGameUI(bool flag)
    {
        inGameUI.gameObject.SetActive(flag);
        if(flag) SyncInGameUI();
    }

    public void SyncInGameUI()
    {
        inGameUI.coinsValue.text = GameControl.Instance.playerData.currentPoints.ToString();
    }

    #endregion

    #region MENU_UI_METHODS

    public void ShowMenuUI(bool flag)
    {
        menuUI.gameObject.SetActive(flag);
        if (flag) SyncMenuUI();
    }

    public void SyncMenuUI()
    {
        menuUI.coinsValue.text = GameControl.Instance.playerData.currentPoints.ToString();
    }

    #endregion
}
