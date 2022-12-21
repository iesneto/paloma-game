using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gamob
{
    public class SceneController : MonoBehaviour
    {
        //[System.Serializable] 
        //private struct ScenesForPlayerLevel
        //{
        //    public int[] scenesReferences;
        //}
        //[SerializeField] private ScenesForPlayerLevel[] scenesForPlayerLevel;

        [SerializeField] private GameObject PlayerPrefab;
        [SerializeField] private Transform playerStartLocation;
        [SerializeField] private GameObject CameraPrefab;
        [SerializeField] private GameObject[] spawners;
        [SerializeField] private int sceneCowMultiplier;
        [SerializeField] private int sceneTotalCows;
        [SerializeField] private bool isLoading = false;

        [SerializeField] private float fadeSpeed;
        // [SerializeField] private Vector2Int numberOfScenes;
        [SerializeField] private int outGameScenes;
        [SerializeField] private int inGameScenes;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private int mainMenuSceneBuildIndex;
        [SerializeField] private string mainMenuSceneName;

        private void Start()
        {
            uiManager = GetComponent<UIManager>();

            inGameScenes = SceneManager.sceneCountInBuildSettings - outGameScenes;

            // Find the Main Menu scene index in Build
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                Scene tempScene = SceneManager.GetSceneByBuildIndex(i);
                if (tempScene.name == mainMenuSceneName) mainMenuSceneBuildIndex = i;
            }
        }

        public void LoadLevel(int _scene)
        {
            int _sceneBuildID = GameControl.Instance.SceneBuildIndex(_scene);
            GameControl.Instance.SetPlayerDataSelectStage();
            //GameControl.Instance.SetStage(_scene);
            StartCoroutine(LoadSceneAsync(_sceneBuildID));
        }
        public void LoadRandomLevel()
        {
            List<int> stages = GameControl.Instance.StagesPurchased();
            int stageIndex = Random.Range(0, stages.Count);
            int _scene = stages[stageIndex];
            int _sceneBuildID = GameControl.Instance.SceneBuildIndex(_scene);
            GameControl.Instance.SetStage(_scene);
            GameControl.Instance.SetPlayerDataRandomStage();
            StartCoroutine(LoadSceneAsync(_sceneBuildID));
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadSceneAsync(mainMenuSceneBuildIndex));
        }
        IEnumerator LoadSceneAsync(int scene)
        {
            if (isLoading) yield break;
            isLoading = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
            uiManager.SetLoadingScreen(true);


            while (!operation.isDone)
            {
                //float progress = Mathf.Clamp01(operation.progress);
                uiManager.SetLoadingBarValue(operation.progress);
                yield return null;
            }
            uiManager.SetLoadingBarValue(1);
            //StartCoroutine(FadeOutLoadingScreen());

            if (scene != mainMenuSceneBuildIndex)
            {
                InitializeScene();
                GameControl.Instance.LevelLoaded();
                StartCoroutine(FadeOutLoadingScreen(false));
            }
            else
            {
                GameControl.Instance.StartMainMenu();
                StartCoroutine(FadeOutLoadingScreen(true));
            }
            isLoading = false;
        }

        IEnumerator FadeOutLoadingScreen(bool _menu)
        {
            yield return new WaitForSeconds(fadeSpeed);
            uiManager.SetLoadingScreen(false);
            uiManager.FadeOutBlackScreen(_menu);

        }

        public int ReturnSceneIndex()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        private void InitializeScene()
        {
            spawners = GameObject.FindGameObjectsWithTag("Respawn");
            playerStartLocation = GameObject.FindGameObjectsWithTag("PlayerStartLocation")[0].transform;
            SetupScene();
        }

        public void SetupScene()
        {
            GameObject player = Instantiate(PlayerPrefab, playerStartLocation.position, Quaternion.identity);
            GameObject cam = Instantiate(CameraPrefab, playerStartLocation.position, Quaternion.identity);
            cam.GetComponent<CameraFollow>().SetPlayer(player);
            player.GetComponent<PlayerBehavior>().SetCameraEffectReference(cam.GetComponent<CameraEffects>());
            GameControl.Instance.SetPlayer(player);
            int playerLevel = GameControl.Instance.playerData.level;
            int cowsPerSpawner = sceneCowMultiplier + (playerLevel / sceneCowMultiplier);
            sceneTotalCows = cowsPerSpawner * spawners.Length;
            GameControl.Instance.SceneTotalCows = sceneTotalCows;

            foreach (GameObject obj in spawners)
            {
                CowSpawner cs = obj.GetComponent<CowSpawner>();
                // Inicializar os spawners com um numero calculado de vacas de acordo com o level do player
                cs.Initialize(cowsPerSpawner);
            }
        }

        public int SceneTotalCows()
        {
            return sceneTotalCows;
        }
    }
}