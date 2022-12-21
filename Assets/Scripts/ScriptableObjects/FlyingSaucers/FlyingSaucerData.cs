using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    [CreateAssetMenu(fileName = "NewFlyingSaucerData", menuName = "ScriptableObjects/FlyingSaucer/FlyingSaucerDataAsset", order = 3)]
    public class FlyingSaucerData : ScriptableObject
    {
        public int id;
        public int value;
        public int levelToUnlock;
        public Sprite icon;
        public GameObject model;
    }
}