using System;
using System.Collections.Generic;
using UnityEngine;


//namespace Gamob
//{
    [System.Serializable]
    public class PlayerData
    {

        public SystemLanguage language;
        public int rayRadius;
        public int rayForce;
        public int rayMultiplier;
        public int playerSpeed;
        public int playerEnergy;
        public int playerEnergyConsume;
        public long currentCoins;
        public long totalCoins;
        //public int numCow01;
        //public int numCow02;
        //public int numCow03;
        //public int numCow04;
        public List<int> numCows;
        public int level;
        // Update 01/10/2022 - Ivo Seitenfus
        [SerializeField] public DateTime createDate;
        public DateTime lastSaved;
        public TimeSpan timePlayed;
        public double flyDistance;
        // Update 15/10/2022 - Ivo Seitenfus
        public long experience;
        // Update 17/10/2022 - Ivo Seitenfus
        public List<int> purchasedCows;
        //Update 21/10/2022 - Ivo Seitenfus
        public List<int> purchasedFlyingSaucerModels;
        public int flyingSaucerModelId;
        // Update 26/10/2022 - Ivo Seitenfus
        public List<bool> tutorials;
        // Update 04/11/2022 - Ivo Seitenfus
        public List<int> purchasedStages;
        // Update 01/12/2022 - Ivo Seitenfus
        public int timesRandomPlay;
        public int timesSelectPlay;
        public int timesRewardedAds;
        public long inGameTime;
        public long outGameTime;
        // Update 08/12/2022 - Ivo Seitenfus
        public bool music;
        public bool sfx;
        // Update 14/12/2022 - Ivo Seitenfus
        public int timesStageClearInTime;
        public int timesStageClearOutTime;
        public int timesDropStage;

        public PlayerData()
        {
            //language = Application.systemLanguage;
            rayRadius = 0;
            rayForce = 1;
            rayMultiplier = 0;
            playerSpeed = 0;
            playerEnergy = 0;
            playerEnergyConsume = 0;
            currentCoins = 0;
            totalCoins = 0;
            //numCow01 = 0;
            //numCow02 = 0;
            //numCow03 = 0;
            //numCow04 = 0;
            level = 1;
            // Update 01/10/2022 - Ivo Seitenfus
            createDate = DateTime.Now;
            lastSaved = DateTime.Now;
            timePlayed = lastSaved - createDate;
            flyDistance = 0f;
            experience = 0;
            numCows = new List<int>();
            // Update 17/10/2022 - Ivo Seitenfus
            purchasedCows = new List<int>();
            //Update 21/10/2022 - Ivo Seitenfus
            purchasedFlyingSaucerModels = new List<int>();
            flyingSaucerModelId = 0;
            // Update 26/10/2022 - Ivo Seitenfus
            tutorials = new List<bool>();
            // Update 04/11/2022 - Ivo Seitenfus
            purchasedStages = new List<int>();
            // Update 01/12/2022 - Ivo Seitenfus
            timesRandomPlay = 0;
            timesSelectPlay = 0;
            timesRewardedAds = 0;
            inGameTime = 0;
            outGameTime = 0;
            // Update 08/12/2022 - Ivo Seitenfus
            music = true;
            sfx = true;
            // Update 14/12/2022 - Ivo Seitenfus
            timesStageClearInTime = 0;
            timesStageClearOutTime = 0;
            timesDropStage = 0;
        }

        public void InitLanguage()
        {
            language = Application.systemLanguage;
        }

        public void SetLocalization(SystemLanguage p_language)
        {
            language = p_language;
        }

    }
//}