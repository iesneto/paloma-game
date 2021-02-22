using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        public TextMeshProUGUI coinsValue;


    }
    [SerializeField]private InGameUI inGameUI;

    [System.Serializable]
    private struct MenuUI
    {
        public GameObject gameObject;
        public MainMenu mainMenuScript;
        public TextMeshProUGUI coinsValue;
    }
    [SerializeField] private MenuUI menuUI;

    private void Awake()
    {
        SetLoadingScreen(false);
    }

    #region GENERAL_METHODS

    public void LoadLevel()
    {
        GameControl.Instance.StartPlaying();
        GameControl.Instance.gameObject.GetComponent<SceneController>().LoadRandomLevel();
    }

    #endregion

    #region LOADING_SCREEN_METHODS

    public void SetLoadingScreen(bool s)
    {
        loadingScreenObject.SetActive(s);
    }

    public void SetLoadingBarValue(float v)
    {
        loadingBar.value = v;
    }
    #endregion

    #region IN_GAME_UI_METHODS

    public void InGameUIStart()
    {
        InGameUIShow(true);
        MainMenuShowUI(false);
    }

    public void InGameUIShow(bool flag)
    {
        inGameUI.gameObject.SetActive(flag);
        if(flag) InGameUISync();
    }

    public void InGameUISync()
    {
        inGameUI.coinsValue.SetText(GameControl.Instance.playerData.currentPoints.ToString());
    }

    #endregion

    #region MENU_UI_METHODS

    public void MainMenuStart(bool _startedPlaying)
    {
        MainMenuShowUI(true);
        InGameUIShow(false);
        if (_startedPlaying) menuUI.mainMenuScript.ShowPlayMenu();
            else menuUI.mainMenuScript.ShowIntroMenu();
    }



    public void MainMenuShowUI(bool flag)
    {
        menuUI.gameObject.SetActive(flag);
        if (flag) MainMenuSyncUI();
    }

    public void MainMenuShowPlayMenu()
    {
        menuUI.mainMenuScript.ShowPlayMenu();
    }

    public void MainMenuSyncUI()
    {
        menuUI.coinsValue.SetText(GameControl.Instance.playerData.currentPoints.ToString());
    }

    public void MainMenuCloseModalWindow()
    {
        menuUI.mainMenuScript.CloseModal();
    }

    public void MainMenuShowModalPurchase(Upgrade up)
    {
        menuUI.mainMenuScript.ShowModalPurchase(up);
    }

    public void MainMenuCanPurchaseUpgrade()
    {
        menuUI.mainMenuScript.CanPurchaseUpgrade();
    }

    #endregion
}
