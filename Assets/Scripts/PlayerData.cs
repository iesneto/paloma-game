using System;
using UnityEngine;

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
    public long currentPoints;
    public long totalPoints;
    public int numCow01;
    public int numCow02;
    public int numCow03;
    public int numCow04;
    public int level;
    // Update 01/10/2022 - Ivo Seitenfus
    [SerializeField]public DateTime createDate;
    public DateTime lastSaved;
    public TimeSpan timePlayed;
    public double flyDistance;


    public PlayerData()
    {
        //language = Application.systemLanguage;
        rayRadius = 0;
        rayForce = 0;
        rayMultiplier = 0;
        playerSpeed = 0;
        playerEnergy = 0;
        playerEnergyConsume = 0;
        currentPoints = 0;
        totalPoints = 0;
        numCow01 = 0;
        numCow02 = 0;
        numCow03 = 0;
        numCow04 = 0;
        level = 1;
        // Update 01/10/2022 - Ivo Seitenfus
        createDate = DateTime.Now;
        lastSaved = DateTime.Now;
        timePlayed = lastSaved - createDate;
        flyDistance = 0f;
    }

    public void InitLanguage()
    {
        language = Application.systemLanguage;
        Debug.Log(language);
    }

    public void SetLocalization(SystemLanguage p_language)
    {
        language = p_language;
    }

}
