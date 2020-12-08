using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private Transform playerStartLocation;
    [SerializeField] private GameObject CameraPrefab;
    [SerializeField] private GameObject[] spawners;

    private void Awake()
    {
        spawners = GameObject.FindGameObjectsWithTag("Respawn");
        SetupScene();
    }

    public void SetupScene()
    {
        GameObject player = Instantiate(PlayerPrefab, playerStartLocation.position, Quaternion.identity);
        GameObject cam = Instantiate(CameraPrefab, playerStartLocation.position, Quaternion.identity);
        cam.GetComponent<CameraFollow>().SetPlayer(player);

        foreach(GameObject obj in spawners)
        {
            CowSpawner cs = obj.GetComponent<CowSpawner>();
            // Inicializar os spawners com um numero calculado de vacas de acordo com o level do player
            cs.Initialize(10);
        }
    }
}
