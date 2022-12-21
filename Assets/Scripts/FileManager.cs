using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

namespace Gamob
{
    public class FileManager
    {
        string filePathStandAlone = "Resource";
        string fileName = "/appData.db";




        public void Save()
        {

            PlayerData playerData = PreparaDadosSalvamento();

            // Prepara o arquivo que será salvo em disco
            FileStream file;
            BinaryFormatter bf = new BinaryFormatter();
            string nomeArquivo = fileName;
            string filePath = FilePath();

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

            string nomeArquivo = fileName;
            string filePath = FilePath();


            if (VerificaSeArquivoExiste(filePath, nomeArquivo))
            {
                file = File.Open(filePath + nomeArquivo, FileMode.Open);
            }
            else
            {
                file = File.Create(filePath + nomeArquivo);
            }
            // Prepara a estrutura de dados no app
            PlayerData playerData = GameControl.Instance.playerData;

            if (file.Length != 0)
                //playerData = (PlayerData)bf.Deserialize(file);
                playerData = bf.Deserialize(file) as PlayerData;

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
            // Update 17-09-2022: Não precisa verificar versão, da forma que está feito pode ser incluso qualquer
            // dado novo e inicializa-lo no construtor que ele vai ser gravado no arquivo, a deserialização sempre mantem
            // a estrutura dos dados da classe, se não possuir algum campo simplesmente é ignorado, e a gravação
            // é sempre sobrescrita então não verifica se há ou não o campo no arquivo antigo.
            playerData.rayRadius = GameControl.Instance.playerData.rayRadius;
            playerData.rayForce = GameControl.Instance.playerData.rayForce;
            playerData.rayMultiplier = GameControl.Instance.playerData.rayMultiplier;
            playerData.playerSpeed = GameControl.Instance.playerData.playerSpeed;
            playerData.playerEnergy = GameControl.Instance.playerData.playerEnergy;
            playerData.playerEnergyConsume = GameControl.Instance.playerData.playerEnergyConsume;
            playerData.currentCoins = GameControl.Instance.playerData.currentCoins;
            playerData.totalCoins = GameControl.Instance.playerData.totalCoins;
            //playerData.numCow01 = GameControl.Instance.playerData.numCow01;
            //playerData.numCow02 = GameControl.Instance.playerData.numCow02;
            //playerData.numCow03 = GameControl.Instance.playerData.numCow03;
            //playerData.numCow04 = GameControl.Instance.playerData.numCow04;
            playerData.level = GameControl.Instance.playerData.level;
            playerData.language = GameControl.Instance.playerData.language;
            // Update 01/10/2022 - Ivo Seitenfus
            playerData.createDate = GameControl.Instance.playerData.createDate;
            playerData.lastSaved = GameControl.Instance.playerData.lastSaved;
            playerData.timePlayed = GameControl.Instance.playerData.timePlayed;
            playerData.flyDistance = GameControl.Instance.playerData.flyDistance;
            // Update 15/10/2022 - Ivo Seitenfus
            playerData.experience = GameControl.Instance.playerData.experience;
            playerData.numCows = GameControl.Instance.playerData.numCows;
            // Update 17/10/2022 - Ivo Seitenfus
            playerData.purchasedCows = GameControl.Instance.playerData.purchasedCows;
            //Update 21/10/2022 - Ivo Seitenfus
            playerData.purchasedFlyingSaucerModels = GameControl.Instance.playerData.purchasedFlyingSaucerModels;
            playerData.flyingSaucerModelId = GameControl.Instance.playerData.flyingSaucerModelId;
            // Update 26/10/2022 - Ivo Seitenfus
            playerData.tutorials = GameControl.Instance.playerData.tutorials;
            // Update 04/11/2022 - Ivo Seitenfus
            playerData.purchasedStages = GameControl.Instance.playerData.purchasedStages;
            // Update 01/12/2022 - Ivo Seitenfus
            playerData.timesSelectPlay = GameControl.Instance.playerData.timesSelectPlay;
            playerData.timesRandomPlay = GameControl.Instance.playerData.timesRandomPlay;
            playerData.timesRewardedAds = GameControl.Instance.playerData.timesRewardedAds;
            playerData.outGameTime = GameControl.Instance.playerData.outGameTime;
            playerData.inGameTime = GameControl.Instance.playerData.inGameTime;

            // Update 08/12/2022 - Ivo Seitenfus
            playerData.music = GameControl.Instance.playerData.music;
            playerData.sfx = GameControl.Instance.playerData.sfx;

            // Update 14/12/2022 - Ivo Seitenfus
            playerData.timesStageClearInTime = GameControl.Instance.playerData.timesStageClearInTime;
            playerData.timesStageClearOutTime = GameControl.Instance.playerData.timesStageClearOutTime;
            playerData.timesDropStage = GameControl.Instance.playerData.timesDropStage;

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
            GameControl.Instance.playerData.currentCoins = playerData.currentCoins;
            GameControl.Instance.playerData.totalCoins = playerData.totalCoins;
            //GameControl.Instance.playerData.numCow01 = playerData.numCow01;
            //GameControl.Instance.playerData.numCow02 = playerData.numCow02;
            //GameControl.Instance.playerData.numCow03 = playerData.numCow03;
            //GameControl.Instance.playerData.numCow04 = playerData.numCow04;
            GameControl.Instance.playerData.level = playerData.level;
            GameControl.Instance.playerData.language = playerData.language;
            // Update 01/10/2022 - Ivo Seitenfus
            GameControl.Instance.playerData.createDate = playerData.createDate;
            GameControl.Instance.playerData.lastSaved = playerData.lastSaved;
            GameControl.Instance.playerData.timePlayed = playerData.timePlayed;
            GameControl.Instance.playerData.flyDistance = playerData.flyDistance;
            // Update 15/10/2022 - Ivo Seitenfus
            GameControl.Instance.playerData.experience = playerData.experience;
            for (int i = 0; i < playerData.numCows.Count; i++)
            {
                GameControl.Instance.playerData.numCows[i] = playerData.numCows[i];
            }

            // Update 17/10/2022 - Ivo Seitenfus
            if (playerData.purchasedCows != null && playerData.purchasedCows.Count > 0)
            {
                for (int i = 0; i < playerData.purchasedCows.Count; i++)
                {
                    //GameControl.Instance.playerData.unlockedCows[i] = playerData.unlockedCows[i];
                    GameControl.Instance.playerData.purchasedCows.Add(playerData.purchasedCows[i]);
                }
            }
            else
            {
                GameControl.Instance.playerData.purchasedCows.Add(0);
            }

            //Update 21/10/2022 - Ivo Seitenfus
            if (playerData.purchasedFlyingSaucerModels != null && playerData.purchasedFlyingSaucerModels.Count > 0)
            {
                for (int i = 0; i < playerData.purchasedFlyingSaucerModels.Count; i++)
                {
                    GameControl.Instance.playerData.purchasedFlyingSaucerModels.Add(playerData.purchasedFlyingSaucerModels[i]);
                }
            }
            else
            {
                GameControl.Instance.playerData.purchasedFlyingSaucerModels.Add(0);

            }

            GameControl.Instance.playerData.flyingSaucerModelId = playerData.flyingSaucerModelId;
            // Update 26/10/2022 - Ivo Seitenfus
            for (int i = 0; i < playerData.tutorials.Count; i++)
            {
                GameControl.Instance.playerData.tutorials[i] = playerData.tutorials[i];
            }
            // Update 04/11/2022 - Ivo Seitenfus
            if (playerData.purchasedStages != null && playerData.purchasedStages.Count > 0)
            {
                for (int i = 0; i < playerData.purchasedStages.Count; i++)
                {
                    GameControl.Instance.playerData.purchasedStages.Add(playerData.purchasedStages[i]);
                }
            }
            else
            {
                GameControl.Instance.playerData.purchasedStages.Add(0);
            }

            // Update 01/12/2022 - Ivo Seitenfus
            GameControl.Instance.playerData.timesSelectPlay = playerData.timesSelectPlay;
            GameControl.Instance.playerData.timesRandomPlay = playerData.timesRandomPlay;
            GameControl.Instance.playerData.timesRewardedAds = playerData.timesRewardedAds;
            GameControl.Instance.playerData.outGameTime = playerData.outGameTime;
            GameControl.Instance.playerData.inGameTime = playerData.inGameTime;

            // Update 08/12/2022 - Ivo Seitenfus
            GameControl.Instance.playerData.music = playerData.music;
            GameControl.Instance.playerData.sfx = playerData.sfx;

            // Update 14/12/2022 - Ivo Seitenfus
            GameControl.Instance.playerData.timesStageClearInTime = playerData.timesStageClearInTime;
            GameControl.Instance.playerData.timesStageClearOutTime = playerData.timesStageClearOutTime;
            GameControl.Instance.playerData.timesDropStage = playerData.timesDropStage;
        }

        public void VerifySavedFiles()
        {

            DirectoryInfo directory = new DirectoryInfo(FilePath());
            FileInfo[] fileInfo = directory.GetFiles("*.db");
            foreach (FileInfo file in fileInfo)
            {
                // FUTURE: Manipulation of multiple instances of saved files
            }
        }

        string FilePath()
        {
            string filePath;
            //#if UNITY_STANDALONE && !UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IPHONE

            //        filePath = filePathStandAlone;
            //#else
            // este código executará no editor, iphone e android builds
            // filePath = Application.persistentDataPath;
            //#endif
            filePath = Application.persistentDataPath;
            return filePath;
        }


    }


}


