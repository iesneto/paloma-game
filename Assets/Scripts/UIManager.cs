using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenObject;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float cowsBarFillRate;

    [System.Serializable]
    private struct InGameUI
    {

        // Put Any In-GameUI Here
        public GameObject gameObject;
        public Image cowFillBar;
        public TextMeshProUGUI cowsNumber;
        public TextMeshProUGUI coinsValue;
        public TextMeshProUGUI levelValue;
        public TextMeshProUGUI experienceValue;
        public Image experienceFillBar;
        public GameObject leaveStage;
        public TextMeshProUGUI currentTime;
        public TextMeshProUGUI bestTime;
        public Image playPauseButton;
        public Sprite playImage;
        public Sprite pauseImage;
        public bool play;
        public Image pauseBackground;
        public GameObject playerStartedUI;
        public Animation playerStartedUIAnimation;
    }
    [SerializeField]private InGameUI inGameUI;

    [System.Serializable]
    private struct MenuUI
    {
        public GameObject gameObject;
        public MainMenu mainMenuScript;
        public TextMeshProUGUI coinsValue;
        public TextMeshProUGUI levelValue;
        public TextMeshProUGUI experienceValue;
        public Image experienceFillBar;
        public TextMeshProUGUI flyingSaucerStatSpeed;
        public TextMeshProUGUI flyingSaucerStatRaySize;
        public TextMeshProUGUI flyingSaucerStatRayMultiplier;
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

    public void FadeOutBlackScreen()
    {
        fadeImage.gameObject.transform.parent.gameObject.SetActive(true);
        StartCoroutine("FadeScreen");
    }

    IEnumerator FadeScreen()
    {
        Color color = fadeImage.color;
        for(float i = 1; i > 0; i -= fadeSpeed)
        {
            color = fadeImage.color;
            color.a = i;
            fadeImage.color = color;
            yield return new WaitForSeconds(0.01f);
        }
        color.a = 1;
        fadeImage.color = color;
        fadeImage.gameObject.transform.parent.gameObject.SetActive(false);
        GameControl.Instance.ScreenFadeOut();
    }
    #endregion

    #region IN_GAME_UI_METHODS

    public void InGameUIStart()
    {
        InGameUIShow(true);
        MainMenuShowUI(false);
        inGameUI.play = true;
        ShowWaitPlayerStartUI();
    }

    public void ShowWaitPlayerStartUI()
    {
        inGameUI.playerStartedUI.SetActive(true);
        inGameUI.playerStartedUIAnimation.Play();
    }

    public void ClosePlayerStartedUI()
    {
        inGameUI.playerStartedUI.SetActive(false);
    }

    public void InGameUIShow(bool flag)
    {
        inGameUI.gameObject.SetActive(flag);
        if(flag) InGameUISync();
    }

    public void InGameUISync()
    {
        inGameUI.coinsValue.SetText(GameControl.Instance.playerData.currentCoins.ToString());
        inGameUI.cowsNumber.SetText(GameControl.Instance.SceneCurrentCows + "/" + GameControl.Instance.SceneTotalCows);
        StartCoroutine("FillCowsBar");
        inGameUI.experienceValue.SetText(GameControl.Instance.playerData.experience.ToString());
        inGameUI.levelValue.SetText(GameControl.Instance.playerData.level.ToString());
        inGameUI.experienceFillBar.fillAmount = ((float)(GameControl.Instance.playerData.experience - GameControl.Instance.PlayerStartExperienceOfLevel()) 
                                                / (float)(GameControl.Instance.PlayerExperienceToLevelUp() - GameControl.Instance.PlayerStartExperienceOfLevel()));
    }

    IEnumerator FillCowsBar()
    {
        while(inGameUI.cowFillBar.fillAmount < (((float)GameControl.Instance.SceneCurrentCows / (float)GameControl.Instance.SceneTotalCows)))
        {
            inGameUI.cowFillBar.fillAmount += cowsBarFillRate;
            yield return null;
        }
        
    }

    public void ResetCowsFillBar()
    {
        inGameUI.cowFillBar.fillAmount = 0;
    }

    public void ResetTimers()
    {
        inGameUI.bestTime.SetText("00:00");
        inGameUI.currentTime.SetText("00:00");
    }

    public void OpenLeaveStage()
    {
        inGameUI.leaveStage.SetActive(true);
        inGameUI.leaveStage.GetComponentInChildren<Animator>().SetBool("Open", true);
    }

    public void CloseLeaveStage()
    {
        inGameUI.leaveStage.GetComponentInChildren<Animator>().SetBool("Open", false);
    }

    public void DisableLeaveStage()
    {       
        inGameUI.leaveStage.SetActive(false);
    }

    public void UpdateCurrentTime(string time)
    {
        inGameUI.currentTime.SetText(time);
    }

    public void UpdateBestTime(string time)
    {
        inGameUI.bestTime.SetText(time);
    }

    public void PlayPauseButton()
    {
        if (inGameUI.play)
        {
            inGameUI.play = false;
            inGameUI.playPauseButton.sprite = inGameUI.playImage;
            inGameUI.pauseBackground.enabled = true;
            GameControl.Instance.PauseGame();
        }
        else
        {
            inGameUI.play = true;
            inGameUI.playPauseButton.sprite = inGameUI.pauseImage;
            inGameUI.pauseBackground.enabled = false;
            GameControl.Instance.UnpauseGame();
        }
    }

    

    #endregion

    #region MENU_UI_METHODS

    public void MainMenuStart()
    {
        MainMenuShowUI(true);
        InGameUIShow(false);
    }

    public void LoadMainMenuIntro()
    {
        MainMenuStart();
        menuUI.mainMenuScript.ShowIntroMenu();
    }

    public void LoadMainMenuPlay()
    {
        MainMenuStart();
        menuUI.mainMenuScript.ShowPlayMenu();
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
        menuUI.coinsValue.SetText(GameControl.Instance.playerData.currentCoins.ToString());
        menuUI.levelValue.SetText(GameControl.Instance.playerData.level.ToString());
        menuUI.experienceValue.SetText(GameControl.Instance.playerData.experience.ToString());
        menuUI.experienceFillBar.fillAmount = ((float)(GameControl.Instance.playerData.experience - GameControl.Instance.PlayerStartExperienceOfLevel())
                                                / (float)(GameControl.Instance.PlayerExperienceToLevelUp() - GameControl.Instance.PlayerStartExperienceOfLevel()));
        menuUI.flyingSaucerStatSpeed.SetText((5 + GameControl.Instance.playerData.playerSpeed).ToString());
        menuUI.flyingSaucerStatRaySize.SetText((2 + (0.5f * GameControl.Instance.playerData.rayRadius)).ToString());
        menuUI.flyingSaucerStatRayMultiplier.SetText((1 + GameControl.Instance.playerData.rayMultiplier).ToString());
        menuUI.mainMenuScript.UpdateCows();
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
