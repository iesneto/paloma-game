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
    private struct WindowClearStage
    {
        public GameObject stageTimeLabel;
        public GameObject stageTimeValue;
        public TextMeshProUGUI stageTimeValueText;        
        public GameObject stageXPValue;
        public TextMeshProUGUI stageXPValueText;
        public GameObject playerTimeLabel;
        public GameObject playerTimeValue;
        public TextMeshProUGUI playerTimeValueText;
        public GameObject extraXPValue;
        public TextMeshProUGUI extraXPValueText;
        public GameObject totalXPValue;
        public TextMeshProUGUI totalXPValueText;
        public GameObject okButton;
    }

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
        public GameObject stageCleared;
        public GameObject currentOpenedWindow;
        public TextMeshProUGUI currentTime;
        public TextMeshProUGUI bestTime;
        public Image playPauseButton;
        public Sprite playImage;
        public Sprite pauseImage;
        public bool play;
        public Image pauseBackground;
        public GameObject playerStartedUI;
        public Animation playerStartedUIAnimation;
        public WindowClearStage windowClearStage;
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

    public void LoadRandomLevel()
    {
        GameControl.Instance.StartPlaying();
        GameControl.Instance.gameObject.GetComponent<SceneController>().LoadRandomLevel();
    }
    public void LoadLevel()
    {
        GameControl.Instance.StartPlaying();
        StageData stage = GameControl.Instance.StageDatabyIndex(menuUI.mainMenuScript.CurrentStage());
        
        GameControl.Instance.gameObject.GetComponent<SceneController>().LoadLevel(stage.id);
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
        inGameUI.experienceValue.SetText(GameControl.Instance.playerData.experience.ToString() + "/" + GameControl.Instance.PlayerExperienceToLevelUp().ToString());
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
        inGameUI.currentOpenedWindow = inGameUI.leaveStage;
        inGameUI.leaveStage.GetComponentInChildren<Animator>().SetBool("Open", true);
    }

    public void CloseLeaveStage()
    {
        inGameUI.leaveStage.GetComponentInChildren<Animator>().SetBool("Open", false);
    }

    

    public void DisableOpenedWindow()
    {       
        inGameUI.currentOpenedWindow.SetActive(false);
    }

    public void OpenStageCleared()
    {
        inGameUI.stageCleared.SetActive(true);
        inGameUI.windowClearStage.stageTimeLabel.SetActive(false);
        inGameUI.windowClearStage.stageTimeValue.SetActive(false);
        inGameUI.windowClearStage.stageXPValue.SetActive(false);
        inGameUI.windowClearStage.playerTimeLabel.SetActive(false);
        inGameUI.windowClearStage.playerTimeValue.SetActive(false);
        inGameUI.windowClearStage.extraXPValue.SetActive(false);
        inGameUI.windowClearStage.totalXPValue.SetActive(false);
        inGameUI.windowClearStage.okButton.SetActive(false);
        SetupStageClearAnimations();
        inGameUI.currentOpenedWindow = inGameUI.stageCleared;
        inGameUI.stageCleared.GetComponentInChildren<Animator>().SetBool("Open", true);
        StartCoroutine("ShowStageClearedRewards");
    }

    void SetupStageClearAnimations()
    {
        
        //inGameUI.windowClearStage.stageTimeLabel.GetComponent<Animation>().Rewind();
        inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Rewind();
        inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Rewind();
        //inGameUI.windowClearStage.playerTimeLabel.GetComponent<Animation>().Rewind();
        inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Rewind();
        inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Rewind();
        inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Rewind();
        inGameUI.windowClearStage.okButton.GetComponent<Animation>().Rewind();
        //inGameUI.windowClearStage.stageTimeLabel.GetComponent<Animation>().Play();
        inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Play();
        inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Play();
        //inGameUI.windowClearStage.playerTimeLabel.GetComponent<Animation>().Play();
        inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Play();
        inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Play();
        inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Play();
        inGameUI.windowClearStage.okButton.GetComponent<Animation>().Play();
        //inGameUI.windowClearStage.stageTimeLabel.GetComponent<Animation>().Sample();
        inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Sample();
        inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Sample();
        //inGameUI.windowClearStage.playerTimeLabel.GetComponent<Animation>().Sample();
        inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Sample();
        inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Sample();
        inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Sample();
        inGameUI.windowClearStage.okButton.GetComponent<Animation>().Sample();
        //inGameUI.windowClearStage.stageTimeLabel.GetComponent<Animation>().Stop();
        inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Stop();
        inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Stop();
        //inGameUI.windowClearStage.playerTimeLabel.GetComponent<Animation>().Stop();
        inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Stop();
        inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Stop();
        inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Stop();
        inGameUI.windowClearStage.okButton.GetComponent<Animation>().Stop();
    }

    IEnumerator ShowStageClearedRewards()
    {
        inGameUI.windowClearStage.stageTimeLabel.SetActive(true);
        inGameUI.windowClearStage.playerTimeLabel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        
        inGameUI.windowClearStage.stageTimeValue.SetActive(true);
        inGameUI.windowClearStage.stageXPValue.SetActive(true);        
        inGameUI.windowClearStage.playerTimeValue.SetActive(true);
        inGameUI.windowClearStage.extraXPValue.SetActive(true);
        inGameUI.windowClearStage.totalXPValue.SetActive(true);
        inGameUI.windowClearStage.okButton.SetActive(true);
        inGameUI.windowClearStage.extraXPValueText.SetText("+XP " + GameControl.Instance.StageExtraXP().ToString());
        inGameUI.windowClearStage.totalXPValueText.SetText("XP " + GameControl.Instance.StageTotalXP().ToString());
        inGameUI.windowClearStage.stageXPValueText.SetText("XP " + GameControl.Instance.StageXP().ToString());
        //inGameUI.windowClearStage.stageTimeLabel.GetComponent<Animation>().Play();
        //yield return new WaitForSeconds(0.2f);
        inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.2f);
        inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.2f);
        //inGameUI.windowClearStage.playerTimeLabel.GetComponent<Animation>().Play();
        //yield return new WaitForSeconds(0.2f);
        inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.2f);
        inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.2f);
        inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(0.2f);
        inGameUI.windowClearStage.okButton.GetComponent<Animation>().Play();
        


    }

    public void CloseStageCleared()
    {
        inGameUI.stageCleared.GetComponentInChildren<Animator>().SetBool("Open", false);
    }

    public void UpdateCurrentTime(string time)
    {
        inGameUI.currentTime.SetText(time);
        inGameUI.windowClearStage.playerTimeValueText.SetText(time);
    }

    public void UpdateBestTime(string time)
    {
        inGameUI.bestTime.SetText(time);
        inGameUI.windowClearStage.stageTimeValueText.SetText(time);
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
        menuUI.experienceValue.SetText(GameControl.Instance.playerData.experience.ToString() + "/" + GameControl.Instance.PlayerExperienceToLevelUp().ToString());
        menuUI.experienceFillBar.fillAmount = ((float)(GameControl.Instance.playerData.experience - GameControl.Instance.PlayerStartExperienceOfLevel())
                                                / (float)(GameControl.Instance.PlayerExperienceToLevelUp() - GameControl.Instance.PlayerStartExperienceOfLevel()));
        menuUI.flyingSaucerStatSpeed.SetText((5 + GameControl.Instance.playerData.playerSpeed).ToString());
        menuUI.flyingSaucerStatRaySize.SetText((2 + (0.5f * GameControl.Instance.playerData.rayRadius)).ToString());
        menuUI.flyingSaucerStatRayMultiplier.SetText((1 + GameControl.Instance.playerData.rayMultiplier).ToString());
        menuUI.mainMenuScript.UpdateCows();
        menuUI.mainMenuScript.UpdateStages();
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

    public void MainMenuLevelUp()
    {
        menuUI.mainMenuScript.LevelUp();
    }

    #endregion
}
