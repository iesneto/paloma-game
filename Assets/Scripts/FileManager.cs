using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


public class FileManager 
{
    string filePathStandAlone = "Resource";

    

    public void Save()
    {

        PlayerData playerData = PreparaDadosSalvamento();

        // Prepara o arquivo que será salvo em disco
        FileStream file;
        BinaryFormatter bf = new BinaryFormatter();
        string nomeArquivo = "./appData.db";
        string filePath;

#if UNITY_STANDALONE && !UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IPHONE

        filePath = filePathStandAlone;
#else
        // este código executará no editor, iphone e android builds
        filePath = Application.persistentDataPath;
#endif

        if (VerificaSeArquivoExiste(filePath, nomeArquivo))
        {
            file = File.Open(filePath + nomeArquivo, FileMode.Open);
        }
        else
        {
            file = File.Create(filePath + nomeArquivo);
        }

        //Copia os dados para o arquivo
        bf.Serialize(file, playerData);
        file.Close();
    }

    public void Load()
    {
        //abre o arquivo que contem os dados em disco
        FileStream file;
        BinaryFormatter bf = new BinaryFormatter();
        string nomeArquivo = "./appData.db";
        string filePath;

#if UNITY_STANDALONE && !UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IPHONE

        filePath = filePathStandAlone;
#else
        // este código executará no editor, iphone e android builds
        filePath = Application.persistentDataPath;
#endif


        if (VerificaSeArquivoExiste(filePath, nomeArquivo))
        {
            file = File.Open(filePath + nomeArquivo, FileMode.Open);
        }
        else
        {
            file = File.Create(filePath + nomeArquivo);
            //return;
        }
        // Prepara a estrutura de dados no app
        PlayerData playerData = new PlayerData();
        if (file.Length != 0)
            playerData = (PlayerData)bf.Deserialize(file);

        file.Close();


        PreparaDadosCarregamento(playerData);
    }

    public bool VerificaSeArquivoExiste(string filepath, string nome)
    {
        return File.Exists(filepath + nome);
    }

    private PlayerData PreparaDadosSalvamento()
    {
        // Cria a Classe Serializable e que conterá os dados criados no app
        PlayerData playerData = new PlayerData();

        // Em atualizações futuras deve-se verificar a versão do arquivo para inclusão de novos dados
        playerData.rayRadius = GameControl.Instance.playerData.rayRadius;
        playerData.rayForce = GameControl.Instance.playerData.rayForce;
        playerData.rayMultiplier = GameControl.Instance.playerData.rayMultiplier;
        playerData.playerSpeed = GameControl.Instance.playerData.playerSpeed;
        playerData.playerEnergy = GameControl.Instance.playerData.playerEnergy;
        playerData.playerEnergyConsume = GameControl.Instance.playerData.playerEnergyConsume;
        playerData.currentPoints = GameControl.Instance.playerData.currentPoints;
        playerData.totalPoints = GameControl.Instance.playerData.totalPoints;
        playerData.numCow01 = GameControl.Instance.playerData.numCow01;
        playerData.numCow02 = GameControl.Instance.playerData.numCow02;
        playerData.numCow03 = GameControl.Instance.playerData.numCow03;
        playerData.numCow04 = GameControl.Instance.playerData.numCow04;
    
        //appData.name = Core.Instance.currentProjectName;


        return playerData;
    }

    private void PreparaDadosCarregamento(PlayerData playerData)
    {
        GameControl.Instance.playerData.rayRadius = playerData.rayRadius;
        GameControl.Instance.playerData.rayForce = playerData.rayForce;
        GameControl.Instance.playerData.rayMultiplier = playerData.rayMultiplier;
        GameControl.Instance.playerData.playerSpeed = playerData.playerSpeed;
        GameControl.Instance.playerData.playerEnergy = playerData.playerEnergy;
        GameControl.Instance.playerData.playerEnergyConsume = playerData.playerEnergyConsume;
        GameControl.Instance.playerData.currentPoints = playerData.currentPoints;
        GameControl.Instance.playerData.totalPoints = playerData.totalPoints;
        GameControl.Instance.playerData.numCow01 = playerData.numCow01;
        GameControl.Instance.playerData.numCow02 = playerData.numCow02;
        GameControl.Instance.playerData.numCow03 = playerData.numCow03;
        GameControl.Instance.playerData.numCow04 = playerData.numCow04;

    }


}

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

    public PlayerData()
    {
        language = Application.systemLanguage;
        rayRadius = 0;
        rayForce = 0 ;
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
    }

}



