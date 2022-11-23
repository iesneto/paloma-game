using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Movendo a UI para o UIManager
//using UnityEngine.UI;
using System;

public class GameControl : MonoBehaviour
{

    
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
    [SerializeField] private int tutorialsNumber;
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private bool playerStartedMoving;
    [SerializeField] private List<FlyingSaucerData> flyingSaucers;
    [SerializeField] private List<CowData> cows;
    [SerializeField] private List<StageData> stages;
    [SerializeField] private int currentStage;
    //[SerializeField] private List<int> availableFlyingSaucersIds;
    
    private bool joystickChange;
    private int sceneBestTimeMinutes;
    private int sceneBestTimeSeconds;
    private int currentMinutes;
    private int currentSeconds;
    private int stageExtraXP;
    private int stageTotalXP;
    private int stageXP;
    private bool stageCleared;

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

    public delegate void  GameEvent();
    public static event GameEvent playerUpdate, addCoins, addEnergy, addCows;

    // [SerializeField] private GameObject inGameUI;
    // [SerializeField] private Text coinsValueShadow;
    // [SerializeField] private Text coinsValue;



    //[System.Serializable]
    //public struct ControlPlayerData
    //{
    //    public SystemLanguage language;
    //    public int rayRadius;
    //    public int rayForce;
    //    public int rayMultiplier;
    //    public int playerSpeed;
    //    public int playerEnergy;
    //    public int playerEnergyConsume;
    //    public long currentPoints;
    //    public long totalPoints;
    //    public int numCow01;
    //    public int numCow02;
    //    public int numCow03;
    //    public int numCow04;

    //    public ControlPlayerData(int p)
    //    {
    //        language = Application.systemLanguage;
    //        rayRadius = p;
    //        rayForce = p;
    //        rayMultiplier = p;
    //        playerSpeed = p;
    //        playerEnergy = p;
    //        playerEnergyConsume = p;
    //        currentPoints = 0;
    //        totalPoints = 0;
    //        numCow01 = 0;
    //        numCow02 = 0;
    //        numCow03 = 0;
    //        numCow04 = 0;
    //    }
    //}
    //public ControlPlayerData playerData;
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

        DontDestroyOnLoad(gameObject);
        _instance = this;
        playerData = new PlayerData();
        InitializePlayerDataLists();
        playerData.InitLanguage();
        
        fileManager = new FileManager();

        LoadData();
        
        sceneController = GetComponent<SceneController>();
        uiManager = GetComponent<UIManager>();
        StartMainMenu();

    }

    void InitializePlayerDataLists()
    {
        for(int i = 0; i < cows.Count; i++)
        {
            playerData.numCows.Add(0);
        }

        for (int i = 0; i < tutorialsNumber; i++)
        {
            playerData.tutorials.Add(false);
        }
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
        stagePlaying = true;
    }

    void CalculateStageTime()
    {
        //int totalTime =  60 + (sceneTotalCows * 5 / playerData.level)  - (5 * playerData.level);
        int totalTime = stages[currentStage].time;
        float timeAmortizationByPlayerLevel = (totalTime * 5 * playerData.level)/100;
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
        while(!playerStartedMoving)
        {
            yield return null;
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
    }

    void VerifyLevelUp()
    {
        if (playerData.level == playerExperienceToLevelUp.Count - 1) return; 
        
        if (playerData.experience >= playerExperienceToLevelUp[playerData.level])
        {
            // TO DO:  Level Up magic
            playerData.level++;
            uiManager.MainMenuLevelUp();
            // stats upgrades: speed, rayradius, raymultiplier
            switch (playerData.level)
            {
                case 2: playerData.playerSpeed += 1;
                    break;
                case 3: playerData.rayRadius += 1;
                    break;
                case 4: playerData.rayMultiplier += 1;
                    break;
                case 5: playerData.playerSpeed += 1;
                    break;
                case 6: playerData.rayRadius += 1;
                    break;
                case 7: playerData.rayMultiplier += 1;
                    break;
                case 8: playerData.playerSpeed += 1;
                    break;
                case 9: playerData.rayRadius += 1;
                    break;
                case 10: playerData.playerSpeed += 1;
                    break;
            }            
        }
        
        
    }

    public void AddCoins(CowBehavior cow)
    {
        playerData.currentCoins += cow.GetReward();
        playerData.totalCoins += cow.GetReward();
        AddExperience(cow.GetExperience());
        //switch(cow.cowData.id)
        //{
        //    case 0: 
        //        playerData.numCow01++;
        //        break;
        //    case 1:
        //        playerData.numCow02++;
        //        break;
        //    case 2:
        //        playerData.numCow03++;
        //        break;
        //    case 3:
        //        playerData.numCow04++;
        //        break;
        //    default:
        //        break;
        //}
        playerData.numCows[cow.cowData.id]++;
        StageState();
        uiManager.InGameUISync();
        SaveData();
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

    IEnumerator FinishStageShowRewards()
    {
        int playerTime = currentMinutes * 60 + currentSeconds;
        if(playerTime <= ((sceneBestTimeMinutes * 60) + sceneBestTimeSeconds))
        {
            stageExtraXP = stageXP / 2;            
        }
        else
        {
            stageExtraXP = 0;
        }
        
        stageTotalXP = stageXP + stageExtraXP;        
        yield return new WaitForSeconds(0.5f);        
        AddExperience(stageTotalXP);
        SaveData();
        uiManager.InGameUISync();
        uiManager.OpenStageCleared();
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
        switch(upgradeType)
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
        TriggerPlayerUpdate();
    }

    public void LoadData() 
    {
        fileManager.Load();
        TriggerPlayerUpdate();
    }

    private void TriggerPlayerUpdate()
    {
        if(playerUpdate != null)
        {
            playerUpdate();
        }
    }

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
        Time.timeScale = 0;
        player.GetComponent<PlayerBehavior>().Lock();
    }


    public void UnpauseGame()
    {
        Time.timeScale = 1;
        player.GetComponent<PlayerBehavior>().Unlock();
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

    public int cowsNumber()
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
        if(playerData.currentCoins >= flyingSaucers[id].value)
        {
            playerData.currentCoins -= flyingSaucers[id].value;
            playerData.purchasedFlyingSaucerModels.Add(id);
            SaveData();
            uiManager.MainMenuSyncUI();
        }
    }

    public void PurchaseCow(int id)
    {
        if(playerData.currentCoins >= cows[id].value)
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

    public int stagesNumber()
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
    //public void OnChangeJoystick(Vector2 position)
    //{

    //    //if((position.x <= 0.1f && position.x >= -0.1f) && (position.y <= 0.1f && position.y >= -0.1f))
    //    //{
    //    //    if (joystickChange)
    //    //    {
    //    //        joystickChange = false;
    //    //        player.GetComponent<PlayerBehavior>().StopPlayer();
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    joystickChange = true;
    //    //    player.GetComponent<PlayerBehavior>().MovePlayer(position);
    //    //}
    //}
    //public void MainMenuLoaded()
    //{
    //    if(MainMenuObj == null) MainMenuObj = GameObject.Find("MainMenuObject").GetComponent<MainMenu>();
    //    MainMenuObj.ShowPlayMenu();
    //}
}
