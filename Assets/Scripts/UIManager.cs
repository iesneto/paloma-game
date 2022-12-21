using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gamob
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreenObject;
        [SerializeField] private Slider loadingBar;
        [SerializeField] private float fadeSpeed;
        [SerializeField] private Image fadeImage;
        [SerializeField] private float blackScreenDelayScene;
        [SerializeField] private float blackScreenDelayMenu;
        [SerializeField] private float cowsBarFillRate;
        [SerializeField] private float popUpsDelayTime;
        [SerializeField] private GameObject configWindow;
        [SerializeField] private Toggle music;
        [SerializeField] private Toggle sfx;


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
            public GameObject rewardedAdsButton;
            public TextMeshProUGUI rewardedAdsButtonXPText;
            public TextMeshProUGUI rewardedAdsButtonCoinText;
            public GameObject rewardedXPClaimButton;
            public TextMeshProUGUI rewardedXPClaimButtonText;
            public TextMeshProUGUI rewardedCoinClaimButtonText;
            public GameObject okButton;
            public GameObject rewardPopUp;
            public TextMeshProUGUI rewardPopUpXpText;
            public TextMeshProUGUI rewardPopUpCoinText;
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
            public GameObject sideBar;
            public Sprite sideBarOpen;
            public Sprite sideBarClose;
            public Image sideBarIcon;
            public Image musicIcon;
            public Image sfxIcon;
            public Sprite musicON;
            public Sprite musicOFF;
            public Sprite sfxON;
            public Sprite sfxOFF;

        }
        [SerializeField] private InGameUI inGameUI;

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

            //GameControl.Instance.StartPlaying();
            // StageData stage = GameControl.Instance.StageDatabyIndex(menuUI.mainMenuScript.CurrentStage());
            // GameControl.Instance.gameObject.GetComponent<SceneController>().LoadLevel(stage.id);
            GameControl.Instance.SetStageToLoadShowInterstitialAds(menuUI.mainMenuScript.CurrentStage());
        }

        public void OpenConfigurationWindow()
        {
            configWindow.SetActive(true);
            configWindow.transform.GetComponentInChildren<Animator>().SetBool("Open", true);
        }

        public void CloseConfigurationWindow()
        {
            configWindow.transform.GetComponentInChildren<Animator>().SetBool("Open", false);
        }

        public void DisableConfigurationWindow()
        {
            configWindow.SetActive(false);
        }

        public void SetSoundConfiguration(bool _music, bool _sfx)
        {
            music.isOn = _music;
            sfx.isOn = _sfx;
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

        public void FadeOutBlackScreen(bool _menu)
        {
            fadeImage.gameObject.transform.parent.gameObject.SetActive(true);
            if (_menu)
            {
                StartCoroutine(FadeScreen(blackScreenDelayMenu));
            }
            else
            {
                StartCoroutine(FadeScreen(blackScreenDelayScene));
            }
            //StartCoroutine("FadeScreen");
        }

        IEnumerator FadeScreen(float delay)
        {
            yield return new WaitForSeconds(delay);
            Color color = fadeImage.color;
            for (float i = 1; i > 0; i -= fadeSpeed)
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

        void SetSideBarIcons()
        {
            inGameUI.musicIcon.sprite = GameControl.Instance.playerData.music ?
                inGameUI.musicON :
                inGameUI.musicOFF;

            inGameUI.sfxIcon.sprite = GameControl.Instance.playerData.sfx ?
                 inGameUI.sfxON :
                 inGameUI.sfxOFF;

            inGameUI.sideBarIcon.sprite = inGameUI.sideBarOpen;
        }

        public void ToggleMusic()
        {
            if (GameControl.Instance.playerData.music)
            {
                inGameUI.musicIcon.sprite = inGameUI.musicOFF;
                GameControl.Instance.ToggleMusic(false);
            }
            else
            {
                inGameUI.musicIcon.sprite = inGameUI.musicON;
                GameControl.Instance.ToggleMusic(true);
            }
        }

        public void ToggleSFX()
        {
            if (GameControl.Instance.playerData.sfx)
            {
                inGameUI.sfxIcon.sprite = inGameUI.sfxOFF;
                GameControl.Instance.ToggleSFX(false);
            }
            else
            {
                inGameUI.sfxIcon.sprite = inGameUI.sfxON;
                GameControl.Instance.ToggleSFX(true);
            }
        }
        public void SideBar()
        {

            if (inGameUI.sideBar.GetComponent<Animator>().IsInTransition(0)) return;
            if (inGameUI.sideBar.GetComponent<Animator>().GetBool("open"))
            {
                inGameUI.sideBar.GetComponent<Animator>().SetBool("open", false);
                inGameUI.sideBarIcon.sprite = inGameUI.sideBarOpen;
                GameControl.Instance.AudioManager().CloseWindow();
            }
            else
            {
                inGameUI.sideBar.GetComponent<Animator>().SetBool("open", true);
                inGameUI.sideBarIcon.sprite = inGameUI.sideBarClose;
                GameControl.Instance.AudioManager().OpenWindow();
            }

        }



        public void InGameUIStart()
        {
            InGameUIShow(true);
            MainMenuShowUI(false);
            inGameUI.play = true;
            ShowWaitPlayerStartUI();
            SetSideBarIcons();
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
            if (flag) InGameUISync();
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
            while (inGameUI.cowFillBar.fillAmount < (((float)GameControl.Instance.SceneCurrentCows / (float)GameControl.Instance.SceneTotalCows)))
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
            inGameUI.windowClearStage.rewardedAdsButton.SetActive(false);
            inGameUI.windowClearStage.rewardedXPClaimButton.SetActive(false);
            inGameUI.windowClearStage.rewardPopUp.SetActive(false);
            SetupStageClearAnimations();
            inGameUI.currentOpenedWindow = inGameUI.stageCleared;
            inGameUI.stageCleared.GetComponentInChildren<Animator>().SetBool("Open", true);
            StartCoroutine("ShowStageClearedRewards");
            GameControl.Instance.AudioManager().StageClear();
            GameControl.Instance.PlayerAudio().StageMuteFlyingSaucer();
        }

        void SetupStageClearAnimations()
        {

            inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Rewind();
            inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Rewind();
            inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Rewind();
            inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Rewind();
            inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Rewind();
            inGameUI.windowClearStage.okButton.GetComponent<Animation>().Rewind();
            inGameUI.windowClearStage.rewardedAdsButton.GetComponent<Animation>().Rewind();
            inGameUI.windowClearStage.rewardedXPClaimButton.GetComponent<Animation>().Rewind();
            inGameUI.windowClearStage.rewardPopUp.GetComponent<Animation>().Rewind();

            inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Play();
            inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Play();
            inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Play();
            inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Play();
            inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Play();
            inGameUI.windowClearStage.okButton.GetComponent<Animation>().Play();
            inGameUI.windowClearStage.rewardedAdsButton.GetComponent<Animation>().Play();
            inGameUI.windowClearStage.rewardedXPClaimButton.GetComponent<Animation>().Play();
            inGameUI.windowClearStage.rewardPopUp.GetComponent<Animation>().Rewind();

            inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Sample();
            inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Sample();
            inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Sample();
            inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Sample();
            inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Sample();
            inGameUI.windowClearStage.okButton.GetComponent<Animation>().Sample();
            inGameUI.windowClearStage.rewardedAdsButton.GetComponent<Animation>().Sample();
            inGameUI.windowClearStage.rewardedXPClaimButton.GetComponent<Animation>().Sample();
            inGameUI.windowClearStage.rewardPopUp.GetComponent<Animation>().Rewind();

            inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Stop();
            inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Stop();
            inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Stop();
            inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Stop();
            inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Stop();
            inGameUI.windowClearStage.okButton.GetComponent<Animation>().Stop();
            inGameUI.windowClearStage.rewardedAdsButton.GetComponent<Animation>().Stop();
            inGameUI.windowClearStage.rewardedXPClaimButton.GetComponent<Animation>().Stop();
            inGameUI.windowClearStage.rewardPopUp.GetComponent<Animation>().Rewind();
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

            //if (gameObject.GetComponent<AdsManager>().rewardedLoaded)
            if (gameObject.GetComponent<Advertisements>().rewardedLoaded)
            {
                inGameUI.windowClearStage.rewardedAdsButton.SetActive(true);
                inGameUI.windowClearStage.rewardedAdsButtonXPText.SetText(GameControl.Instance.StageTotalXP().ToString());
                inGameUI.windowClearStage.rewardedAdsButtonCoinText.SetText(GameControl.Instance.StageRewardCoins().ToString());
                inGameUI.windowClearStage.rewardPopUpXpText.SetText(GameControl.Instance.StageTotalXP().ToString());
                inGameUI.windowClearStage.rewardPopUpCoinText.SetText(GameControl.Instance.StageRewardCoins().ToString());
            }
            //inGameUI.windowClearStage.stageTimeLabel.GetComponent<Animation>().Play();
            //yield return new WaitForSeconds(0.2f);
            inGameUI.windowClearStage.stageTimeValue.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(popUpsDelayTime);
            inGameUI.windowClearStage.stageXPValue.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(popUpsDelayTime);
            //inGameUI.windowClearStage.playerTimeLabel.GetComponent<Animation>().Play();
            //yield return new WaitForSeconds(0.2f);
            inGameUI.windowClearStage.playerTimeValue.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(popUpsDelayTime);
            inGameUI.windowClearStage.extraXPValue.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(popUpsDelayTime);
            inGameUI.windowClearStage.totalXPValue.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(popUpsDelayTime);
            inGameUI.windowClearStage.rewardedAdsButton.GetComponent<Animation>().Play();
            yield return new WaitForSeconds(popUpsDelayTime);
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
                GameControl.Instance.AudioManager().PauseGame();

            }
            else
            {
                inGameUI.play = true;
                inGameUI.playPauseButton.sprite = inGameUI.pauseImage;
                inGameUI.pauseBackground.enabled = false;
                GameControl.Instance.UnpauseGame();
                GameControl.Instance.AudioManager().ResumeGame();

            }
        }

        public void NotifyRewardedAdsLoaded()
        {
            inGameUI.windowClearStage.rewardedAdsButton.SetActive(true);
            inGameUI.windowClearStage.rewardedAdsButton.GetComponent<Animation>().Play();
        }

        public void AdsDoneShowClaimButton()
        {
            inGameUI.windowClearStage.rewardedAdsButton.SetActive(false);
            inGameUI.windowClearStage.rewardedXPClaimButton.SetActive(true);
            inGameUI.windowClearStage.rewardedXPClaimButtonText.SetText(GameControl.Instance.StageTotalXP().ToString());
            inGameUI.windowClearStage.rewardedCoinClaimButtonText.SetText(GameControl.Instance.StageRewardCoins().ToString());
            inGameUI.windowClearStage.rewardedXPClaimButton.GetComponent<Animation>().Play();
        }

        public void ClaimRewardAds()
        {

            GameControl.Instance.AdsDoneRewardPlayer();
            inGameUI.windowClearStage.rewardedXPClaimButton.SetActive(false);
            inGameUI.windowClearStage.rewardPopUp.SetActive(true);
            Invoke("HideRewardedPopUp", 1f);
            inGameUI.windowClearStage.rewardPopUp.GetComponent<Animation>().Play();
            InGameUISync();
        }

        void HideRewardedPopUp()
        {
            inGameUI.windowClearStage.rewardPopUp.SetActive(false);
        }


        #endregion

        #region MENU_UI_METHODS

        public void IntroMenuPlayButton()
        {
            StartCoroutine(IntroToMainMenuPlay());
        }

        IEnumerator IntroToMainMenuPlay()
        {
            yield return new WaitForSeconds(0.3f);
            GameControl.Instance.PlayDockStationMusic();
            yield return new WaitForSeconds(0.3f);
            MainMenuShowPlayMenu();
        }

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
            menuUI.flyingSaucerStatSpeed.SetText((1 + GameControl.Instance.playerData.playerSpeed).ToString());
            menuUI.flyingSaucerStatRaySize.SetText((1 + GameControl.Instance.playerData.rayRadius).ToString());
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

        public void MainMenuLevelUp(bool _speed, bool _raySize, bool _rayMultiplier)
        {
            menuUI.mainMenuScript.LevelUp(_speed, _raySize, _rayMultiplier);
        }

        public void NotifyInterstitialAdsLoaded()
        {
            menuUI.mainMenuScript.EnableStageLoadButton();
        }



        #endregion
    }
}