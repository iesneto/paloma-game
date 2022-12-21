using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    public class GameControlStarter : MonoBehaviour
    {
        [SerializeField] private GameObject gameControlPrefab;

        // Start is called before the first frame update
        void Start()
        {
            if (GameControl.Instance == null)
            {
                Instantiate(gameControlPrefab);
            }

        }

    }
}