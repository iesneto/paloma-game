using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Movendo a UI para o UIManager
//using UnityEngine.UI;
using System;

namespace Gamob
{
    public class GameControl : MonoBehaviour
    {

        //[SerializeField] private AdsManager _adsManager;
        [SerializeField] private Advertisements _adsManager;
        [SerializeField] private bool startedPlaying;
        // [SerializeField] private MainMenu MainMenuObj;
        [SerializeField] private GameObject player;
        [SerializeField] private FileManager fileManager;
        [SerializeField] private SceneController sceneController;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private Audio audioManager;
        //[SerializeField] private List<GameObject> cowPrefabs;
        [SerializeField] private List<int> playerExperienceToLevelUp;
        //[SerializeField] private List<int> playerLevelToUnlockCows;
        //[SerializeField] private List<int> playerLevelToUnlockFlysaucers;
        [SerializeField] private int sceneTotalCows;
        [SerializeField] private bool stagePlaying;
        //[SerializeField] private int stageClearedExperience;
        //[SerializeField] private int tutorialsNumber;
        [SerializeField] private Tutorial tutorial;
        [SerializeField] private bool playerStartedMoving;
        [SerializeField] private List<FlyingSaucerData> flyingSaucers;
        [SerializeField] private List<CowData> cows;
        [SerializeField] private List<StageData> stages;
        [SerializeField] private int currentStage;
        [SerializeField] private int selectPlayCounter;
        [SerializeField] private int selectPlayMaxWithoutAds;
        [SerializeField] private bool randomPlay;
        [SerializeField] private float amortizationFactor;
        private bool dropped;
        public bool StageVictory { get; set; }

        //[SerializeField] private List<int> availableFlyingSaucersIds;

        private bool joystickChange;
        private int sceneBestTimeMinutes;
        private int sceneBestTimeSeconds;
        private int currentMinutes;
        private int currentSeconds;
        private int stageExtraXP;
        private int rewardXP;
        private int stageTotalXP;
        private int stageXP;
        private bool stageCleared;
        private int stageRewardCoin;


        public int SceneTotalCows
        {
            get
            {
                return sceneTotalCows;
            }
            set
            {
                sceneTotalCows = value;
            }
        }

        [SerializeField] private int sceneCurrentCows;
        public int SceneCurrentCows
        {
            get
            {
                return sceneCurrentCows;
            }
            set
            {
                sceneCurrentCows = value;
            }
        }

        //public delegate void  GameEvent();
        //public static event GameEvent playerUpdate, addCoins, addEnergy, addCows;

        public PlayerData playerData;

        private static GameControl _instance;
        public static GameControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    //Debug.Log("Core is null");
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != this && _instance != null)
            {
                Destroy(gameObject);
                return;
            }

            //TODO: OnButtonCallRateUS
            //Application.OpenURL ("market://details?id=" + Application.productName);

            DontDestroyOnLoad(gameObject);
            _instance = this;
            playerData = new PlayerData();
            InitializePlayerDataLists();
            playerData.InitLanguage();

            fileManager = new FileManager();

            LoadData();

