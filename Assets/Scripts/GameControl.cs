using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameControl : MonoBehaviour
{
    
    
    [SerializeField] private bool startedPlaying;
    [SerializeField] private MainMenu MainMenuObj;
    
    [SerializeField] private FileManager fileManager;
    [SerializeField] private SceneController sceneController;

    [SerializeField] private GameObject inGameUI;
    [SerializeField] private Text coinsValueShadow;
    [SerializeField] private Text coinsValue;
   
    

    [System.Serializable]
    public struct ControlPlayerData
    {
        public SystemLanguage language;
        public int rayRadius;
        public int rayForce;
        public int rayMultiplier;
        public int playerSpeed;
        public int playerEnergy;
        public int playerEnergyConsume;
        public long currentPoints;
        public long totalPoints;
        public int numCow01;
        public int numCow02;
        public int numCow03;
        public int numCow04;
        

        public ControlPlayerData(int p)
        {
            language = Application.systemLanguage;
            rayRadius = p;
            rayForce = p;
            rayMultiplier = p;
            playerSpeed = p;
            playerEnergy = p;
            playerEnergyConsume = p;
            currentPoints = 0;
            totalPoints = 0;
            numCow01 = 0;
            numCow02 = 0;
            numCow03 = 0;
            numCow04 = 0;
        }
    }
    public ControlPlayerData playerData;

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
        fileManager = new FileManager();
        playerData = new ControlPlayerData(0);
        fileManager.Load();
        sceneController = GetComponent<SceneController>();
        coinsValue.text = playerData.currentPoints.ToString();
        coinsValueShadow.text = coinsValue.text;

        
    }


    

    public void StartPlaying()
    {
        startedPlaying = true;
    }

    public void SetupMainMenu(MainMenu m)
    {
        MainMenuObj = m;
        if (startedPlaying) MainMenuObj.ShowPlayMenu();
        else MainMenuObj.ShowIntroMenu();
    }


    public void LoadMainMenu()
    {
        sceneController.LoadMainMenu();
        inGameUI.SetActive(false);
        
    }

    public void ShowInGameUI()
    {
        inGameUI.SetActive(true);
        coinsValue.text = playerData.currentPoints.ToString();
        coinsValueShadow.text = coinsValue.text;
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
        coinsValue.text = playerData.currentPoints.ToString();
        coinsValueShadow.text = coinsValue.text;
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
        MainMenuObj.ShowPlayMenu();
    }

    //public void MainMenuLoaded()
    //{
    //    if(MainMenuObj == null) MainMenuObj = GameObject.Find("MainMenuObject").GetComponent<MainMenu>();
    //    MainMenuObj.ShowPlayMenu();
    //}
}
