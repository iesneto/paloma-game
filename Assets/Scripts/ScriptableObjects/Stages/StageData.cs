using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageData", menuName = "ScriptableObjects/Stages/StageDataAsset", order = 4)]
public class StageData :  ScriptableObject
{
    public int id;
    public string nameID;
    public int buildID;
    public int xp;
    public int time;
    public int levelToUnlock;
    public int value;
    public Sprite image;
}
