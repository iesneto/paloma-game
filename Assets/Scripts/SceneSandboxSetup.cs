using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gamob
{
    public class SceneSandboxSetup : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerPrefab;
        [SerializeField] private Transform playerStartLocation;
        [SerializeField] private GameObject CameraPrefab;
        [SerializeField] private GameObject[] spawners;
        [SerializeField] private int sceneCowMultiplier;
        [SerializeField] private int playerLevel;


        private void Start()
        {
            InitializeScene();
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

            int cowsPerSpawner = 0;
            if (spawners.Length > 0)
            {
                cowsPerSpawner = (playerLevel * sceneCowMultiplier) / spawners.Length;
            }


            foreach (GameObject obj in spawners)
            {
                CowSpawner cs = obj.GetComponent<CowSpawner>();
                // Inicializar os spawners com um numero calculado de vacas de acordo com o level do player
                cs.Initialize(cowsPerSpawner);
            }
        }
    }
}