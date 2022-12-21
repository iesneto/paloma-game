using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gamob
{
    [CreateAssetMenu(fileName = "NewCowData", menuName = "ScriptableObjects/Cows/CowDataAsset", order = 2)]
    public class CowData : ScriptableObject
    {
        public int id;
        public int reward;
        public int xp;
        public float mass;
        public float movementSpeed;
        public int tier;
        public int value;
        public int levelToUnlock;
        public Sprite icon;
        public GameObject gamePrefab;
        public GameObject menuPrefab;
    }
}