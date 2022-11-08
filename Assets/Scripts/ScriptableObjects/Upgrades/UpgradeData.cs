using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/Upgrades/UpgradeAsset", order = 1)]
public class UpgradeData : ScriptableObject
{
    public enum ControlVar { RAYRADIUS, RAYFORCE, RAYMULTIPLIER, PLAYERSPEED, PLAYERENERGY, PLAYERENERGYCONSUME, PLAYERHEALTH}
    public ControlVar type;
    public string[] title;
    public long[] value;
    public Sprite[] valueIcon;
    public Sprite icon;

}
