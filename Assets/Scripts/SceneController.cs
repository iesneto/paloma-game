using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform playerStartLocation;
    [SerializeField] private GameObject CameraPrefab;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private int sceneCowMultiplier;
    [SerializeField] private bool isLoading = false;

    [SerializeField] private float fadeSpeed;
   // [SerializeField] private Vector2Int numberOfScenes;
    [SerializeField] private int outGameScenes;
    [SerializeField] private int inGameScenes;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int mainMenuSceneBuildIndex;
    [SerializeField] private string mainMenuSceneName;

    private void Awake()
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

    public void LoadRandomLevel()
    {
        //int scn = Random.Range(numberOfScenes.x, numberOfScenes.y + 1);
        int scn = Random.Range(outGameScenes, inGameScenes + 1);
        
        StartCoroutine(LoadSceneAsync(scn));
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
        StartCoroutine(FadeOutLoadingScreen());
        if (scene != mainMenuSceneBuildIndex)
        {
            GameControl.Instance.LevelLoaded();
            InitializeScene();
        }
        else
        {
            GameControl.Instance.StartMainMenu();
        }
        isLoading = false;
    }

    IEnumerator FadeOutLoadingScreen()
    {
        yield return new WaitForSeconds(fadeSpeed);
        uiManager.SetLoadingScreen(false);
        uiManager.FadeOutBlackScreen();

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
        int playerLevel = GameControl.Instance.playerData.level;
        int cowsPerSpawner = (playerLevel * sceneCowMultiplier) / spawners.Length;

        foreach (GameObject obj in spawners)
        {
            CowSpawner cs = obj.GetComponent<CowSpawner>();
            // Inicializar os spawners com um numero calculado de vacas de acordo com o level do player
            cs.Initialize(cowsPerSpawner);
        }
    }
}