            sceneController = GetComponent<SceneController>();
            uiManager = GetComponent<UIManager>();
            _adsManager = GetComponent<Advertisements>();
            InitializeSoundConfig();
            StartMainMenu();

        }

        void InitializeSoundConfig()
        {
            audioManager.Initialize();
            uiManager.SetSoundConfiguration(playerData.music, playerData.sfx);
            if (!playerData.music) audioManager.DisableMusic();
            if (!playerData.sfx) audioManager.DisableSFX();
        }

        void InitializePlayerDataLists()
        {
            for (int i = 0; i < cows.Count; i++)
            {
                playerData.numCows.Add(0);
            }

            for (int i = 0; i < tutorial.TutorialsNumber(); i++)
            {
                
                playerData.tutorials.Add(false);
            }
        }

        public void Shocked()
        {
            playerData.timesShocked++;
            SaveData();
        }



        public void StartPlaying()
        {
            startedPlaying = true;
        }

        //public void SetupMainMenu()
        //{
        //    MainMenuObj = m;
        //    // AQUI, deve trocar para o UIManager controlar o MainMenu
        //    if (startedPlaying) MainMenuObj.ShowPlayMenu();
        //    else MainMenuObj.ShowIntroMenu();
        //}


        public void LoadMainMenu()
        {
            audioManager.StopAmbientDay();
            audioManager.StopAmbientNight();
            sceneController.LoadMainMenu();
            SaveData();
            stagePlaying = false;

            //StartMainMenu();
        }

        void ResetInGameData()
        {
            currentSeconds = 0;
            currentMinutes = 0;
            sceneBestTimeMinutes = 0;
            sceneBestTimeSeconds = 0;
            sceneCurrentCows = 0;
            sceneTotalCows = 0;
            stageCleared = true;
            uiManager.ResetCowsFillBar();
            uiManager.ResetTimers();            
            
        }

        public void StartMainMenu()
        {

            if (startedPlaying)
            {
                uiManager.LoadMainMenuPlay();
                PlayDockStationMusic();

                if (_adsManager.isReady && !_adsManager.interstitialLoaded)
                {
                    _adsManager.LoadInterstitialAd();
                }
            }
            else
            {
                uiManager.LoadMainMenuIntro();
                PlayIntroMusic();


            }
            ResetInGameData();
        }



        public void PlayDockStationMusic()
        {
            audioManager.PlayDockStationMusic();
        }

        public void PlayIntroMusic()
        {
            audioManager.PlayIntroMusic();
        }

        public void ScreenFadeOut()
        {
            if (stagePlaying)
            {
                playerStartedMoving = false;
                uiManager.ShowWaitPlayerStartUI();
                tutorial.WakeUp();
                
                StartCoroutine("StartLevel");
            }
        }

        public void SetPlayer(GameObject _player)
        {
            player = _player;
        }

        public void SetStage(int stage)
        {
            currentStage = stage;
            stageXP = stages[currentStage].xp;
        }


        public void LevelLoaded()
        {
            CalculateStageTime();
            //player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerBehavior>().Lock();
            uiManager.InGameUIStart();
            audioManager.PlayStageMusic();
            if (stages[currentStage].shift == StageData.StageShift.DAY)
            {
                audioManager.PlayAmbientDay();
            }
            else
            {
                audioManager.PlayAmbientNight();
            }



            stagePlaying = true;

            if (_adsManager.isReady && !_adsManager.rewardedLoaded)
            {

                _adsManager.LoadRewardedAd();
            }
        }

        void CalculateStageTime()
        {
            //int totalTime =  60 + (sceneTotalCows * 5 / playerData.level)  - (5 * playerData.level);
            int totalTime = stages[currentStage].time;
            float timeAmortizationByPlayerLevel = (totalTime * amortizationFactor * playerData.level) / 100;
            //Debug.Log("time amortization: " + timeAmortizationByPlayerLevel);
            int stageTime = totalTime - (int)Mathf.Ceil(timeAmortizationByPlayerLevel);

            sceneBestTimeMinutes = stageTime / 60;
            sceneBestTimeSeconds = stageTime % 60;

            string seconds = (sceneBestTimeSeconds < 10) ? "0" + sceneBestTimeSeconds.ToString() : sceneBestTimeSeconds.ToString();
            string minutes = sceneBestTimeMinutes.ToString();

            uiManager.UpdateBestTime(minutes + ":" + seconds);


        }

        public void StartMoving()
        {

            playerStartedMoving = true;
        }



        IEnumerator StartLevel()
        {
            dropped = false;
            while (!playerStartedMoving)
            {
                yield return null;
                if (dropped)
                {                    
                    yield break;
                }
            }           
            uiManager.ClosePlayerStartedUI();
            stageCleared = false;
            player.GetComponent<PlayerBehavior>().Unlock();
            StartCoroutine("StartTimer");
        }

        IEnumerator StartTimer()
        {

            while (!stageCleared)
            {

                currentSeconds++;
                if (currentSeconds >= 60)
                {
                    currentMinutes++;
                    currentSeconds = 0;
                }
                string seconds = (currentSeconds < 10) ? "0" + currentSeconds.ToString() : currentSeconds.ToString();
                string minutes = currentMinutes.ToString();

                uiManager.UpdateCurrentTime(minutes + ":" + seconds);
                yield return new WaitForSeconds(1);
            }
        }

        public void AddExperience(int xp)
        {

            playerData.experience += xp;
            VerifyLevelUp();
            SaveData();
        }

        void VerifyLevelUp()
        {
            if (playerData.level == playerExperienceToLevelUp.Count - 1) return;

            if (playerData.experience >= playerExperienceToLevelUp[playerData.level])
            {                
                playerData.level++;
                bool levelUpSpeed, levelUpRayRadius, levelUpRayMultiplier;
                levelUpSpeed = levelUpRayRadius = levelUpRayMultiplier = false;
                // stats upgrades: speed, rayradius, raymultiplier
                switch (playerData.level)
                {
                    case 2:
                        playerData.playerSpeed += 1;
                        levelUpSpeed = true;
                        break;
                    case 3:
                        playerData.rayRadius += 1;
                        levelUpRayRadius = true;
                        break;
                    case 4:
                        playerData.rayMultiplier += 1;
                        levelUpRayMultiplier = true;
                        break;
                    case 5:
                        playerData.playerSpeed += 1;
                        levelUpSpeed = true;
                        break;
                    case 6:
                        playerData.rayRadius += 1;
                        levelUpRayRadius = true;
                        break;
                    case 7:
                        playerData.rayMultiplier += 1;
                        levelUpRayMultiplier = true;
                        break;
                    case 8:
                        playerData.playerSpeed += 1;
                        levelUpSpeed = true;
                        break;
                    case 9:
                        playerData.rayRadius += 1;
                        levelUpRayRadius = true;
                        break;
                    case 10:
                        playerData.rayMultiplier += 1;
                        levelUpRayMultiplier = true;
                        break;
                    case 11:
                        playerData.playerSpeed += 1;
                        levelUpSpeed = true;
                        break;
                    case 12:
                        playerData.rayRadius += 1;
                        levelUpRayRadius = true;
                        break;
                    case 13:
                        playerData.rayMultiplier += 1;
                        levelUpRayMultiplier = true;
                        break;
                    case 14:
                        playerData.playerSpeed += 1;
                        levelUpSpeed = true;
                        break;
                    case 15:
                        playerData.rayRadius += 1;
                        levelUpRayRadius = true;
                        break;
                }
                uiManager.MainMenuLevelUp(levelUpSpeed, levelUpRayRadius, levelUpRayMultiplier);
            }


        }

        public void AddCowCoinsAndXp(CowBehavior cow)
        {
            AddCoins(cow.GetReward());
            playerData.numCows[cow.cowData.id]++;
            StageState();
            AddExperience(cow.GetExperience());
            uiManager.InGameUISync();
        }

        void AddCoins(int _coins)
        {
            playerData.currentCoins += _coins;
            playerData.totalCoins += _coins;
        }

        void StageState()
        {
            SceneCurrentCows++;
            if (SceneCurrentCows == SceneTotalCows)
            {
                stageCleared = true;
                StartCoroutine("FinishStageShowRewards");
            }
        }

        public void DropStage()
        {
            dropped = true;            
            playerData.timesDropStage++;
            SaveData();
        }

        IEnumerator FinishStageShowRewards()
        {
            int playerTime = currentMinutes * 60 + currentSeconds;
            if (playerTime <= ((sceneBestTimeMinutes * 60) + sceneBestTimeSeconds))
            {
                playerData.timesStageClearInTime++;
                stageExtraXP = stageXP / 2;
                StageVictory = true;
            }
            else
            {
                playerData.timesStageClearOutTime++;
                stageExtraXP = 0;
                StageVictory = false;
            }

            stageTotalXP = stageXP + stageExtraXP;
            rewardXP = stageTotalXP;
            stageRewardCoin = stages[currentStage].rewardCoins;
            yield return new WaitForSeconds(0.5f);
            AddExperience(stageTotalXP);
            uiManager.InGameUISync();
            uiManager.OpenStageCleared();
            uiManager.DisableWarningIcon();
            audioManager.WarningStop();
            player.GetComponent<PlayerBehavior>().Lock();
        }

        public int StageMinutes()
        {
            return sceneBestTimeMinutes;
        }
        public int StageSeconds()
        {
            return sceneBestTimeSeconds;
        }

        public int PlayerMinutes()
        {
            return currentMinutes;
        }

        public int PlayerSeconds()
        {
            return currentSeconds;
        }

        public int StageXP()
        {
            return stageXP;
        }

        public int StageExtraXP()
        {
            return stageExtraXP;
        }

        public int StageTotalXP()
        {
            return stageTotalXP;
        }

        public void PurchaseUpgrade(UpgradeData.ControlVar upgradeType, long value)
        {
            switch (upgradeType)
            {
                case UpgradeData.ControlVar.RAYRADIUS:
                    playerData.rayRadius++;
                    break;
                case UpgradeData.ControlVar.RAYFORCE:
                    playerData.rayForce++;
                    break;
                case UpgradeData.ControlVar.RAYMULTIPLIER:
                    playerData.rayMultiplier++;
                    break;
                case UpgradeData.ControlVar.PLAYERSPEED:
                    playerData.playerSpeed++;
                    break;
                case UpgradeData.ControlVar.PLAYERENERGY:
                    playerData.playerEnergy++;
                    break;
                case UpgradeData.ControlVar.PLAYERENERGYCONSUME:
                    playerData.playerEnergyConsume++;
                    break;
                default:
                    break;
            }
            playerData.currentCoins -= value;
            SaveData();
            uiManager.MainMenuShowPlayMenu();
        }

        public void SaveData()
        {
            playerData.lastSaved = DateTime.Now;
            playerData.timePlayed = playerData.lastSaved - playerData.createDate;


            fileManager.Save();
            //TriggerPlayerUpdate();
        }

        public void LoadData()
        {
            fileManager.Load();
            //TriggerPlayerUpdate();
        }

        //private void TriggerPlayerUpdate()
        //{
        //    if(playerUpdate != null)
        //    {
        //        playerUpdate();
        //    }
        //}

        public void AddFlyDistance(float distance)
        {
            playerData.flyDistance += distance;
        }

        public int PlayerExperienceToLevelUp()
        {
            return (playerData.level < playerExperienceToLevelUp.Count) ? playerExperienceToLevelUp[playerData.level] : 0;
        }

        public int PlayerStartExperienceOfLevel()
        {
            return playerExperienceToLevelUp[playerData.level - 1];
        }



        public void PauseGame()
        {
            StartCoroutine(MuteAudioListener());
            Time.timeScale = 0;
            player.GetComponent<PlayerBehavior>().Lock();
        }

        IEnumerator MuteAudioListener()
        {
            yield return new WaitForSecondsRealtime(0.7f);
            AudioListener.volume = 0;
        }


        public void UnpauseGame()
        {
            AudioListener.volume = 1;
            Time.timeScale = 1;
            if (playerStartedMoving) player.GetComponent<PlayerBehavior>().Unlock();
        }

        public UIManager GameControlUI()
        {
            return uiManager;
        }

        public List<CowData> PurchasedCows()
        {
            List<CowData> _cows = new List<CowData>();
            foreach (CowData c in cows)
            {
                if (playerData.purchasedCows.Contains(c.id))
                {
                    _cows.Add(c);
                }
            }
            return _cows;
        }

        public CowData CowByIndex(int index)
        {
            return cows[index];
        }

        public int CowsNumber()
        {
            return cows.Count;
        }

        public bool IsCowUnlocked(int index)
        {
            return (cows[index].levelToUnlock <= playerData.level);
        }

        public GameObject FlyingSaucerModelByIndex(int index)
        {
            return flyingSaucers[index].model;

        }

        public FlyingSaucerData FlyingSaucerDataByIndex(int index)
        {
            return flyingSaucers[index];
        }

        public int FlyingSaucersNumber()
        {
            return flyingSaucers.Count;
        }

        public bool IsFlyingSaucerUnlocked(int index)
        {
            return (flyingSaucers[index].levelToUnlock <= playerData.level);
        }

        //public int LeveltoUnlockFlyingSaucer(int index)
        //{
        //    return flyingSaucers[index].levelToUnlock;
        //}

        public void SelectFlyingSaucer(int index)
        {
            playerData.flyingSaucerModelId = index;
            SaveData();
        }

        public void PurchaseFlyingSaucer(int id)
        {
            if (playerData.currentCoins >= flyingSaucers[id].value)
            {
                playerData.currentCoins -= flyingSaucers[id].value;
                playerData.purchasedFlyingSaucerModels.Add(id);
                SaveData();
                uiManager.MainMenuSyncUI();
            }
        }

        public void PurchaseCow(int id)
        {
            if (playerData.currentCoins >= cows[id].value)
            {
                playerData.currentCoins -= cows[id].value;
                playerData.purchasedCows.Add(id);
                SaveData();
                uiManager.MainMenuSyncUI();
            }
        }

        public List<int> StagesPurchased()
        {
            return playerData.purchasedStages;
        }

        public StageData StageDatabyIndex(int index)
        {
            return stages[index];
        }

        public int StagesNumber()
        {
            return stages.Count;
        }

        public int SceneBuildIndex(int _stage)
        {
            return stages[_stage].buildID;
        }

        public bool IsStageUnlocked(int index)
        {
            return (stages[index].levelToUnlock <= playerData.level);
        }

        public void PurchaseStage(int id)
        {

            if (playerData.currentCoins >= stages[id].value)
            {
                playerData.currentCoins -= stages[id].value;
                playerData.purchasedStages.Add(id);
                SaveData();
                uiManager.MainMenuSyncUI();
            }

        }

        public bool CanShowAdsIcon()
        {
            if (selectPlayCounter >= selectPlayMaxWithoutAds - 1 && _adsManager.interstitialLoaded) return true;

            return false;
        }

        public void PrepareToLoadStage(int stageNumber)
        {
            if (stageNumber < 0)
            {
                randomPlay = true;
                LoadStage();
                
                //randomPlayCounter++;
                //if(randomPlayCounter >= randomPlayMaxWithoutAds && _adsManager.interstitialLoaded)
                //{
                //    randomPlayCounter = 0;
                //    _adsManager.ShowInterstitialAd();
                //}
                //else
                //{
                //    LoadStage();
                //}
            }
            else
            {
                selectPlayCounter++;
                if(selectPlayCounter >= selectPlayMaxWithoutAds && _adsManager.interstitialLoaded)
                {
                    selectPlayCounter = 0;
                    SetStageToLoadShowInterstitialAds(stageNumber);
                }
                else
                {
                    SetStage(stageNumber);
                    LoadStage();
                }



            }
        }

        public void SetStageToLoadShowInterstitialAds(int stageIndex)
        {
            SetStage(stageIndex);
            _adsManager.ShowInterstitialAd();
        }

        public void LoadStage()
        {
                       
            StartPlaying();
            if(randomPlay)
            {
                sceneController.LoadRandomLevel();                
            }
            else
            {
                sceneController.LoadLevel(StageDatabyIndex(currentStage).id);
            }
            randomPlay = false;
        }

        public void AdsDoneRewardPlayer()
        {
            playerData.timesRewardedAds++;
            AddCoins(stageRewardCoin);
            AddExperience(rewardXP);
        }

        public int StageRewardCoins()
        {
            return stageRewardCoin;
        }

        public void SetPlayerDataSelectStage()
        {
            playerData.timesSelectPlay++;
            SaveData();
        }

        public void SetPlayerDataRandomStage()
        {
            playerData.timesRandomPlay++;
            SaveData();
        }

        public Audio AudioManager()
        {
            return audioManager;
        }

        public FlyingSaucerAudio PlayerAudio()
        {
            return player.GetComponentInChildren<FlyingSaucerAudio>();
        }

        public void ToggleSFX(bool sfx)
        {
            if (fileManager == null) return;
            playerData.sfx = sfx;
            SaveData();
            if (sfx)
            {
                audioManager.EnableSFX();
            }
            else
            {
                audioManager.DisableSFX();
            }
            uiManager.SetSoundConfiguration(playerData.music, playerData.sfx);
        }

        public void ToggleMusic(bool music)
        {
            if (fileManager == null) return;
            playerData.music = music;
            SaveData();
            if (music)
            {
                audioManager.EnableMusic();
            }
            else
            {
                audioManager.DisableMusic();
            }
            uiManager.SetSoundConfiguration(playerData.music, playerData.sfx);
        }

        public void SetTutorialCloud()
        {
            if (!playerData.tutorials[2]) tutorial.ShowTutorial02();
        }

        public String CurrentStage()
        {
            return stages[currentStage].nameID.ToString();
        }
    }
}