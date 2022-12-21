using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamob
{
    [CreateAssetMenu(fileName = "NewStageData", menuName = "ScriptableObjects/Stages/StageDataAsset", order = 4)]


    public class StageData : ScriptableObject
    {
        public enum StageShift { DAY, NIGHT }
        public int id;
        public string nameID;
        public int buildID;
        public int xp;
        public int rewardCoins;
        public int time;
        public int levelToUnlock;
        public int value;
        public Sprite image;
        public StageShift shift;
    }
}