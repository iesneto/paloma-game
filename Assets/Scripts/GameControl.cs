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
    
    [SerializeField] private FileManager fileManager;
    [SerializeField] private SceneController sceneController;
    [SerializeField] private UIManager uiManager;

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
                Debug.LogError("Core is null");
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
        
        fileManager = new FileManager();
        //playerData = new ControlPlayerData(0);
        
        fileManager.Load();
        sceneController = GetComponent<SceneController>();
        uiManager = GetComponent<UIManager>();
        uiManager.MainMenuStart(startedPlaying);

        
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
        uiManager.MainMenuStart(startedPlaying);
        //ShowInGameUI();
        
    }

    public void LevelLoaded()
    {
        uiManager.InGameUIStart();
    }

    public void AddCoins(CowBehavior cow)
    {
        playerData.currentPoints += cow.GetReward();
        playerData.totalPoints += cow.GetReward();
        switch(cow.cowData.id)
        {
            case 0: 
                playerData.numCow01++;
                break;
            case 1:
                playerData.numCow02++;
                break;
            case 2:
                playerData.numCow03++;
                break;
            case 3:
                playerData.numCow04++;
                break;
            default:
                break;
        }
        uiManager.InGameUISync();
        fileManager.Save();
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
        playerData.currentPoints -= value;
        fileManager.Save();
        uiManager.MainMenuShowPlayMenu();
    }

    //public void MainMenuLoaded()
    //{
    //    if(MainMenuObj == null) MainMenuObj = GameObject.Find("MainMenuObject").GetComponent<MainMenu>();
    //    MainMenuObj.ShowPlayMenu();
    //}
}
